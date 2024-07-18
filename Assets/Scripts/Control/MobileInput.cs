using System;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class MobileInput : MonoBehaviour, IInput
    {
        private readonly float clickDuration = 0.5f;

        private bool clicking;

        private UnityAction<Vector2> onClick;
        private UnityAction<Vector2> onEndTouch;
        private UnityAction<Vector2> onLongPress;
        private UnityAction<Vector2, Vector2> onMove;
        private UnityAction<Vector2> onStartTouch;
        private float totalDownTime;

        private void Update()
        {
            if (Input.touchCount == 1)
            {
                var touch = Input.GetTouch(0);
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(touch.position);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        clicking = true;
                        totalDownTime = 0;
                        // moved = false;
                        onStartTouch?.Invoke(worldPosition);
                        break;
                    case TouchPhase.Moved:
                        var delta = touch.deltaPosition;
                        var deltaWorldPosition =
                            worldPosition - (Vector2)Camera.main.ScreenToWorldPoint(touch.position - delta);

                        onMove?.Invoke(worldPosition, deltaWorldPosition);

                        break;
                    case TouchPhase.Stationary:
                        totalDownTime += Time.deltaTime;
                        if (totalDownTime >= clickDuration && clicking)
                        {
                            onLongPress?.Invoke(worldPosition);
                            clicking = false;
                        }

                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        onEndTouch?.Invoke(worldPosition);
                        if (clicking && totalDownTime < clickDuration) onClick?.Invoke(worldPosition);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        #region Subscribe

        public void AddOnMove(UnityAction<Vector2, Vector2> newOnMove)
        {
            onMove += newOnMove;
        }

        public void AddStartTouch(UnityAction<Vector2> newOnStartTouch)
        {
            onStartTouch += newOnStartTouch;
        }

        public void AddEndTouch(UnityAction<Vector2> newOnEndTouch)
        {
            onEndTouch += newOnEndTouch;
        }

        public void AddOnClick(UnityAction<Vector2> newOnClick)
        {
            onClick += newOnClick;
        }

        public void AddOnLongPress(UnityAction<Vector2> newOnLongPress)
        {
            onLongPress += newOnLongPress;
        }

        #endregion
    }
}