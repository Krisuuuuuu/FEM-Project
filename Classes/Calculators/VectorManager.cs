using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM_Project.Classes
{
    public abstract class VectorManager
    {
        protected double[] FillVectorByValue(double[] vector, double value)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] = value;
            }

            return vector;
        }
        protected double[] MultiplyNumberAndVector(double value, double[] vector)
        {
            double[] result = new double[vector.Length];

            for (int i = 0; i < 4; i++)
            {
                result[i] = vector[i] * value;
            }

            return result;
        }
        protected double[] DivideVectorByNumber(double value, double[] vector)
        {
            double[] result = new double[vector.Length];
            try
            {
                for (int i = 0; i < vector.Length; i++)
                {
                    result[i] = vector[i] / value;
                }
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Error. Dividing by zero.");
            }

            return result;
        }

        protected double[] SumTwoVectors(double[] internalX, double[] internalY)
        {
            double[] result = new double[internalX.Length];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = internalX[i] + internalY[i];
            }

            return result;
        }
        protected double[,] MultiplyTwoVectors(double[] vector, double[] transposedVector)
        {
            double value;
            double[,] result = new double[4, 4];

            for (int i = 0; i < vector.Length; i++)
            {
                value = vector[i];

                for (int j = 0; j < vector.Length; j++)
                {
                    result[i, j] = value * transposedVector[j];
                }
            }

            return result;
        }
    }
}
