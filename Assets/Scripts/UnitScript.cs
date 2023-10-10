using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class UnitScript : MonoBehaviour
{
    public UnitStats stats;
    public int currentHP;//, XP; Leveling System?
    public float nextAttackTime = 0;
    //public AudioClip[] audioClips; 
    public TilemapCollider2D tilemapCollider;
    public BoxCollider2D hitBox;
    public GameObject unitRange;
    Vector2 newPos = new Vector2();

    // Start is called before the first frame update
    void Start()
    {
        tilemapCollider = GetComponent<TilemapCollider2D>();
        hitBox = GetComponent<BoxCollider2D>();
        unitRange.transform.localScale = unitRange.transform.localScale * stats.attackRange;
        newPos = transform.position;
        currentHP = stats.totalHP;
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

    void Attack(GameObject enemy)
    {
        GameObject Projectile = stats.projectilePrefab;
        Instantiate(Projectile);
        Projectile.transform.rotation = enemy.transform.rotation;
        Projectile.transform.position = Vector2.MoveTowards(transform.position, enemy.transform.position, stats.projectileSpeed * Time.deltaTime);   
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;

        if (currentHP <= 0)
        {
            unitRange.gameObject.SetActive(false);
            Debug.Log(gameObject.name + " is dead!");
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (Time.time >= nextAttackTime)
            {
                Attack(collision.gameObject);
                Debug.Log(gameObject.name + " is attacking " + collision.gameObject.name);
                nextAttackTime = Time.time + 1f / stats.attackRate;
            }
        }
    }
}
