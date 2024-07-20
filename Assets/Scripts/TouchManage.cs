using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

internal enum ControlState
{
    Normal,
    Edit
}

public class TouchManage : MonoBehaviour, ILongPressable, IClickable, IEndTouch, IStartTouch, IMoveable
{
    public GameObject buildingPrefab;
    public GameObject buildingContainer;
    public List<BuildingOBJ> buildingList = new();
    [SerializeField] private BuildingOBJ movingGameObject;
    [SerializeField] private bool isMovingBuilding;
    [SerializeField] private GameObject EditEndButton;
    private ControlState controlState;

    private BuildingOBJ flagGameObject;

    private bool isTouchBuilding;
    private UnityAction onUpdateNavMesh;
    private Vector3 startPoint;

    private static float CameraSizeMin => 1;

    private static float CameraSizeMax => 8;

    private void Awake()
    {
        buildingList = new List<BuildingOBJ>();
        controlState = ControlState.Normal;
    }

    private void Start()
    {
        UpdateNavMesh();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.touchCount == 2)
        {
            var touchZero = Input.GetTouch(0);
            var touchOne = Input.GetTouch(1);
            var preZero = touchZero.position - touchZero.deltaPosition;
            var preOne = touchOne.position - touchOne.deltaPosition;
            var prevMagnitude = (preOne - preZero).magnitude;
            var currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            var diff = currentMagnitude - prevMagnitude;

            Zoom(diff * 0.001f);
        }
    }

    public void AddOnUpdateNavMesh(UnityAction action)
    {
        onUpdateNavMesh += action;
    }


    private void Zoom(float diff)
    {
        Camera.main.orthographicSize
            = Mathf.Clamp(Camera.main.orthographicSize - diff, CameraSizeMin, CameraSizeMax);
    }

    private BuildingOBJ TouchBuilding(Vector2 position)
    {
        var collider = Physics2D.OverlapPoint(position);
        if (collider != null && collider.TryGetComponent(out BuildingOBJ building)) return building;

        return null;
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
        EditEndButton.SetActive(true);
    }


    public void UpdateNavMesh()
    {
        onUpdateNavMesh?.Invoke();
    }

    #region input

    public void OnClickEditEndButton()
    {
        if (controlState == ControlState.Edit && !movingGameObject.IsOverlapping())
        {
            movingGameObject.SetColor(BuildingOBJ.EditState.Normal);
            movingGameObject.SetNormalPosition();
            movingGameObject = null;
            UpdateNavMesh(); // maybe need Invoke("UpdateNavMesh", 0.02f);
            controlState = ControlState.Normal;
            EditEndButton.SetActive(false);
        }
    }

    public void OnLongPress(Vector2 position)
    {
        // TODO
        isMovingBuilding = false;
        if (controlState == ControlState.Normal)
        {
            var tb = TouchBuilding(position);
            if (tb != null)
            {
                controlState = ControlState.Edit;
                tb.SetColor(BuildingOBJ.EditState.NoOverlapping);
                movingGameObject = tb;
            }
            EditEndButton.SetActive(true);
        }
    }

    public void OnStartTouch(Vector2 position)
    {
        if (controlState == ControlState.Edit)
            if (TouchBuilding(position) == movingGameObject)
                isMovingBuilding = true;
    }

    public void OnEndTouch(Vector2 position)
    {
        if (controlState == ControlState.Edit)
            isMovingBuilding = false;
    }

    public void OnClick(Vector2 position)
    {
        switch (controlState)
        {
            case ControlState.Normal:
                break;
            case ControlState.Edit:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void OnMove(Vector2 position, Vector2 delta)
    {
        if (isMovingBuilding) //movingGameObject.transform.position = position;
        {
            var realMovePos = GridManage.RealToGridToReal(position.x, position.y);
            movingGameObject.SetHoverPosition(realMovePos); 
            if (movingGameObject.IsOverlapping())
                movingGameObject.SetColor(BuildingOBJ.EditState.Overlapping);
            else
                movingGameObject.SetColor(BuildingOBJ.EditState.NoOverlapping);
        }
    }

    #endregion
}