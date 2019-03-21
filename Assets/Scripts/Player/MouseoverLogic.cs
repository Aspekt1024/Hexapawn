using UnityEngine;

namespace AspektML.Player
{
    public class MouseoverLogic
    {
        private readonly LineRenderer moveIndicator;

        public MouseoverLogic(LineRenderer moveIndicator)
        {
            this.moveIndicator = moveIndicator;
            moveIndicator.enabled = false;
        }

        public void HideIndicator()
        {
            moveIndicator.enabled = false;
        }

        public SelectionDetails CheckPawnSelection(SelectionDetails s, Vector2 inputPosition)
        {
            var pawn = GetPawnUnderCursor(inputPosition);
            if ((pawn == null || pawn.Team == Teams.TeamB) && s.selectedPawn != null)
            {
                s.selectedPawn.Unhighlight();
                s.selectedPawn = null;
            }
            else if (pawn != null && pawn.Team == Teams.TeamA && pawn != s.selectedPawn)
            {
                s.selectedPawn?.Unhighlight();
                s.selectedPawn = pawn;
                s.selectedPawn.Highlight();
            }
            return s;
        }

        public SelectionDetails CheckTileSelection(SelectionDetails s, Vector2 inputPosition)
        {
            s = CheckSecondaryPawnSelection(s, inputPosition);
            if (s.secondaryPawn != null) return s;

            var tile = GetTileUnderCursor(inputPosition);
            SetMoveIndicator(s, tile);

            if (tile == null && s.selectedTile != null)
            {
                s.selectedTile.Unhighlight();
                s.selectedTile = null;
            }
            else if (tile != null && tile != s.selectedTile)
            {
                s.selectedTile?.Unhighlight();
                s.selectedTile = tile;
                s.selectedTile.Highlight();
            }

            return s;
        }
        
        private Pawn GetPawnUnderCursor(Vector2 pos)
        {
            return GetObjectUnderCursor<Pawn>(pos, Layers.GetMask(Layers.ObjectLayers.Pawn));
        }

        private Tile GetTileUnderCursor(Vector2 pos)
        {
            return GetObjectUnderCursor<Tile>(pos, Layers.GetMask(Layers.ObjectLayers.Tile));
        }

        private T GetObjectUnderCursor<T>(Vector2 pos, int layerMask) where T : MonoBehaviour
        {
            Ray ray = GameManager.Camera.CurrentCamera.ScreenPointToRay(pos);
            bool hit = Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, layerMask);
            if (!hit) return null;

            T component = hitInfo.collider.GetComponent<T>();
            if (component == null)
            {
                component = hitInfo.collider.GetComponentInParent<T>();
            }
            return component;
        }

        private void SetMoveIndicator(SelectionDetails s, Tile tile)
        {
            if (tile == null)
            {
                moveIndicator.enabled = false;
                return;
            }

            moveIndicator.enabled = true;
            var color = MoveValidator.IsValidMove(s.selectedPawn, tile) ? Color.white : Color.red;
            moveIndicator.SetPositions(new Vector3[]
            {
                s.selectedPawn.transform.position + Vector3.up * 0.5f,
                tile.transform.position + Vector3.up * 0.5f
            });
            var gradient = moveIndicator.colorGradient;
            gradient.SetKeys(
                new GradientColorKey[1] { new GradientColorKey(color, 0f) },
                new GradientAlphaKey[1] { new GradientAlphaKey(1f, 0f) }
            );
            moveIndicator.colorGradient = gradient;
        }

        private SelectionDetails CheckSecondaryPawnSelection(SelectionDetails s, Vector2 inputPosition)
        {
            var pawn = GetPawnUnderCursor(inputPosition);
            if (pawn == null)
            {
                s.enemyPawn?.Unhighlight();
                s.enemyPawn = null;
                s.secondaryPawn?.Unhighlight();
                s.secondaryPawn = null;
                s.selectedPawn.Highlight();
            }
            else if (pawn.Team == Teams.TeamA)
            {
                s.secondaryPawn?.Unhighlight();
                s.secondaryPawn = pawn;
                s.secondaryPawn.Highlight();
                s.selectedPawn.Unhighlight();
                moveIndicator.enabled = false;
                return s;
            }
            else
            {
                s.enemyPawn?.Unhighlight();
                s.enemyPawn = pawn;
                s.enemyPawn.Highlight();
            }
            return s;
        }
    }
}
