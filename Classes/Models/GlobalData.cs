using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEM_Project.Classes
{
    public class GlobalData
    {
        public double InitialTemperature { get; set; }
        public double SimulationTime { get; set; }
        public double Step { get; set; }
        public double AmbientTemperature { get; set; }
        public double Alpha { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public int HeightNodesNumber { get; set; }
        public int WidthNodesNumber { get; set; }
        public double SpecificHeat { get; set; }
        public double Conductivity { get; set; }
        public double Density { get; set; }
        public int NodesNumber { get; set; }
        public int ElementsNumber { get; set; }

        public GlobalData()
        {
            InitialTemperature = 0;
            SimulationTime = 0;
            Step = 0;
            AmbientTemperature = 0;
            Height = 0;
            Width = 0;
            HeightNodesNumber = 0;
            WidthNodesNumber = 0;
            Conductivity = 0;
            Density = 0;
        }

        public GlobalData(double height, double width, int heightNumber, int widthNumber)
        {
            Height = height;
            Width = width;
            HeightNodesNumber = heightNumber;
            WidthNodesNumber = widthNumber;
        }

        public void RefreshNodesAndElementsNumber()
        {
            NodesNumber = HeightNodesNumber * WidthNodesNumber;
            ElementsNumber = (HeightNodesNumber - 1) * (WidthNodesNumber - 1);
        }
    }
}
