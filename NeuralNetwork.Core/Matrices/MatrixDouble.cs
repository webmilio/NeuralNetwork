using System;

namespace NeuralNetwork.Core.Matrices;

public struct MatrixDouble : IMatrix<double>
{
    private volatile object _lock = new();
    public double[,] _matrix;

    public MatrixDouble(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;

        _matrix = new double[rows, columns];
    }

    public MatrixDouble(double[,] matrix)
    {
        Rows = matrix.GetLength(0);
        Columns = matrix.GetLength(1);

        _matrix = matrix;
    }

    public void Resize(int columns, int rows)
    {
        lock (_lock)
        {
            Rows = rows;
            Columns = columns;

            _matrix = new double[rows, columns];
        }
    }

    public void Loop(IMatrix<double>.LoopDelegate action)
    {
        for (int r = 0; r < Rows; r++)
            for (int c = 0; c < Columns; c++)
                action(this, r, c);
    }

    public IMatrix<double> Submatrix(int rows, int columns) => Submatrix(0, rows, 0, columns);

    public IMatrix<double> Submatrix(int sourceRow, int rows, int sourceColumn, int columns)
    {
        const string ErrorTemplate = "The addition of {0} and {1} must be a value between 0 and the upper corresponding dimension of the matrix.";

        if (sourceColumn + rows < 0 || sourceColumn + rows > Columns)
            throw new ArgumentOutOfRangeException(string.Format(ErrorTemplate, nameof(sourceColumn), nameof(rows)));

        if (sourceRow + columns < 0 || sourceRow + columns > Rows)
            throw new ArgumentOutOfRangeException(string.Format(ErrorTemplate, nameof(sourceRow), nameof(columns)));

        MatrixDouble submatrix = new(rows, columns);
        var source = this;

        submatrix.Loop((m, r, c) =>
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
            Array.Copy(_matrix, inverse._matrix, _matrix.Length);
        }

        return inverse;
    }

    public void Apply(IMatrix<double> matrix) => Apply(matrix, 0, 0);
    public void Apply(IMatrix<double> matrix, int destinationX, int destinationY) => Apply(matrix, 0, destinationX, 0, destinationY);

    public void Apply(IMatrix<double> matrix, int sourceX, int destinationX, int sourceY, int destinationY)
    {
        const string ErrorTemplate = "Parameter {0} or {1} is out of range; must be a positive integer within the boundaries of {2}. ";

        if (destinationX < 0 || destinationX >= Columns ||
            destinationY < 0 || destinationY >= Rows)
            throw new ArgumentOutOfRangeException(string.Format(ErrorTemplate, nameof(destinationX), nameof(destinationY), "the target matrix"));

        if (sourceX < 0 || sourceX >= matrix.Columns ||
            sourceY < 0 || sourceY >= matrix.Rows)
            throw new ArgumentOutOfRangeException(string.Format(ErrorTemplate, nameof(sourceX), nameof(sourceY), $"the provided {nameof(matrix)}"));

        if (matrix.Columns - sourceX + destinationX > Columns ||
            matrix.Rows - sourceY + destinationY > Rows)
            throw new ArgumentOutOfRangeException("The addition of destination and source arguments to the length of the supplied matrix " +
                "must not result in a row index or column index outside the bounds of the target matrix.");

        Loop((m, r, c) =>
        {
            m[destinationY + r, destinationX + c] = matrix.InnerMatrix[sourceY + r, sourceX + c];
        });
    }

    public double this[int row, int column]
    {
        get
        {
            return _matrix[row, column];
        }
        set
        {
            _matrix[row, column] = value;
        }
    }

    private void Invalidate()
    {
        _determinant = null;
    }

    public int Rows { get; private set; }
    public int Columns { get; private set; }

    public double[,] InnerMatrix => _matrix;

    private double? _determinant = null;
    public double Determinant
    {
        get
        {
            lock (_lock)
            {
                if (_determinant.HasValue)
                    return _determinant.Value;

                // Calculate and Store.
                double sum = 0;
                double tmp = 0;

                for (int x = 0; x < Columns; x++)
                {
                    for (int y = 0; x < Rows; y++)
                    {

                    }
                }

                return 0;
            }
        }
    }
}
