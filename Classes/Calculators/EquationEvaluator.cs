using FEM_Project.Classes.Calculators;
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

            for (int i = (int)step; i < simulationTime; i+=50)
            {
                //Left Side
                tempCMatrix = DivideMatrixByNumber(ConstCMatrix, dimension, step);
                tempHMatrix = SumTwoMatrices(ConstHMatrix, tempCMatrix, ConstPVector.Length, ConstPVector.Length);

                //Right Side
                tVector = MultiplyVectorAndMatrix(tVector, tempCMatrix);
                tempPVector = SumTwoVectors(ConstPVector, tVector);
            }
        }

    }
}
