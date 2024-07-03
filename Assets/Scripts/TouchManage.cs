using System.Collections.Generic;
using UnityEngine;

internal enum ControlState
{
    Normal,
    Edit
}

public class TouchManage : MonoBehaviour
{
    public GameObject buildingPrefab;
    public GameObject buildingContainer;
    public List<BuildingOBJ> buildingList = new();
    private ControlState controlState;

    private BuildingOBJ flagGameObject;

    private bool isTouchBuilding;
    private BuildingOBJ movingGameObject;
    private Vector3 startPoint;

    private void Awake()
    {
        buildingList = new List<BuildingOBJ>();
        controlState = ControlState.Normal;
    }

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);

            switch (controlState)
            {
                case ControlState.Normal:
                    NormalControl(touch);
                    break;
                case ControlState.Edit:
                    EditControl(touch);
                    break;
            }
        }
    }

    private void NormalControl(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                startPoint = Camera.main.ScreenToWorldPoint(touch.position);
                flagGameObject = TouchBuilding(startPoint);
                break;

            case TouchPhase.Moved:
                break;

            case TouchPhase.Ended:
                var endPoint = Camera.main.ScreenToWorldPoint(touch.position);
                if ((Vector2)endPoint == (Vector2)startPoint && flagGameObject != null)
                {
                    movingGameObject = flagGameObject;
                    movingGameObject.SetColor(BuildingOBJ.EditState.NoOverlapping);
                    controlState = ControlState.Edit;
                }

                flagGameObject = null;
                break;
        }
    }

    private BuildingOBJ TouchBuilding(Vector2 position)
    {
        var collider = Physics2D.OverlapPoint(position);
        if (collider != null && collider.TryGetComponent(out BuildingOBJ building)) return building;

        return null;
    }

    private void EditControl(Touch touch)
    {
        // update grid color
        if (movingGameObject != null)
        {
            if (movingGameObject.IsOverlapping())
            {
                movingGameObject.SetColor(BuildingOBJ.EditState.Overlapping);
            }
            else
            {
                movingGameObject.SetColor(BuildingOBJ.EditState.NoOverlapping);
            }
        }
        
        switch (touch.phase)
        {
            case TouchPhase.Began:
            {
                startPoint = Camera.main.ScreenToWorldPoint(touch.position);
                var touchBuilding = TouchBuilding(startPoint);
                if (touchBuilding != null)
                    isTouchBuilding = true;
                else
                    isTouchBuilding = false;
            }
                break;

            case TouchPhase.Moved:
                var displacement = Camera.main.ScreenToWorldPoint(touch.position);
                if (isTouchBuilding)
                {
                    var realMovePos = GridManage.RealToGridToReal(displacement.x, displacement.y);
                    if (realMovePos != (Vector2)movingGameObject.transform.position) //whether move
                    {
                        movingGameObject.transform.position = new Vector3(realMovePos.x, realMovePos.y, -5);
                        movingGameObject.editGrid.transform.position = new Vector3(
                        movingGameObject.transform.position.x, movingGameObject.transform.position.y, -4.99999f);
                    }
                }

                break;

            case TouchPhase.Ended:
                if (isTouchBuilding)
                {
                    var touchBuilding = TouchBuilding(Camera.main.ScreenToWorldPoint(touch.position));
                    var endPoint = Camera.main.ScreenToWorldPoint(touch.position);
                    if ((Vector2)endPoint == (Vector2)startPoint && !movingGameObject.IsOverlapping() &&
                        touchBuilding == movingGameObject)
                    {
                        movingGameObject.transform.position = new Vector3(
                        movingGameObject.transform.position.x, movingGameObject.transform.position.y, movingGameObject.transform.position.y);

                        movingGameObject.editGrid.transform.position = new Vector3(
                        movingGameObject.transform.position.x, movingGameObject.transform.position.y, 100f);

                        movingGameObject.SetColor(BuildingOBJ.EditState.Normal);
                        movingGameObject = null;
                        controlState = ControlState.Normal;
                    }
                }

                break;
        }
    }

    public void CreateBuilding()
    {
        //todo:building type
        if (controlState == ControlState.Edit) return;
        var b = Instantiate(buildingPrefab, buildingContainer.transform);
        var bobj = b.GetComponent<BuildingOBJ>();
        if (movingGameObject != null) movingGameObject.SetColor(BuildingOBJ.EditState.Normal);
        movingGameObject = bobj;
        movingGameObject.SetColor(BuildingOBJ.EditState.NoOverlapping);
        startPoint = movingGameObject.transform.position;
        controlState = ControlState.Edit;
        buildingList.Add(bobj);
    }
}