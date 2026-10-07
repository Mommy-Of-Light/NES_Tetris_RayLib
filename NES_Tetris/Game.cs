using Raylib_cs;
using System.Numerics;

namespace Nes_Tetris
{
    public class Game
    {
        private readonly Board board = new();
        private readonly NesRandomizer randomizer = new();
        private readonly ScoreManager score = new();

        private Tetromino currentPiece;
        private Tetromino nextPiece;

        // Hold piece
        private PieceType? holdPiece = null;
        private bool holdUsed = false; // prevent multiple holds until next spawn

        private enum GameState { Start, Playing, Paused, GameOver }
        private GameState state = GameState.Start;

        // Timing
        private float fallTimer;
        private float lockTimer;
        private const float LockDelay = 0.5f;

        // DAS (simplified NES-style)
        private float dasTimer;
        private bool dasActive;
        private int dasDirection;

        public Game()
        {
            currentPiece = new Tetromino(randomizer.Next());
            nextPiece = new Tetromino(randomizer.Next());
        }

        public void Update()
        {
            HandleInput();

            if (state == GameState.Playing)
            {
                UpdateGravity();

                HandleLocking();
            }
        }

        private void HandleInput()
        {
            // Global inputs
            if (state == GameState.Start)
            {
                if (Raylib.IsKeyPressed(KeyboardKey.Enter))
                {
                    StartGame();
                }
                return;
            }

            if (state == GameState.GameOver)
            {
                if (Raylib.IsKeyPressed(KeyboardKey.Enter))
                {
                    StartGame();
                }
                return;
            }

            if (state == GameState.Paused)
            {
                if (Raylib.IsKeyPressed(KeyboardKey.P))
                {
                    state = GameState.Playing;
                }
                if (Raylib.IsKeyPressed(KeyboardKey.Enter))
                {
                    StartGame();
                }
                return;
            }

            // Toggle pause
            if (Raylib.IsKeyPressed(KeyboardKey.P))
            {
                state = GameState.Paused;
                return;
            }

            // HOLD (Left Shift)
            if (Raylib.IsKeyPressed(KeyboardKey.LeftShift))
            {
                HandleHold();
            }

            // LEFT
            if (Raylib.IsKeyPressed(KeyboardKey.Left))
            {
                Move(-1);
                dasDirection = -1;
                dasTimer = 0;
                dasActive = true;
            }

            // RIGHT
            if (Raylib.IsKeyPressed(KeyboardKey.Right))
            {
                Move(1);
                dasDirection = 1;
                dasTimer = 0;
                dasActive = true;
            }

            // DAS repeat movement
            if (Raylib.IsKeyDown(KeyboardKey.Left) ||
                Raylib.IsKeyDown(KeyboardKey.Right))
            {
                dasTimer += Raylib.GetFrameTime();

                if (dasTimer > 0.15f)
                {
                    Move(dasDirection);
                    dasTimer = 0.05f; // repeat rate
                }
            }
            else
            {
                dasActive = false;
            }

            // ROTATE (NES-style: simple rotation, no wall kicks)
            if (Raylib.IsKeyPressed(KeyboardKey.Up))
            {
                TryRotate();
            }

            // SOFT DROP
            if (Raylib.IsKeyDown(KeyboardKey.Down))
            {
                if (board.CanMove(currentPiece, 0, 1))
                {
                    currentPiece.Y++;
                    //score.Score += 1; // soft drop bonus (optional NES-ish variant)
                }
            }

            // HARD DROP (not NES original but common modern addition)
            if (Raylib.IsKeyPressed(KeyboardKey.Space))
            {
                while (board.CanMove(currentPiece, 0, 1))
                {
                    currentPiece.Y++;
                    //score.Score += 2;
                }

                LockPiece();
            }
        }

        private void HandleHold()
        {
            if (holdUsed)
                return;

            if (holdPiece == null)
            {
                holdPiece = currentPiece.Type;
                SpawnNextPiece();
            }
            else
            {
                // swap
                PieceType tmp = holdPiece.Value;
                holdPiece = currentPiece.Type;
                currentPiece = new Tetromino(tmp);
                currentPiece.X = 3;
                currentPiece.Y = 0;
            }

            holdUsed = true;
        }

        private void UpdateGravity()
        {
            fallTimer += Raylib.GetFrameTime();

            float gravity = GetGravityTime(score.GetCappedLevel());

            if (fallTimer >= gravity)
            {
                fallTimer = 0;

                if (board.CanMove(currentPiece, 0, 1))
                {
                    currentPiece.Y++;
                }
                else
                {
                    lockTimer += Raylib.GetFrameTime();
                }
            }
        }

