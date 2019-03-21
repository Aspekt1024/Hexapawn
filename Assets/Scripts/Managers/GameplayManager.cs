using System.Collections;
using UnityEngine;

namespace AspektML
{
    public class GameplayManager : ManagerBase
    {
        private int turnNumber;
        private Teams activeTeam;

        private enum States
        {
            None, StartingUp, InGame, GameOver
        }
        private States state;

        public override void Init()
        {
            state = States.None;
        }

        public void StartGame()
        {
            switch (state)
            {
                case States.None:
                    StartCoroutine(StartGameRoutine());
                    break;
                case States.StartingUp:
                    Debug.LogWarning("Start game called in starting up phase");
                    break;
                case States.InGame:
                    throw new System.NotImplementedException();
                case States.GameOver:
                    throw new System.NotImplementedException();
                default:
                    break;
            }
        }

        public bool CommitMove(Pawn pawn, Tile tile)
        {
            if (MoveValidator.IsValidMove(pawn, tile))
            {
                StartCoroutine(MoveRoutine(pawn, tile));
                return true;
            }
            return false;
        }

        public void GameoverAcknowledged()
        {
            state = States.None;
            GameManager.UI.GameOver.Hide();
            GameManager.Board.ClearTiles();
            GameManager.Pawns.ResetPawns();
            StartGame();
        }
        
        private IEnumerator StartGameRoutine()
        {
            state = States.StartingUp;
            // TODO game start animations etc
            yield return null;
            state = States.InGame;
            BeginPlay();
        }

        private IEnumerator MoveRoutine(Pawn pawn, Tile tile)
        {
            yield return StartCoroutine(pawn.MoveTo(tile));
            NextTurn();
        }

        private void BeginPlay()
        {
            turnNumber = 1;
            activeTeam = Teams.TeamA;
            GameManager.Player.StartTurn();
        }

        private void NextTurn()
        {
            activeTeam = activeTeam == Teams.TeamA ? Teams.TeamB : Teams.TeamA;
            var winConditionState = WinConditionLogic.CheckWinConditions(activeTeam);
            if (winConditionState == WinConditionState.TeamA)
            {
                GameOver(Teams.TeamA);
                return;
            }
            else if (winConditionState == WinConditionState.TeamB)
            {
                GameOver(Teams.TeamB);
                return;
            }

            turnNumber++;
            if (activeTeam == Teams.TeamB)
            {
                GameManager.AI.MakeMove();
            }
            else
            {
                GameManager.Player.StartTurn();
            }
        }

        private void GameOver(Teams team)
        {
            if (team == Teams.TeamA)
            {
                GameManager.AI.ActionLoss();
            }
            else
            {
                GameManager.AI.ActionWin();
            }

            state = States.GameOver;
            GameManager.Data.AddWin(team);
            GameManager.UI.GameOver.Show();
        }
    }
}
