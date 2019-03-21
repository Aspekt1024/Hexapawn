using UnityEngine;

namespace AspektML
{
    public class GameManager : MonoBehaviour
    {
        private PawnManager pawnManager;
        private BoardManager boardManager;
        private GameplayManager gameplayManager;
        private AIManager aiManager;
        private PlayerManager playerManager;
        private UIManager uiManager;
        private CameraManager cameraManager;
        private DataManager dataManager;

        public static PawnManager Pawns { get { return instance.pawnManager; } }
        public static BoardManager Board { get { return instance.boardManager; } }
        public static GameplayManager Gameplay { get { return instance.gameplayManager; } }
        public static AIManager AI { get { return instance.aiManager; } }
        public static PlayerManager Player { get { return instance.playerManager; } }
        public static UIManager UI { get { return instance.uiManager; } }
        public static CameraManager Camera { get { return instance.cameraManager; } }
        public static DataManager Data { get { return instance.dataManager; } }

        private static GameManager instance;
        
        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError($"{nameof(GameManager)} has awoken but an instance already exists");
                return;
            }
            instance = this;

            Init();
        }

        private void Init()
        {
            dataManager = GetAndInitialise<DataManager>();
            boardManager = GetAndInitialise<BoardManager>();
            pawnManager = GetAndInitialise<PawnManager>();
            aiManager = GetAndInitialise<AIManager>();
            gameplayManager = GetAndInitialise<GameplayManager>();
            playerManager = GetAndInitialise<PlayerManager>();
            uiManager = GetAndInitialise<UIManager>();
            cameraManager = GetAndInitialise<CameraManager>();

            gameplayManager.StartGame();
        }

        private T GetAndInitialise<T>() where T : ManagerBase
        {
            var objects = FindObjectsOfType<T>();
            if (objects.Length == 0)
            {
                Debug.LogError($"Attempting to get {typeof(T).Name} in scene, but none were found. " +
                    "Please add it to the scene, or remove it from GameManager if this is no longer required.");
                return null;
            }
            if (objects.Length > 1)
            {
                Debug.LogWarning($"Multiple {typeof(T).Name}s found in scene. This could cause unexpected setup behaviour");
            }

            var manager = objects[0];
            manager.Init();

            return manager;
        }

        private void OnDestroy()
        {
            if (instance.Equals(this))
            {
                instance = null;
            }
        }
    }
}
