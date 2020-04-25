using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM_Project.Classes
{
    public class Node
    {
        public int NodeId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Temp { get; set; }
        public bool BoundanaryCondition { get; set; }
        public Node()
        {
            X = 0;
            Y = 0;
            Temp = 0;
            BoundanaryCondition = false;
        }

        public Node(int id, int x, int y, double temp, bool boundaryCondition)
        {
            NodeId = id;
            X = x;
            Y = y;
            temp = Temp;
            boundaryCondition = BoundanaryCondition;
        }
    }
}
