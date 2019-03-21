using UnityEngine;

namespace AspektML
{
    public class Tile : MonoBehaviour
    {
        // TODO doing this manually because small board and prototype. Should be automatic in larger project
        public int Row;
        public int Col;

        private Pawn occupyingPawn;

        public Pawn GetOccupyingPawn()
        {
            return occupyingPawn;
        }

        public void Occupy(Pawn pawn)
        {
            occupyingPawn = pawn;
        }

        public void SetUnoccupied()
        {
            occupyingPawn = null;
        }

        public bool IsOccupied { get { return occupyingPawn != null; } }
        public bool IsUnoccupied { get { return occupyingPawn == null; } }

        public void Highlight()
        {
            //Debug.Log($"highlight {name}");
        }

        public void Unhighlight()
        {
            //Debug.Log($"unhighlight {name}");
        }
    }
}
