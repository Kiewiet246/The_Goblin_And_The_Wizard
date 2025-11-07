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
    [SerializeField] private LineRenderer futurePath;
    [SerializeField] private List<TileInfo> pathTiles;

    [Header("Creating Enemies")] [SerializeField]
    private int totalEnemyPool = 30;
    [SerializeField] private List<LeaderScript> enemiesInPool;
    [SerializeField] private Transform storeEnemiesParent;
    [SerializeField] private GameObject leaderPrefab;
    [SerializeField] private float spawnForce = 100f;
    [SerializeField] private float forceUp;
    [SerializeField] private float setHealth = 10f;
    
    [Header("Controlling Enemies")]
    public List<LeaderScript> enemiesInField;
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
    [SerializeField] private SpellManager spellManager;

    [SerializeField] private Transform activateField;
    [SerializeField] private float adjustActFieldHeight;
    void Awake()
    {
        CreateLeaders();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 spawnPosition = new Vector3(saveStartTile.transform.position.x, saveStartTile.transform.position.y + (adjustActFieldHeight*saveStartTile.height), saveStartTile.transform.position.z);
        activateField.position = spawnPosition;
        forceUp = spawnForce * saveStartTile.height;
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

    public void CheckifPathWillChange(TileInfo newTileInfo)
    {
        if (pathTiles.Contains(newTileInfo))
        {
            CalculateFuturePath();
        }
    }

    public void CalculateFuturePath()
    {
        
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
      // canSpawn = true;
    }

    public void CheckIfPathHasChanged(TileInfo checkTile)
    {
        if (pathTiles.Contains(checkTile))
        {
            AdjustPath();
        }
    }

    public void AdjustPath()
    {
        pathTiles.Clear();
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

    public void GiveLeadersPath(LeaderScript leader)
    {
       leader.SetWaypoints(pathTiles);
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
            leaderScript.SetEnemyMan(this);
            leaderScript.enemyCollider.enabled = false;
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
            LeadersIsSpawnde(leader);
        }
        else
        {
            GameObject leader = Instantiate(leaderPrefab, fieldEnemiesParent);
            LeaderScript leaderScript = leader.GetComponent<LeaderScript>();
            leaderScript.SetEnemyMan(this);
            enemiesInField.Add(leaderScript);
            LeadersIsSpawnde(leaderScript);
            
        }

        if (enemiesSpawnedInWave == totalEnemiesInWave)
        {
            canSpawn = false;
            enemiesSpawnedInWave = 0;
        }
    }

    private void LeadersIsSpawnde(LeaderScript leader)
    {
        Vector3 placePos = new Vector3(saveStartTile.transform.position.x, saveStartTile.transform.position.y+ saveStartTile.height+adjustable, saveStartTile.transform.position.z);
        leader.transform.position = placePos;
        leader.enemyCollider.enabled = false;
        leader.rb.linearVelocity = Vector3.zero;
        leader.rb.AddForce(Vector3.up*(forceUp), ForceMode.Impulse);
        leader.transform.parent = fieldEnemiesParent;
        leader.health = setHealth;
        leader.spellManager = spellManager;
        GiveLeadersPath(leader);
    }

    public void RemoveLeaderFromField(LeaderScript leader)
    {
        enemiesInField.Remove(leader);
        enemiesInPool.Add(leader);
        leader.transform.parent = storeEnemiesParent;
        leader.enemyCollider.enabled = false;
        leader.rb.linearVelocity = Vector3.zero;
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
