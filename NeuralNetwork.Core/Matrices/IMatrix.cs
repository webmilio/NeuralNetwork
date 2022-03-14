using System;

namespace NeuralNetwork.Core.Matrices;

public interface IMatrix<T>
{
    public delegate void LoopDelegate(IMatrix<T> matrix, int row, int column);

    public void Resize(int rows, int columns);
    public void Loop(LoopDelegate action);

    public IMatrix<T> Submatrix(int sourceX, int lengthX, int sourceY, int lengthY);

    public IMatrix<T> Inverse();
    public void Apply(IMatrix<T> matrix, int sourceX, int destinationX, int sourceY, int destinationY);

    public int Rows { get; }
    public int Columns { get; }

    public T[,] InnerMatrix { get; }

    public T Determinant { get; }

    public T this[int row, int column] { get; set; }
}
