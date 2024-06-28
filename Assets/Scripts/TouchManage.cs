using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManage : MonoBehaviour
{

    GameObject flagGameObject = null;
    private bool isDragging = false;
    private Vector3 startPoint;

    public GameObject buildingPrefab;
    public GameObject buildingContainer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPoint = Camera.main.ScreenToWorldPoint(touch.position);
                    Collider2D collider = Physics2D.OverlapPoint(startPoint);

                    if (collider != null && collider.TryGetComponent(out ClickableOBJ clickableOBJ))
                    {
                        flagGameObject = collider.gameObject;
                        if (flagGameObject.TryGetComponent(out BuildingOBJ building) && building.isEditing)
                        {
                            building.startPos = flagGameObject.transform.position;//update building's startPos
                            isDragging = true;
                        }
                    }
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        Vector3 displacement = (Camera.main.ScreenToWorldPoint(touch.position) - startPoint);
                        flagGameObject.GetComponent<BuildingOBJ>().Move(displacement);
                    }
                    break;

                case TouchPhase.Ended:
                    Vector3 endPoint = Camera.main.ScreenToWorldPoint(touch.position);
                    if (flagGameObject != null)
                    {
                        if((Vector2)endPoint == (Vector2)startPoint)
                            flagGameObject.GetComponent<ClickableOBJ>().Click();
                    }
                    flagGameObject = null;
                    isDragging = false;
                    break;
            }
        }


    }

    public void CreateBuilding()
    {
        //todo:building type
        var b = Instantiate(buildingPrefab, buildingContainer.transform);
        b.GetComponent<BuildingOBJ>().Click();
    }
}
