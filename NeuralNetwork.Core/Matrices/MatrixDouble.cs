using System;

namespace NeuralNetwork.Core.Matrices;

public class MatrixDouble : IMatrix<double>
{
    private volatile object _lock = new();

    public MatrixDouble(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;

        InnerMatrix = new double[rows, columns];
    }

    public MatrixDouble(double[,] matrix)
    {
        Rows = matrix.GetLength(0);
        Columns = matrix.GetLength(1);

        InnerMatrix = matrix;
    }

    public void Resize(int rows, int columns) => Resize(rows, 0, columns, 0);

    public void Resize(int rows, int originRow, int columns, int originColumn)
    {
        if (originRow + rows <= 0 || originColumn + columns <= 0)
            throw new ArgumentOutOfRangeException("The combination of rows/columns and origin points must be bigger than 0.");
        
        lock (_lock)
        {
            var previousRows = Rows;
            var previousColumns = Columns;

            Rows = rows;
            Columns = columns;

            var previous = InnerMatrix;
            InnerMatrix = new double[rows, columns];

            Loop(delegate(IMatrix<double> matrix, int row, int column)
            {
                if (row + originRow >= previousRows |
                    column + originColumn >= previousColumns)
                    return;

                matrix[row, column] = previous[row + originRow, column + originColumn];
            });
        }
    }

    public void Loop(IMatrix<double>.LoopDelegate action)
    {
        for (int r = 0; r < Rows; r++)
            for (int c = 0; c < Columns; c++)
                action(this, r, c);
    }

    public bool All(IMatrix<double>.LoopConditionDelegate condition)
    {
        for (int r = 0; r < Rows; r++)
            for (int c = 0; c < Columns; c++)
                if (!condition(this, r, c))
                    return false;

        return true;
    }

    public bool Any(IMatrix<double>.LoopConditionDelegate condition)
    {
        for (int r = 0; r < Rows; r++)
            for (int c = 0; c < Columns; c++)
                if (condition(this, r, c))
                    return true;

        return false;
    }

    public IMatrix<double> Submatrix(int rows, int columns) => Submatrix(rows, 0, columns, 0);

    public IMatrix<double> Submatrix(int rows, int sourceRow, int columns, int sourceColumn)
    {
        const string ErrorTemplate = "The addition of {0} and {1} must be a value between 0 and the upper corresponding dimension of the matrix.";

        if (sourceRow + rows < 0 || sourceRow + rows > Rows)
            throw new ArgumentOutOfRangeException(string.Format(ErrorTemplate, nameof(sourceColumn), nameof(columns)));

        if (sourceColumn + columns < 0 || sourceColumn + columns > Columns)
            throw new ArgumentOutOfRangeException(string.Format(ErrorTemplate, nameof(sourceRow), nameof(rows)));

        MatrixDouble submatrix = new(rows, columns);
        var source = this;

        submatrix.Loop(delegate (IMatrix<double> m, int r, int c)
        {
            m[r, c] = source[r + sourceRow, c + sourceColumn];
        });

        return submatrix;
    }

    public IMatrix<double> Inverse()
    {
        MatrixDouble inverse;

        lock (_lock)
        {
            inverse = new(Rows, Columns);
            Array.Copy(InnerMatrix, inverse.InnerMatrix, InnerMatrix.Length);
        }



        return inverse;
    }

    public void Apply(IMatrix<double> matrix) => Apply(matrix, 0, 0);
    public void Apply(IMatrix<double> matrix, int destinationRow, int destinationColumn) => Apply(matrix, 0, destinationRow, 0, destinationColumn);

    public void Apply(IMatrix<double> matrix, int sourceRow, int destinationRow, int sourceColumn, int destinationColumn)
    {
        const string ErrorTemplate = "Parameter {0} or {1} is out of range; must be a positive integer within the boundaries of {2}. ";

        if (destinationColumn < 0 || destinationColumn >= Columns ||
            destinationRow < 0 || destinationRow >= Rows)
            throw new ArgumentOutOfRangeException(string.Format(ErrorTemplate, nameof(destinationColumn), nameof(destinationRow), "the target matrix"));

        if (sourceColumn < 0 || sourceColumn >= matrix.Columns ||
            sourceRow < 0 || sourceRow >= matrix.Rows)
            throw new ArgumentOutOfRangeException(string.Format(ErrorTemplate, nameof(sourceColumn), nameof(sourceRow), $"the provided {nameof(matrix)}"));

        if (matrix.Columns - sourceColumn + destinationColumn > Columns ||
            matrix.Rows - sourceRow + destinationRow > Rows)
            throw new ArgumentOutOfRangeException("The addition of destination and source arguments to the length of the supplied matrix " +
                "must not result in a row index or column index outside the bounds of the target matrix.");

        Invalidate();

        Loop(delegate (IMatrix<double> m, int r, int c)
        {
            m[r + destinationRow, c + destinationColumn] = matrix.InnerMatrix[r + sourceRow, c + sourceColumn];
        });
    }

    protected void Invalidate()
    {
        _determinant = null;
    }

    public IMatrix<double> Clone()
    {
        var clone = (MatrixDouble) Submatrix(Rows, Columns);
        clone._determinant = _determinant;

        return clone;
    }

    public double this[int row, int column]
    {
        get => InnerMatrix[row, column];
        set
        {
            Invalidate();

            InnerMatrix[row, column] = value;
        }
    }

    public int Rows { get; private set; }
    public int Columns { get; private set; }

    public double[,] InnerMatrix { get; private set; }

    private double? _determinant;
    public double Determinant
    {
        get
        {
            lock (_lock)
            {
                if (_determinant.HasValue)
                    return _determinant.Value;

                if (Rows != Columns)
                    return double.NaN;

                if (Rows == 1)
                    return this[0, 0];

                // Calculate and Store.
                double sum = 0;
                
                int sign = 1;
                int upperBound = Columns - 1;

                for (int c = 0; !(c == Columns && sign < 0); c++)
                {
                    double tmp = sign;

                    for (int i = 0; i < Rows; i++)
                    {
                        int nC = c + i * sign;

                        if (nC >= Columns)
                            nC -= Columns;
                        else if (nC < 0)
                            nC += Columns;

                        tmp *= this[i, nC];
                    }

                    sum += tmp;

                    if (c == upperBound && sign > 0)
                    {
                        c = -1;
                        sign = -1;
                    }
                }

                _determinant = sum;
                return sum;
            }
        }
    }
}
