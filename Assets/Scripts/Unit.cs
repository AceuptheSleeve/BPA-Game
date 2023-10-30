using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class Unit : MonoBehaviour
{
    public UnitStats stats;
    public EnemyUnit currentTarget;
    public float currentHP, nextAttackTime = 0;//, XP; Leveling System?
    public AudioClip[] audioClips; //Have audio and animations whenever a unit is idling, attacking, and when it dies?
    public AudioSource audioSource;
    public TilemapCollider2D tilemapCollider;
    public BoxCollider2D hitBox;
    Vector2 newPos = new Vector2();
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        //Assigning values
        tilemapCollider = GetComponent<TilemapCollider2D>();
        newPos = transform.position;
        hitBox = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        currentHP = stats.totalHP;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        if (stats.building)
        {
            Debug.Log("The HQ was successfully spawned in at " +transform.position+ "!");
        }

        else if (stats.worker)
        {
            Debug.Log(gameObject.name+ " has been spawned in at " +transform.position+ "!");
        }

        else
        {
            GiveName();
            Debug.Log(stats.unitName + ", " +gameObject.name+ " has been spawned in at " +transform.position+ "!");
            gameManager.electricPool = gameManager.electricPool -= stats.electricUsage;
        }

    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, newPos, stats.speed * Time.deltaTime);
        Debug.DrawLine(transform.position, newPos);

        //The unit will move to the mouse position on right click
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
        }

        //Searching
        else
        {
            EnemyDetection();
            nextAttackTime = 0;
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
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2 (stats.attackRange, stats.attackRange), 0);

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

    public IEnumerator SpawnTime()
    {
        yield return new WaitForSeconds(stats.spawnTimer);

        while (true)
        {
            Debug.Log(Time.time);
        }
    }

    public void SpawnUnit(Vector2 spawnPos)
    {
        Instantiate(gameObject, spawnPos, new Quaternion(0, 0, 0, 0));
    }

    /*
    public void DelayedSpawnUnit(Vector2 spawnPos)
    {
        StartCoroutine(SpawnTime());
        Instantiate(gameObject, spawnPos, new Quaternion(0, 0, 0, 0));
    }
    */

    //Giving the unit a random name
    public void GiveName()
    {
        int index = Random.Range(0, gameManager.names.Count);
        gameObject.name = gameManager.names[index];
        gameManager.names.RemoveAt(index);
    }
}
