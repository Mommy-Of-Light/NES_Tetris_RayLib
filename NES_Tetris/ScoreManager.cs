namespace Nes_Tetris
{
    public class ScoreManager
    {
        public int Score { get; set; }

        public int Lines { get; private set; }

        public int Level { get; private set; }

        public void Reset()
        {
            Score = 0;
            Lines = 0;
            Level = 0;
        }

        /// <summary>
        /// Call this when lines are cleared.
        /// NES scoring depends on number of lines at once.
        /// </summary>
        public void AddLines(int cleared)
        {
            if (cleared <= 0)
                return;

            Lines += cleared;

            // NES level progression:
            // Level increases every 10 lines
            Level = Lines / 10;

            int basePoints = cleared switch
            {
                1 => 40,
                2 => 100,
                3 => 300,
                4 => 1200,
                _ => 0
            };

            Score += basePoints * (Level + 1);
        }

        /// <summary>
        /// Optional: NES caps level at 29
        /// </summary>
        public int GetCappedLevel()
        {
            return Level > 29 ? 29 : Level;
        }
    }
}