        private void HandleLocking()
        {
            if (!board.CanMove(currentPiece, 0, 1))
            {
                lockTimer += Raylib.GetFrameTime();

                if (lockTimer >= LockDelay)
                {
                    LockPiece();
                }
            }
            else
            {
                lockTimer = 0;
            }
        }

        private void LockPiece()
        {
            board.LockPiece(currentPiece);

            int cleared = board.ClearLines();

            score.AddLines(cleared);

            // After locking we allow hold again on next piece
            SpawnNextPiece();

            if (board.IsGameOver())
            {
                state = GameState.GameOver;
            }
        }

        private void SpawnNextPiece()
        {
            currentPiece = nextPiece;
            currentPiece.X = 3;
            currentPiece.Y = 0;

            nextPiece = new Tetromino(randomizer.Next());

            // allow holding after spawn
            holdUsed = false;
        }

        private void ResetGame()
        {
            board.Reset();
            score.Reset();

            currentPiece = new Tetromino(randomizer.Next());
            nextPiece = new Tetromino(randomizer.Next());
        }

        private void StartGame()
        {
            board.Reset();
            score.Reset();
            randomizer.Reset();

            holdPiece = null;
            holdUsed = false;

            currentPiece = new Tetromino(randomizer.Next());
            nextPiece = new Tetromino(randomizer.Next());

            state = GameState.Playing;
        }

        private void Move(int dx)
        {
            if (board.CanMove(currentPiece, dx, 0))
            {
                currentPiece.X += dx;
            }
        }

        private void TryRotate()
        {
            int newRotation = (currentPiece.Rotation + 1) % 4;

            if (board.IsValidRotation(currentPiece, newRotation))
            {
                currentPiece.RotateClockwise();
            }
        }

        private float GetGravityTime(int level)
        {
            // NES gravity table (simplified but authentic feel)
            return level switch
            {
                0 => 0.80f,
                1 => 0.72f,
                2 => 0.63f,
                3 => 0.55f,
                4 => 0.47f,
                5 => 0.40f,
                6 => 0.33f,
                7 => 0.27f,
                8 => 0.22f,
                9 => 0.18f,
                10 => 0.15f,
                11 => 0.12f,
                12 => 0.10f,
                13 => 0.08f,
                14 => 0.07f,
                15 => 0.06f,
                16 => 0.05f,
                17 => 0.05f,
                18 => 0.05f,
                19 => 0.05f,
                20 => 0.04f,
                21 => 0.04f,
                22 => 0.04f,
                23 => 0.04f,
                24 => 0.03f,
                25 => 0.03f,
                26 => 0.03f,
                27 => 0.02f,
                28 => 0.02f,
                29 => 0.02f,
                _ => 0.02f
            };
        }

        public void Draw()
        {
            DrawBoard();

            // Project current piece to its landing position.
            DrawGhostPiece();

            // Draw actual falling piece on top.
            DrawCurrentPiece();

            DrawUI();

            DrawPreviews();

            DrawOverlay();
        }

        private void DrawBoard()
        {
            for (int y = 0; y < Constants.BoardHeight; y++)
            {
                for (int x = 0; x < Constants.BoardWidth; x++)
                {
                    int id = board.Grid[y, x];
                    if (id != 0)
                    {
                        PieceType pieceType = (PieceType)(id - 1);
                        Raylib.DrawRectangle(
                            Constants.BoardOffsetX + x * Constants.CellSize,
                            Constants.BoardOffsetY + y * Constants.CellSize,
                            Constants.CellSize,
                            Constants.CellSize,
                            GetColorForPiece(pieceType));
                    }
                }
            }

            // Vertical grid lines between columns
            for (int x = 0; x <= Constants.BoardWidth; x++)
            {
                int px = Constants.BoardOffsetX + x * Constants.CellSize;
                Raylib.DrawLine(px, Constants.BoardOffsetY, px, Constants.BoardOffsetY + Constants.BoardHeight * Constants.CellSize, Color.DarkGray);
            }

            // Border
            Raylib.DrawRectangleLines(Constants.BoardOffsetX - 1, Constants.BoardOffsetY - 1, Constants.BoardWidth * Constants.CellSize + 2, Constants.BoardHeight * Constants.CellSize + 2, Color.White);
        }

