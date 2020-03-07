using FEM_Project.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace FEM_Project.Classes
{
    public class FileManager : IFileManager
    {
        private string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TextFiles\Mes.json";      
        public GlobalData GetDataFromTheFile()
        {
            try
            {
                using(StreamReader streamReader = new StreamReader(path))
                {
                    string json = streamReader.ReadToEnd();
                    var globalData = JsonConvert.DeserializeObject<GlobalData>(json);
                    globalData.RefreshNodesAndElementsNumber();
                    return globalData;
                }
            }
            catch(Exception)
            {
                Console.WriteLine("Getting data from the file failed. Try again later.");
            }

            return null;
        }
    }
}
