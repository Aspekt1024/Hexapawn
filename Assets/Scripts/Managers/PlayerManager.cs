using AspektML.Player;
using UnityEngine;

namespace AspektML
{
    public class PlayerManager : ManagerBase
    {
#pragma warning disable 649
        [SerializeField] private LineRenderer moveIndicator;
#pragma warning restore 649

        private enum States
        {
            Idle, SelectingPawn, SelectingTile
        }
        private States state;

        private MouseoverLogic mouseoverLogic;
        private SelectionDetails selectionDetails;

        public override void Init()
        {
            state = States.Idle;
            mouseoverLogic = new MouseoverLogic(moveIndicator);
        }

        public void StartTurn()
        {
            state = States.SelectingPawn;
            GameManager.UI.PlayerCursor.Show();
        }

        private void Update()
        {
            if (state == States.Idle) return;

            PositionCursor(Input.mousePosition);

            if (Input.GetMouseButtonUp(0))
            {
                HandleCommit(Input.mousePosition);
            }
        }

        private void PositionCursor(Vector2 inputPosition)
        {
            GameManager.UI.PlayerCursor.SetPosition(inputPosition);

            switch (state)
            {
                case States.Idle:
                    break;
                case States.SelectingPawn:
                    selectionDetails = mouseoverLogic.CheckPawnSelection(selectionDetails, inputPosition);
                    break;
                case States.SelectingTile:
                    selectionDetails = mouseoverLogic.CheckTileSelection(selectionDetails, inputPosition);
                    break;
                default:
                    break;
            }
        }

        private void HandleCommit(Vector2 inputPosition)
        {
            var s = selectionDetails;
            switch (state)
            {
                case States.Idle:
                    break;
                case States.SelectingPawn:
                    if (s.selectedPawn != null && s.selectedPawn.Team == Teams.TeamA)
                    {
                        state = States.SelectingTile;
                    }
                    break;
                case States.SelectingTile:
                    if (s.secondaryPawn != null)
                    {
                        s.selectedPawn = s.secondaryPawn;
                        s.secondaryPawn = null;
                        selectionDetails = s;
                        return;
                    }

                    if (s.selectedTile != null)
                    {
                        bool success = GameManager.Gameplay.CommitMove(s.selectedPawn, s.selectedTile);
                        if (success)
                        {
                            SetIdle();
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        
        private void SetIdle()
        {
            state = States.Idle;

            selectionDetails.selectedPawn?.Unhighlight();
            selectionDetails.selectedTile?.Unhighlight();
            selectionDetails.secondaryPawn?.Unhighlight();
            selectionDetails.enemyPawn?.Unhighlight();

            selectionDetails = new SelectionDetails();

            GameManager.UI.PlayerCursor.Hide();
            mouseoverLogic.HideIndicator();
        }
    }
}
