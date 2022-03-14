using System;

namespace NeuralNetwork.Core;

public static class Functions
{
    public static class Activation
    {
        public static double LeakyReLU(double alpha, double z)
        {
            return Math.Max(alpha * z, z);
        }

        public static double LeakyReLUDerivative(double alpha, double z)
        {
            return z <= 0 ? alpha : 1;
        }

        public static double ReLU(double z)
        {
            return Math.Max(0, z);
        }

        public static double ReLUDerivative(double z)
        {
            return z <= 0 ? 0 : 1;
        }

        public static double Sigmoid(double z)
        {
            return 1 / (1 + Math.Pow(Math.E, -z));
        }

        public static double SigmoidDerivative(double z)
        {
            var sig = Sigmoid(z);

            return sig * (1 - sig);
        }

        public static double TanH(double z)
        {
            var ePz = Math.Pow(Math.E, z);
            var eNz = Math.Pow(Math.E, -z);

            return (ePz - eNz) / (ePz + eNz);
        }

        public static double TanHDerivative(double z)
        {
            return 1 - Math.Pow(TanH(z), 2);
        }
    }

    public static class Exit
    {
        /*public static double Sigmoid(double z)
        {

        }*/
    }
}
