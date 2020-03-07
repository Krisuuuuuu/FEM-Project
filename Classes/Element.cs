using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM_Project.Classes
{
    public class Element
    {
        public Node[] Id { get; set; } = new Node[4];

        public Element()
        {

        }

    }
}
