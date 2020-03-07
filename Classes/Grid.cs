using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM_Project.Classes
{
    public class Grid
    {
        public Node[] Nodes { get; set; } 
        public Element[] Elements { get; set; }

        public Grid(int nodesnumber, int elementsNumber)
        {
            Nodes = new Node[nodesnumber];
            Elements = new Element[elementsNumber];
        }

    }
}
