using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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
                        if (EventSystem.current.IsPointerOverGameObject())
                            return;
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

        public void AddOnClick(IClickable clickable)
        {
            onClick += clickable.OnClick;
        }

        public void AddOnLongPress(ILongPressable longPressable)
        {
            onLongPress += longPressable.OnLongPress;
        }

        public void AddOnMove(IMoveable moveable)
        {
            onMove += moveable.OnMove;
        }

        public void AddStartTouch(IStartTouch startTouch)
        {
            onStartTouch += startTouch.OnStartTouch;
        }

        public void AddEndTouch(IEndTouch endTouch)
        {
            onEndTouch += endTouch.OnEndTouch;
        }

        #endregion
    }
}