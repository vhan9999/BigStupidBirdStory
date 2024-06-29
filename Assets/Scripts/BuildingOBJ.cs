using System;
using UnityEngine;

public class BuildingOBJ : ClickableOBJ
{
    public enum EditState
    {
        Normal,
        Overlapping,
        NoOverlapping
    }

    public bool isEditing;
    public Vector2 startPos = Vector2.zero;

    public LayerMask layerMask;
    private ContactFilter2D contactFilter;

    private GameObject editGrid;
    private PolygonCollider2D editGridCollider;
    private SpriteRenderer editGridSprite;

    private void Awake()
    {
        contactFilter.SetLayerMask(layerMask);
        editGrid = transform.GetChild(0).gameObject;
        editGridCollider = editGrid.GetComponent<PolygonCollider2D>();
        editGridSprite = editGrid.GetComponent<SpriteRenderer>();
        SetColor(EditState.Normal);
    }

    public void SetColor(EditState state)
    {
        switch (state)
        {
            case EditState.Normal:
                editGridSprite.material.color = new Color(1f, 1f, 1f, 0f);
                break;
            case EditState.Overlapping:
                editGridSprite.material.color = new Color(0.792f, 0.255f, 0.227f, 1.0f);
                break;
            case EditState.NoOverlapping:
                editGridSprite.material.color = new Color(0.227f, 0.666f, 0.792f, 1.0f);
                break;
        }
    }

    public override void Click()
    {
        if (isEditing) //edit stop
        {
            isEditing = false;
            SetColor(EditState.Normal);

            //edit z
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
            editGrid.transform.position = new Vector3(transform.position.x, transform.position.y, 100);
        }
        else //edit start
        {
            isEditing = true;

            startPos = transform.position;
            IsOverlapping();

            //edit z
            transform.position = new Vector3(transform.position.x, transform.position.y, -5f);
            editGrid.transform.position = new Vector3(transform.position.x, transform.position.y, -4.99999f);
        }
    }

    public void Move(Vector3 displacement)
    {
        var gridMovePos = GridManage.RealToGrid(displacement.x, displacement.y);
        gridMovePos =
            new Vector2((float)Math.Truncate(gridMovePos.x),
                (float)Math.Truncate(gridMovePos.y)); //remove numbers behind "."

        var realMovePos = GridManage.GridToReal(gridMovePos.x, gridMovePos.y);
        if (startPos + realMovePos != (Vector2)transform.position) //whether move
        {
            transform.position = new Vector3(startPos.x + realMovePos.x, startPos.y + realMovePos.y, -5);
            editGrid.transform.position = new Vector3(transform.position.x, transform.position.y, -4.99999f);
            Invoke("IsOverlapping", 0.02f);
        }
    }

    public bool IsOverlapping()
    {
        var results = new Collider2D[10];
        var count = editGridCollider.OverlapCollider(contactFilter, results);
        if (count != 1)
        {
            editGridSprite.material.color = new Color(0.792f, 0.255f, 0.227f, 1.0f);//red
            SetColor(EditState.Overlapping);
            return true;
        }
        else
        {
            editGridSprite.material.color = new Color(0.227f, 0.666f, 0.792f, 1.0f);//blue
        }

        SetColor(EditState.NoOverlapping);
        return false;
    }

    public void SetStartPos(Vector2 pos)
    {
        startPos = pos;
    }
}