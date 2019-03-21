using System.Collections.Generic;
using UnityEngine;

namespace AspektML.AI
{
    public class AIMemory
    {
        private const float winWeight = 0.75f;
        private const float lossWeight = 0.75f;

        private struct BoardLayout
        {
            public string ID;
            public List<Move> Moves;
        }

        private struct MoveMemory
        {
            public string LayoutID;
            public Move Move;
        }

        private List<BoardLayout> layouts = new List<BoardLayout>();
        private List<MoveMemory> moveMemory = new List<MoveMemory>();
        
        public void MoveChosen(Move move)
        {
            string layoutID = GetLayoutID();
            moveMemory.Add(new MoveMemory()
            {
                LayoutID = layoutID,
                Move = move,
            });
        }

        public float GetWeights(List<Move> moves)
        {
            float totalWeight = 0f;

            string layoutID = GetLayoutID();
            int layoutIndex = layouts.FindIndex(x => x.ID == layoutID);
            if (layoutIndex >= 0)
            {
                var layout = layouts[layoutIndex];
                for (int i = 0; i < moves.Count; i++)
                {
                    int moveIndex = layout.Moves.FindIndex(x => x.pawn == moves[i].pawn && x.tile == moves[i].tile);
                    if (moveIndex >= 0)
                    {
                        var moveData = moves[i];
                        moveData.weight = layout.Moves[moveIndex].weight;
                        moves[i] = moveData;
                        totalWeight += layout.Moves[moveIndex].weight;
                    }
                }
            }
            else
            {
                layouts.Add(new BoardLayout()
                {
                    ID = layoutID,
                    Moves = new List<Move>(moves),
                });
                totalWeight = moves.Count;
            }
            return totalWeight;
        }

        public void ActionWin()
        {
            ApplyWeightsToMoves(1 + winWeight);
        }

        public void ActionLoss()
        {
            ApplyWeightsToMoves(1 - lossWeight);
        }

        private void ApplyWeightsToMoves(float scaleFactor)
        {
            foreach (var move in moveMemory)
            {
                int layoutIndex = layouts.FindIndex(x => x.ID == move.LayoutID);
                if (layoutIndex >= 0)
                {
                    var layout = layouts[layoutIndex];
                    int moveIndex = layout.Moves.FindIndex(m => m.pawn == move.Move.pawn && m.tile == move.Move.tile);
                    if (moveIndex >= 0)
                    {
                        var moveData = layout.Moves[moveIndex];
                        moveData.weight = move.Move.weight * scaleFactor;
                        layout.Moves[moveIndex] = moveData;
                    }
                    layouts[layoutIndex] = layout;
                }
            }
            moveMemory.Clear();
        }

        private static string GetLayoutID()
        {
            string id = "";
            for (int row = 0; row < BoardManager.BOARD_ROWS; row++)
            {
                for (int col = 0; col < BoardManager.BOARD_COLS; col++)
                {
                    var tile = GameManager.Board.GetTile(row, col);
                    if (tile.IsUnoccupied)
                    {
                        id += "u";
                    }
                    else if (tile.GetOccupyingPawn().Team == Teams.TeamA)
                    {
                        id += "a";
                    }
                    else
                    {
                        id += "b";
                    }
                }
                id += "|";
            }
            return id;
        }
    }
}
