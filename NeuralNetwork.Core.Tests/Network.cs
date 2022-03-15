using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeuralNetwork.Core.Tests;

[TestClass]
public class Network
{
    private Core.Network _network;

    [TestMethod]
    public void Setup()
    {
        _network = new();
    }
}