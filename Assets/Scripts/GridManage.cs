using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridManage : MonoBehaviour
{
    private static float gridWidth = 0.56f;

    private static float gridHeight = 0.28f;

    public static Vector2 GridToReal(float gridX, float gridY) {
        float realX = (gridX - gridY) * (gridWidth / 2);
        float realY = (gridX + gridY) * (gridHeight / 2);
        return new Vector2(realX, realY);
    }
    public static Vector2 RealToGrid(float realX, float realY)
    {
        float gridX = (realY / (gridHeight / 2) + realX / (gridWidth / 2)) / 2;
        float gridY = (realY / (gridHeight / 2) - realX / (gridWidth / 2)) / 2;
        return new Vector2(gridX, gridY);
    }
}
