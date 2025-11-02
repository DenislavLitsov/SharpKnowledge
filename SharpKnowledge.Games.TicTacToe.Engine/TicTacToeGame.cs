using SharpKnowledge.Playing;

namespace SharpKnowledge.Games.TicTacToe.Engine
{
    public class TicTacToeGame : BaseGame
    {
        private int totalMoves = 0;

        private float[][] board;

        public int Score { get; set; }

        public override float[] GetBrainInputs()
        {
            var inputs = new float[9];
            for (int col = 0; col < 3; col++)
            {
                for (int row = 0; row < 3; row++)
                {
                    inputs[col * 3 + row] = board[col][row];
                }
            }

            return inputs;
        }

        public override float GetScore()
        {
            throw new NotImplementedException();
        }

        public override void Initialize()
        {
            this.board = new float[3][]
            {
                new float[3] { 0, 0, 0 },
                new float[3] { 0, 0, 0 },
                new float[3] { 0, 0, 0 }
            };
        }

        public override GameResult Update(float[] takenDecisions)
        {
            this.totalMoves++;

            float maxValue = takenDecisions.Max();
            float maxIndex = Array.IndexOf(takenDecisions, maxValue);

            int row = (int)(maxIndex % 3);
            int col = (int)(maxIndex / 3);

            if (this.board[col][row] != 0)
            {
                // Invalid move
                return GameResult.GameOver;
            }

            this.board[col][row] = 1;

            return GameResult.Continue;
        }

        private bool IsGameOver()
        {
            // Check rows for a win
            for (int row = 0; row < 3; row++)
            {
                if (board[0][row] == 1 &&
                    board[0][row] == board[1][row] &&
                    board[1][row] == board[2][row])
                {
                    return true;
                }
            }

            // Check columns for a win
            for (int col = 0; col < 3; col++)
            {
                if (board[col][0] == 1 &&
                    board[col][0] == board[col][1] &&
                    board[col][1] == board[col][2])
                {
                    return true;
                }
            }

            // Check diagonal (top-left to bottom-right)
            if (board[0][0] == 1 &&
                board[0][0] == board[1][1] &&
                board[1][1] == board[2][2])
            {
                return true;
            }

            // Check diagonal (top-right to bottom-left)
            if (board[0][2] == 1 &&
                board[0][2] == board[1][1] &&
                board[1][1] == board[2][0])
            {
                return true;
            }

            return true;
        }

        private bool IsBoardFull()
        {
            for (int col = 0; col < 3; col++)
            {
                for (int row = 0; row < 3; row++)
                {
                    if (board[col][row] == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}