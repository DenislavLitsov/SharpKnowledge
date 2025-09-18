using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpKnowledge.Games.Snake.Engine;

public class SnakeGame
{
    private readonly int _width;
    private readonly int _height;
    private readonly Random _random;
    private readonly List<Position> _snake;
    private Position _food;
    private Direction _currentDirection;
    private GameState _gameState;
    private int _score;

    public int Width => _width;
    public int Height => _height;
    public IReadOnlyList<Position> Snake => _snake.AsReadOnly();
    public Position Food => _food;
    public Direction CurrentDirection => _currentDirection;
    public GameState GameState => _gameState;
    public int Score => _score;

    public event EventHandler<int>? ScoreChanged;
    public event EventHandler<GameState>? GameStateChanged;
    public event EventHandler? FoodEaten;

    public SnakeGame(int width, int height)
    {
        if (width < 5 || height < 5)
            throw new ArgumentException("Map size must be at least 5x5");

        _width = width;
        _height = height;
        _random = new Random();
        _snake = new List<Position>();
        _currentDirection = Direction.Right;
        _gameState = GameState.Playing;
        _score = 0;

        InitializeGame();
    }

    private void InitializeGame()
    {
        // Initialize snake in the center
        _snake.Clear();
        int centerX = _width / 2;
        int centerY = _height / 2;
        
        _snake.Add(new Position(centerX, centerY));
        _snake.Add(new Position(centerX - 1, centerY));
        _snake.Add(new Position(centerX - 2, centerY));

        GenerateFood();
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

    public void Update()
    {
        if (_gameState != GameState.Playing)
            return;

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
            return;
        }

        // Add new head
        _snake.Insert(0, newHead);

        // Check if food was eaten
        if (newHead == _food)
        {
            _score += 10;
            ScoreChanged?.Invoke(this, _score);
            FoodEaten?.Invoke(this, EventArgs.Empty);
            GenerateFood();
        }
        else
        {
            // Remove tail if no food was eaten
            _snake.RemoveAt(_snake.Count - 1);
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

        _food = availablePositions[_random.Next(availablePositions.Count)];
    }

    public void Reset()
    {
        _score = 0;
        _gameState = GameState.Playing;
        _currentDirection = Direction.Right;
        
        InitializeGame();
        
        ScoreChanged?.Invoke(this, _score);
        GameStateChanged?.Invoke(this, _gameState);
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
}