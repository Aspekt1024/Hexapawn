using UnityEngine;

namespace AspektML.UI
{
    public class PlayerCursorUI : MonoBehaviour
    {
        private CanvasGroup canvasGroup;

        private enum States
        {
            Hidden, Visible
        }
        private States state;
        
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            Hide();
        }

        public void Hide()
        {
            state = States.Hidden;
            UIHelpers.HideUI(canvasGroup);
        }

        public void Show()
        {
            state = States.Visible;
            UIHelpers.ShowUI(canvasGroup);
        }

        public void SetPosition(Vector2 position)
        {
            if (state == States.Hidden) return;

            Vector3 pos = transform.position;
            pos.x = position.x;
            pos.y = position.y;
            transform.position = pos;
        }
    }
}
