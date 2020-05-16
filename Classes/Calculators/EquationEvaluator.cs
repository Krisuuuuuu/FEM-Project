using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace FEM_Project.Classes
{
    public class EquationEvaluator : MatrixManager
    {
        private readonly double simulationTime;
        private readonly double step;
        private readonly double initialTemperature;

        private Matrix<double> hMatrix;
        private Matrix<double> cMatrix;
        private Matrix<double> pVector;

        public EquationEvaluator(double simulationTime, double step, double initialTemperature, 
            double[,] globalHMatrix, double[,] globalCMatrix, double[] globalPVector)
        {
            this.simulationTime = simulationTime;
            this.step = step;
            this.initialTemperature = initialTemperature;

            CreateMatricesAndVectors(globalHMatrix, globalCMatrix, globalPVector);
        }

        private void CreateMatricesAndVectors(double[,] globalHMatrix, double[,] globalCMatrix, double[] globalPVector)
        {
            hMatrix = Matrix<double>.Build.Dense(globalPVector.Length, globalPVector.Length);
            cMatrix = Matrix<double>.Build.Dense(globalPVector.Length, globalPVector.Length);
            pVector = Matrix<double>.Build.Dense(globalPVector.Length, 1);

            for (int i = 0; i < globalPVector.Length; i++)
            {
                for (int j = 0; j < globalPVector.Length; j++)
                {
                    hMatrix[i, j] = globalHMatrix[i, j];
                    cMatrix[i, j] = globalCMatrix[i, j];
                }
            }

            for(int i = 0; i<globalPVector.Length; i++)
            {
                pVector[i, 0] = globalPVector[i];
            }
        }

        public void EvaluateEquation(int dimension, ref IDictionary<int, double> minDictionary, ref IDictionary<int, double> maxDictionary)
        {
            double[] tInitValues = new double[dimension];
            Matrix<double> tempHMatrix = Matrix<double>.Build.Dense(dimension, dimension);
            Matrix<double> tempCMatrix = Matrix<double>.Build.Dense(dimension, dimension);
            Matrix<double> tempPVector = Matrix<double>.Build.Dense(1, dimension);

            Matrix<double> tVector = Matrix<double>.Build.Dense(1, dimension, initialTemperature);

            tInitValues = FillVectorByValue(tInitValues, initialTemperature);

            for (int i = 0; i < dimension; i++)
            {
                tempPVector[0, i] = pVector[i, 0]; 
            }

            double min = 0;
            double max = 0;

            for (int i = (int)step; i < simulationTime + step; i+=(int)step)
            {
                //Left Side
                cMatrix.Divide(step, tempCMatrix);
                hMatrix.Add(tempCMatrix, tempHMatrix);

                //Right Side
                tVector.Multiply(tempCMatrix, tVector);
                tempPVector.Add(tVector, tVector);

                //Inverse and solve
                tempHMatrix = tempHMatrix.Inverse();
                tVector.Multiply(tempHMatrix, tVector);

                //Get min and max
                min = tVector.Enumerate(Zeros.Include).Min();
                max = tVector.Enumerate(Zeros.Include).Max();

                minDictionary.Add(i, min);
                maxDictionary.Add(i, max);
            }
        }     

    }
}
