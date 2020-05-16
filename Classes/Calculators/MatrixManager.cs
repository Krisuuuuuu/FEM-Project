using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM_Project.Classes
{
    public abstract class MatrixManager : VectorManager
    {
        protected double[,] CreateZerosMatrix(int sizeX, int sizeY)
        {
            double[,] result = new double[sizeX, sizeY];

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    result[i, j] = 0;
                }
            }

            return result;
        }
        protected double[,] MultiplyNumberAndMatrix(double value, double[,] matrix, int size)
        {
            double[,] result = new double[size, size];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result[i, j] = value * matrix[i, j];
                }
            }

            return result;
        }

        protected double[,] DivideMatrixByNumber(double[,] matrix, int size, double value)
        {
            double[,] result = new double[size, size];

            try
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        result[i, j] = matrix[i, j] / value;
                    }
                }
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Error. Dividing by zero.");
            }

            return result;
        }

        protected double[,] SumTwoMatrices(double[,] internalX, double[,] internalY, int sizeX, int sizeY)
        {
            double[,] result = CreateZerosMatrix(sizeX, sizeY);

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    result[i, j] = internalX[i, j] + internalY[i, j];
                }
            }

            return result;
        }
        protected double[] MultiplyVectorAndMatrix(double[] vector, double[,] matrix)
        {
            int dimension = 1;
            double[,] tempResult = new double[vector.Length, dimension];
            double[] result = new double[vector.Length];

            for (int j = 0; j < dimension; j++)
            {
                for (int k = 0; k < vector.Length; k++)
                {
                    for (int i = 0; i < vector.Length; i++)
                    {
                        tempResult[i, j] += matrix[i, k] * vector[j];
                    }
                }
            }

            for (int i = 0; i < vector.Length; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    result[i] = tempResult[i, j];
                }
            }

            return result;
        }
    }
}
