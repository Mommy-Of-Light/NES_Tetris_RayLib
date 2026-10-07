namespace Nes_Tetris
{
    public class Board
    {
        private readonly int[,] grid =
            new int[Constants.BoardHeight,
                    Constants.BoardWidth];

        public int[,] Grid => grid;

        public bool IsValidPosition(Tetromino piece)
        {
            foreach ((int X, int Y) cell in piece.GetCells())
            {
                int x = cell.X;
                int y = cell.Y;

                if (x < 0 || x >= Constants.BoardWidth)
                    return false;

                if (y >= Constants.BoardHeight)
                    return false;

                if (y >= 0 && grid[y, x] != 0)
                    return false;
            }

            return true;
        }

        public bool IsValidRotation(
            Tetromino piece,
            int rotation)
        {
            foreach ((int X, int Y) cell in piece.GetCellsForRotation(rotation))
            {
                int x = cell.X;
                int y = cell.Y;

                if (x < 0 || x >= Constants.BoardWidth)
                    return false;

                if (y >= Constants.BoardHeight)
                    return false;

                if (y >= 0 && grid[y, x] != 0)
                    return false;
            }

            return true;
        }

        public bool CanMove(
            Tetromino piece,
            int dx,
            int dy)
        {
            foreach ((int X, int Y) cell in piece.GetCells())
            {
                int x = cell.X + dx;
                int y = cell.Y + dy;

                if (x < 0 || x >= Constants.BoardWidth)
                    return false;

                if (y >= Constants.BoardHeight)
                    return false;

                if (y >= 0 && grid[y, x] != 0)
                    return false;
            }

            return true;
        }

        public void LockPiece(Tetromino piece)
        {
            int pieceId = (int)piece.Type + 1;

            foreach ((int X, int Y) cell in piece.GetCells())
            {
                if (cell.Y >= 0)
                {
                    grid[cell.Y, cell.X] = pieceId;
                }
            }
        }

        public int ClearLines()
        {
            int linesCleared = 0;

            for (int y = Constants.BoardHeight - 1; y >= 0; y--)
            {
                bool full = true;

                for (int x = 0; x < Constants.BoardWidth; x++)
                {
                    if (grid[y, x] == 0)
                    {
                        full = false;
                        break;
                    }
                }

                if (full)
                {
                    RemoveLine(y);

                    linesCleared++;

                    y++;
                }
            }

            return linesCleared;
        }

        private void RemoveLine(int line)
        {
            for (int y = line; y > 0; y--)
            {
                for (int x = 0; x < Constants.BoardWidth; x++)
                {
                    grid[y, x] =
                        grid[y - 1, x];
                }
            }

            for (int x = 0; x < Constants.BoardWidth; x++)
            {
                grid[0, x] = 0;
            }
        }

        public bool IsGameOver()
        {
            for (int x = 0; x < Constants.BoardWidth; x++)
            {
                if (grid[0, x] != 0)
                    return true;
            }

            return false;
        }

        public void Reset()
        {
            for (int y = 0; y < Constants.BoardHeight; y++)
            {
                for (int x = 0; x < Constants.BoardWidth; x++)
                {
                    grid[y, x] = 0;
                }
            }
        }
    }
}