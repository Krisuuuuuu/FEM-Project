using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM_Project.Classes
{
    public class UniversalElementFunctions
    {
        public double N1Function(double ksi, double eta)
        {
            return 0.25 * (1 - ksi) * (1 - eta);
        }

        public double N2Function(double ksi, double eta)
        {
            return 0.25 * (1 + ksi) * (1 - eta);
        }

        public double N3Function(double ksi, double eta)
        {
            return 0.25 * (1 + ksi) * (1 + eta);
        }

        public double N4Function(double ksi, double eta)
        {
            return 0.25 * (1 - ksi) * (1 + eta);
        }

        public double DN1DKsiFunction(double eta)
        {
            return -0.25 * (1 - eta);
        }

        public double DN2DKsiFunction(double eta)
        {
            return 0.25 * (1 - eta);
        }

        public double DN3DKsiFunction(double eta)
        {
            return 0.25 * (1 + eta);
        }

        public double DN4DKsiFunction(double eta)
        {
            return -0.25 * (1 + eta);
        }

        public double DN1DEtaFunction(double ksi)
        {
            return -0.25 * (1 - ksi);
        }

        public double DN2DEtaFunction(double ksi)
        {
            return -0.25 * (1 + ksi);
        }

        public double DN3DEtaFunction(double ksi)
        {
            return 0.25 * (1 + ksi);
        }

        public double DN4DEtaFunction(double ksi)
        {
            return 0.25 * (1 - ksi);
        }
    }
}
