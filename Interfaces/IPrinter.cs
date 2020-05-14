using FEM_Project.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM_Project.Interfaces
{
    public interface IPrinter
    {
        void PrintMatrix(double[,] matrix, int sizeX, int sizeY, string title);
        void PrintVector(double[] vector, int size, string title);
        void PrintElements(Element[] elements);
        void PrintAllLocalHMatrices(Element[] elements);
        void PrintAllLocalCMatrices(Element[] elements);
        void PrintAllLocalPVectors(Element[] elements);
    }
}
