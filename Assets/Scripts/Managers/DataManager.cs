using AspektML.Data;

namespace AspektML
{ 
    public class DataManager : ManagerBase
    {
        private readonly WinData winData = new WinData();

        private int gameNum;

        public override void Init()
        {
            gameNum = 1;
        }

        public void ResetData()
        {
            gameNum = 1;
            winData.ResetData();
        }

        public void AddWin(Teams team)
        {
            winData.AddResult(gameNum, team);
            GameManager.UI.HUD.SetScore(winData.GetPlayerWins(), winData.GetAIWins());
            gameNum++;
        }
    }
}
