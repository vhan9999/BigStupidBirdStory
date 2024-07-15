using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class ComputerInput : MonoBehaviour, IInput
    {
        private readonly float clickDuration = 0.1f;
        private bool clicking;
        private bool moved;
        private Vector2 oldMousePosition;
        private UnityAction<Vector2> onClick;
        private UnityAction<Vector2> onLongPress;
        private UnityAction<Vector2> onMove;

        private float totalDownTime;

        private static float moveOffset => 0.1f;

        private void Update()
        {
            // Detect the first click
            if (Input.GetMouseButtonDown(0))
            {
                totalDownTime = 0;
                clicking = true;
                oldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                moved = false;
            }

            // If a first click detected, and still clicking,
            // measure the total click time, and fire an event
            // if we exceed the duration specified
            if (clicking && Input.GetMouseButton(0))
            {
                Vector2 mousePosition = Input.mousePosition;
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                totalDownTime += Time.deltaTime;
                if (totalDownTime >= clickDuration && clicking && !moved)
                {
                    clicking = false;
                    onLongPress?.Invoke(worldPosition);
                }

                if (Vector2.Distance(worldPosition, oldMousePosition) > moveOffset)
                {
                    var delta = worldPosition - oldMousePosition;
                    onMove?.Invoke(delta);
                    moved = true;
                }

                oldMousePosition = worldPosition;
            }

            // If a first click detected, and we release before the
            // duraction, do nothing, just cancel the click
            if (clicking && Input.GetMouseButtonUp(0))
            {
                if (clicking && totalDownTime < clickDuration)
                {
                    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    onClick?.Invoke(worldPosition);
                }

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