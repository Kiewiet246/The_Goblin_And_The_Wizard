using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyManager : MonoBehaviour
{
    [Header("Grid Stuff")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private TileInfo saveStartTile, saveEndTile;

    [Header("Pathfinding Stuff")]
    [SerializeField] private LineRenderer enemyPath;
    [SerializeField] private List<TileInfo> pathTiles;

    [Header("Creating Enemies")] [SerializeField]
    private int totalEnemyPool = 30;
    [SerializeField] private List<LeaderScript> enemiesInPool;
    [SerializeField] private Transform storeEnemiesParent;
    [SerializeField] private GameObject leaderPrefab;
    
    [Header("Controlling Enemies")]
    [SerializeField] private List<LeaderScript> enemiesInField;
    [SerializeField] private Transform fieldEnemiesParent;
    [SerializeField] private int adjustLeaderDiff; 
    

    [Header(("WaveValues"))] [SerializeField]
    private int totalEnemiesInWave;
    [SerializeField] private int enemiesSpawnedInWave = 0;
    [SerializeField] private int currentWave = 0;
    [SerializeField] private int totalWaves;
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private float currentTime;
    [SerializeField] private bool canSpawn = false;
    
    [Header("Extra")]
    [SerializeField] private float adjustable;

    void Awake()
    {
        CreateLeaders();
    }
    
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
        if (canSpawn)
        {
            Countdown();
        }
    }

    #region Pathfinding
    public void SetPath()
    {
       saveStartTile = gridManager.startTile;
       saveEndTile = gridManager.endTile;
        Queue<TileInfo> createPath = gridManager.FindPath(saveStartTile, saveEndTile);
        int count = createPath.Count;
        enemyPath.positionCount = count;
       
        for (int i = 0; i < count; i++)
        {
            TileInfo pathTile = createPath.Dequeue();
            pathTiles.Add(pathTile);
            Vector3 pathPos = new Vector3();
            if (pathTile.tilesOnTOp.Count > 0)
            {
              pathPos = pathTile.tilesOnTOp.Last().transform.position;
            }
            else
            {
                pathPos = pathTile.transform.position;
            }
            
            pathPos = new Vector3(pathPos.x, pathPos.y + adjustable, pathPos.z);
            enemyPath.SetPosition(i, pathPos);
        }
       // GiveLeadersPath();
       canSpawn = true;
    }

    public void AdjustPath()
    {
        Queue<TileInfo> createPath = gridManager.FindPath(saveStartTile, saveEndTile);
        int count = createPath.Count;
        enemyPath.positionCount = count;
       
        for (int i = 0; i < count; i++)
        {
            TileInfo pathTile = createPath.Dequeue();
            pathTiles.Add(pathTile);
            Vector3 pathPos = new Vector3();
            if (pathTile.tilesOnTOp.Count > 0)
            {
                pathPos = pathTile.tilesOnTOp.Last().transform.position;
            }
            else
            {
                pathPos = pathTile.transform.position;
            }
            
            pathPos = new Vector3(pathPos.x, pathPos.y + adjustable, pathPos.z);
            enemyPath.SetPosition(i, pathPos);
        }
    }

    public void GiveLeadersPath()
    {
        if (enemiesInField != null)
        {
            for (int i = 0; i < enemiesInField.Count; i++)
            {
                enemiesInField[i].SetWaypoints(pathTiles);
            }
        }
    }

    public void ClearPath()
    {
        enemyPath.positionCount = 0;
    }
    
    #endregion

    #region Leader Controls

    public void CreateLeaders()
    {
        for (int i = 0; i < totalEnemyPool; i++)
        {
            GameObject leader = Instantiate(leaderPrefab, storeEnemiesParent);
            LeaderScript leaderScript = leader.GetComponent<LeaderScript>();
            enemiesInPool.Add(leaderScript);
            leaderScript.SetEemyMan(this);
            leader.SetActive(false);
        }
    }

    public void AddLeaderToField()
    {
        currentTime = Time.time;
        enemiesSpawnedInWave += 1;
        if (enemiesInPool.Count > 0)
        {
            LeaderScript leader = enemiesInPool[0];
            enemiesInPool.RemoveAt(0);
            enemiesInField.Add(leader);
            leader.gameObject.SetActive(true);
            Vector3 placePos = new Vector3(saveStartTile.transform.position.x, saveStartTile.transform.position.y+ saveStartTile.height+adjustable, saveStartTile.transform.position.z);
            leader.transform.position = placePos;
            leader.transform.parent = fieldEnemiesParent;
            leader.health = 6;
            SetLeaderDifficulty(leader);
            GiveLeadersPath();
        }

        if (enemiesSpawnedInWave == totalEnemiesInWave)
        {
            canSpawn = false;
            enemiesSpawnedInWave = 0;
        }
    }

    public void RemoveLeaderFromField(LeaderScript leader)
    {
        enemiesInField.Remove(leader);
        enemiesInPool.Add(leader);
        leader.transform.parent = storeEnemiesParent;
        leader.gameObject.SetActive(false);
    }
    
    public void SetLeaderDifficulty(LeaderScript leader)
    {
        leader.SetDifficulty(adjustLeaderDiff);
    }
    #endregion
    
    #region Wave Controls

    public void Countdown()
    {
        float difference = Time.time - currentTime;
        if (difference >= spawnRate)
        {
            AddLeaderToField();
        }
    }

    public void IncreaseWave()
    {
        currentWave += 1;
        canSpawn = true;
        currentTime = Time.time;
        totalEnemiesInWave += 5;
    }
    #endregion
}
