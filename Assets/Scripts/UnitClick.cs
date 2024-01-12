using Unity.VisualScripting;
using UnityEngine;

public class UnitClick : MonoBehaviour
{
    private Camera myCam;

    public LayerMask clickable;
    public LayerMask ground;

    void Start()
    {
        myCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // If left click
        {
            RaycastHit2D hit = Physics2D.Raycast(myCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); // Makes a ray at the location of the mouse

            if (hit) // If ray hits something
            {
                if (Input.GetKey(KeyCode.LeftShift)) // If left shift
                {
                    UnitSelections.Instance.ShiftClickSelect(hit.collider.gameObject);
                }
                else
                {
                    UnitSelections.Instance.ClickSelect(hit.collider.gameObject);
                    Debug.Log(hit.collider.gameObject.name);
                }
            }
            else // If the ray doesn't hit anything
            {
                if (!Input.GetKey(KeyCode.LeftShift)) // If not left shift
                {
                    UnitSelections.Instance.DeselectAll();
                }
            }
        }
    }
}
