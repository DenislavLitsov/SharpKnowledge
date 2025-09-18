using SharpKnowledge.Common.RandomGenerators;
using SharpKnowledge.Playing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpKnowledge.Games.Snake.Engine;

public class SnakeGame : BaseGame
{
    private readonly int _width;
    private readonly int _height;
    private readonly IRandomGenerator _random;
    private readonly List<Position> _snake;
    private readonly float[,] _map;
    private Position _food;
    private Direction _currentDirection;
    private GameState _gameState;
    private long _score;
    private int _moves;
    private Position _lastFoodPosition = new Position(-1, -1);
    private int _totalWentThroughLastFood = 0;

    public int Width => _width;
    public int Height => _height;
    public IReadOnlyList<Position> Snake => _snake.AsReadOnly();
    public Position Food => _food;
    public Direction CurrentDirection => _currentDirection;
    public GameState GameState => _gameState;
    public long Score => _score;
    public int Moves => _moves;
    public float[,] Map => _map;

    public event EventHandler<long>? ScoreChanged;
    public event EventHandler<GameState>? GameStateChanged;
    public event EventHandler? FoodEaten;

    public SnakeGame(int width, int height, IRandomGenerator randomGenerator)
    {
        if (width < 5 || height < 5)
            throw new ArgumentException("Map size must be at least 5x5");

        _width = width;
        _height = height;
        _random = randomGenerator;
        _snake = new List<Position>();
        _map = new float[width, height];
        _currentDirection = Direction.Right;
        _gameState = GameState.Playing;
        _score = 0;
        _moves = 0;

        InitializeGame();
    }

    private void InitializeGame()
    {
    }

    public void SetDirection(Direction direction)
    {
        if (_gameState != GameState.Playing)
            return;

        // Prevent the snake from moving backwards into itself
        //if (IsOppositeDirection(direction, _currentDirection))
        //    return;

        _currentDirection = direction;
    }

    private static bool IsOppositeDirection(Direction direction1, Direction direction2)
    {
        return (direction1 == Direction.Up && direction2 == Direction.Down) ||
               (direction1 == Direction.Down && direction2 == Direction.Up) ||
               (direction1 == Direction.Left && direction2 == Direction.Right) ||
               (direction1 == Direction.Right && direction2 == Direction.Left);
    }

    public override bool Update(float takenDecision)
    {
        if (_gameState != GameState.Playing)
            return false;

        // Map takenDecision (0 to 1) to Direction
        if (takenDecision < 0.25f)
            SetDirection(Direction.Up);
        else if (takenDecision < 0.5f)
            SetDirection(Direction.Down);
        else if (takenDecision < 0.75f)
            SetDirection(Direction.Left);
        else
            SetDirection(Direction.Right);

        _moves++;

        // Calculate new head position
        Position head = _snake[0];
        Position newHead = _currentDirection switch
        {
            Direction.Up => new Position(head.X, head.Y - 1),
            Direction.Down => new Position(head.X, head.Y + 1),
            Direction.Left => new Position(head.X - 1, head.Y),
            Direction.Right => new Position(head.X + 1, head.Y),
            _ => head
        };

        // Check for collisions
        if (IsWallCollision(newHead) || IsSnakeCollision(newHead))
        {
            _gameState = GameState.GameOver;
            GameStateChanged?.Invoke(this, _gameState);
            return false;
        }

        // Add new head
        _snake.Insert(0, newHead);

        // Check if food was eaten
        if (newHead == _food)
        {
            _score += 1_000_000;
            ScoreChanged?.Invoke(this, _score);
            FoodEaten?.Invoke(this, EventArgs.Empty);
            this._lastFoodPosition = new Position(_food.X, _food.Y);
            _totalWentThroughLastFood = 0;
            GenerateFood();
        }
        else
        {
            // Remove tail if no food was eaten
            _snake.RemoveAt(_snake.Count - 1);
            if (this._lastFoodPosition == _snake[0])
            {
                this._totalWentThroughLastFood++;
                if (_totalWentThroughLastFood >= 3)
                {
                    return false;
                }
            }
        }

        UpdateMap();
        return true;
    }

