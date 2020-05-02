using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM_Project.Classes
{
    public class JacobiTransformationManager
    {
        private UniversalElement universalElement = new UniversalElement();
        public double[] XValues { get; private set; }
        public double[] YValues { get; private set; }
        public double[] DXDKsi { get; private set; }
        public double[] DYDKsi { get; private set; }
        public double[] DXDEta { get; private set; }
        public double[] DYDEta { get; private set; }
        public double[,] DNDXValuesMatrix { get; private set; }
        public double[,] DNDYValuesMatrix { get; private set; }
        public double[,] NValuesMatrix{ get; private set; }
        public Edge[] Edges { get; private set; }
        public double[] TabOfDeterminants { get; private set; }

        public JacobiTransformationManager()
        {
            DXDKsi = new double[4];
            DYDKsi = new double[4];
            DXDEta = new double[4];
            DYDEta = new double[4];
            DNDXValuesMatrix = new double[4, 4];
            DNDYValuesMatrix = new double[4, 4];
            TabOfDeterminants = new double[4];
            Edges = universalElement.Edges;
            NValuesMatrix = universalElement.NValuesMatrix;
        }
        private void ClearAllComponents()
        {
            DXDKsi = new double[4];
            DYDKsi = new double[4];
            DXDEta = new double[4];
            DYDEta = new double[4];
            DNDXValuesMatrix = new double[4, 4];
            DNDYValuesMatrix = new double[4, 4];
            TabOfDeterminants = new double[4];
        }

        public void UpdateTabsOfCoordinates(double x1, double x2, double x3, double x4, double y1, double y2, double y3, double y4)
        {
            XValues = new double[4] { 0, 0.025, 0.025, 0};
            YValues = new double[4] { 0, 0, 0.025, 0.025};

            //XValues = new double[4] { x1, x2, x3, x4 };
            //YValues = new double[4] { y1, y2, y3, y4 };
        }

        public void CalculateDeterminantsForPVector(double x1, double x2, double x3, double x4, double y1, double y2, double y3, double y4)
        {
            ClearAllComponents();
            UpdateTabsOfCoordinates(x1, x2, x3, x4, y1, y2, y3, y4);

            for(int i=0; i<4; i++)
            {
                if(i==3)
                {
                    TabOfDeterminants[i] = CalculateLengthInGlobalCoordinateSystem(XValues[3], XValues[0], YValues[3], YValues[0])/2;
                }
                else
                {
                    TabOfDeterminants[i] = CalculateLengthInGlobalCoordinateSystem(XValues[i], XValues[i + 1], YValues[i], YValues[i + 1]) / 2;
                }
            }
        }

        private void CalculateDXDKsi()
        {
            for(int i = 0; i<4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    DXDKsi[i] += universalElement.DNDKsiValuesMatrix[i,j] * XValues[j];
                }
            }

        }

        private void CalculateDYDKsi()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    DYDKsi[i] += universalElement.DNDKsiValuesMatrix[i, j] * YValues[j];
                }
            }
        }

        private void CalculateDXDEta()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    DXDEta[i] += universalElement.DNDEtaValuesMatrix[i, j] * XValues[j];
                }
            }

        }

        private void CalculateDYDEta()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    DYDEta[i] += universalElement.DNDEtaValuesMatrix[i, j] * YValues[j];
                }
            }
        }

        private double Calculate2DimDeterminant(double [,] matrix)
        {
            double det = 0;

            try
            {
                det = matrix[0, 0] * matrix[1, 1] - matrix[1, 0] * matrix[0, 1];
            }
            catch(ArgumentOutOfRangeException)
            {
                Console.WriteLine("This operation is not allowed in matrix with this dimension.");
            }

            return det;
        }

        private double CalculateLengthInGlobalCoordinateSystem(double x1, double x2, double y1, double y2)
        {
            return Math.Sqrt(Math.Pow(x2-x1, 2) + Math.Pow(y2-y1, 2));
        }

        private double[,] FillTempMatrix(int index, double [,] matrix)
        {
            try
            {
                matrix[0, 0] = DXDKsi[index];
                matrix[0, 1] = DYDKsi[index];
                matrix[1, 0] = DXDEta[index];
                matrix[1, 1] = DYDEta[index];
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("This operation is not allowed in matrix with this dimension.");
            }

            return matrix;
        }

        private double [,] FillReverseTempMatrix(int index, double[,] matrix)
        {
            try
            {
                matrix[0, 0] = DYDEta[index];
                matrix[0, 1] = -DYDKsi[index];
                matrix[1, 0] = -DXDEta[index];
                matrix[1, 1] = DXDKsi[index];
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("This operation is not allowed in matrix with this dimension.");
            }

            return matrix;
        }

        private double [,] MultiplyDetAnd2DMatrix(double det, double [,] matrix)
        {
            try
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        matrix[i, j] = det * matrix[i, j];
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("This operation is not allowed in matrix with this dimension.");
            }

            return matrix;
        }

        private void Fill2DMainMatrices(int index, double [,] temp)
        {
            double ax1 = temp[0, 0];
            double ax2 = temp[0, 1];
            double ay1 = temp[1, 0];
            double ay2 = temp[1, 1];

            for(int i = 0; i < 4; i++)
            {
                DNDXValuesMatrix[index, i] = ax1 * universalElement.DNDKsiValuesMatrix[index, i] + ax2 * universalElement.DNDEtaValuesMatrix[index, i];
                DNDYValuesMatrix[index, i] = ay1 * universalElement.DNDKsiValuesMatrix[index, i] + ay2 * universalElement.DNDEtaValuesMatrix[index, i];
            }
        }



        public void CalculateMatricesOfDerivatives(double x1, double x2, double x3, double x4, double y1, double y2, double y3, double y4)
        {
            ClearAllComponents();
            UpdateTabsOfCoordinates(x1, x2, x3, x4, y1, y2, y3, y4);

            CalculateDXDKsi();
            CalculateDYDKsi();
            CalculateDXDEta();
            CalculateDYDEta();
        }

        public void JacobiTransformation()
        {
            double [,] tempMatrix = new double[2, 2];
            double det;

            for (int i = 0; i < 4; i++)
            {
                tempMatrix = FillTempMatrix(i, tempMatrix);
                det = Calculate2DimDeterminant(tempMatrix);
                TabOfDeterminants[i] = det;
                det = 1 / det;
                tempMatrix = FillReverseTempMatrix(i, tempMatrix);
                tempMatrix = MultiplyDetAnd2DMatrix(det, tempMatrix);

                Fill2DMainMatrices(i, tempMatrix);
            }
        }
    }
}
