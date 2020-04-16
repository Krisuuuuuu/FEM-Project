using FEM_Project.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            //FEMCalculator fEMCalculator = new FEMCalculator();
            //fEMCalculator.CreateGrid();

            MatrixCalculator hMatrixCalculator = new MatrixCalculator();  

            Console.ReadKey();
        }
    }
}
