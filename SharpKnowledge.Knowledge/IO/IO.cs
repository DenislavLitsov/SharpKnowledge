using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpKnowledge.Knowledge.IO
{
    public class IO
    {
        public IO()
        {
        }

        public void Save(Brain brain, string description, string dataPath)
        {
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            var saveModel = new SaveModel(brain, description);
            var options = new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true,
            };
            string jsonString = System.Text.Json.JsonSerializer.Serialize(saveModel, options);

            string fileTitle = $"gen_{brain.Generation}.json";
            string fullPath = Path.Combine(dataPath, fileTitle);

            System.IO.File.WriteAllText(fullPath, jsonString);
        }

        public SaveModel Load(int generation, string dataPath)
        {
            string fileTitle = $"gen_{generation}.json";
            string fullPath = Path.Combine(dataPath, fileTitle);

            if (!System.IO.File.Exists(fullPath))
            {
                throw new FileNotFoundException($"The file at path {fullPath} was not found.");
            }
            string jsonString = System.IO.File.ReadAllText(fullPath);
            var options = new System.Text.Json.JsonSerializerOptions
            {
                IncludeFields = true,
            };
            SaveModel? saveModel = System.Text.Json.JsonSerializer.Deserialize<SaveModel>(jsonString, options);
            if (saveModel == null)
            {
                throw new Exception("Failed to deserialize the SaveModel from the JSON file.");
            }
            return saveModel;
        }

        public int[] GetAllSavedGenerations(string dataPath)
        {
            if (!Directory.Exists(dataPath) || Directory.GetFiles(dataPath).Length == 0)
            {
                return [];
            }

            var allGens = Directory.GetFiles(dataPath)
                .Select(x => x.Replace("gen_", ""))
                .Select(x => x.Replace(".json", ""))
                .Select(x=>int.Parse(x))
                .ToArray();

            return allGens;
        }
    }
}
