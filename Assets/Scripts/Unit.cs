using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class Unit : MonoBehaviour
{
    public UnitStats stats;
    public EnemyUnit currentTarget;
    public float currentHP, distanceToTarget,nextAttackTime = 0;//, XP; Leveling System?
    public AudioClip[] audioClips; //Have audio and animations whenever a unit is idling, attacking, and when it dies?
    public AudioSource audioSource;
    public TilemapCollider2D tilemapCollider;
    public BoxCollider2D hitBox;
    public PlayerController playerController;
    Vector2 newPos = new Vector2();
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        //Assigning values
        playerController = Camera.main.GetComponent<PlayerController>();
        tilemapCollider = GetComponent<TilemapCollider2D>();
        newPos = transform.position;
        hitBox = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        currentHP = stats.totalHP;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        if (stats.building)
        {
            Debug.Log("The HQ was successfully spawned in at " + new Vector2(transform.position.x, transform.position.y)+ "!");
        }

        else if (stats.worker)
        {
            Debug.Log(gameObject.name + " has been spawned in at " + new Vector2(transform.position.x, transform.position.y) + "!");
            gameManager.electricPool = gameManager.electricPool -= stats.electricUsage;
        }

        else
        {
            GiveName();
            Debug.Log(stats.unitName + ", " +gameObject.name+ " has been spawned in at " + new Vector2(transform.position.x, transform.position.y) + "!");
            gameManager.electricPool = gameManager.electricPool -= stats.electricUsage;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Debug.DrawLine(transform.position, newPos);
        transform.position = Vector2.MoveTowards(transform.position, newPos, stats.speed * Time.deltaTime);

        //The unit will move to the mouse position on right click (left click right now for testing purposes) 
        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int gridPos = gameManager.map.WorldToCell(playerController.mousePos);

            RaycastHit2D castPos = Physics2D.Raycast(Camera.main.transform.position, new Vector2(gridPos.x, gridPos.y));

            if (CheckSpace(castPos.transform.gameObject))
            {
                newPos = new Vector2(castPos.transform.gameObject.transform.position.x, castPos.transform.gameObject.transform.position.y);
                Debug.Log("Moving " + gameObject.name + " to " + newPos);
            }

            else
            {
                Debug.Log(gameObject.name + " tried to move to an invalid position");
            }

        }

        //Attacking when possible
        if (currentTarget != null)
        {
            //Establishes a distance between the unit and its target
            distanceToTarget = Vector2.Distance(transform.position, currentTarget.transform.position);

            //Attack when in range
            if (Time.time >= nextAttackTime && distanceToTarget <= stats.attackRange)
            {
                if (!stats.worker)
                {
                    Attack();
                    nextAttackTime = Time.time + 1f / stats.attackRate;
                }

                else
                {
                    Gather();
                    nextAttackTime = Time.time + 1f / stats.attackRate;
                }
            }

            //Remove target when the target is out of range
            if (distanceToTarget > stats.attackRange)
            {
                Debug.Log(gameObject.name+ " has lost sight of " +currentTarget.name);
                currentTarget = null;
            }
        }

        //Searching
        else
        {
            EnemyDetection();
            nextAttackTime = 0;
            distanceToTarget = 0;
        }

        //Dying
        if (currentHP <= 0)
        {
            //audioSource.PlayOneShot(audioClips[0]);
            hitBox.enabled = false;
            Debug.Log(gameObject.name + " is dead!");
            gameManager.names.Add(gameObject.name);
            gameManager.electricPool = gameManager.electricPool += stats.electricUsage;
            Destroy(gameObject);
        }
    }

    //Attacking enemies
    public void Attack()
    {
        //audioSource.PlayOneShot(audioClips[1]);
        currentTarget.TakeDamage(stats.damage);
        Debug.Log(gameObject.name + " dealt " + stats.damage + " damage to " + currentTarget.name + ". " + currentTarget.name + " now has " + currentTarget.currentHP + " left.");
    }

    //If the unit is a worker, it'll gather nearby resources instead of attacking hostile units. Not working right now
    public void Gather()
    {
        if (currentTarget.stats.coal)
        {
            gameManager.currentCoal = gameManager.currentCoal += stats.damage;
            Debug.Log(gameObject.name + " added " + stats.damage + " to the coal total. The total is now " + gameManager.currentCoal);
        }

        else if (currentTarget.stats.iron)
        {
            gameManager.currentIron = gameManager.currentIron += stats.damage / 2f;
            Debug.Log(gameObject.name + " added " + stats.damage + " to the coal total. The total is now " + gameManager.currentIron);
        }
    }

    //Taking and dealing damage
    public void TakeDamage(float amount)
    {
        currentHP -= amount;
    }

    public void EnemyDetection()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(stats.attackRange, stats.attackRange), 0);

        foreach (Collider2D collider in colliders)
        {

            EnemyUnit indentifer = collider.GetComponent<EnemyUnit>();

            if (indentifer && !stats.worker && indentifer.tag != "Resource")
            {
                Debug.Log(gameObject.name + " has detected " + indentifer.gameObject.name);
                currentTarget = indentifer;
            }

            else if (indentifer && stats.worker)
            {
                if (indentifer.stats.coal)
                {
                    Debug.Log(gameObject.name + " has detected a coal deposit at " + indentifer.transform.position);
                    currentTarget = indentifer;
                }

                else if (indentifer.stats.iron)
                {
                    Debug.Log(gameObject.name + " has detected a iron deposit at " + indentifer.transform.position);
                    currentTarget = indentifer;
                }
            }
        }
    }

    //Needs work
    public IEnumerator SpawnTime()
    {
        yield return new WaitForSeconds(stats.spawnTime);

        while (true)
        {
            Debug.Log(Time.time);
        }
    }

    public void SpawnUnit(Vector2 spawnPos)
    {
        Instantiate(gameObject, spawnPos, new Quaternion());
    }

    //Needs work
    public void DelayedSpawnUnit(Vector2 spawnPos)
    {
        Debug.Log("Yes, this will spawn in after " + stats.spawnTime);
        Invoke("SpawnUnit(spawnPos)", stats.spawnTime);
    }

    //Giving the unit a random name
    public void GiveName()
    {
        int index = UnityEngine.Random.Range(0, gameManager.names.Count);
        gameObject.name = gameManager.names[index];
        gameManager.names.RemoveAt(index);
    }

    public bool CheckSpace(GameObject gameObject)
    {
        if (gameObject.layer > 1 && gameObject != null)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}
