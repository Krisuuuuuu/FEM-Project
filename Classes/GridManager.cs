using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM_Project.Classes
{
    public class GridManager
    {
        public double DistanceBetweenX { get; set; }
        public double DistanceBetweenY { get; set; }

        private int CalculateNumberOfElements(int heightNodesNumber, int widthNodesNumber)
        {
            int elementsNumber = (heightNodesNumber - 1) * (widthNodesNumber - 1);

            return elementsNumber;
        }
        public void CalculateDistance(double width, double height, int heightNodesNumber, int widthNodesNumber)
        {
            DistanceBetweenX = width / (widthNodesNumber - 1);
            DistanceBetweenY = height / (heightNodesNumber - 1);
        }

        public void GenerateGrid(Grid grid, int heightNodesNumber, int widthNodesNumber)
        {
            int k = 0;
            for(int i = 0; i<heightNodesNumber; i++)
            {
                for(int j=0; j<widthNodesNumber; j++)
                {
                    Node node = new Node();
                    grid.Nodes[k] = node;
                    grid.Nodes[k].X = i * DistanceBetweenX;
                    grid.Nodes[k].Y = j * DistanceBetweenY;
                    Console.WriteLine("Node " + k + " (" + grid.Nodes[k].X + "," + grid.Nodes[k].Y + ")");
                    k++;
                }
            }

        }

        public void AssignNodesToElements(Grid grid, int heightNodesNumber, int widthNodesNumber, int nodesNumber)
        {
            int elementsNumber = CalculateNumberOfElements(heightNodesNumber, widthNodesNumber);
            int k = 0;
            int j = 0;
            int counter = 0;

            for (int i = 0; i <= nodesNumber; i++)
            {
                try
                {
                    Element element = new Element();
                    counter += 1;

                    if(counter == (heightNodesNumber))
                    {
                        counter = 0;
                        continue;
                    }

                    element.Id[k] = grid.Nodes[i];
                    Console.WriteLine("Element " + j + " otrzymal wezel " + i + " na miejscu " + k);
                    k++;
                    element.Id[k] = grid.Nodes[i + 4];
                    Console.WriteLine("Element " + j + " otrzymal wezel " + (i + 4) + " na miejscu " + k);
                    k++;
                    element.Id[k] = grid.Nodes[i + 5];
                    Console.WriteLine("Element " + j + " otrzymal wezel " + (i + 5) + " na miejscu " + k);
                    k++;
                    element.Id[k] = grid.Nodes[i + 1];
                    Console.WriteLine("Element " + j + " otrzymal wezel " + (i + 1) + " na miejscu " + k);
                    k = 0;
                    j++;

                    if(j == elementsNumber)
                    {
                        break;
                    }

                }
                catch (IndexOutOfRangeException)
                {
                    continue;
                }
             
            }
        }
    }
}
