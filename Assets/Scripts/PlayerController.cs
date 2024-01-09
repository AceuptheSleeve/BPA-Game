using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerController : MonoBehaviour
{
    public Vector2 mousePos = new Vector2();
    public List<Unit> selectedUnits = new List<Unit>();
    public GameManager gameManager;
    public Vector3 currentPos;
    private Vector3 screenSpace;
    public float leftBarrier, rightBarrier;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        leftBarrier = 30;
        rightBarrier = 30;

        transform.position = new Vector3(0, 0, -10);
        Camera.main.orthographicSize = 5f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        currentPos = transform.position;
        screenSpace = Camera.main.WorldToViewportPoint(new Vector3(0.8f, 0.8f, 0));

        //Registering inputs
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        //Zoom the screen in and out using the srollwheel
        Camera.main.orthographicSize -= scrollInput * 8f * 100f * Time.deltaTime;

        //Camera movement
        if (horizontalInput != 0)
        {
            transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * 8);
        }

        if (verticalInput != 0)
        {
            transform.Translate(Vector3.up * verticalInput * Time.deltaTime * 8);
        }

        //Top and bottom barrier
        switch (transform.position.y)
        {
            case >= 20:
                transform.position = new Vector3(transform.position.x, 20, transform.position.z);
                break;

            case <= -30:
                transform.position = new Vector3(transform.position.x, -30, transform.position.z);
                break;
        }


        //Right and left barriers
        switch (transform.position.x)
        {
            case >= 45:
                transform.position = new Vector3(45, transform.position.y, transform.position.z);
                break;

            case <= -45:
                transform.position = new Vector3(-45, transform.position.y, transform.position.z);
                break;
        }

        //Camera zoom limits
        switch (Camera.main.orthographicSize)
        {
            case < 2.5f:
                Camera.main.orthographicSize = 2.5f;
                break;

            case > 9:
                Camera.main.orthographicSize = 9;
                break;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = new Vector3(0, 0, -10);
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
