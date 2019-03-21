using UnityEngine;

namespace AspektML
{
    public class BoardManager : ManagerBase
    {
        public const int BOARD_ROWS = 3;
        public const int BOARD_COLS = 3;

        private readonly Tile[,] tiles = new Tile[BOARD_ROWS, BOARD_COLS];
        
        public override void Init()
        {
            InitialiseTiles();
            ValidateTiles();
        }

        public Tile GetTile(int row, int col)
        {
            return tiles[row, col];
        }

        public void ClearTiles()
        {
            for (int row = 0; row < BOARD_ROWS; row++)
            {
                for (int col = 0; col < BOARD_COLS; col++)
                {
                    tiles[row, col].SetUnoccupied();
                }
            }
        }

        private void InitialiseTiles()
        {
            var allTiles = FindObjectsOfType<Tile>();
            foreach (var tile in allTiles)
            {
                if (tile.Row >= BOARD_ROWS || tile.Row < 0 || tile.Col >= BOARD_COLS || tile.Col < 0)
                {
                    Debug.LogWarning("Tile piece defined outside the range of the board. This tile will be ignored");
                    continue;
                }
                tiles[tile.Row, tile.Col] = tile;
            }
        }

        private void ValidateTiles()
        {
            for (int row = 0; row < BOARD_ROWS; row++)
            {
                for (int col = 0; col < BOARD_COLS; col++)
                {
                    if (tiles[row, col] == null)
                    {
                        Debug.LogError($"{nameof(BoardManager)}: missing tile at row:{row} col:{col}");
                    }
                }
            }
        }
    }
}
