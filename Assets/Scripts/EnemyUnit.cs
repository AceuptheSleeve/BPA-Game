using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class EnemyUnit : MonoBehaviour
{
    public UnitStats stats;
    public Unit currentTarget;
    public float currentHP, distanceToTarget, nextAttackTime = 0;//, XP; Leveling System?
    public AudioSource audioSource;
    public BoxCollider2D hitBox;
    Vector2 playerPos = new Vector2();
    public GameManager gameManager;
    public Animator animator;
    public Rigidbody2D rb;
    public bool enemyInRange;

    // Start is called before the first frame update
    void Start()
    {
        //Assigning values
        hitBox = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHP = stats.totalHP;

        //Resources don't get custom names
        if (stats.iron || stats.coal)
        {
            Debug.Log(gameObject.name + " has spawned in at " + new Vector2(transform.position.x, transform.position.y) + "!");
        }

        else
        {
            GiveName();
            Debug.Log(stats.unitName + ", " + gameObject.name + " has been spawned in at " + new Vector2(transform.position.x, transform.position.y) + "!");
            gameManager.enemyUnits.Add(gameObject);
        }

        playerPos = gameManager.playerUnits[0].transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!enemyInRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerPos, stats.speed * Time.deltaTime);
        }

        //Attacking when possible
        if (currentTarget != null)
        {
            //Establishes a distance between the unit and its target
            distanceToTarget = Vector2.Distance(transform.position, currentTarget.transform.position);
            enemyInRange = true;

            //Attack when in range
            if (Time.time >= nextAttackTime && distanceToTarget <= stats.attackRange)
            {
                Attack();
                nextAttackTime = Time.time + 1f / stats.attackRate;
            }

            //Remove target when the target is out of range
            if (distanceToTarget > stats.attackRange)
            {
                Debug.Log(gameObject.name + " has lost sight of " + currentTarget.name);
                currentTarget = null;
            }
        }

        //Searching
        else
        {
            EnemyDetection();
            nextAttackTime = 0;
            distanceToTarget = 0;
            enemyInRange = false;
        }


        //Dying
        if (currentHP <= 0)
        {
            hitBox.enabled = false;
            gameManager.enemyUnits.Remove(gameObject);
            gameManager.names.Add(gameObject.name);
            if (audioSource.isPlaying) { audioSource.Stop(); }
            AudioSource.PlayClipAtPoint(gameManager.soundBank[UnityEngine.Random.Range(10, 13)], transform.position);
            Debug.Log(gameObject.name + " is dead!");
            Destroy(gameObject);
        }


    }

    //Attacking enemies
    public void Attack()
    {
        if (audioSource.isPlaying) { audioSource.Stop(); }
        audioSource.PlayOneShot(gameManager.soundBank[0]);
        currentTarget.TakeDamage(stats.damage);
        Debug.Log(gameObject.name + " dealt " + stats.damage + " damage to " + currentTarget.name + ". " + currentTarget.name + " now has " + currentTarget.currentHP + " left.");
    }

    //Taking and dealing damage
    public void TakeDamage(float amount)
    {
        currentHP -= amount;
    }

    //Since the 'enemy' don't need to gather resources, they can only detect player units
    public void EnemyDetection()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(stats.attackRange, stats.attackRange), 0);

        foreach (Collider2D collider in colliders)
        {

            Unit indentifer = collider.GetComponent<Unit>();

            if (indentifer)
            {
                Debug.Log(gameObject.name + " has detected " + indentifer.gameObject.name);
                currentTarget = indentifer;
            }
        }
    }

    public void SpawnUnit(Vector2 spawnPos)
    {
        Instantiate(gameObject, spawnPos, new Quaternion());
    }

    /* Needs work, currently rendered inactive
    public void DelayedSpawnUnit(Vector2 spawnPos)
    {
        Debug.Log("Yes, this will spawn in after " + stats.spawnTime);
        Invoke("SpawnUnit(spawnPos)", stats.spawnTime);
    }
    */

    //Giving the unit a random name
    public void GiveName()
    {
        int index = UnityEngine.Random.Range(0, gameManager.names.Count);
        gameObject.name = gameManager.names[index];
        gameManager.names.RemoveAt(index);
    }
}
