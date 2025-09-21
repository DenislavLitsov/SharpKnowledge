namespace SharpKnowledge.Knowledge.Factories
{
    public interface IBrainFactory
    {
        CpuBrain GetCpuBrain();

        GpuBrain GetGpuBrain();
    }
}
