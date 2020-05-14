using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM_Project.Interfaces
{
    public interface IPrinter
    {
        void PrintMatrix(int size);
        void PrintVector(int size);
    }
}
