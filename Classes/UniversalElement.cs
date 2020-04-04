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

        public double[] KsiValues { get; private set; }
        public double[] EtaValues { get; private set; }

        public double[,] NValuesMatrix { get; private set; }
        public double[,] DNDKsiValuesMatrix { get; set; }
        public double[,] DNDEtaValuesMatrix { get; set; }

        public UniversalElement()
        {
            KsiValues = new double[4] {-1/(Math.Sqrt(3)), 1 / (Math.Sqrt(3)), 1 / (Math.Sqrt(3)), -1 / (Math.Sqrt(3)) };
            EtaValues = new double[4] { -1 / (Math.Sqrt(3)), -1 / (Math.Sqrt(3)), 1 / (Math.Sqrt(3)), 1 / (Math.Sqrt(3)) };

            NValuesMatrix = new double[4, 4];
            DNDKsiValuesMatrix = new double[4, 4];
            DNDEtaValuesMatrix = new double[4, 4];

            FillNMatrix();
            FillDNDKsiMatrix();
            FillDNDEtaMatrix();
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
    }
}
