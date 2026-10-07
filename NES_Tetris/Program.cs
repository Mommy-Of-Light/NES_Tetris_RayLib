using Raylib_cs;

namespace Nes_Tetris
{
    internal static class Program
    {
        private static void Main()
        {
            Raylib.InitWindow(
                Constants.WindowWidth,
                Constants.WindowHeight,
                "NES Tetris");

            Raylib.SetTargetFPS(60);

            Game game = new();

            while (!Raylib.WindowShouldClose())
            {
                game.Update();

                Raylib.BeginDrawing();

                Raylib.ClearBackground(Color.Black);

                game.Draw();

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}