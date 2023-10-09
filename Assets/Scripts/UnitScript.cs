using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class UnitScript : MonoBehaviour
{
    public UnitStats stats;
    public AudioClip[] audioClips;
    public TilemapCollider2D collider;
    Vector2 newPos = new Vector2();

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<TilemapCollider2D>();
        newPos = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, newPos, stats.speed * Time.deltaTime);

        if (Input.GetMouseButtonDown(0))
        {
            newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Moving " + gameObject.name + " to " + newPos);
        }
    }
}
