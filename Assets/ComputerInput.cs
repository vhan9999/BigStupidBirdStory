using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class ComputerInput : MonoBehaviour, IInput
    {
        private readonly float clickDuration = 0.5f;
        private bool clicking;
        private Vector2 oldMousePosition;
        private UnityAction<Vector2> onClick;
        private UnityAction<Vector2> onLongPress;
        private UnityAction<Vector2> onMove;

        private float totalDownTime;

        private void Update()
        {
            // Detect the first click
            if (Input.GetMouseButtonDown(0))
            {
                totalDownTime = 0;
                clicking = true;
                oldMousePosition = Input.mousePosition;
            }

            // If a first click detected, and still clicking,
            // measure the total click time, and fire an event
            // if we exceed the duration specified
            if (clicking && Input.GetMouseButton(0))
            {
                totalDownTime += Time.deltaTime;
                Vector2 mousePosition = Input.mousePosition;
                onMove.Invoke(mousePosition - oldMousePosition);
                oldMousePosition = mousePosition;

                if (totalDownTime >= clickDuration)
                {
                    Debug.Log("Long click");
                    clicking = false;
                    onLongPress.Invoke(Input.mousePosition);
                }
            }

            // If a first click detected, and we release before the
            // duraction, do nothing, just cancel the click
            if (clicking && Input.GetMouseButtonUp(0))
            {
                onClick.Invoke(Input.mousePosition);
                clicking = false;
            }
        }

        #region Subscribe

        public void AddOnClick(UnityAction<Vector2> newOnClick)
        {
            onClick += newOnClick;
        }


        public void AddOnLongPress(UnityAction<Vector2> newOnLongPress)
        {
            onLongPress += newOnLongPress;
        }

        public void AddOnMove(UnityAction<Vector2> newOnMove)
        {
            onMove += newOnMove;
        }

        #endregion
    }
}