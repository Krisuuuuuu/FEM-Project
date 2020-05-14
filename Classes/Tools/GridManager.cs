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

        private bool SetBoundaryCondition(double width, double height, Node node)
        {
            if(node.X == 0 || node.X == width || node.Y == 0 || node.Y == height)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CalculateDistance(double width, double height, int heightNodesNumber, int widthNodesNumber)
        {
            DistanceBetweenX = width / (widthNodesNumber - 1);
            DistanceBetweenY = height / (heightNodesNumber - 1);
        }

        public void GenerateGrid(Grid grid, int heightNodesNumber, int widthNodesNumber, double width, double height)
        {
            int k = 0;
            for(int i = 0; i<heightNodesNumber; i++)
            {
                for(int j=0; j<widthNodesNumber; j++)
                {
                    Node node = new Node();
                    grid.Nodes[k] = node;
                    grid.Nodes[k].NodeId = k;
                    grid.Nodes[k].X = i * DistanceBetweenX;
                    grid.Nodes[k].Y = j * DistanceBetweenY;
                    grid.Nodes[k].BoundanaryCondition = SetBoundaryCondition(width, height, grid.Nodes[k]);
                    Console.WriteLine("Node " + grid.Nodes[k].NodeId + " (" + grid.Nodes[k].X + "," + grid.Nodes[k].Y + ")");
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

            for (int i = 0; i < nodesNumber; i++)
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

                    if (j == elementsNumber)
                    {
                        break;
                    }

                    element.Id[k] = grid.Nodes[i];
                    Console.WriteLine("Element " + j + " otrzymal wezel " + i + " na miejscu " + k);
                    k++;
                    element.Id[k] = grid.Nodes[i + heightNodesNumber];
                    Console.WriteLine("Element " + j + " otrzymal wezel " + (i + 4) + " na miejscu " + k);
                    k++;
                    element.Id[k] = grid.Nodes[i + (heightNodesNumber+1)];
                    Console.WriteLine("Element " + j + " otrzymal wezel " + (i + 5) + " na miejscu " + k);
                    k++;
                    element.Id[k] = grid.Nodes[i + 1];
                    Console.WriteLine("Element " + j + " otrzymal wezel " + (i + 1) + " na miejscu " + k);
                    SetBoundaryConditionEdgesFlags(element);
                    grid.Elements[j] = element;
                    k = 0;
                    j++;

                }
                catch (IndexOutOfRangeException)
                {
                    continue;
                }
             
            }
        }

        public void SetBoundaryConditionEdgesFlags(Element element)
        {
            if(element.Id[0].BoundanaryCondition && element.Id[1].BoundanaryCondition)
            {
                element.EdgesBoundaryCondition[0] = true;
            }

            if(element.Id[1].BoundanaryCondition && element.Id[2].BoundanaryCondition)
            {
                element.EdgesBoundaryCondition[1] = true;
            }

            if (element.Id[2].BoundanaryCondition && element.Id[3].BoundanaryCondition)
            {
                element.EdgesBoundaryCondition[2] = true;
            }

            if (element.Id[3].BoundanaryCondition && element.Id[0].BoundanaryCondition)
            {
                element.EdgesBoundaryCondition[3] = true;
            }
        }
    }
}
