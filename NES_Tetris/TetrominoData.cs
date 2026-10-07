using System.Collections.Generic;

namespace Nes_Tetris
{
    public static class TetrominoData
    {
        public static readonly Dictionary<PieceType, int[][][]> Shapes =
            new()
            {
                // I PIECE
                {
                    PieceType.I,
                    new[]
                    {
                        // Rotation 0
                        new[]
                        {
                            new[] {0,0,0,0},
                            new[] {1,1,1,1},
                            new[] {0,0,0,0},
                            new[] {0,0,0,0}
                        },

                        // Rotation 1
                        new[]
                        {
                            new[] {0,0,1,0},
                            new[] {0,0,1,0},
                            new[] {0,0,1,0},
                            new[] {0,0,1,0}
                        },

                        // Rotation 2
                        new[]
                        {
                            new[] {0,0,0,0},
                            new[] {0,0,0,0},
                            new[] {1,1,1,1},
                            new[] {0,0,0,0}
                        },

                        // Rotation 3
                        new[]
                        {
                            new[] {0,1,0,0},
                            new[] {0,1,0,0},
                            new[] {0,1,0,0},
                            new[] {0,1,0,0}
                        }
                    }
                },

                // O PIECE
                {
                    PieceType.O,
                    new[]
                    {
                        new[]
                        {
                            new[] {0,1,1,0},
                            new[] {0,1,1,0},
                            new[] {0,0,0,0},
                            new[] {0,0,0,0}
                        },

                        new[]
                        {
                            new[] {0,1,1,0},
                            new[] {0,1,1,0},
                            new[] {0,0,0,0},
                            new[] {0,0,0,0}
                        },

                        new[]
                        {
                            new[] {0,1,1,0},
                            new[] {0,1,1,0},
                            new[] {0,0,0,0},
                            new[] {0,0,0,0}
                        },

                        new[]
                        {
                            new[] {0,1,1,0},
                            new[] {0,1,1,0},
                            new[] {0,0,0,0},
                            new[] {0,0,0,0}
                        }
                    }
                },

                // T PIECE
                {
                    PieceType.T,
                    new[]
                    {
                        new[]
                        {
                            new[] {0,1,0,0},
                            new[] {1,1,1,0},
                            new[] {0,0,0,0},
                            new[] {0,0,0,0}
                        },

                        new[]
                        {
                            new[] {0,1,0,0},
                            new[] {0,1,1,0},
                            new[] {0,1,0,0},
                            new[] {0,0,0,0}
                        },

                        new[]
                        {
                            new[] {0,0,0,0},
                            new[] {1,1,1,0},
                            new[] {0,1,0,0},
                            new[] {0,0,0,0}
                        },

                        new[]
                        {
                            new[] {0,1,0,0},
                            new[] {1,1,0,0},
                            new[] {0,1,0,0},
                            new[] {0,0,0,0}
                        }
                    }
                },

                // S PIECE
                {
                    PieceType.S,
                    new[]
                    {
                        new[]
                        {
                            new[] {0,1,1,0},
                            new[] {1,1,0,0},
                            new[] {0,0,0,0},
                            new[] {0,0,0,0}
                        },

                        new[]
                        {
                            new[] {0,1,0,0},
                            new[] {0,1,1,0},
                            new[] {0,0,1,0},
                            new[] {0,0,0,0}
                        },

                        new[]
                        {
                            new[] {0,0,0,0},
                            new[] {0,1,1,0},
                            new[] {1,1,0,0},
                            new[] {0,0,0,0}
                        },

                        new[]
                        {
                            new[] {1,0,0,0},
                            new[] {1,1,0,0},
                            new[] {0,1,0,0},
                            new[] {0,0,0,0}
                        }
                    }
                },

                // Z PIECE
                {
                    PieceType.Z,
                    new[]
                    {
                        new[]
                        {
                            new[] {1,1,0,0},
                            new[] {0,1,1,0},
                            new[] {0,0,0,0},
                            new[] {0,0,0,0}
                        },

                        new[]
                        {
                            new[] {0,0,1,0},
                            new[] {0,1,1,0},
                            new[] {0,1,0,0},
                            new[] {0,0,0,0}
                        },

                        new[]
                        {
                            new[] {0,0,0,0},
                            new[] {1,1,0,0},
                            new[] {0,1,1,0},
                            new[] {0,0,0,0}
                        },

                        new[]
                        {
                            new[] {0,1,0,0},
                            new[] {1,1,0,0},
                            new[] {1,0,0,0},
                            new[] {0,0,0,0}
                        }
                    }
                },

                // J PIECE
                {
                    PieceType.J,
                    new[]
                    {
                        new[]
                        {
                            new[] {1,0,0,0},
                            new[] {1,1,1,0},
                            new[] {0,0,0,0},
                            new[] {0,0,0,0}
                        },

                        new[]
                        {
                            new[] {0,1,1,0},
                            new[] {0,1,0,0},
                            new[] {0,1,0,0},
                            new[] {0,0,0,0}
                        },

                        new[]
                        {
                            new[] {0,0,0,0},
                            new[] {1,1,1,0},
                            new[] {0,0,1,0},
                            new[] {0,0,0,0}
                        },

                        new[]
                        {
                            new[] {0,1,0,0},
                            new[] {0,1,0,0},
                            new[] {1,1,0,0},
                            new[] {0,0,0,0}
                        }
                    }
                },

                // L PIECE
                {
                    PieceType.L,
                    new[]
                    {
                        new[]
                        {
                            new[] {0,0,1,0},
                            new[] {1,1,1,0},
                            new[] {0,0,0,0},
                            new[] {0,0,0,0}
                        },

                        new[]
                        {
                            new[] {0,1,0,0},
                            new[] {0,1,0,0},
                            new[] {0,1,1,0},
                            new[] {0,0,0,0}
                        },

                        new[]
                        {
                            new[] {0,0,0,0},
                            new[] {1,1,1,0},
                            new[] {1,0,0,0},
                            new[] {0,0,0,0}
                        },

                        new[]
                        {
                            new[] {1,1,0,0},
                            new[] {0,1,0,0},
                            new[] {0,1,0,0},
                            new[] {0,0,0,0}
                        }
                    }
                }
            };
    }
}