using System.Collections.Generic;

namespace NeuralNetwork.Core;

public class Network
{
    protected IList<Node> _entryLayer;
    protected IList<Node> _hiddenLayer;
    protected IList<Node> _exitLayer;
}