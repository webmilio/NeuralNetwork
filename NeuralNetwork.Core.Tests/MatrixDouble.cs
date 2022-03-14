using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetwork.Core.Matrices;

namespace NeuralNetwork.Core.Tests;

[TestClass]
public class MatrixDouble
{
    private const int Rows = 4, Columns = 6; // Make sure these are always up-to-date!
    private const int
        Element1_1 = 74, Element1_3 = 13,
        Element2_1 = 66, Element2_5 = 13,
        Element3_4 = 17;

    private double[,] _inner;
    private Matrices.MatrixDouble _matrix;

    [TestInitialize]
    public void Setup()
    {
        _inner = new double[,]
        {
            { 1, 2, 3, 4, 5, 6 }, // Do not change, used in a for-loop for columns vs rows access.
            { 2, Element1_1, 4, Element1_3, 8, 35 },
            { 3, Element2_1, 75, 32, 35, Element2_5 },
            { 4, 79, 51, 12, Element3_4, 1 }
        };
        _matrix = new(_inner);
    }

    [TestMethod]
    public void ArrayConstructor()
    {
        Assert.AreEqual(Rows, _matrix.Rows);
        Assert.AreEqual(Columns, _matrix.Columns);
    }

    [TestMethod]
    public void SizeConstructor()
    {
        const int SizeRows = 10, SizeColumns = 5;

        var matrix = new Core.Matrices.MatrixDouble(SizeRows, SizeColumns);

        Assert.AreEqual(SizeRows, matrix.Rows);
        Assert.AreEqual(SizeColumns, matrix.Columns);
    }

    [TestMethod]
    public void Accessor()
    {
        for (int i = 0; i < _matrix.Columns; i++)
            Assert.AreEqual(i + 1, _matrix[0, i]);

        Assert.AreEqual(Element1_1, _matrix[1, 1]);
        Assert.AreEqual(Element1_3, _matrix[1, 3]);
        Assert.AreEqual(Element2_1, _matrix[2, 1]);
        Assert.AreEqual(Element2_5, _matrix[2, 5]);
        Assert.AreEqual(Element3_4, _matrix[3, 4]);
    }

    [TestMethod]
    public void InnerMatrix()
    {
        for (int r = 0; r < _matrix.Rows; r++)
            for (int c = 0; c < _matrix.Columns; c++)
                Assert.AreEqual(_inner[r, c], _matrix[r, c]);
    }

    [TestMethod]
    public void Resize()
    {
        const int ResizeRows = Rows * 2, ResizeColumns = Columns / 2;

        var submatrix = _matrix.Submatrix(Rows, Columns);
        submatrix.Resize(ResizeRows, ResizeColumns);

        Assert.AreEqual(ResizeRows, submatrix.Rows);
        Assert.AreEqual(ResizeColumns, submatrix.Columns);

        for (int i = 0; i < ResizeRows; i++)
            ;
    }

    [TestMethod]
    public void Submatrix()
    {
        const int SubRows = 2, SubColumns = 3;

        var submatrix = _matrix.Submatrix(1, SubRows, 1, SubColumns);

        Assert.AreEqual(SubRows, submatrix.Rows);
        Assert.AreEqual(SubColumns, submatrix.Columns);

        Assert.AreEqual(Element1_1, submatrix[0, 0]);
        Assert.AreEqual(Element1_3, submatrix[0, 2]);
        Assert.AreEqual(Element2_1, submatrix[1, 0]);
    }
}
