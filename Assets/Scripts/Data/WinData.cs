using System.Collections.Generic;

namespace AspektML.Data
{
    public struct GameResult
    {
        public int GameNumber;
        public Teams Team;

        public GameResult(int gameNum, Teams team)
        {
            GameNumber = gameNum;
            Team = team;
        }
    }

    public class WinData
    {
        public List<GameResult> Results { get; } = new List<GameResult>();

        private int playerWins;
        private int aiWins;

        public void AddResult(int gameNum, Teams team)
        {
            Results.Add(new GameResult(gameNum, team));

            if (team == Teams.TeamA)
            {
                playerWins++;
            }
            else
            {
                aiWins++;
            }
        }

        public void ResetData()
        {
            Results.Clear();
        }

        public int GetPlayerWins() { return playerWins; }
        public int GetAIWins() { return aiWins; }
    }
}
