using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspektML
{
    public static class MoveValidator
    {
        public static bool IsValidMove(Pawn pawn, Tile tile)
        {
            Tile pawnTile = pawn.State.CurrentTile;

            int rowDiff = pawn.Team == Teams.TeamA ? 1 : -1;

            // Must be a forward movement of 1
            if (tile.Row != pawnTile.Row + rowDiff) return false;

            // Direct forward move possible only if the tile is unoccupied
            if (tile.Col == pawnTile.Col)
            {
                return tile.IsUnoccupied;
            }

            // Diagonal move possible if it results in enemy capture
            if (Math.Abs(tile.Col - pawnTile.Col) == 1)
            {
                return tile.IsOccupied && tile.GetOccupyingPawn().Team != pawn.Team;
            }

            // All other moves are invalid
            return false;
        }
    }
}
