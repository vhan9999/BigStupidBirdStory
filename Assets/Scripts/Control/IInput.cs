using UnityEngine;

public interface IInput
{
    public void AddOnClick(IClickable clickable);

    // public void RmOnClick(UnityAction<Vector3> newOnClick);
    public void AddOnLongPress(ILongPressable longPressable);

    // public void RmOnLongPress(UnityAction<Vector3> newOnLongPress);
    public void AddOnMove(IMoveable moveable);
    public void AddStartTouch(IStartTouch startTouch);
    public void AddEndTouch(IEndTouch endTouch);
}

public interface IEndTouch
{
    void OnEndTouch(Vector2 position);
}

public interface IStartTouch
{
    void OnStartTouch(Vector2 position);
}

public interface IMoveable
{
    void OnMove(Vector2 position, Vector2 delta);
}

public interface IClickable
{
    void OnClick(Vector2 position);
}

public interface ILongPressable
{
    void OnLongPress(Vector2 position);
}