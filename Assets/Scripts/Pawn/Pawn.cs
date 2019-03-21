using System.Collections;
using UnityEngine;

namespace AspektML
{
    public class Pawn : MonoBehaviour
    {
        public Teams Team { get { return State.Team; } }

        public PawnState State;

#pragma warning disable 649
        [SerializeField] private Material selectedMaterial;
        [SerializeField] private Material unselectedMaterial;
        [SerializeField] private MeshRenderer meshRenderer;
#pragma warning restore 649

        private const float MOVE_DURATION = 0.5f;

        public void Init(Tile tile, Teams team)
        {
            gameObject.SetActive(true);
            State = new PawnState()
            {
                CurrentTile = tile,
                IsAlive = true,
                Team = team,
            };
            transform.position = tile.transform.position;
            tile.Occupy(this);
        }

        public IEnumerator MoveTo(Tile tile)
        {
            if (tile.IsOccupied)
            {
                tile.GetOccupyingPawn().Capture();
            }

            if (tile == State.CurrentTile) yield break;
            State.CurrentTile.SetUnoccupied();
            tile.Occupy(this);

            var state = State;
            state.CurrentTile = tile;
            State = state;

            yield return StartCoroutine(MoveRoutine(tile.transform.position));
        }

        public void Highlight()
        {
            meshRenderer.material = selectedMaterial;
        }

        public void Unhighlight()
        {
            meshRenderer.material = unselectedMaterial;
        }

        public void Capture()
        {
            var state = State;
            state.IsAlive = false;
            State = state;
            gameObject.SetActive(false);
        }

        private IEnumerator MoveRoutine(Vector3 newPos)
        {
            Vector3 originalPos = transform.position;
            
            float timer = 0f;
            while (timer < MOVE_DURATION)
            {
                timer += Time.deltaTime;
                transform.position = Vector3.Lerp(originalPos, newPos, timer / MOVE_DURATION);
                yield return null;
            }
        }
    }
}
