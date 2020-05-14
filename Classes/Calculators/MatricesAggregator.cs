using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM_Project.Classes
{
    public static class MatricesAggregator
    {
        public static double[,] AggregateLocalMatrices(Grid grid, bool hMatrix)
        {
            double[,] globalMatrix = CreateZerosMatrix(grid);
            int[] ids;

            for (int i = 0; i < grid.Elements.Length; i++)
            {
                ids = GetNodesIds(grid, i);

                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        if(hMatrix == true)
                        {
                            globalMatrix[ids[j], ids[k]] += grid.Elements[i].HMatrix[j, k];
                        }
                        else
                        {
                            globalMatrix[ids[j], ids[k]] += grid.Elements[i].CMatrix[j, k];
                        }
                    }
                }
            }

            return globalMatrix;
        }

        public static double[] AggregateLocalVectors(Grid grid)
        {
            double[] globalVector = CreateZerosVector(grid);
            int[] ids;

            for (int i = 0; i < grid.Elements.Length; i++)
            {
                ids = GetNodesIds(grid, i);

                for (int j = 0; j < 4; j++)
                {
                    globalVector[ids[j]] += grid.Elements[i].PVector[j];
                }
            }

            return globalVector;
        }

        private static int[] GetNodesIds(Grid grid, int index)
        {
            int[] ids = new int[4];

            try
            {
                for (int i = 0; i < 4; i++)
                {
                    ids[i] = grid.Elements[index].Id[i].NodeId;
                }
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Something went wrong. Grid does not have any elements. Try again later.");
            }

            return ids;
        }
        private static double[,] CreateZerosMatrix(Grid grid)
        {
            double[,] result = new double[grid.Nodes.Length, grid.Nodes.Length];

            for (int i = 0; i < grid.Nodes.Length; i++)
            {
                for (int j = 0; j < grid.Nodes.Length; j++)
                {
                    result[i, j] = 0;
                }
            }

            return result;
        }

        private static double[] CreateZerosVector(Grid grid)
        {
            double[] result = new double[grid.Nodes.Length];

            for(int i=0; i<grid.Nodes.Length; i++)
            {
                result[i] = 0;
            }

            return result;
        }

    }
}
