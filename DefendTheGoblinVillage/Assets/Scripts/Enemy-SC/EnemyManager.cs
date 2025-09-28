using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Grid Stuff")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private TileInfo saveStartTile, saveEndTile;

    [Header("Pathfinding Stuff")]
    [SerializeField] private LineRenderer enemyPath;
    [SerializeField] private List<TileInfo> pathTiles;

    [Header("Enemies Stuff")]
    [SerializeField] private List<LeaderScript> enemies;
    
    [Header("Extra")]
    [SerializeField] private float adjustable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPath()
    {
       // Debug.Log("Set path");
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
        
        GiveLeadersPath();
    }

    public void GiveLeadersPath()
    {
        if (enemies != null)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].SetWaypoints(pathTiles);
            }
        }
    }

    public void ClearPath()
    {
        enemyPath.positionCount = 0;
    }
    
    
}
