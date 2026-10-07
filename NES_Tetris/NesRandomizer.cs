using System;

namespace Nes_Tetris
{
    public class NesRandomizer
    {
        private readonly Random random = new();

        private PieceType lastPiece = (PieceType)(-1);

        /// <summary>
        /// Generates next piece using NES-style randomness.
        /// </summary>
        public PieceType Next()
        {
            PieceType piece;

            // NES-style rule:
            // If we roll the same piece twice in a row,
            // we reroll once (not infinite rerolls).
            int rollAttempts = 0;

            do
            {
                piece = (PieceType)random.Next(0, 7);
                rollAttempts++;

                // Only one reroll allowed (important for NES feel)
                if (rollAttempts > 1)
                    break;

            } while (piece == lastPiece);

            lastPiece = piece;

            return piece;
        }

        /// <summary>
        /// Optional reset (useful for restart).
        /// </summary>
        public void Reset()
        {
            lastPiece = (PieceType)(-1);
        }
    }
}