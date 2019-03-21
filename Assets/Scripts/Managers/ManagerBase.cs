using UnityEngine;

namespace AspektML
{
    public abstract class ManagerBase : MonoBehaviour
    {
        
        /// <summary>
        /// Init is called by the GameManager once key object references have been established
        /// </summary>
        public abstract void Init();
    }
}
