
namespace AspektML
{
    public enum WinConditionState
    {
        None, TeamA, TeamB
    }

    public static class WinConditionLogic
    {
        public static WinConditionState CheckWinConditions(Teams currentTeam)
        {
            Teams winningTeam = currentTeam == Teams.TeamA ? Teams.TeamB : Teams.TeamA;
            if (CheckTeamHasNoPawns(currentTeam))
            {
                GameManager.UI.GameOver.SetText(winningTeam, "All pawns captured");
                return currentTeam == Teams.TeamA ? WinConditionState.TeamB : WinConditionState.TeamA;
            }
            else if (CheckTeamReachedEnd(winningTeam))
            {
                GameManager.UI.GameOver.SetText(winningTeam, "Reached the other side");
                return currentTeam == Teams.TeamA ? WinConditionState.TeamB : WinConditionState.TeamA;
            }
            else if (CheckTeamHasNoMoves(currentTeam))
            {
                GameManager.UI.GameOver.SetText(winningTeam, "Opponent has no moves");
                return currentTeam == Teams.TeamA ? WinConditionState.TeamB : WinConditionState.TeamA;
            }

            return WinConditionState.None;
        }

        private static bool CheckTeamHasNoMoves(Teams team)
        {
            int totalMoves = 0;
            var pawns = team == Teams.TeamA ? GameManager.Pawns.TeamA : GameManager.Pawns.TeamB;
            foreach (var pawn in pawns)
            {
                if (!pawn.State.IsAlive) continue;
                var tiles = MoveCalculator.GetValidTiles(pawn);
                totalMoves += tiles.Count;
            }
            return totalMoves == 0;
        }

        private static bool CheckTeamHasNoPawns(Teams team)
        {
            var pawns = team == Teams.TeamA ? GameManager.Pawns.TeamA : GameManager.Pawns.TeamB;
            foreach (var pawn in pawns)
            {
                if (pawn.State.IsAlive) return false;
            }
            return true;
        }

        private static bool CheckTeamReachedEnd(Teams team)
        {
            var pawns = team == Teams.TeamA ? GameManager.Pawns.TeamA : GameManager.Pawns.TeamB;
            int targetRow = team == Teams.TeamA ? BoardManager.BOARD_ROWS - 1 : 0;
            foreach (var pawn in pawns)
            {
                if (pawn.State.CurrentTile.Row == targetRow)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
