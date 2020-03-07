using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM_Project.Classes
{
    public class FEMCalculator
    {
        private FileManager fileManager = new FileManager();
        private GridManager gridManager = new GridManager();
        private Grid grid;
        private GlobalData globalData;


        public void CreateGrid()
        {
            globalData = fileManager.GetDataFromTheFile();

            if(globalData != null)
            {
                grid = new Grid(globalData.NodesNumber, globalData.ElementsNumber);
                gridManager.CalculateDistance(globalData.Width, globalData.Height, globalData.HeightNodesNumber, globalData.WidthNodesNumber);
                gridManager.GenerateGrid(grid, globalData.HeightNodesNumber, globalData.WidthNodesNumber);
                gridManager.AssignNodesToElements(grid, globalData.HeightNodesNumber, globalData.WidthNodesNumber, globalData.NodesNumber);

            }
            else
            {
                Console.WriteLine("Creating grid failed.Try again later.");
            }
        }

    }
}
