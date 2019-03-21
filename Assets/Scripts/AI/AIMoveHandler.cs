using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace AspektML.AI
{
    public class AIMoveHandler
    {
        private AIMemory memory = new AIMemory();
        private readonly List<Move> moves = new List<Move>();

        private readonly IndicatorPool indicatorPool;

        public AIMoveHandler(IndicatorPool indicatorPool)
        {
            this.indicatorPool = indicatorPool;
        }

        public bool MakeMove()
        {
            bool success = GetMoves();
            if (!success) return false;

            float totalWeight = memory.GetWeights(moves);
            ShowIndicators(totalWeight);

            float decision = Random.Range(0f, totalWeight);
            float runningWeight = 0f;
            for (int i = 0; i < moves.Count; i++)
            {
                if (moves[i].weight < 0.001f) continue;

                runningWeight += moves[i].weight;
                if (runningWeight >= decision)
                {
                    memory.MoveChosen(moves[i]);
                    GameManager.AI.StartCoroutine(MakeMoveRoutine(moves[i]));
                    return true;
                }
            }

            return false;
        }

        public void ActionWin()
        {
            memory.ActionWin();
        }

        public void ActionLoss()
        {
            memory.ActionLoss();
        }

        private bool GetMoves()
        {
            moves.Clear();
            foreach (var pawn in GameManager.Pawns.TeamB)
            {
                if (!pawn.State.IsAlive) continue;

                var tiles = MoveCalculator.GetValidTiles(pawn);
                foreach (var tile in tiles)
                {
                    moves.Add(new Move(pawn, tile));
                }
            }
            return moves.Count > 0;
        }

        private IEnumerator MakeMoveRoutine(Move move)
        {
            yield return new WaitForSeconds(0.5f);
            GameManager.Gameplay.CommitMove(move.pawn, move.tile);
            HideIndicators();
        }

        private void ShowIndicators(float totalWeight)
        {
            foreach (var move in moves)
            {
                var indicator = indicatorPool.GetFreeIndicator();
                indicator.enabled = true;
                indicator.SetPositions(new Vector3[2] {
                    move.pawn.transform.position + Vector3.up * 0.5f,
                    move.tile.transform.position + Vector3.up * 0.5f,
                });
                var gradient = indicator.colorGradient;
                var color = Color.Lerp(Color.blue, Color.red, move.weight / totalWeight);
                float alpha = Mathf.Lerp(0.5f, 1f, move.weight / totalWeight);
                gradient.SetKeys(
                    new GradientColorKey[2] { new GradientColorKey(color, 0f), new GradientColorKey(color, 1f) },
                    new GradientAlphaKey[2] { new GradientAlphaKey(alpha, 0f), new GradientAlphaKey(1f, 0.1f) }
                    );
                indicator.colorGradient = gradient;
            }
        }

        private void HideIndicators()
        {
            indicatorPool.ReturnIndicators();
        }
    }
}
