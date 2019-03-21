using UnityEngine;

namespace AspektML
{
    public class CameraManager : ManagerBase
    {
        public enum Cameras
        {
            Main
        }

#pragma warning disable 649
        [SerializeField] private Camera mainCamera;
#pragma warning restore 649

        public Camera CurrentCamera { get; private set; }

        public override void Init()
        {
            CurrentCamera = mainCamera;
        }

        public void SwitchCamera(Cameras camera)
        {
            switch (camera)
            {
                case Cameras.Main:
                    CurrentCamera = mainCamera;
                    break;
                default:
                    break;
            }
        }
    }
}
