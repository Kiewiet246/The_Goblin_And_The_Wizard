using System.Collections.Generic;
using UnityEngine;

public class LeaderScript : MonoBehaviour
{
    [Header("Components")] public Collider enemyCollider;
    public Rigidbody rb;

    [Header("Target Movement")]
    [SerializeField] private List<TileInfo> waypoints;
    [SerializeField] private Vector3 leaderTarget;
    [SerializeField] private float distanceToTarget; //How far the Target is
    [SerializeField] private float closeEnough; //How far the leader needs to be to switch target
    [SerializeField] private float movementSpeed;
    
    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float sightRange;
    [SerializeField] private int layer;
    
    [Header("Followers")]
    [SerializeField] private List<EnemyContoller> followers;

    [Header("Health")]
    public int health = 6;
    public int damage = 3;
    
    [Header("Other")] [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private int difficulty = 1;

    public Vector3 endPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ActivateFollowers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        MoveEnemy();
        CheckDistToTarget();
        CheckForStep();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            waypoints.Clear();
            leaderTarget = Vector3.zero;
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

    public void ActivateFollowers()
    {
        for (int i = 0; i < difficulty; i++)
        {
            followers[i].gameObject.SetActive(true);
        }
    }

    public void DeactivateFollowers()
    {
        for (int i = 0; i < followers.Count; i++)
        {
            followers[i].gameObject.SetActive(false);
        }
    }

    private void MoveEnemy()
    {
        if (leaderTarget != Vector3.zero)
        {
            Vector3 direction = leaderTarget - transform.position;
            Vector3 movement = direction.normalized * (movementSpeed * Time.deltaTime);
           endPoint = transform.position + movement;

            rb.MovePosition(endPoint);
        }
    }

    private void CheckDistToTarget()
    {
        if (waypoints.Count > 0 && leaderTarget != Vector3.zero)
        {
            Vector3 stablilizedTarget = new Vector3(leaderTarget.x, transform.position.y, leaderTarget.z);
            distanceToTarget = Vector3.Distance(transform.position, stablilizedTarget);

            if (distanceToTarget <= closeEnough)
            {
                waypoints.RemoveAt(0);
                if (waypoints.Count > 0)
                {
                    leaderTarget = new Vector3(waypoints[0].transform.position.x, transform.position.y, waypoints[0].transform.position.z);
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
        if (leaderTarget != Vector3.zero)
        {
            LayerMask layerMask = 1 << layer;
            RaycastHit hit = new RaycastHit();
            Vector3 stablilizedTarget = new Vector3(leaderTarget.x, transform.position.y, leaderTarget.z);
            Vector3 direction = (stablilizedTarget - transform.position).normalized;
            Physics.Raycast(transform.position, direction, out hit, sightRange, layerMask);
            Debug.DrawRay(transform.position, direction*sightRange, Color.red);
            if (hit.collider != null)
            {
               // Debug.Log("Wall");
                rb.AddForce(rb.transform.up * jumpForce, ForceMode.Impulse);
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
            leaderTarget = new Vector3(waypoints[0].transform.position.x, transform.position.y, waypoints[0].transform.position.z);
         //   AssignTargetForFollowers();
        }

        else
        {
            
        }
        
    }

    private void AssignTargetForFollowers()
    {
        if (followers != null)
        {
            for (int i = 0; i < followers.Count; i++)
            {
                //followers[i].SetTarget(leaderTarget);
            }
        }
        else
        {
            Debug.Log("Null Follows");
        }
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
        }
    }
}
