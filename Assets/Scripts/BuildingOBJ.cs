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

    public GameObject editGrid;
    private ContactFilter2D contactFilter;
    private PolygonCollider2D editGridCollider;
    private SpriteRenderer editGridSprite;

    private void Awake()
    {
        contactFilter.SetLayerMask(layerMask);
        editGrid = transform.GetChild(0).gameObject;
        editGridCollider = editGrid.GetComponent<PolygonCollider2D>();
        editGridSprite = editGrid.GetComponent<SpriteRenderer>();
        SetColor(EditState.Normal);
        Debug.Log("awake done");
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

    public bool IsOverlapping()
    {
        var results = new Collider2D[10];
        var count = editGridCollider.OverlapCollider(contactFilter, results);
        if (count != 1)
        {
            editGridSprite.material.color = new Color(0.792f, 0.255f, 0.227f, 1.0f); //red
            SetColor(EditState.Overlapping);
            return true;
        }

        editGridSprite.material.color = new Color(0.227f, 0.666f, 0.792f, 1.0f); //blue

        SetColor(EditState.NoOverlapping);
        return false;
    }

    public void SetStartPos(Vector2 pos)
    {
        startPos = pos;
    }
}