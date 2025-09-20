using Newtonsoft.Json;
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

        public void Save(Brain brain, long totalRuns,string description, string dataPath, string gameName)
        {
            if (!Directory.Exists(Path.Combine(dataPath, gameName)))
            {
                Directory.CreateDirectory(Path.Combine(dataPath, gameName));
            }

            var saveModel = new SaveModel(brain, description, totalRuns);
            var options = new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true,
            };

            var jsonString = JsonConvert.SerializeObject(saveModel);
            //string jsonString = System.Text.Json.JsonSerializer.Serialize(saveModel, options);

            string fileTitle = $"gen_{brain.Generation}.json";
            string fullPath = Path.Combine(Path.Combine(dataPath, gameName), fileTitle);

            System.IO.File.WriteAllText(fullPath, jsonString);
        }

        public SaveModel Load(int generation, string dataPath, string gameName)
        {
            string fileTitle = $"gen_{generation}.json";
            string fullPath = Path.Combine(Path.Combine(dataPath, gameName), fileTitle);

            if (!System.IO.File.Exists(fullPath))
            {
                throw new FileNotFoundException($"The file at path {fullPath} was not found.");
            }
            string jsonString = System.IO.File.ReadAllText(fullPath);
            var options = new System.Text.Json.JsonSerializerOptions
            {
                IncludeFields = true,
            };
            //SaveModel? saveModel = System.Text.Json.JsonSerializer.Deserialize<SaveModel>(jsonString, options);
            SaveModel saveModel = JsonConvert.DeserializeObject<SaveModel>(jsonString);

            if (saveModel == null)
            {
                throw new Exception("Failed to deserialize the SaveModel from the JSON file.");
            }
            return saveModel;
        }

        public SaveModel LoadLatest(string dataPath, string gameName)
        {
            if (!Directory.Exists(Path.Combine(dataPath, gameName)) || Directory.GetFiles(Path.Combine(dataPath, gameName)).Length == 0)
            {
                return null;
            }
            var allGens = GetAllSavedGenerations(dataPath, gameName);
            int latestGen = allGens.Max();
            return Load(latestGen, dataPath, gameName);
        }

        public int[] GetAllSavedGenerations(string dataPath, string gameName)
        {
            if (!Directory.Exists(Path.Combine(dataPath, gameName)) || Directory.GetFiles(Path.Combine(dataPath, gameName)).Length == 0)
            {
                return [];
            }

            var allGens = Directory.GetFiles(Path.Combine(dataPath, gameName))
                .Select(x=>Path.GetFileName(x))
                .Select(x => x.Replace("gen_", ""))
                .Select(x => x.Replace(".json", ""))
                .Select(x=>int.Parse(x))
                .ToArray();

            return allGens;
        }
    }
}
