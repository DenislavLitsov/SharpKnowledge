using System;
using System.ComponentModel.DataAnnotations;

namespace SharpKnowledge.Data.Models;

public class WeightCol
{

    [Key]
    public int Id { get; set; }

    public float[] WeightData { get; set; }

    public int WeightId { get; set; }
}
