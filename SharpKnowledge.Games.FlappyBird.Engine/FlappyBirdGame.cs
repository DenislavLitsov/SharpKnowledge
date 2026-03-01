using SharpKnowledge.Common.RandomGenerators;
using SharpKnowledge.Playing;

namespace SharpKnowledge.Games.FlappyBird.Engine
{
    public class FlappyBirdGame : BaseGame
    {
        const float gravity = 0.5f;
        const float jumpStrength = -1.5f;

        const int xSize = 40;
        const int ySize = 16;

        const int pipeWidth = 1;
        const int pipeXGap = 15;

        const int pipeMinYGap = 3;
        const int pipeMaxYGap = 10;

        private readonly IRandomGenerator randomGenerator;

        private int birdX = 5;
        private float birdY = ySize / 2;
        private float birdYSpeed;

        private float currentTick = 0;
        private float nextPipeTick = 0;

        private int[,] map;

        public FlappyBirdGame(IRandomGenerator randomGenerator)
        {
            this.randomGenerator = randomGenerator;

            this.map = new int[ySize, xSize];
        }

        public override float[] GetBrainInputs()
        {
            float[] result = new float[5];

            float distanceFromBirdToNextPipe = 

            result[0] = this.birdY;
            result[1] = this.birdYSpeed;
            result[2] = this.CalculateDistanceToNextPipe(); 
            
            var gapInfo = this.CalculateNextPipeGap();
            result[3] = gapInfo.gapStart;
            result[4] = gapInfo.gapEnd;

            return result;
        }

        public override float GetScore()
        {
            return this.currentTick;
        }

        public override void Initialize()
        {
        }

        public override GameResult Update(float[] takenDecisions)
        {
            this.MovePipes();
            if (this.CheckCollision())
            {
                return GameResult.GameOver;
            }

            if (this.currentTick == nextPipeTick)
            {
                this.GeneratePipe();
                this.nextPipeTick += pipeXGap;
            }

            if (takenDecisions[0] > 0.5f)
            {
                this.birdYSpeed += jumpStrength;
            }
            else
            {
                this.birdYSpeed += gravity;
            }

            this.birdY += this.birdYSpeed;

            this.currentTick++;
            return GameResult.Continue;
        }

        public int[,] GetMap()
        {
            return this.map;
        }

        public int[] GetBirdLocation()
        {
            return new int[] { birdX, (int)birdY };
        }

        private void GeneratePipe()
        {
            int gap = (int)(pipeMinYGap + (this.randomGenerator.NextDouble() * (pipeMaxYGap - pipeMinYGap)));
            int topPipeEnd = (int)(this.randomGenerator.NextDouble() * (ySize - gap));

            for (int y = 0; y < ySize; y++)
            {
                if (y <= topPipeEnd || y > topPipeEnd + gap)
                {
                    this.map[y, xSize - 1] = 1;
                }
                else
                {
                    this.map[y, xSize - 1] = 0;
                }
            }
        }

        private void MovePipes()
        {
            for (int y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize - 1; x++)
                {
                    this.map[y, x] = this.map[y, x + 1];
                }
                this.map[y, xSize - 1] = 0;
            }
        }

        private bool CheckCollision()
        {
            int birdYInt = (int)this.birdY;
            if (birdYInt < 0 || birdYInt >= ySize)
            {
                return true;
            }
            return this.map[birdYInt, birdX] == 1;
        }

        private float CalculateDistanceToNextPipe()
        {
            for (int x = this.birdX; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    if (this.map[y, x] == 1)
                    {
                        return x - this.birdX;
                    }
                }
            }
            return xSize - this.birdX; // No pipe found, return max distance
        }

        private (float gapStart, float gapEnd) CalculateNextPipeGap()
        {
            int pipeX = -1;

            // Find the X position of the next pipe
            for (int x = this.birdX; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    if (this.map[y, x] == 1)
                    {
                        pipeX = x;
                        break;
                    }
                }
                if (pipeX != -1) break;
            }

            // No pipe found
            if (pipeX == -1)
            {
                return (0, ySize);
            }

            // Find the gap in that pipe column
            int gapStart = -1;
            int gapEnd = -1;

            for (int y = 0; y < ySize; y++)
            {
                if (this.map[y, pipeX] == 0)
                {
                    if (gapStart == -1)
                    {
                        gapStart = y;
                    }
                    gapEnd = y;
                }
            }

            return (gapStart, gapEnd);
        }
    }
}