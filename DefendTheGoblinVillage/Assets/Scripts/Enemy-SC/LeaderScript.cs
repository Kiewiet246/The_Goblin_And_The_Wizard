using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class LeaderScript : MonoBehaviour
{
    [Header("Components")] public Collider enemyCollider;
    public Rigidbody rb;
    [SerializeField] private ShowDamage showDamage;

    [Header("Target Movement")]
    public List<TileInfo> waypoints;
    public Vector3 vectorTarget;
    [SerializeField] private TileInfo targetTile;
    [FormerlySerializedAs("terrainType")] [SerializeField] private TileInfo.TerrainType standingOnTerrain;
    [SerializeField] private float distanceToTarget; //How far the Target is
    [SerializeField] private float closeEnough; //How far the leader needs to be to switch target
    [SerializeField] private float movementSpeed;
    public float speedBoost = 0;

    [Header("Terrain Modifiers")] [SerializeField]
    private float normalSpeed;
    [SerializeField] private float forrestSpeed;
    [SerializeField] private float muddyspeed;
    [SerializeField] private float stoneSpeed;
    [SerializeField] private bool enteredOnce = false;
    
    [Header("Jumping")] public bool checkforStep = false;
    [SerializeField] private float jumpForce;
    [SerializeField] private float sightRange;
    [SerializeField] private int layer;
    
    [Header("Health")]
    public float baseHealth;
    public float health = 6;
    public float damage = 3;
    public GoblinVillage goblinVillage;
    
    [Header("Spells")]
    public SpellAflections spellAflections;
    public SpellManager spellManager;
    public bool hasBeenFrozen = false;
    
    [Header("Other")] [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private int difficulty = 1;
    public int lived = 0;
    public Vector3 endPoint;
    public EnemyContoller enemyCont;

    [Header("economy Stuff")] public int dropsMoney;
    public int dropsMana;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (enemyCollider.enabled)
        {
            checkforStep = true;
        }
        else
        {
            checkforStep = false;
        }

        if (!hasBeenFrozen)
        {
            MoveEnemy();
            CheckDistToTarget();
            CheckForStep();
        }
        CheckYPos();
    }

    public void ResetHealth()
    {
        health = baseHealth;
    }

    public void TakeDamage(float damage, float stacks, SpellManager.SpellType spell)
    {
        health -= damage;
        switch (spell)
        {
            case SpellManager.SpellType.Normal:
                break;
            case SpellManager.SpellType.Fire:
                spellManager.LeadersGotBurnt(this, stacks);
                break;
            case SpellManager.SpellType.Ice:
                spellManager.LeadersGotIced(this, stacks);
                break;
            case SpellManager.SpellType.Poison:
                spellManager.LeadersGotPoisoned(this, stacks);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(spell), spell, null);
        }
        showDamage.FlashDamage();
        if (health <= 0)
        {
            waypoints.Clear();
            vectorTarget = Vector3.zero;
            enemyManager.RemoveLeaderFromField(this);
        }
    }

    private void CheckYPos()
    {
        if (transform.position.y < -20)
        {
            waypoints.Clear();
            vectorTarget = Vector3.zero;
            enemyManager.RemoveLeaderFromField(this);
        }
    }

    public void SetEnemyMan(EnemyManager enMan)
    {
        enemyManager = enMan;
    }

    public void SetDifficulty(int newDifficulty)
    {
       difficulty = newDifficulty; 
    }

    private void MoveEnemy()
    {
        if (vectorTarget != Vector3.zero)
        {
            float adjustmovemnt = new float();
            switch (standingOnTerrain)
            {
                case TileInfo.TerrainType.Normal:
                    adjustmovemnt = (movementSpeed + speedBoost) * normalSpeed;
                    break;
                case TileInfo.TerrainType.Muddy:
                    adjustmovemnt = (movementSpeed + speedBoost) * muddyspeed;
                    break;
                case TileInfo.TerrainType.Forest:
                   adjustmovemnt = (movementSpeed + speedBoost) * forrestSpeed;
                    break;
                case TileInfo.TerrainType.Stone:
                    if (!enteredOnce)
                    {
                        enteredOnce = true;
                        TakeDamage(stoneSpeed, 0, SpellManager.SpellType.Normal);
                    }
                    adjustmovemnt = (movementSpeed + speedBoost);
                    break;
                case TileInfo.TerrainType.Start:
                    adjustmovemnt = movementSpeed;
                    break;
                case TileInfo.TerrainType.End:
                    adjustmovemnt = movementSpeed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Vector3 direction = vectorTarget - transform.position;
            Vector3 movement = direction.normalized * (adjustmovemnt * Time.deltaTime);
           endPoint = transform.position + movement;

            rb.MovePosition(endPoint);
        }
    }

    private void CheckDistToTarget()
    {
        if (waypoints.Count > 0 && vectorTarget != Vector3.zero)
        {
            Vector3 stablilizedTarget = new Vector3(vectorTarget.x, transform.position.y, vectorTarget.z);
            distanceToTarget = Vector3.Distance(transform.position, stablilizedTarget);

            if (distanceToTarget <= closeEnough)
            {
                waypoints.RemoveAt(0);
                if (waypoints.Count > 0)
                {
                    vectorTarget = new Vector3(waypoints[0].transform.position.x, transform.position.y, waypoints[0].transform.position.z);
                    targetTile = waypoints[0];
                    //AssignTargetForFollowers();
                }
                
                else if (waypoints.Count == 0)
                {
                    enemyManager.RemoveLeaderFromField(this);
                }
            }
        }
    }

    private void CheckForStep()
    {
        if (checkforStep)
        {
            if (vectorTarget != Vector3.zero)
            {
                LayerMask layerMask = 1 << layer;
                RaycastHit hit = new RaycastHit();
                Vector3 stablilizedTarget = new Vector3(vectorTarget.x, transform.position.y, vectorTarget.z);
                Vector3 direction = (stablilizedTarget - transform.position).normalized;
                transform.LookAt(stablilizedTarget, Vector3.up);
                Physics.Raycast(transform.position, direction, out hit, sightRange, layerMask);
                Debug.DrawRay(transform.position, direction*sightRange, Color.red);
                if (hit.collider != null)
                {
                    // Debug.Log("Wall");
                    rb.AddForce(rb.transform.up * jumpForce, ForceMode.Impulse);
                }
            }
        }
    }

    public void SetWaypoints(List<TileInfo> newWaypoints)
    {
        if (waypoints.Count == 0)
        {
            for (int i = 0; i < newWaypoints.Count; i++)
            {
                waypoints.Add(newWaypoints[i]);
            }
            vectorTarget = new Vector3(waypoints[0].transform.position.x, transform.position.y, waypoints[0].transform.position.z);
         //   AssignTargetForFollowers();
        }
    }

    public void SetNewPath(TileInfo tileThatChanged, List<TileInfo> newWaypoints)
    {
        Debug.Log(gameObject.name);
        int countJ = newWaypoints.Count;
        int countI = waypoints.Count;
        List<TileInfo> savedWaypoints = new List<TileInfo>();
        bool foundTheCrossTile = false;
        bool breakOuterLoop = false;
        int savedWaypoint = new int();
        int savedNewpoint = new int();

        for (int i = 0; i < countI; i++)
        {
            for (int j = 0; j < countJ; j++)
            {
                if (waypoints[i] == newWaypoints[j])
                {
                    Debug.Log("Paths Crossed");
                    savedWaypoints.Add(newWaypoints[j]);
                    foundTheCrossTile = true;
                    savedNewpoint = j;
                    savedWaypoint = i;
                    breakOuterLoop = true;
                    break;
                }
                else if (j == countJ - 1)
                {
                    Debug.Log("endOfLine");
                    savedWaypoints.Add(waypoints[i]);
                }
            }

            if (breakOuterLoop)
            {
                break;
            }
        }

        if (foundTheCrossTile)
        {
            for (int j = savedNewpoint; j < newWaypoints.Count; j++)
            {
                savedWaypoints.Add(newWaypoints[j]);
            }
        }
        waypoints = savedWaypoints;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tower"))
        {
            if (collision.gameObject.GetComponent<TowerController>())
            {
                collision.gameObject.GetComponent<TowerController>().TakeDamage(damage);
                enemyManager.RemoveLeaderFromField(this);
            }
            
            else if (collision.gameObject.GetComponentInParent<TowerController>())
            {
                collision.gameObject.GetComponentInParent<TowerController>().TakeDamage(damage);
                enemyManager.RemoveLeaderFromField(this);
            }
        }
        
        else if (collision.gameObject.CompareTag("GoblinVillage"))
        {
            goblinVillage.TakeDamage(damage);
            enemyManager.RemoveLeaderFromField(this);
        }
        
        else if (collision.gameObject.CompareTag("TopTile"))
        {
            if (collision.gameObject.transform == targetTile.topTileTransform)
            {
                standingOnTerrain = targetTile.terrainType;
            }
        }
    }
}
