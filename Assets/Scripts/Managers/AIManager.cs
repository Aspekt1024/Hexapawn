using AspektML.AI;
using UnityEngine;

namespace AspektML
{
    public class AIManager : ManagerBase
    {
#pragma warning disable 649
        [SerializeField] private LineRenderer moveIndicatorPrefab;
        [SerializeField] private Transform indicatorParent;
#pragma warning restore 649

        private AIMoveHandler moveHandler;

        public override void Init()
        {
            IndicatorPool indicatorPool = new IndicatorPool(moveIndicatorPrefab, indicatorParent);
            moveHandler = new AIMoveHandler(indicatorPool);
        }

        public void MakeMove()
        {
            bool success = moveHandler.MakeMove();
            if (!success)
            {
                Debug.LogWarning("No available moves, but this wasn't caught by the GameplayHandler");
            }
        }

        public void ActionWin()
        {
            moveHandler.ActionWin();
        }

        public void ActionLoss()
        {
            moveHandler.ActionLoss();
        }
    }
}
