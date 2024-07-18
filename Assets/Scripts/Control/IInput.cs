﻿using UnityEngine;
using UnityEngine.Events;

public interface IInput
{
    public void AddOnClick(UnityAction<Vector2> newOnClick);

    // public void RmOnClick(UnityAction<Vector3> newOnClick);
    public void AddOnLongPress(UnityAction<Vector2> newOnLongPress);

    // public void RmOnLongPress(UnityAction<Vector3> newOnLongPress);
    public void AddOnMove(UnityAction<Vector2, Vector2> newOnMove);
    public void AddStartTouch(UnityAction<Vector2> newOnStartTouch);
    public void AddEndTouch(UnityAction<Vector2> newOnEndTouch);
}