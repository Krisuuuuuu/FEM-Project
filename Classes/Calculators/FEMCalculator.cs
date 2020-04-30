using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM_Project.Classes
{
    public class FEMCalculator
    {
        private FileManager fileManager;
        private GridManager gridManager;
        private Grid grid;
        private GlobalData globalData;
        private MatrixCalculator matrixCalculator;
        private double[] xNodesCoordinates;
        private double[] yNodesCoordinates;

        public FEMCalculator()
        {
            fileManager = new FileManager();
            gridManager = new GridManager();
            matrixCalculator = new MatrixCalculator();
            xNodesCoordinates = new double[4];
            yNodesCoordinates = new double[4];
        }

        private void CreateGrid()
        {
            globalData = fileManager.GetDataFromTheFile();

            if(globalData != null)
            {
                grid = new Grid(globalData.NodesNumber, globalData.ElementsNumber, globalData.HeightNodesNumber, globalData.WidthNodesNumber);
                gridManager.CalculateDistance(globalData.Width, globalData.Height, globalData.HeightNodesNumber, globalData.WidthNodesNumber);
                gridManager.GenerateGrid(grid, globalData.HeightNodesNumber, globalData.WidthNodesNumber, globalData.Width, globalData.Height);
                gridManager.AssignNodesToElements(grid, globalData.HeightNodesNumber, globalData.WidthNodesNumber, globalData.NodesNumber);
            }
            else
            {
                Console.WriteLine("Creating grid failed.Try again later.");
            }
        }

        private void ClearAllComponents()
        {
            xNodesCoordinates = new double[4];
            yNodesCoordinates = new double[4];
        }

        private void GetCoordinatesOfElementNodes(int index)
        {
            ClearAllComponents();

            try
            {
                for(int i=0; i<4; i++)
                {
                   xNodesCoordinates[i] = grid.Elements[index].Id[i].X;
                   yNodesCoordinates[i] = grid.Elements[index].Id[i].Y;
                }
            }
            catch(NullReferenceException)
            {
                Console.WriteLine("Something went wrong. Grid does not have any elements. Try again later.");
            }
        }


        private void CalculateInternalElementMatrices(int index)
        {
            grid.Elements[index].HMatrix = matrixCalculator.CalculateHMatrix(xNodesCoordinates[0], xNodesCoordinates[1], xNodesCoordinates[2], xNodesCoordinates[3],
                yNodesCoordinates[0], yNodesCoordinates[1], yNodesCoordinates[2], yNodesCoordinates[3]);
            grid.Elements[index].CMatrix = matrixCalculator.CalculateCMatrix(xNodesCoordinates[0], xNodesCoordinates[1], xNodesCoordinates[2], xNodesCoordinates[3],
                yNodesCoordinates[0], yNodesCoordinates[1], yNodesCoordinates[2], yNodesCoordinates[3]);
        }

        public void Calculate()
        {
            CreateGrid();

            for(int i=0; i<grid.Elements.Length; i++)
            {
                GetCoordinatesOfElementNodes(i);
                CalculateInternalElementMatrices(i);
            }

            grid.GlobalHMatrix = MatricesAggregator.AggregateLocalMatrices(grid, true);
            grid.GlobalCMatrix = MatricesAggregator.AggregateLocalMatrices(grid, false);
        }

    }
}
