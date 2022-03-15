using System.Collections.Generic;

namespace NeuralNetwork.Core;

public class Network
{
    protected Layer entryLayer;
    protected IList<Layer> hiddenLayers;
    protected Layer outputLayer;
}