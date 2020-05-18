using FEM_Project.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM_Project.Classes.Tools
{
    public class Printer : IPrinter
    {
        public void PrintMatrix(double[,] matrix, int sizeX, int sizeY, string title)
        {
            Console.WriteLine("\n***** " + title.ToUpper() + " *****");

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    Console.Write(matrix[i, j].ToString("F5") + " ");
                }

                Console.Write("\n");

            }

            Console.WriteLine("");
        }

        public void PrintVector(double[] vector, int size, string title)
        {
            Console.WriteLine("\n***** " + title.ToUpper() + " *****");

            for(int i = 0; i < size; i++)
            {
                if(vector[i] == 0)
                {
                    Console.Write(vector[i].ToString("F2") + " ");
                }
                else
                {
                    Console.Write(vector[i].ToString("F5") + " ");
                }

            }

            Console.WriteLine("");
        }

        public void PrintElements(Element[] elements)
        {
            Console.WriteLine("\n***** Elements *****");

            int k = 0;
            foreach(Element element in elements)
            {
                Console.WriteLine("\nElement " + k + ":");

                for (int i = 0; i < element.Id.Length; i++)
                {
                    Console.WriteLine("- Node ID: " + element.Id[i].NodeId + " X: " + element.Id[i].X.ToString("F5") + " Y: " 
                        + element.Id[i].Y.ToString("F5") + " Boundary Condition: " + element.Id[i].BoundanaryCondition.ToString());
                }

                k++;
                Console.WriteLine("");
            }

            Console.WriteLine("");
        }

        public void PrintAllLocalHMatrices(Element[] elements)
        {
            Console.WriteLine("\n***** Elements *****");

            int k = 0;
            foreach (Element element in elements)
            {
                Console.WriteLine("\nElement " + k + ":");

                PrintMatrix(element.HMatrix, 4, 4, "Element " + k + " Local H Matrix");
                k++;
            }
        }

        public void PrintAllLocalCMatrices(Element[] elements)
        {
            Console.WriteLine("\n***** Elements *****");

            int k = 0;
            foreach (Element element in elements)
            {
                Console.WriteLine("\nElement " + k + ":");

                PrintMatrix(element.CMatrix, 4, 4, "Element " + k + " Local C Matrix");
                k++;
            }
        }

        public void PrintAllLocalPVectors(Element[] elements)
        {
            Console.WriteLine("\n***** Elements *****");

            int k = 0;
            foreach (Element element in elements)
            {
                Console.WriteLine("\nElement " + k + ":");

                PrintVector(element.PVector, element.PVector.Length, "Element " + k + " Local P Vector");
                k++;
            }
        }

        public void PrintResult(IDictionary<int, double> minDictionary, IDictionary<int, double> maxDictionary)
        {
            Console.WriteLine("\n***** Result *****\n");
            Console.WriteLine("***** Min Temperatures *****");

            int counter = 1;
            foreach (KeyValuePair<int, double> element in minDictionary)
            {
                Console.WriteLine("Iteration: " + counter + " Step[s]: " + element.Key + " Min Temperature Value[°C]: " + element.Value.ToString("F5"));
                counter++;
            }

            Console.WriteLine();
            Console.WriteLine("***** Max Temperatures *****");

            counter = 1;
            foreach (KeyValuePair<int, double> element in maxDictionary)
            {
                Console.WriteLine("Iteration: " + counter + " Step[s]: " + element.Key + " Max Temperature Value[°C]: " + element.Value.ToString("F5"));
                counter++;
            }
            Console.WriteLine();
        }

        public void PrintSimulationTime(TimeSpan time)
        {
            Console.WriteLine("\nSimulation time[s]: " + time.TotalSeconds);
            Console.WriteLine();
        }
    }
}
