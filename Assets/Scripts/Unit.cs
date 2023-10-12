using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class Unit : MonoBehaviour
{
    public UnitStats stats;
    public EnemyUnit currentTarget;
    public float currentHP;//, XP; Leveling System?
    public float nextAttackTime = 0;
    //public AudioClip[] audioClips; preferable have audio and animations whenever a unit is idling, attacking, and when it dies
    public TilemapCollider2D tilemapCollider;
    public BoxCollider2D hitBox;
    Vector2 newPos = new Vector2();

    // Start is called before the first frame update
    void Start()
    {
        tilemapCollider = GetComponent<TilemapCollider2D>();
        newPos = transform.position;
        hitBox = GetComponent<BoxCollider2D>();
        currentHP = stats.totalHP;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, newPos, stats.speed * Time.deltaTime);
        Debug.DrawLine(transform.position, newPos);

        if (Input.GetMouseButtonDown(0))
        {
            newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Moving " + gameObject.name + " to " + newPos);
        }

        //Attacking when possible
        if (currentTarget != null)
        {
            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + 1f / stats.attackRate;
            }
        }

        //Searching
        else
        {
            EnemyDetection();
            nextAttackTime = 0;
        }
    }

    public void Attack()
    {
        currentTarget.TakeDamage(stats.damage);
        Debug.Log(gameObject.name + " dealt " + stats.damage + " damage to " + currentTarget.name + ". " + currentTarget.name + " now has " + currentTarget.currentHP + " left.");
    }

    public void TakeDamage(float amount)
    {
        currentHP -= amount;

        if (currentHP <= 0)
        {
            hitBox.enabled = false;
            Debug.Log(gameObject.name + " is dead!");
            Destroy(gameObject);
        }
    }

    public void EnemyDetection()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2 (stats.attackRange, stats.attackRange), 0);

        foreach (Collider2D collider in colliders)
        {
            EnemyUnit indentifer = collider.GetComponent<EnemyUnit>();
            
            if (indentifer)
            {
                Debug.Log(gameObject.name + " has detected " + indentifer.gameObject.name);
                currentTarget = indentifer;
            }
        }
    }
}
