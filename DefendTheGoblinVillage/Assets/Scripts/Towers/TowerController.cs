using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class TowerController : MonoBehaviour
{
    #region Range Variables
    [Header("Range Values")]
    [SerializeField] private int range;
    public float maxDistance;
    [SerializeField] private float calRange;
    public TileInfo towerTile;
    public GridManager gridManager;
    public TowerManager towerManager;

    [Header("Show Range")]
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private float boxRecenter;
    [SerializeField] private List<TileInfo> tilesInRange;
    [SerializeField] private float timeToRecenter;
    
    [Header("Range Types")] [SerializeField]
    private RangeType rangeType;
    
    public enum RangeType
    {
        SingleLine,
        Radius,
        None
    }
    #endregion

    [Header("Rotation Values")]
    [SerializeField] private float angle = 60f;

    #region Tower Type
    public TowerType towerType;
    public enum TowerType
    {
        ArcherTower = 10,
        CanonTower = 15,
        WallTower = 20,
        BalistaTower =  5,
    }

    [SerializeField] private int towerCost;
    #endregion
    
    [Header("Health Values")]
    public float health;
    [SerializeField] private ShowDamage showDamage;
    
    [Header("Attack Variables")]
    [SerializeField] private List<LeaderScript> leaders;
    public float damage = 1;
    [SerializeField] private float attackRate = 1;
    [SerializeField] private float currentTime;
    [SerializeField] private bool canShoot = true;
    [SerializeField] private LeaderScript targetedLeader;
    
    [SerializeField] private float projectileSpeed = 20f;
    public List<Rigidbody> projectiles;
    
    [Header("Couratine Stuff")]
    [SerializeField] private float delay = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // CalculateRange();
       towerCost = (int)towerType;
       health = (int)towerType;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void FixedUpdate()
    {
        if (leaders.Count > 0)
        {
            Countdown();
        }
        else
        {
            currentTime = Time.time;
        }
    }
    
    public void Countdown()
    {
        float difference = Time.time - currentTime;
        if (difference >= attackRate)
        {
            if (canShoot)
            {
                if (leaders.Count > 0)
                {
                    AttackTheLeaders();
                }
                
            }
        }
    }

    private void AttackTheLeaders()
    {
        if (leaders[0] == null)
        {
            leaders.RemoveAt(0);
            return;
        }
        switch (towerType)
        {
            case TowerType.ArcherTower:
                    if (leaders[0].gameObject.activeSelf == false)
                    {
                        leaders.RemoveAt(0);
                        if (leaders.Count > 0)
                        {
                            AttackTheLeaders();
                            return;
                        }
                        else if (leaders.Count == 0)
                        {
                            return;
                        }
                    }
                    targetedLeader = leaders[0];
                    if (CheckIfInRange(targetedLeader))
                    {
                        ArcherShooting();
                    }
                    else
                    {
                        float playDis = 8f;
                        float diff = Vector3.Distance(transform.position, targetedLeader.endPoint);

                        if (diff <= calRange + playDis)
                        {
                            ArcherShooting();
                        }

                        else
                        {
                            leaders.RemoveAt(0);
                        }
                    }
                
                    break;
            case TowerType.CanonTower:
                Debug.Log("Canon Tower");
                if (leaders[0].gameObject.activeSelf == false)
                {
                    leaders.RemoveAt(0);
                    if (leaders.Count > 0)
                    {
                       AttackTheLeaders();
                        return;
                    }
                    else if (leaders.Count == 0)
                    {
                        return;
                    }
                }
                targetedLeader = leaders[0];
                if (CheckIfInRange(targetedLeader))
                {
                    StartCoroutine(CanonShooting());
                }
                else
                {
                    float playDis = 8f;
                    float diff = Vector3.Distance(transform.position, targetedLeader.endPoint);

                    if (diff <= calRange + playDis)
                    {
                        StartCoroutine(CanonShooting());
                    }

                    else
                    {
                        leaders.RemoveAt(0);
                    }
                }
                
                break;
            case TowerType.WallTower:
                break;
            case TowerType.BalistaTower:
                BalistaShooting();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        Debug.Log("Start Again");
        currentTime = Time.time;
    }

    private void ArcherShooting()
    {
        Vector3 direction = (targetedLeader.endPoint - transform.position).normalized;
        Rigidbody rb = projectiles[0];
        projectiles.Remove(rb);
        rb.linearVelocity = Vector3.zero;
        float distance = Vector3.Distance(transform.position, targetedLeader.endPoint);
        
        
        rb.gameObject.SetActive(true);
        rb.transform.LookAt(targetedLeader.transform.position);
        rb.gameObject.GetComponent<Projectile>().StartProjectile(targetedLeader.transform);
        Vector3 pos  = transform.position;
        rb.position = pos;
        float force = (projectileSpeed*(distance/maxDistance));
        rb.AddForce(direction * force, ForceMode.Impulse);
        targetedLeader = null;
    }

    private IEnumerator CanonShooting()
    {
        Vector3 pos = targetedLeader.endPoint;
        Rigidbody rb = projectiles[0];
        rb.gameObject.GetComponent<Projectile>().StartProjectile(targetedLeader.transform);
        rb.linearVelocity = Vector3.zero;
        projectiles.Remove(rb);
        Vector3 spawnPos = new Vector3(pos.x, pos.y + 4, pos.z);
        Debug.Log("Fire");
        
        yield return new WaitForSeconds(delay);
        
        rb.gameObject.SetActive(true);
        rb.position = spawnPos;
        rb.AddForce(projectileSpeed*Vector3.down, ForceMode.Impulse);
        targetedLeader = null;
        Debug.Log("Splat");

    }

    private void BalistaShooting()
    {
        List<LeaderScript> deadLeaders = new List<LeaderScript>();
        foreach (LeaderScript leader in leaders)
        {
            if (leader.gameObject.activeSelf)
            {
                leader.TakeDamage(damage);
            }
            else
            {
               deadLeaders.Add(leader);
            }
        }

        foreach (LeaderScript leaderDead in deadLeaders)
        {
            leaders.Remove(leaderDead);
        }
    }

    private bool CheckIfInRange(LeaderScript targetedEnemy)
    {
        float distance = Vector3.Distance(targetedEnemy.transform.position, transform.position);

        if (distance <= calRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<LeaderScript>())
            {
                if (!leaders.Contains(other.GetComponent<LeaderScript>()))
                {
                    if (leaders.Count == 0)
                    {
                        leaders.Add(other.GetComponent<LeaderScript>());
                        AttackTheLeaders();
                    }
                    else
                    {
                        leaders.Add(other.GetComponent<LeaderScript>());
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<LeaderScript>())
            {
                LeaderScript outOfRangeLeader = other.GetComponent<LeaderScript>();
                if (leaders.Contains(outOfRangeLeader))
                {
                    leaders.Remove(outOfRangeLeader);
                    if (outOfRangeLeader == targetedLeader)
                    {
                        targetedLeader = null;
                    }
                }
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        showDamage.FlashDamage();
        if (health <= 0)
        {
            towerTile.towerController = null;
            towerManager.ATowerDied(towerTile);
            towerManager.towers.Remove(this);
            Destroy(gameObject);
        }
    }
    
    

    #region Calculate Range

    public void CalculateRange()
    {
        if (towerTile != null)
        {
            int totRange = (range + towerTile.height);
            int useRange = Mathf.FloorToInt((totRange / 2));
            Vector3Int nextBase = towerTile.cubeCoordinates + new Vector3Int(0, 1, -1);
            Vector3Int prevBase = towerTile.cubeCoordinates + new Vector3Int(0, -1,1);
            
            if (gridManager.tiles.TryGetValue(nextBase, out TileInfo outTileN))
            {
                maxDistance = Vector3.Distance(outTileN.transform.position, towerTile.transform.position);
            }
            else if (gridManager.tiles.TryGetValue(prevBase, out TileInfo outTileP))
            {
                maxDistance = Vector3.Distance(outTileP.transform.position, towerTile.transform.position);
            }
           
            calRange = Mathf.FloorToInt(maxDistance*useRange);
            Vector3 endPos = new Vector3();
            endPos = transform.position + (calRange * Vector3.forward);
            //Debug.DrawLine(transform.position, endPos, Color.green, 1000f);
            UpdateCollider();
        }
        else
        {
            Debug.Log("No Tile Selected");
        }
    }

    private void UpdateCollider()
    {
        switch (rangeType)
        {
            case RangeType.SingleLine:
                float notRadius = Mathf.RoundToInt(calRange / 2);
                boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y, notRadius);
                boxRecenter = Mathf.RoundToInt(notRadius / 2);
                boxCollider.center = new Vector3(0, 0,0 + boxRecenter);
                FindTilesInRangeBox();
                break;
            case RangeType.Radius:
                sphereCollider.transform.position = transform.position;
                sphereCollider.radius = calRange / 2;
                FindTilesInRangeSphere();
               // ShowRange();
                break;
            case RangeType.None:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void FindTilesInRangeBox()
    {
        Vector3 startPos = transform.TransformPoint(boxCollider.center);
        Collider[] colliders = Physics.OverlapBox(startPos, boxCollider.size, transform.rotation);
        
        foreach (Collider tile in colliders)
        {
            if (tile.GetComponent<TileInfo>() != null)
            {
                if (!tilesInRange.Contains(tile.GetComponent<TileInfo>()))
                {
                    tilesInRange.Add(tile.GetComponent<TileInfo>());
                }
               
            }
            
            else if (tile.GetComponentInParent<TileInfo>() != null)
            {
                if (!tilesInRange.Contains(tile.GetComponentInParent<TileInfo>()))
                {
                    tilesInRange.Add(tile.GetComponentInParent<TileInfo>());
                }
               
            }
            
        }
    }

    public void ClearTilesInRange()
    {
        tilesInRange.Clear();
    }

    private void FindTilesInRangeSphere()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, calRange);
       // OnDrawGizmosSelected();
        foreach (Collider tile in colliders)
        {
            if (tile.GetComponent<TileInfo>() != null)
            {
                if (!tilesInRange.Contains(tile.GetComponent<TileInfo>()))
                {
                    tilesInRange.Add(tile.GetComponent<TileInfo>());
                }
               
            }
            
            else if (tile.GetComponentInParent<TileInfo>() != null)
            {
                if (!tilesInRange.Contains(tile.GetComponentInParent<TileInfo>()))
                {
                    tilesInRange.Add(tile.GetComponentInParent<TileInfo>());
                }
               
            }
            
        }
    }

    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.DrawSphere(towerTile.transform.position, calRange);
    // }

    public void ShowRange()
    {
        
        foreach (TileInfo tile in tilesInRange)
        {
            float dist = new float();
            Vector3 target = new Vector3();
            target = tile.highLightPos;

            tile.highLight.SetActive(false);
            tile.highLight.transform.position = transform.position;
            tile.SetHighlight();
            //tile.highLight.transform.position = Vector3.Lerp(tile.highLight.transform.position, target, timeToRecenter*Time.fixedDeltaTime);
            StartCoroutine(LerpPosition(tile.highLight.transform, target, timeToRecenter));
        }
    }

    IEnumerator LerpPosition(Transform theHighlight, Vector3 tarPos, float duration)
    {
        float courtime = 0;

        while (courtime < duration && theHighlight.gameObject.activeSelf)
        {
            theHighlight.transform.position = Vector3.Lerp( theHighlight.transform.position, tarPos, courtime / duration);
            courtime += Time.fixedDeltaTime;
            yield return null;
        }
        
        theHighlight.transform.position = tarPos;
    }
    

    public void HideRange()
    {
        foreach (TileInfo tile in tilesInRange)
        {
            tile.DimHighlight();
        }
    }

    #endregion

    public Quaternion RotateTower()
    {
        HideRange();
        
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y+angle, 0);
        if (rangeType == RangeType.SingleLine)
        {
            tilesInRange.Clear();
            FindTilesInRangeBox();
            ShowRange();
        }
        return transform.rotation;
    }
}