        private void DrawCurrentPiece()
        {
            foreach ((int X, int Y) cell in currentPiece.GetCells())
            {
                Raylib.DrawRectangle(
                    Constants.BoardOffsetX + cell.X * Constants.CellSize,
                    Constants.BoardOffsetY + cell.Y * Constants.CellSize,
                    Constants.CellSize,
                    Constants.CellSize,
                    GetColorForPiece(currentPiece.Type));
            }
        }

        private void DrawPreviews()
        {
            // Next piece
            DrawPiecePreview(nextPiece.Type, Constants.NextOffsetX, Constants.NextOffsetY, Constants.PreviewCellSize);

            Raylib.DrawText("Next", Constants.NextOffsetX, Constants.NextOffsetY - 20, 14, Color.White);

            // Hold piece
            Raylib.DrawText("Hold", Constants.HoldOffsetX, Constants.HoldOffsetY - 20, 14, Color.White);

            if (holdPiece != null)
            {
                DrawPiecePreview(holdPiece.Value, Constants.HoldOffsetX, Constants.HoldOffsetY, Constants.PreviewCellSize);
            }
        }

        private void DrawPiecePreview(PieceType type, int offsetX, int offsetY, int cellSize)
        {
            // Use rotation 0 for preview
            int[][] shape = TetrominoData.Shapes[type][0];
            Color color = GetColorForPiece(type);

            for (int row = 0; row < shape.Length; row++)
            {
                for (int col = 0; col < shape[row].Length; col++)
                {
                    if (shape[row][col] != 0)
                    {
                        Raylib.DrawRectangle(offsetX + col * cellSize, offsetY + row * cellSize, cellSize, cellSize, color);
                    }
                }
            }
        }

        private Color GetColorForPiece(PieceType type)
        {
            return type switch
            {
                PieceType.I => Color.FromHSV(180, 1.0f, 1.0f),
                PieceType.O => Color.FromHSV(60, 1.0f, 1.0f),
                PieceType.T => Color.FromHSV(270, 1.0f, 1.0f),
                PieceType.S => Color.FromHSV(120, 1.0f, 1.0f),
                PieceType.Z => Color.FromHSV(0, 1.0f, 1.0f),
                PieceType.J => Color.FromHSV(240, 1.0f, 1.0f),
                PieceType.L => Color.FromHSV(30, 1.0f, 1.0f),
                _ => Color.White,
            };
        }

        private void DrawGhostPiece()
        {
            int originalY = currentPiece.Y;
            int ghostY = originalY;

            // Find the lowest position the current piece can occupy.
            while (board.CanMove(currentPiece, 0, 1))
            {
                currentPiece.Y++;
                ghostY = currentPiece.Y;
            }

            // Draw the ghost at the lowest position.
            foreach ((int X, int Y) cell in currentPiece.GetCells())
            {
                Raylib.DrawRectangleLines(
                    Constants.BoardOffsetX + cell.X * Constants.CellSize + 1,
                    Constants.BoardOffsetY + cell.Y * Constants.CellSize + 1,
                    Constants.CellSize - 2,
                    Constants.CellSize - 2,
                    Color.Gray
                );
            }

            // VERY IMPORTANT:
            // Put the real piece back where it was.
            currentPiece.Y = originalY;
        }

        private void DrawOverlay()
        {
            if (state == GameState.Start)
            {
                DrawCenteredText("NES TETRIS", 40, Color.White);
                DrawCenteredText("Press Enter to Start", 20, Color.LightGray, 60);
            }

            if (state == GameState.Paused)
            {
                DrawCenteredText("PAUSED", 40, Color.Yellow);
                DrawCenteredText("Press P to Resume", 20, Color.LightGray, 60);
                DrawCenteredText("Press Enter to Restart", 18, Color.LightGray, 100);
            }

            if (state == GameState.GameOver)
            {
                DrawCenteredText("GAME OVER", 40, Color.Red);
                DrawCenteredText($"Score: {score.Score}", 20, Color.White, 60);
                DrawCenteredText("Press Enter to Restart", 18, Color.LightGray, 100);
            }
        }

        private void DrawCenteredText(string text, int size, Color color, int yOffset = 0)
        {
            int width = Raylib.MeasureText(text, size);
            Raylib.DrawText(text, (Constants.WindowWidth - width) / 2, (Constants.WindowHeight / 2) + yOffset, size, color);
        }

        private void DrawUI()
        {
            Raylib.DrawText(
                $"Score: {score.Score}",
                10, 10, 20, Color.White);

            Raylib.DrawText(
                $"Lines: {score.Lines}",
                10, 40, 20, Color.White);

            Raylib.DrawText(
                $"Level: {score.Level}",
                10, 70, 20, Color.White);
        }
    }
}