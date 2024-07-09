using System;
using UnityEngine;

public class GridManage : MonoBehaviour
{
    private static readonly float gridWidth = 1f;

    private static readonly float gridHeight = 0.5f;

    public static Vector2 GridToReal(Vector2 gird)
    {
        return GridToReal(gird.x, gird.y);
    }

    public static Vector2 GridToReal(float gridX, float gridY)
    {
        var realX = (gridX - gridY) * (gridWidth / 2);
        var realY = (gridX + gridY) * (gridHeight / 2);
        return new Vector2(realX, realY);
    }

    public static Vector2 RealToGrid(Vector2 real)
    {
        return RealToGrid(real.x, real.y);
    }

    public static Vector2 RealToGrid(float realX, float realY)
    {
        var gridX = (realY / (gridHeight / 2) + realX / (gridWidth / 2)) / 2;
        var gridY = (realY / (gridHeight / 2) - realX / (gridWidth / 2)) / 2;
        return new Vector2((float)Math.Truncate(gridX),
            (float)Math.Truncate(gridY)); //remove numbers behind "."
    }

    public static Vector2 RealToGridFloat(float realX, float realY)
    {
        var gridX = (realY / (gridHeight / 2) + realX / (gridWidth / 2)) / 2;
        var gridY = (realY / (gridHeight / 2) - realX / (gridWidth / 2)) / 2;
        return new Vector2(gridX, gridY);
    }

    public static Vector2 RealToGridToReal(float realX, float realY)
    {
        var grid = RealToGrid(realX, realY);
        return GridToReal(grid);
    }

    public static float CalculateOval(Vector2 direction)//width:hight = 2:1 (output: 0.5 ~ 1)
    {

        Vector2 normDirection = direction.normalized;

        float dx_prime = normDirection.x / gridWidth;
        float dy_prime = normDirection.y / gridHeight;

        float scale = Mathf.Sqrt(dx_prime * dx_prime + dy_prime * dy_prime);

        return 1.0f / scale;
    }
}