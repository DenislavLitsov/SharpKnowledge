using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharpKnowledge.Knowledge.IO
{
    [JsonSerializable(typeof(SaveModel))]
    public class SaveModel
    {
        public SaveModel(Brain brain, string description, long totalRuns)
        {
            Brain = brain;
            Description = description;
            TotalRuns = totalRuns;
            Time = DateTime.UtcNow;
        }

        public Brain Brain { get; set; }

        public string Description { get; set; }

        public DateTime Time { get; set; }
        public long TotalRuns { get; set; }
    }
}
