using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM_Project.Classes
{
    public class MatrixCalculator
    {
        private JacobiTransformationManager jacobiTransformationManager = new JacobiTransformationManager();

        private double[,] H1Matrix = new double[4, 4];
        private double[,] H2Matrix = new double[4, 4];
        private double[,] H3Matrix = new double[4, 4];
        private double[,] H4Matrix = new double[4, 4];

        private double[,] C1Matrix = new double[4, 4];
        private double[,] C2Matrix = new double[4, 4];
        private double[,] C3Matrix = new double[4, 4];
        private double[,] C4Matrix = new double[4, 4];
        public double[,] HMatrix { get; private set; }
        public double[,] CMatrix { get; private set; }


        private const double KFACTOR = 30;
        private const double C = 700;
        private const double RO = 7800;

        public MatrixCalculator()
        {
            HMatrix = new double[4, 4];
            CalculateHMatrix();
            CalculateCMatrix();
        }

        private void FillTempVectors(int index, ref double[] tempX, ref double[] tempY, ref double[] transposedX, ref double[] transposedY)
        {
            for(int i = 0; i<4; i++)
            {
                tempX[i] = jacobiTransformationManager.DNDXValuesMatrix[index, i];
                transposedX[i] = jacobiTransformationManager.DNDXValuesMatrix[index, i];
                tempY[i] = jacobiTransformationManager.DNDYValuesMatrix[index, i];
                transposedY[i] = jacobiTransformationManager.DNDYValuesMatrix[index, i];
            }

        }

        private void FillTempVectors(int index, ref double[] temp, ref double[] transposedTemp)
        {
            for (int i = 0; i < 4; i++)
            {
                temp[i] = jacobiTransformationManager.NValuesMatrix[index, i];
                transposedTemp[i] = jacobiTransformationManager.NValuesMatrix[index, i];
            }

        }

        private double [,] MultiplyTwoVectors(double[] vector, double[] transposedVector)
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

        private double[,] MultiplyNumberAndMatrix(double value, double[,] matrix)
        {

            for (int i = 0; i < 4; i++)
            { 
                for(int j=0; j<4; j++)
                {
                    matrix[i, j] = value * matrix[i, j];
                }
            }

            return matrix;
        }


        private double [,] SumTwoMatrices(double[,] internalX, double[,] internalY)
        {
            double[,] result = internalX;
            
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result[i, j] = internalX[i, j] + internalY[i, j];
                }
            }

            return result;
        }

        private double[,] CreateZerosMatrix()
        {
            double[,] result = new double[4, 4];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result[i, j] = 0;
                }
            }

            return result;

        }
        private double[,] CalculateInternalHMatrices(double[] tempX, double[] tempY, double[] transposedX, double[] transposedY, double det)
        {
            double[,] tempXMatrix = MultiplyTwoVectors(tempX, transposedX);
            double[,] tempYMatrix = MultiplyTwoVectors(tempY, transposedY);
            double[,] result = CreateZerosMatrix();
            double factor = 0;

            result = SumTwoMatrices(tempXMatrix, tempYMatrix);
            factor = KFACTOR * det;
            result = MultiplyNumberAndMatrix(factor, result);
            
            return result;
        }
        private void CalculateHMatrix()
        {
            double[,] tempHMatrix = new double[4, 4];
            double[] tempX = new double[4];
            double[] tempY = new double[4];
            double[] tempTransposedX = new double[4];
            double[] tempTransposedY = new double[4];

            HMatrix = CreateZerosMatrix();
            double det;

            for (int i=0; i<4; i++)
            {
                FillTempVectors(i, ref tempX,  ref tempY, ref tempTransposedX, ref tempTransposedY);
                det = jacobiTransformationManager.TabOfDeterminants[i];

                tempHMatrix = CalculateInternalHMatrices(tempX, tempY, tempTransposedX, tempTransposedY, det);
                HMatrix = SumTwoMatrices(HMatrix, tempHMatrix);
            }


        }

        private double CalculateFactorForCMatrix(double det, double c, double ro)
        {
            return det * c * ro;
        }

        private void CalculateCMatrix()
        {
            double[,] tempCMatrix = new double[4, 4];
            double[] temp = new double[4];
            double[] tempTransposed = new double[4];

            CMatrix = CreateZerosMatrix();
            double det;
            double factor;

            for (int i = 0; i < 4; i++)
            {
                FillTempVectors(i, ref temp, ref tempTransposed);
                det = jacobiTransformationManager.TabOfDeterminants[i];
                tempCMatrix = MultiplyTwoVectors(temp, tempTransposed);
                factor = CalculateFactorForCMatrix(det, C, RO);
                tempCMatrix = MultiplyNumberAndMatrix(factor, tempCMatrix);
                CMatrix = SumTwoMatrices(CMatrix, tempCMatrix);
            }

        }
    }
}
