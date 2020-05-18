using FEM_Project.Classes.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FEM_Project.Classes
{
    public class FEMCalculator
    {
        private FileManager fileManager;
        private GridManager gridManager;
        private Printer printer;
        private Grid grid;
        private GlobalData globalData;
        private MatrixCalculator matrixCalculator;
        private EquationEvaluator equationEvaluator;
        private double[] xNodesCoordinates;
        private double[] yNodesCoordinates;
        private Stopwatch stopwatch;

        private IDictionary<int, double> minTemperatureIterationResult;
        private IDictionary<int, double> maxTemperatureIterationResult;

        private TimeSpan simulationTime;

        public FEMCalculator()
        {
            fileManager = new FileManager();
            gridManager = new GridManager();
            printer = new Printer();
            xNodesCoordinates = new double[4];
            yNodesCoordinates = new double[4];
            minTemperatureIterationResult = new Dictionary<int, double>();
            maxTemperatureIterationResult = new Dictionary<int, double>();
            stopwatch = new Stopwatch();
        }

        private void CreateGrid()
        {
            globalData = fileManager.GetDataFromTheFile();

            if(globalData != null)
            {
                grid = new Grid(globalData.NodesNumber, globalData.ElementsNumber, globalData.HeightNodesNumber, globalData.WidthNodesNumber);
                matrixCalculator = new MatrixCalculator(globalData.Conductivity, globalData.Alpha, globalData.SpecificHeat,
                    globalData.Density, globalData.AmbientTemperature);
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
                yNodesCoordinates[0], yNodesCoordinates[1], yNodesCoordinates[2], yNodesCoordinates[3], grid.Elements[index].EdgesBoundaryCondition);
            grid.Elements[index].PVector = matrixCalculator.CalculatePVector(grid.Elements[index].EdgesBoundaryCondition);
            grid.Elements[index].CMatrix = matrixCalculator.CalculateCMatrix(xNodesCoordinates[0], xNodesCoordinates[1], xNodesCoordinates[2], xNodesCoordinates[3],
                yNodesCoordinates[0], yNodesCoordinates[1], yNodesCoordinates[2], yNodesCoordinates[3]);
        }

        private void AggregateLocalMatricesAndVectors()
        {
            grid.GlobalHMatrix = MatricesAggregator.AggregateLocalMatrices(grid, true);
            grid.GlobalCMatrix = MatricesAggregator.AggregateLocalMatrices(grid, false);
            grid.GlobalPVector = MatricesAggregator.AggregateLocalVectors(grid);
        }

        private void PrintResults()
        {
            printer.PrintElements(grid.Elements);
            printer.PrintAllLocalHMatrices(grid.Elements);
            printer.PrintAllLocalCMatrices(grid.Elements);
            printer.PrintAllLocalPVectors(grid.Elements);
            printer.PrintMatrix(grid.GlobalHMatrix, 16, 16, "H Matrix");
            printer.PrintMatrix(grid.GlobalCMatrix, 16, 16, "C Matrix");
            printer.PrintVector(grid.GlobalPVector, grid.GlobalPVector.Length, "P Vector");
            //printer.PrintResult(minTemperatureIterationResult, maxTemperatureIterationResult);
            printer.PrintSimulationTime(simulationTime);
        }

        public void Calculate()
        {
            stopwatch.Start();

            CreateGrid();

            for(int i=0; i<grid.Elements.Length; i++)
            {
                GetCoordinatesOfElementNodes(i);
                CalculateInternalElementMatrices(i);
            }

            AggregateLocalMatricesAndVectors();
            equationEvaluator = new EquationEvaluator(globalData.SimulationTime, globalData.Step, globalData.InitialTemperature,
                grid.GlobalHMatrix, grid.GlobalCMatrix, grid.GlobalPVector);
            equationEvaluator.EvaluateEquation(grid.GlobalPVector.Length, ref minTemperatureIterationResult, ref maxTemperatureIterationResult);

            stopwatch.Stop();
            simulationTime = stopwatch.Elapsed;
            
            PrintResults();

        }

    }
}
