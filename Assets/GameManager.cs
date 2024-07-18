using NavMeshPlus.Components;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public NavMeshSurface surface2D;
        private IInput input;
        private TouchManage touchManage;

        private void Awake()
        {
            var isMobile = Application.platform == RuntimePlatform.Android;
            input = false && isMobile
                ? gameObject.AddComponent<MobileInput>()
                : gameObject.AddComponent<ComputerInput>();
            touchManage = GetComponent<TouchManage>();
            if (touchManage == null) touchManage = gameObject.AddComponent<TouchManage>();

            touchManage.AddOnUpdateNavMesh(() =>
                surface2D.UpdateNavMesh(surface2D.navMeshData)
            );

            input.AddOnClick(touchManage);
            input.AddOnMove(touchManage);
            input.AddOnLongPress(touchManage);
            input.AddStartTouch(touchManage);
            input.AddEndTouch(touchManage);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A)) input = gameObject.AddComponent<MobileInput>();
        }
    }
}