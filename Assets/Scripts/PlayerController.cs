using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 mousePos = new Vector2();
    public List<Unit> selectedUnits = new List<Unit>();
    public GameManager gameManager;
    private Vector3 screenSpace;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        screenSpace = Camera.main.WorldToViewportPoint(new Vector3(0.8f, 0.8f, 0));

        //Registering inputs
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        //Zoom the screen in and out using the srollwheel
        Camera.main.orthographicSize -= scrollInput * 8f * 100f * Time.deltaTime;
        //Camera zoom limits
        switch (Camera.main.orthographicSize)
        {
            case < 2.5f:
                Camera.main.orthographicSize = 2.5f;
                break;

            case > 6f:
                Camera.main.orthographicSize = 6f;
                break;
        }

        //Camera movement, on the right of the keys are screen boundaries
        if (Input.GetKey("w")/* || mousePos.y >= screenSpace.y */)
        {
            transform.Translate(Vector3.up * Time.deltaTime * 8);
        }

        if (Input.GetKey("a")/* || mousePos.x <= screenSpace.x */)
        {
            transform.Translate(Vector3.left * Time.deltaTime * 8);
        }

        if (Input.GetKey("s")/* || mousePos.y <= screenSpace.y */)
        {
            transform.Translate(Vector3.down * Time.deltaTime * 8);
        }

        if (Input.GetKey("d")/* || mousePos.x >= screenSpace.x */)
        {
            transform.Translate(Vector3.right * Time.deltaTime * 8);
        }
    }

    //Unit selection is handled by the controller for consistency, status determines weather a unit will be add or removed for the selected units
    public void SelectUnit(Unit unit, bool status)
    {
        if (status)
        {
            unit.animator.SetTrigger("Highlight");
            Debug.Log(unit.name + " has been selected");
            unit.isSelected = true;
            selectedUnits.Add(unit);
        }

        else
        {
            unit.animator.SetTrigger("Highlight");
            Debug.Log(unit.name + " has been deselected");
            unit.isSelected = false;
            selectedUnits.Remove(unit);
        }
    }
}
