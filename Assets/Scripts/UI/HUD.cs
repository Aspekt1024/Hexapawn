using TMPro;
using UnityEngine;

namespace AspektML.UI
{
    public class HUD : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private TextMeshProUGUI scoreText;
#pragma warning restore 649

        public void SetScore(int playerWins, int aiWins)
        {
            scoreText.text = "Player: " + playerWins + "\n" +
                             "AI: " + aiWins;
        }
    }
}
