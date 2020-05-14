using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace FEM_Project.Classes
{
    public class MatrixCalculator
    {
        private JacobiTransformationManager jacobiTransformationManager = new JacobiTransformationManager();

        private double[,] h1Matrix = new double[4, 4];
        private double[,] h2Matrix = new double[4, 4];
        private double[,] h3Matrix = new double[4, 4];
        private double[,] h4Matrix = new double[4, 4];

        private double[,] c1Matrix = new double[4, 4];
        private double[,] c2Matrix = new double[4, 4];
        private double[,] c3Matrix = new double[4, 4];
        private double[,] c4Matrix = new double[4, 4];

        private readonly double[] twoPointsGaussianFactors;

        private const double KFACTOR = 25;
        private const double ALPHA = 25;
        private const double C = 700;
        private const double RO = 7800;
        private const double T0 = 1200;

        public MatrixCalculator()
        {
            twoPointsGaussianFactors = new double[2] { 1, 1 };
        }

        private double CalculateFactorForInternalHBCMatrix(int index)
        {
            return twoPointsGaussianFactors[index] * ALPHA;
        }

        private double CalculateFactorForHMatrix(double det)
        {
            return det * KFACTOR;
        }

        private double CalculateFactorForCMatrix(double det)
        {
            return det * C * RO;
        }

        private double CalculateFactorForPVector(int index)
        {
            return twoPointsGaussianFactors[index] * ALPHA * T0;
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
        private void FillTempVectors(Edge edge, int index, ref double[] tempP1, ref double[] transposedTempP1,
            ref double[] tempP2, ref double[] transposedTempP2)
        {
            for (int i = 0; i < 4; i++)
            {
                tempP1[i] = edge.NValuesMatrix[0, i];
                transposedTempP1[i] = edge.NValuesMatrix[0, i];
                tempP2[i] = edge.NValuesMatrix[1, i];
                transposedTempP2[i] = edge.NValuesMatrix[1, i];
            }

        }

        private void FillTempVectors(Edge edge, int index, ref double[] tempP1, ref double[] tempP2)
        {
            for (int i = 0; i < 4; i++)
            {
                tempP1[i] = edge.NValuesMatrix[0, i];
                tempP2[i] = edge.NValuesMatrix[1, i];
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

        private double[] MultiplyNumberAndVector(double value, double[] vector)
        {

            for (int i = 0; i < 4; i++)
            {
                vector[i] = vector[i] * value;
            }

            return vector;
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

        private double[] SumTwoVectors(double[] internalX, double[] internalY)
        {
            double[] result = internalX;

            for (int i = 0; i < 4; i++)
            {
                result[i] = internalX[i] + internalY[i];
            }

            return result;
        }

        private double[,] CalculateInternalMatrices(double[] tempX, double[] tempY, double[] transposedX, double[] transposedY, double factor)
        {
            double[,] tempXMatrix = MultiplyTwoVectors(tempX, transposedX);
            double[,] tempYMatrix = MultiplyTwoVectors(tempY, transposedY);
            double[,] result = CreateZerosMatrix();

            result = SumTwoMatrices(tempXMatrix, tempYMatrix);
            result = MultiplyNumberAndMatrix(factor, result);
            
            return result;
        }

        private double[,] CalculateInternalMatrices(double[] tempP1, double[] tempP2, double[] transposedP1, double[] transposedP2, double[] gaussianFactors
            , double det)
        {
            double[,] result = CreateZerosMatrix();
            double p1Factor = CalculateFactorForInternalHBCMatrix(0);
            double p2Factor = CalculateFactorForInternalHBCMatrix(1);

            double[,] tempP1Matrix = MultiplyTwoVectors(tempP1, transposedP1);
            double[,] tempP2Matrix = MultiplyTwoVectors(tempP2, transposedP2);
            tempP1Matrix = MultiplyNumberAndMatrix(p1Factor, tempP1Matrix);
            tempP2Matrix = MultiplyNumberAndMatrix(p2Factor, tempP2Matrix);

            result = SumTwoMatrices(tempP1Matrix, tempP2Matrix);
            result = MultiplyNumberAndMatrix(det, result);
           
            return result;
        }

        private double[] CalculateInternalVector(double[] tempP1, double[] tempP2, double[] gaussianFactors, double det)
        {
            double[] result = new double[4] { 0, 0, 0, 0 };
            double p1Factor = CalculateFactorForPVector(0);
            double p2Factor = CalculateFactorForPVector(1);

            tempP1 = MultiplyNumberAndVector(p1Factor, tempP1);
            tempP2 = MultiplyNumberAndVector(p2Factor, tempP2);

            result = SumTwoVectors(tempP1, tempP2);
            result = MultiplyNumberAndVector(det, result);

            return result;
        }

        public double[,] CalculateHMatrix(double x1, double x2, double x3, double x4, double y1, double y2, double y3, double y4, bool[] flags)
        {
            double[,] hMatrix = CreateZerosMatrix();
            double[,] tempHMatrix = CreateZerosMatrix();
            double[,] hBCMatrix = CreateZerosMatrix();
            double[,] tempHBCMatrix = CreateZerosMatrix();
            double[] tempX = new double[4];
            double[] tempY = new double[4];
            double[] tempTransposedX = new double[4];
            double[] tempTransposedY = new double[4];

            jacobiTransformationManager.CalculateMatricesOfDerivatives(x1, x2, x3, x4, y1, y2, y3, y4);
            jacobiTransformationManager.JacobiTransformation();

            hMatrix = CreateZerosMatrix();
            double det;
            double factor;

            for (int i=0; i<4; i++)
            {
                FillTempVectors(i, ref tempX,  ref tempY, ref tempTransposedX, ref tempTransposedY);
                det = jacobiTransformationManager.TabOfDeterminants[i];
                factor = CalculateFactorForHMatrix(det);
                tempHMatrix = CalculateInternalMatrices(tempX, tempY, tempTransposedX, tempTransposedY, factor);
                hMatrix = SumTwoMatrices(hMatrix, tempHMatrix);

                if(flags[i] == true)
                {
                    tempHBCMatrix = CalculateHBCMatrix(i);
                    hBCMatrix = SumTwoMatrices(hBCMatrix, tempHBCMatrix);
                }
            }

            hMatrix = SumTwoMatrices(hMatrix, hBCMatrix);

            return hMatrix;

        }

        public double[,] CalculateCMatrix(double x1, double x2, double x3, double x4, double y1, double y2, double y3, double y4)
        {
            double[,] cMatrix = new double[4, 4];
            double[,] tempCMatrix = new double[4, 4];
            double[] temp = new double[4];
            double[] tempTransposed = new double[4];
            double det;
            double factor;

            jacobiTransformationManager.CalculateMatricesOfDerivatives(x1, x2, x3, x4, y1, y2, y3, y4);
            jacobiTransformationManager.JacobiTransformation();
            cMatrix = CreateZerosMatrix();

            for (int i = 0; i < 4; i++)
            {
                FillTempVectors(i, ref temp, ref tempTransposed);
                det = jacobiTransformationManager.TabOfDeterminants[i];
                tempCMatrix = MultiplyTwoVectors(temp, tempTransposed);
                factor = CalculateFactorForCMatrix(det);
                tempCMatrix = MultiplyNumberAndMatrix(factor, tempCMatrix);
                cMatrix = SumTwoMatrices(cMatrix, tempCMatrix);
            }

            return cMatrix;

        }
        public double[,] CalculateHBCMatrix(int index)
        {
            jacobiTransformationManager.CalculateDeterminantsForPVector();
            double[,] hBCMatrix = CreateZerosMatrix();
            hBCMatrix = CreateZerosMatrix();
            double[] tempP1 = new double[4];
            double[] tempTransposedP1 = new double[4];
            double[] tempP2 = new double[4];
            double[] tempTransposedP2 = new double[4];          

            double det = jacobiTransformationManager.TabOfDeterminantsForPVector[index];
            FillTempVectors(jacobiTransformationManager.Edges[index], index, ref tempP1, ref tempTransposedP1, ref tempP2, ref tempTransposedP2);
            hBCMatrix = CalculateInternalMatrices(tempP1, tempP2, tempTransposedP1, tempTransposedP2, twoPointsGaussianFactors, det);

            return hBCMatrix;
        }

        public double[] CalculatePVector(bool[] flags)
        {
            jacobiTransformationManager.CalculateDeterminantsForPVector();
            double[] pVector = new double[4] { 0, 0, 0, 0 };
            double[] tempPVector = new double[4] { 0, 0, 0, 0 };

            double[] tempP1 = new double[4];
            double[] tempP2 = new double[4];
            double det;

            for(int i=0; i<4; i++)
            {
                if (flags[i] == true)
                {
                    det = jacobiTransformationManager.TabOfDeterminantsForPVector[i];
                    FillTempVectors(jacobiTransformationManager.Edges[i], i, ref tempP1, ref tempP2);
                    tempPVector = CalculateInternalVector(tempP1, tempP2, twoPointsGaussianFactors, det);
                    pVector = SumTwoVectors(pVector, tempP1);
                }
            }

            return pVector;
        }

    }
}
