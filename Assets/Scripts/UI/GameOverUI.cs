using System.Collections;
using TMPro;
using UnityEngine;

namespace AspektML.UI
{
    public class GameOverUI : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private TextMeshProUGUI teamText;
        [SerializeField] private TextMeshProUGUI conditionText;
#pragma warning disable 649

        private CanvasGroup canvasGroup;

        private enum States
        {
            Visible, Hidden
        }
        private States state;

        private const float TRANSITION_DURATION = 0.3f;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            HideImmediate();
        }

        private void Update()
        {
            if (state == States.Hidden) return;

            if (Input.GetMouseButtonUp(0))
            {
                GameManager.Gameplay.GameoverAcknowledged();
            }
        }

        public void SetText(Teams team, string condition)
        {
            teamText.text = (team == Teams.TeamA ? "Player" : "AI") + " Wins!";
            conditionText.text = condition;
        }
        
        public void Show()
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
            StartCoroutine(ShowRoutine());
        }

        public void Hide()
        {
            state = States.Hidden;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            StartCoroutine(HideRoutine());
        }

        private void HideImmediate()
        {
            state = States.Hidden;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            canvasGroup.alpha = 0f;
        }

        private IEnumerator ShowRoutine()
        {
            float timer = 0f;
            while (timer < TRANSITION_DURATION)
            {
                timer += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / TRANSITION_DURATION);
                yield return null;
            }
            state = States.Visible;
        }

        private IEnumerator HideRoutine()
        {
            float timer = 0f;
            while (timer < TRANSITION_DURATION)
            {
                timer += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / TRANSITION_DURATION);
                yield return null;
            }
        }
    }
}
