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

        private readonly double[,] constHMatrix;
        private readonly double[,] constCMatrix;
        private readonly double[] constPVector;

        public EquationEvaluator(double simulationTime, double step, double initialTemperature, 
            double[,] globalHMatrix, double[,] globalCMatrix, double[] globalPVector)
        {
            this.simulationTime = simulationTime;
            this.step = step;
            this.initialTemperature = initialTemperature;
            constHMatrix = globalHMatrix;
            constCMatrix = globalCMatrix;
            constPVector = globalPVector;
        }

        public void EvaluateEquation(int dimension)
        {
            double[] tVector = new double[dimension];
            double[,] tempHMatrix;
            double[,] tempCMatrix;
            double[] tempPVector;

            tVector = FillVectorByValue(tVector, initialTemperature);

            for (int i = (int)step; i < simulationTime; i+=50)
            {
                //Left Side
                tempCMatrix = DivideMatrixByNumber(constCMatrix, dimension, step);
                tempHMatrix = SumTwoMatrices(constHMatrix, tempCMatrix, constPVector.Length, constPVector.Length);

                //Right Side
                tVector = MultiplyVectorAndMatrix(tVector, tempCMatrix);
                tempPVector = SumTwoVectors(constPVector, tVector);
            }
        }     

    }
}
