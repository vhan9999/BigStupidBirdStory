using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class BuildingOBJ : ClickableOBJ
{
    public bool isEditing = false;
    public Vector2 startPos = Vector2.zero;

    public LayerMask layerMask;
    ContactFilter2D contactFilter = new ContactFilter2D();

    private GameObject editGrid = null;
    private PolygonCollider2D editGridCollider = null;
    private SpriteRenderer editGridSprite = null;
    private void Awake()
    {
        contactFilter.SetLayerMask(layerMask);
        Debug.Log("aaa");
        editGrid = transform.GetChild(0).gameObject;
        editGridCollider = editGrid.GetComponent<PolygonCollider2D>();
        editGridSprite = editGrid.GetComponent<SpriteRenderer>();
        editGridSprite.material.color = new Color(1f, 1f, 1f, 0f);
    }

    public override void Click()
    {
        if (isEditing)//edit stop
        {
            isEditing = false;
            editGridSprite.material.color = new Color(1f, 1f, 1f, 0f);

            //edit z
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
            editGrid.transform.position = new Vector3(transform.position.x, transform.position.y, 100);
        }
        else//edit start
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
        Vector2 gridMovePos = GridManage.RealToGrid(displacement.x, displacement.y);
        gridMovePos = new Vector2((float)Math.Truncate(gridMovePos.x), (float)Math.Truncate(gridMovePos.y));//remove numbers behind "."

        Vector2 realMovePos = GridManage.GridToReal(gridMovePos.x, gridMovePos.y);
        if (startPos + realMovePos != (Vector2)transform.position)//whether move
        {
            transform.position = new Vector3(startPos.x + realMovePos.x, startPos.y + realMovePos.y,-5);
            editGrid.transform.position = new Vector3(transform.position.x, transform.position.y, -4.99999f);
            Invoke("IsOverlapping", 0.02f);
        }
    }

    private void IsOverlapping()
    {
        Collider2D[] results = new Collider2D[10];
        int count = editGridCollider.OverlapCollider(contactFilter,results);

        

        if (count != 1)
        {
            editGridSprite.material.color = new Color(0.792f, 0.255f, 0.227f, 1.0f);//red
        }
        else
        {
            editGridSprite.material.color = new Color(0.227f, 0.666f, 0.792f, 1.0f);//blue
        }
    }
}