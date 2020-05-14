using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM_Project.Classes
{
    public class Element
    {
        public Node[] Id { get; set; }
        public double[,] HMatrix { get; set; }
        public double[,] CMatrix { get; set; }
        public double[] PVector { get; set; }

        public bool[] EdgesBoundaryCondition;

        public Element()
        {
            Id = new Node[4];
            EdgesBoundaryCondition = new bool[4] { false, false, false, false };
        }

    }
}
