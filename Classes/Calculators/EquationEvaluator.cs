using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace FEM_Project.Classes.Calculators
{
    public class EquationEvaluator
    {
        private readonly double simulationTime;
        private double step;
        private double initialTemperature;

        public EquationEvaluator(double simulationTime, double step, double initialTemperature)
        {
            this.simulationTime = simulationTime;
            this.step = step;
            this.initialTemperature = initialTemperature;
        }

        public void EvaluateEquation(double[,] globalHMatrix, double[,] globalCMatrix, double[] globalPVector, int dimension)
        {
            double[,] ConstHMatrix = globalHMatrix;
            double[,] ConstCMatrix = globalCMatrix;
            double[] ConstPVector = globalPVector;
            double[] tVector = new double[dimension];
            double[,] tempHMatrix;
            double[,] tempCMatrix;
            double[] tempPVector;

            tVector = FillVectorByValue(tVector, initialTemperature);

            for (int i = (int)step; i < simulationTime; step+=50)
            {
                //Left Side
                tempCMatrix = DivideMatrixByNumber(ConstCMatrix, dimension, step);
                tempHMatrix = SumTwoMatrices(ConstHMatrix, tempCMatrix, ConstPVector.Length, ConstPVector.Length);

                //Right Side
                tVector = MultiplyVectorAndMatrix(tVector, tempCMatrix);
                tempPVector = SumTwoVectors(ConstPVector, tVector);
            }
        }
        protected double[] SumTwoVectors(double[] internalX, double[] internalY)
        {
            double[] result = internalX;

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = internalX[i] + internalY[i];
            }

            return result;
        }

        protected double[,] SumTwoMatrices(double[,] internalX, double[,] internalY, int sizeX, int sizeY)
        {
            double[,] result = internalX;

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    result[i, j] = internalX[i, j] + internalY[i, j];
                }
            }

            return result;
        }

        private double[] MultiplyVectorAndMatrix(double[] vector, double[,] matrix)
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

            for(int i=0; i < vector.Length; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    result[i] = tempResult[i, j];    
                }
            }

            return result;
        }

        protected double[] FillVectorByValue(double[] vector, double value)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] = value;
            }

            return vector;
        }

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

        protected double[,] DivideMatrixByNumber(double[,] matrix, int size, double value)
        {
            try
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        matrix[i, j] = matrix[i, j] / value;
                    }
                }
            }
            catch(DivideByZeroException)
            {
                Console.WriteLine("Error. Dividing by zero.");
            }

            return matrix;
        }

        protected double[] DivideVectorByNumber(double value, double[] vector)
        {
            try
            {
                for (int i = 0; i < vector.Length; i++)
                {
                    vector[i] = vector[i] / value;
                }
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Error. Dividing by zero.");
            }

            return vector;
        }
    }
}
