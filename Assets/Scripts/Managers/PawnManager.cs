using System.Collections.Generic;
using UnityEngine;

namespace AspektML
{
    public sealed class PawnManager : ManagerBase
    {
#pragma warning disable 649
        [SerializeField] private Transform pawnParent;
        [SerializeField] private GameObject pawnPrefabA;
        [SerializeField] private GameObject pawnPrefabB;
#pragma warning restore 649

        public List<Pawn> TeamA { get; } = new List<Pawn>();
        public List<Pawn> TeamB { get; } = new List<Pawn>();

        public override void Init()
        {
            SpawnPawns();
        }

        public void ResetPawns()
        {
            for (int i = 0; i < BoardManager.BOARD_COLS; i++)
            {
                TeamA[i].Init(GameManager.Board.GetTile(0, i), Teams.TeamA);
                TeamB[i].Init(GameManager.Board.GetTile(BoardManager.BOARD_ROWS - 1, i), Teams.TeamB);
            }
        }

        private void SpawnPawns()
        {
            for (int i = 0; i < BoardManager.BOARD_COLS; i++)
            {
                Pawn newPawnA = Instantiate(pawnPrefabA, pawnParent).GetComponent<Pawn>();
                Pawn newPawnB = Instantiate(pawnPrefabB, pawnParent).GetComponent<Pawn>();

                newPawnA.Init(GameManager.Board.GetTile(0, i), Teams.TeamA);
                newPawnB.Init(GameManager.Board.GetTile(BoardManager.BOARD_ROWS - 1, i), Teams.TeamB);

                TeamA.Add(newPawnA);
                TeamB.Add(newPawnB);
            }
        }
    }
}
