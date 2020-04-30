using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM_Project.Classes
{
    public class UniversalElement
    {
        private UniversalElementFunctions universalElementFunctions = new UniversalElementFunctions();

        public Edge[] Edges { get; private set; }
        public double[] KsiValues { get; private set; }
        public double[] EtaValues { get; private set; }
        public double[] KsiValuesForEdges { get; private set; }
        public double[] EtaValuesForEdges { get; private set; }
        public double[,] NValuesMatrix { get; private set; }
        public double[,] DNDKsiValuesMatrix { get; set; }
        public double[,] DNDEtaValuesMatrix { get; set; }

        public UniversalElement()
        {
            KsiValues = new double[4] {-1/(Math.Sqrt(3)), 1 / (Math.Sqrt(3)), 1 / (Math.Sqrt(3)), -1 / (Math.Sqrt(3)) };
            EtaValues = new double[4] { -1 / (Math.Sqrt(3)), -1 / (Math.Sqrt(3)), 1 / (Math.Sqrt(3)), 1 / (Math.Sqrt(3)) };
            KsiValuesForEdges = new double[8] { -1 / (Math.Sqrt(3)), 1 / (Math.Sqrt(3)), 1, 1, 1 / (Math.Sqrt(3)), -1 / (Math.Sqrt(3)), -1, -1 };
            EtaValuesForEdges = new double[8] { -1, -1, -1 / (Math.Sqrt(3)), 1 / (Math.Sqrt(3)), 1, 1, 1 / (Math.Sqrt(3)), -1 / (Math.Sqrt(3)) };
            Edges = new Edge[4];
            NValuesMatrix = new double[4, 4];
            DNDKsiValuesMatrix = new double[4, 4];
            DNDEtaValuesMatrix = new double[4, 4];

            FillNMatrix();
            FillDNDKsiMatrix();
            FillDNDEtaMatrix();
            GenerateEdges();
        }

        private void FillNMatrix()
        {
            for (int i = 0; i < 4; i++)
            {
                NValuesMatrix[i, 0] = universalElementFunctions.N1Function(KsiValues[i], EtaValues[i]);
                NValuesMatrix[i, 1] = universalElementFunctions.N2Function(KsiValues[i], EtaValues[i]);
                NValuesMatrix[i, 2] = universalElementFunctions.N3Function(KsiValues[i], EtaValues[i]);
                NValuesMatrix[i, 3] = universalElementFunctions.N4Function(KsiValues[i], EtaValues[i]);
            }
        }

        private void FillInternalNMatrix(Edge edge)
        {
            for(int i=0; i<2; i++)
            {
                edge.NValuesMatrix[i, 0] = universalElementFunctions.N1Function(edge.KsiValues[i], edge.EtaValues[i]);
                edge.NValuesMatrix[i, 1] = universalElementFunctions.N2Function(edge.KsiValues[i], edge.EtaValues[i]);
                edge.NValuesMatrix[i, 2] = universalElementFunctions.N3Function(edge.KsiValues[i], edge.EtaValues[i]);
                edge.NValuesMatrix[i, 3] = universalElementFunctions.N4Function(edge.KsiValues[i], edge.EtaValues[i]);
            }
        }

        private void FillDNDKsiMatrix()
        {
            for (int i = 0; i < 4; i++)
            {
                DNDKsiValuesMatrix[i, 0] = universalElementFunctions.DN1DKsiFunction(EtaValues[i]);
                DNDKsiValuesMatrix[i, 1] = universalElementFunctions.DN2DKsiFunction(EtaValues[i]);
                DNDKsiValuesMatrix[i, 2] = universalElementFunctions.DN3DKsiFunction(EtaValues[i]);
                DNDKsiValuesMatrix[i, 3] = universalElementFunctions.DN4DKsiFunction(EtaValues[i]);
            }
        }

        private void FillDNDEtaMatrix()
        {
            for (int i = 0; i < 4; i++)
            {
                DNDEtaValuesMatrix[i, 0] = universalElementFunctions.DN1DEtaFunction(KsiValues[i]);
                DNDEtaValuesMatrix[i, 1] = universalElementFunctions.DN2DEtaFunction(KsiValues[i]);
                DNDEtaValuesMatrix[i, 2] = universalElementFunctions.DN3DEtaFunction(KsiValues[i]);
                DNDEtaValuesMatrix[i, 3] = universalElementFunctions.DN4DEtaFunction(KsiValues[i]);
            }
        }

        private void GenerateEdges()
        {
            int k = 0;
            double[] tempKsi = new double[2];
            double[] tempEta = new double[2];

            for (int i = 0; i < 8; i+=2)
            {
                Edge edge = new Edge(tempKsi, tempEta, (Side)k);
                tempKsi[0] = KsiValuesForEdges[i]; 
                tempKsi[1] = KsiValuesForEdges[i + 1];
                tempEta[0] = EtaValuesForEdges[i];
                tempEta[1] = EtaValuesForEdges[i + 1];
                FillInternalNMatrix(edge);
                Edges[k] = edge;
                k++;
            }
        }
    }
}
