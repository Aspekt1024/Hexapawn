using System.Collections.Generic;
using UnityEngine;

namespace AspektML.AI
{
    public class IndicatorPool
    {
        private readonly LineRenderer moveIndicatorPrefab;
        private readonly Transform indicatorParent;

        private const int MIN_POOL_SIZE = 3;
        private readonly List<LineRenderer> indicatorPool = new List<LineRenderer>();
        
        public IndicatorPool(LineRenderer moveIndicatorPrefab, Transform indicatorParent)
        {
            this.moveIndicatorPrefab = moveIndicatorPrefab;
            this.indicatorParent = indicatorParent;

            for (int i = 0; i < MIN_POOL_SIZE; i++)
            {
                CreateLineRenderer();
            }
        }

        public LineRenderer GetFreeIndicator()
        {
            foreach (var indicator in indicatorPool)
            {
                if (!indicator.enabled) return indicator;
            }
            return CreateLineRenderer();
        }

        public void ReturnIndicators()
        {
            foreach (var indicator in indicatorPool)
            {
                indicator.enabled = false;
            }
        }

        private LineRenderer CreateLineRenderer()
        {
            LineRenderer lr = Object.Instantiate(moveIndicatorPrefab, indicatorParent);
            lr.enabled = false;
            indicatorPool.Add(lr);
            return lr;
        }
        
    }
}
