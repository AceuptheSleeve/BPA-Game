using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 mousePos = new Vector2();


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); ;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Movement
        if (horizontalInput != 0)
        {
            transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * 8);
        }

        if (verticalInput != 0)
        {
            transform.Translate(Vector3.up * verticalInput * Time.deltaTime * 8);
        }
    }
}
