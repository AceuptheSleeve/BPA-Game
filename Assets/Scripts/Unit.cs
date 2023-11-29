using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Unit : MonoBehaviour
{
    public UnitStats stats;
    public EnemyUnit currentTarget;
    public float currentHP, distanceToTarget, nextAttackTime = 0;//, XP; Leveling System?
    public AudioSource audioSource;
    public BoxCollider2D hitBox;
    public PlayerController playerController;
    Vector2 newPos = new Vector2();
    public GameManager gameManager;
    public Animator animator;
    public Rigidbody2D rb;
    public bool isSelected = false;

    // Start is called before the first frame update
    void Start()
    {
        //Assigning values
        playerController = Camera.main.GetComponent<PlayerController>();
        newPos = transform.position;
        hitBox = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        currentHP = stats.totalHP;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        //Building spawn
        if (stats.building)
        {
            Debug.Log("The HQ was successfully spawned in at " + new Vector2(transform.position.x, transform.position.y)+ "!");
        }

        //Worker spawn
        else if (stats.worker)
        {
            Debug.Log(gameObject.name + " has been spawned in at " + new Vector2(transform.position.x, transform.position.y) + "!");
            gameManager.electricPool = gameManager.electricPool -= stats.electricUsage;
        }

        //Everything else that spawns
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

        //The unit will move to the mouse position on left click since right click is messy
        if (Input.GetMouseButtonDown(0) && !stats.building && isSelected)
        {
            Vector3Int gridPos = gameManager.mapLayers[1].WorldToCell(playerController.mousePos);

            //Check if the movement position is valid
            if (gameManager.mapLayers[1].HasTile(gridPos))
            {
                if (audioSource.isPlaying) { audioSource.Stop(); }
                audioSource.PlayOneShot(gameManager.soundBank[UnityEngine.Random.Range(1, 4)]);
                newPos = playerController.mousePos;
                Debug.Log("Moving " + gameObject.name + " to " + newPos);
                
            }

            //Gives the debug for where the invaild location is
            else
            {
                if (audioSource.isPlaying) { audioSource.Stop(); }
                audioSource.PlayOneShot(gameManager.soundBank[UnityEngine.Random.Range(4, 6)]);
                Debug.Log(gameObject.name + " tried to move to an invalid position at " +playerController.mousePos);
            }
        }

        //Deselect the unit
        if (Input.GetKeyDown(KeyCode.C) && isSelected)
        {
            animator.SetTrigger("Highlight");
            isSelected = false;
            Debug.Log(gameObject.name + " has been deselected");
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

                //Workers use the gather method instead of ordinary attacks since they can't fight
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
            if (audioSource.isPlaying) { audioSource.Stop(); }
            AudioSource.PlayClipAtPoint(gameManager.soundBank[UnityEngine.Random.Range(11, 15)], transform.position);
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
        audioSource.PlayOneShot(gameManager.soundBank[0]);
        currentTarget.TakeDamage(stats.damage);
        Debug.Log(gameObject.name + " dealt " + stats.damage + " damage to " + currentTarget.name + ". " + currentTarget.name + " now has " + currentTarget.currentHP + " left.");
    }

    //Gather nearby resources instead of attacking hostile units
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

            //Unit detection
            if (indentifer && !stats.worker && indentifer.tag != "Resource")
            {
                if (audioSource.isPlaying) { audioSource.Stop(); }
                audioSource.PlayOneShot(gameManager.soundBank[UnityEngine.Random.Range(7, 11)]);
                Debug.Log(gameObject.name + " has detected " + indentifer.gameObject.name);
                currentTarget = indentifer;
            }

            //Resource detection
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

    /* Needs work, currently rendered inactive
    public IEnumerator SpawnTime()
    {
        yield return new WaitForSeconds(stats.spawnTime);

        while (true)
        {
            Debug.Log(Time.time);
        }
    }
    */
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

    //Highlights the enemy when clicked on
    private void OnMouseDown()
    {
        if (!isSelected && !stats.building)
        {
            animator.SetTrigger("Highlight");
            Debug.Log(gameObject.name + " has been selected");
            isSelected = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y);
            newPos = transform.position;
        }
    }
}
