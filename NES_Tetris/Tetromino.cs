using System.Collections.Generic;
using Raylib_cs;

namespace Nes_Tetris
{
    public class Tetromino
    {
        public PieceType Type { get; private set; }

        public int Rotation { get; private set; }

        public Raylib_cs.Color Color { get; private set; }

        public int X { get; set; }

        public int Y { get; set; }

        public Tetromino(PieceType type)
        {
            Type = type;
            Rotation = 0;

            // NES spawn position
            X = 3;
            Y = 0;

            SetColor();
        }

        public Tetromino(Tetromino other)
        {
            Type = other.Type;
            Rotation = other.Rotation;
            Color = other.Color;
            X = other.X;
            Y = other.Y;
        }

        private void SetColor()
        {
            Color = Type switch
            {
                PieceType.I => Color.FromHSV(180, 1.0f, 1.0f),
                PieceType.O => Color.FromHSV(60, 1.0f, 1.0f),
                PieceType.T => Color.FromHSV(270, 1.0f, 1.0f),
                PieceType.S => Color.FromHSV(120, 1.0f, 1.0f),
                PieceType.Z => Color.FromHSV(0, 1.0f, 1.0f),
                PieceType.J => Color.FromHSV(240, 1.0f, 1.0f),
                PieceType.L => Color.FromHSV(30, 1.0f, 1.0f),
                _ => Color.White
            };
        }

        public int[][] CurrentShape
        {
            get
            {
                return TetrominoData.Shapes[Type][Rotation];
            }
        }

        public void RotateClockwise()
        {
            Rotation++;

            if (Rotation >= 4)
                Rotation = 0;
        }

        public void RotateCounterClockwise()
        {
            Rotation--;

            if (Rotation < 0)
                Rotation = 3;
        }

        public IEnumerable<(int X, int Y)> GetCells()
        {
            int[][] shape = CurrentShape;

            for (int row = 0; row < shape.Length; row++)
            {
                for (int col = 0; col < shape[row].Length; col++)
                {
                    if (shape[row][col] != 0)
                    {
                        yield return (X + col, Y + row);
                    }
                }
            }
        }

        public IEnumerable<(int X, int Y)> GetCellsForRotation(int rotation)
        {
            int[][] shape =
                TetrominoData.Shapes[Type][rotation];

            for (int row = 0; row < shape.Length; row++)
            {
                for (int col = 0; col < shape[row].Length; col++)
                {
                    if (shape[row][col] != 0)
                    {
                        yield return (X + col, Y + row);
                    }
                }
            }
        }
    }
}