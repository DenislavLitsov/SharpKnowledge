using System;
using System.ComponentModel.DataAnnotations;

namespace SharpKnowledge.Data.Models;

public class Bias
{
    [Key]
    public int Id { get; set; }

    public float[] Biases { get; set; }

    public Guid BrainModelId { get; set; }
}
