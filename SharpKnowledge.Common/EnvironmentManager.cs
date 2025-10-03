using System;

namespace SharpKnowledge.Common;

public static class EnvironmentManager
{
    public static string GetDBConnectionString()
    {
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = "Server=localhost;port=5432;Database=AIModels;Username=AIModelsLogin;Password=asdfasdf";
            System.Console.WriteLine($"No environment variable for DB_CONNECTION_STRING found, using default connection string: {connectionString}");
        }

        return connectionString;
    }

    public static string GetDataPath()
    {
        var dataPath = Environment.GetEnvironmentVariable("DATA_PATH");
        if (string.IsNullOrEmpty(dataPath))
        {
            dataPath = "Data";
            System.Console.WriteLine($"No environment variable for DATA_PATH found, using default data path: {dataPath}");
        }

        return dataPath;
    }

    public static int GetAggresiveCPUSnakeLearningTotalThreads()
    {
        var totalThreadsString = Environment.GetEnvironmentVariable("AGGRESIVE_CPU_SNAKE_LEARNING_TOTAL_THREADS");
        int totalThreads;
        if (string.IsNullOrEmpty(totalThreadsString) || !int.TryParse(totalThreadsString, out totalThreads))
        {
            totalThreads = 500;
            System.Console.WriteLine($"No environment variable for AGGRESIVE_CPU_SNAKE_LEARNING_TOTAL_THREADS found or invalid, using default value: {totalThreads}");
        }

        return totalThreads;
    }

    public static int GetAggresiveCPUSnakeBigLearningTotalThreads()
    {
        var totalThreadsString = Environment.GetEnvironmentVariable("AGGRESIVE_CPU_SNAKE_BIG_LEARNING_TOTAL_THREADS");
        int totalThreads;
        if (string.IsNullOrEmpty(totalThreadsString) || !int.TryParse(totalThreadsString, out totalThreads))
        {
            totalThreads = 500;
            System.Console.WriteLine($"No environment variable for AGGRESIVE_CPU_SNAKE_BIG_LEARNING_TOTAL_THREADS found or invalid, using default value: {totalThreads}");
        }

        return totalThreads;
    }
}
