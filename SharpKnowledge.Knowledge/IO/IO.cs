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
    public class IO
    {
        private readonly BrainModelsService _brainModelsService;

        public IO()
        {
            _brainModelsService = new BrainModelsService();
        }

        public void Save(BaseBrain baseBrain, long totalRuns, string description, string gameName)
        {
            var brainModel = new Data.Models.BrainModel
            {
                Name = gameName,
                Generation = baseBrain.Generation,
                BestScore = baseBrain.BestScore,
                Description = description,
                TotalRuns = totalRuns,
                Time = DateTime.UtcNow,
            };

            brainModel.SetWeightsArray(baseBrain.weights.Array);
            brainModel.SetBiasesArray(baseBrain.biases.Array);

            _brainModelsService.Create(brainModel);
        }

        public (BrainModel brainModel, CpuBrain cpuBrain) LoadCpuBrain(Guid brainId)
        {
            var brainModel = _brainModelsService.GetById(brainId);
            var cpuBrain = new CpuBrain(new Utility.ThreeDArray(brainModel.GetWeightsArray()), new Utility.TwoDArray(brainModel.GetBiasesArray()), brainModel.Generation);
            return (brainModel, cpuBrain);
        }

        public (BrainModel brainModel, CpuBrain cpuBrain) LoadLatestCpuBrain(string game)
        {
            var brainId = _brainModelsService.GetLatestGenerationId(game);
            if (brainId == Guid.Empty)
            {
                return (null, null);
            }
            return LoadCpuBrain(brainId);
        }

        public (BrainModel brainModel, GpuBrain gpuBrain) LoadGpuBrain(Guid brainId)
        {
            var brainModel = _brainModelsService.GetById(brainId);
            var gpuBrain = new GpuBrain(new Utility.ThreeDArray(brainModel.GetWeightsArray()), new Utility.TwoDArray(brainModel.GetBiasesArray()), brainModel.Generation);
            return (brainModel, gpuBrain);
        }

        public (BrainModel brainModel, GpuBrain gpuBrain) LoadLatestGpuBrain(string game)
        {
            var brainId = _brainModelsService.GetLatestGenerationId(game);
            if (brainId != Guid.Empty)
            {
                return (null, null);
            }
            return LoadGpuBrain(brainId);
        }


        public Guid GetLatestId(string gameName)
        {
            var bestBrainId = this._brainModelsService.GetLatestGenerationId(gameName);
            return bestBrainId;
        }

        public int[] GetAllSavedGenerations(string gameName)
        {
            this._brainModelsService.GetAllByName(gameName);
            var generations = this._brainModelsService.GetAllByName(gameName).Select(b => b.Generation).Distinct().OrderBy(g => g).ToArray();
            return generations;
        }

        public void DeleteAllBrains(string gameName)
        {
            var allBrains = this._brainModelsService.GetAllByName(gameName);
            foreach (var brain in allBrains)
            {
                this._brainModelsService.Delete(brain.Id);
            }
        }
    }
}