    private void UpdateMap()
    {
        // Clear the map first - set all positions to free space (0)
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _map[y, x] = 0f; // Free space
            }
        }

        // Set border positions as walls (0.1) - these represent the game boundaries
        for (int x = 0; x < _width; x++)
        {
            _map[0, x] = 0.1f; // Top border
            _map[_height - 1, x] = 0.1f; // Bottom border
        }
        for (int y = 0; y < _height; y++)
        {
            _map[y, 0] = 0.1f; // Left border
            _map[y, _width - 1] = 0.1f; // Right border
        }

        // Set snake body positions to 0.5
        for (int i = 1; i < _snake.Count; i++)
        {
            Position pos = _snake[i];
            if (pos.X >= 0 && pos.X < _width && pos.Y >= 0 && pos.Y < _height)
            {
                _map[pos.Y, pos.X] = 0.5f; // Snake body
            }
        }

        // Set snake head to 1
        if (_snake.Count > 0)
        {
            Position head = _snake[0];
            if (head.X >= 0 && head.X < _width && head.Y >= 0 && head.Y < _height)
            {
                _map[head.Y, head.X] = 1f; // Snake head
            }
        }
    }

    private bool IsWallCollision(Position position)
    {
        return position.X < 0 || position.X >= _width ||
               position.Y < 0 || position.Y >= _height;
    }

    private bool IsSnakeCollision(Position position)
    {
        return _snake.Contains(position);
    }

    private void GenerateFood()
    {
        List<Position> availablePositions = new List<Position>();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Position pos = new Position(x, y);
                if (!_snake.Contains(pos))
                {
                    availablePositions.Add(pos);
                }
            }
        }

        if (availablePositions.Count == 0)
        {
            // Game won - no more space for food
            _gameState = GameState.GameOver;
            GameStateChanged?.Invoke(this, _gameState);
            return;
        }

        int index = (int)(_random.NextDouble() * availablePositions.Count);
        if (index == availablePositions.Count)
            index--;

        _food = availablePositions[index];
    }

    public void Reset()
    {
        _score = 0;
        _gameState = GameState.Playing;
        _currentDirection = Direction.Right;

        InitializeGame();

        ScoreChanged?.Invoke(this, _score);
        GameStateChanged?.Invoke(this, _gameState);
        UpdateMap();
    }

    public void Pause()
    {
        if (_gameState == GameState.Playing)
        {
            _gameState = GameState.Paused;
            GameStateChanged?.Invoke(this, _gameState);
        }
    }

    public void Resume()
    {
        if (_gameState == GameState.Paused)
        {
            _gameState = GameState.Playing;
            GameStateChanged?.Invoke(this, _gameState);
        }
    }

    public char GetCellContent(int x, int y)
    {
        Position pos = new Position(x, y);

        if (pos == _food)
            return 'F';

        if (_snake.Contains(pos))
        {
            if (_snake[0] == pos)
                return 'H'; // Head
            return 'S'; // Snake body
        }

        return ' '; // Empty space
    }

    public override void Initialize()
    {
        // Initialize snake in the center
        _snake.Clear();
        int centerX = _width / 2;
        int centerY = _height / 2;

        _snake.Add(new Position(centerX, centerY));
        _snake.Add(new Position(centerX - 1, centerY));
        _snake.Add(new Position(centerX - 2, centerY));

        GenerateFood();
        UpdateMap();
    }

    public override float GetScore()
    {
        return this.Moves + this.Score;
    }

    public override float[] GetBrainInputs()
    {
        float headLocationX = _snake[0].X;
        float headLocationY = _snake[0].Y;

        float foodLocationX = _food.X;
        float foodLocationY = _food.Y;

        float[] inputs = new float[4 + this.Width * this.Height];

        inputs[0] = headLocationX;
        inputs[1] = headLocationY;
        inputs[2] = foodLocationX;
        inputs[3] = foodLocationY;

        int index = 4;
        for (int y = 0; y < this.Height; y++)
        {
            for (int x = 0; x < this.Width; x++)
            {
                inputs[index] = this.Map[y, x];
                index++;
            }
        }

        return inputs;
    }
}