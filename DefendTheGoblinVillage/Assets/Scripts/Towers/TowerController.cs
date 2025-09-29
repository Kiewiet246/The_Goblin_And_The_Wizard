using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TowerController : MonoBehaviour
{
    #region Range Variables
    [Header("Range Values")]
    [SerializeField] private int range;
    [SerializeField] private float calRange;
    public TileInfo towerTile;
    public GridManager gridManager;
    public TowerManager towerManager;

    [Header("Show Range")]
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private float boxRecenter;
    [SerializeField] private List<TileInfo> tilesInRange;
    
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
    #endregion
    
    [Header("Health Values")]
    public int health;
    
    [Header("Attack Variables")]
    [SerializeField] private List<LeaderScript> leaders;
    public int damage = 1;
    [SerializeField] private float attackRate = 1;
    [SerializeField] private float currentTime;
    [SerializeField] private bool canShoot = true;

    [SerializeField] private float projectileSpeed = 20f;
    public List<Rigidbody> projectiles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // CalculateRange();
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
                AttackTheLeaders();
            }
        }
    }

    private void AttackTheLeaders()
    {
        if (leaders[0].gameObject.activeSelf == false)
        {
            leaders.RemoveAt(0);
        }
        currentTime = Time.time;
        Vector3 direction = (leaders[0].endPoint - transform.position).normalized;
        Rigidbody rb = projectiles[0];
        
        rb.gameObject.SetActive(true);
        rb.gameObject.GetComponent<Projectile>().StartProjectile();
        Vector3 pos  = transform.position;
        rb.position = pos;
        rb.AddForce(direction * projectileSpeed, ForceMode.Impulse);
        projectiles.Remove(rb);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<LeaderScript>())
            {
                leaders.Add(other.GetComponent<LeaderScript>());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<LeaderScript>())
            {
                leaders.Remove(other.GetComponent<LeaderScript>());
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            towerTile.structureWeight = 0;
            towerTile.structure = null;
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
            range = range + towerTile.height;
            Vector3Int towerBase = towerTile.cubeCoordinates;
            Vector3Int nextBase = towerTile.cubeCoordinates + new Vector3Int(0, 1, -1);
          //  Debug.Log(nextBase);
            Vector3Int prevBase = towerTile.cubeCoordinates - new Vector3Int(0, 1,1);
            float distance = new float();
            
            if (gridManager.tiles.TryGetValue(nextBase, out TileInfo outTileN))
            {
               // Debug.Log(outTileN.name);
                distance = Vector3.Distance(outTileN.transform.position, towerTile.transform.position);
            }
            else if (gridManager.tiles.TryGetValue(prevBase, out TileInfo outTileP))
            {
                //Debug.Log(outTileP.name);
                distance = Vector3.Distance(outTileP.transform.position, towerTile.transform.position);
            }

            calRange = Mathf.RoundToInt(distance * range);
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
                boxCollider.center = new Vector3(boxCollider.center.x, boxCollider.center.y,boxCollider.center.z + boxRecenter);
                FindTilesInRangeBox();
                break;
            case RangeType.Radius:
                sphereCollider.radius = calRange/2;
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
                tilesInRange.Add(tile.GetComponent<TileInfo>());
            }
            
        }
    }

    private void FindTilesInRangeSphere()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, calRange);
        foreach (Collider tile in colliders)
        {
            if (tile.GetComponent<TileInfo>() != null)
            {
                tilesInRange.Add(tile.GetComponent<TileInfo>());
            }
            
        }
    }

    public void ShowRange()
    {
        for (int i = 0; i < tilesInRange.Count; i++)
        {
            float dist = Vector3.Distance(towerTile.transform.position, tilesInRange[i].transform.position);
            dist = Mathf.RoundToInt(dist);
            Debug.Log(dist + " : " + tilesInRange[i].name);
            if (dist <= calRange)
            {
                tilesInRange[i].SetHighlight();
            }
        }
    }

    public void HideRange()
    {
        for (int i = 0; i < tilesInRange.Count; i++)
        {
            tilesInRange[i].DimHighlight();
        }
    }

    #endregion

    public void RotateTower()
    {
        HideRange();
        
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y+angle, 0);
        if (rangeType == RangeType.SingleLine)
        {
            tilesInRange.Clear();
            FindTilesInRangeBox();
            ShowRange();
        }
    }
}
