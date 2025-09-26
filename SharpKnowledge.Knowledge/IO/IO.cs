using Newtonsoft.Json;
using SharpKnowledge.Data.Models;
using SharpKnowledge.Data.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SharpKnowledge.Knowledge.IO
{
    public class IO<BrainType> where BrainType : BaseBrain
    {
        private readonly BrainModelsService _brainModelsService;

        public IO()
        {
            _brainModelsService = new BrainModelsService();
        }

        public void Save(BaseBrain baseBrain, long totalRuns, string description, string gameName)
        {

            float[][,] weightsArray = baseBrain.weights.Array;
            int layers = weightsArray.Length;
            int rows = weightsArray[0].GetLength(0);
            int cols = weightsArray[0].GetLength(1);

            float[,,] weights3D = new float[layers, rows, cols];
            for (int l = 0; l < layers; l++)
            {
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        weights3D[l, r, c] = weightsArray[l][r, c];
                    }
                }
            }

            int rows2 = baseBrain.biases.Array.Length;
            int cols2 = baseBrain.biases.Array[0].Length;
            float[,] parsedBiases = new float[rows2, cols2];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols2; j++)
                {
                    parsedBiases[i, j] = baseBrain.biases.Array[i][j];
                }
            }

            var brainModel = new Data.Models.BrainModel
            {
                Name = gameName,
                Generation = baseBrain.Generation,
                BestScore = baseBrain.BestScore,
                Description = description,
                TotalRuns = totalRuns,
                Time = DateTime.UtcNow,
                Weights = weights3D,
                Biases = parsedBiases
            };

            _brainModelsService.CreateAsync(brainModel).Wait();
        }

        public Tuple<BrainModel, CpuBrain> LoadCpuBrain(Guid brainId)
        {
            var brainModel = _brainModelsService.GetByIdAsync(brainId).Result;
            var cpuBrain = new CpuBrain(brainModel.Weights, brainModel.Biases);
            return Tuple.Create(brainModel, cpuBrain);
        }

        public Tuple<BrainModel, GpuBrain> LoadGpuBrain(Guid brainId)
        {
            var brainModel = _brainModelsService.GetByIdAsync(brainId).Result;
            var gpuBrain = new GpuBrain(brainModel.Weights, brainModel.Biases);
            return Tuple.Create(brainModel, gpuBrain);
        }

        public void Save(BrainType brain, long totalRuns, string description, string dataPath, string gameName)
        {
            if (!Directory.Exists(Path.Combine(dataPath, gameName)))
            {
                Directory.CreateDirectory(Path.Combine(dataPath, gameName));
            }

            var saveModel = new SaveModel<BrainType>(brain, description, totalRuns);
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

        public SaveModel<BrainType> Load(int generation, string dataPath, string gameName)
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
            SaveModel<BrainType> saveModel = JsonConvert.DeserializeObject<SaveModel<BrainType>>(jsonString);

            if (saveModel == null)
            {
                throw new Exception("Failed to deserialize the SaveModel from the JSON file.");
            }
            return saveModel;
        }

        public SaveModel<BrainType> LoadLatest(string dataPath, string gameName)
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
                .Select(x => Path.GetFileName(x))
                .Select(x => x.Replace("gen_", ""))
                .Select(x => x.Replace(".json", ""))
                .Select(x => int.Parse(x))
                .ToArray();

            return allGens;
        }
    }
}
