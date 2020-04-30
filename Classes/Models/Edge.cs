using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM_Project.Classes
{
    public class Edge
    {
        public double[] KsiValues { get; private set; }
        public double[] EtaValues { get; private set; }      
        public Side Side { get; private set; }
        public double[,] NValuesMatrix { get; private set; }

        public Edge(double[] ksiValues, double[] etaValues, Side side)
        {
            KsiValues = ksiValues;
            EtaValues = etaValues;
            Side = side;
            NValuesMatrix = new double[2, 4];           
        }

    }
}
