using System.Collections.Generic;

namespace AspektML
{
    public static class MoveCalculator
    {
        public static List<Tile> GetValidTiles(Pawn pawn)
        {
            var validTiles = new List<Tile>();
            Tile pawnTile = pawn.State.CurrentTile;

            int rowDiff = pawn.Team == Teams.TeamA ? 1 : -1;
            int toRow = pawnTile.Row + rowDiff;

            if (toRow < 0 || toRow > BoardManager.BOARD_ROWS - 1)
            {
                return validTiles;
            }

            // Check forward move
            var tile = GameManager.Board.GetTile(toRow, pawnTile.Col);
            if (MoveValidator.IsValidMove(pawn, tile))
            {
                validTiles.Add(tile);
            }

            // Check diagonal moves
            if (pawnTile.Col > 0)
            {
                tile = GameManager.Board.GetTile(toRow, pawnTile.Col - 1);
                if (MoveValidator.IsValidMove(pawn, tile))
                {
                    validTiles.Add(tile);
                }
            }

            if (pawnTile.Col < BoardManager.BOARD_COLS - 1)
            {
                tile = GameManager.Board.GetTile(toRow, pawnTile.Col + 1);
                if (MoveValidator.IsValidMove(pawn, tile))
                {
                    validTiles.Add(tile);
                }
            }
            return validTiles;
        }
    }
}
