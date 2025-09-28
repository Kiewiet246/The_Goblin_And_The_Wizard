using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private List<TileInfo> pathTiles;
    
    [SerializeField] private TileInfo saveStartTile, saveEndTile;

    [SerializeField] private LineRenderer enemyPath;

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
       // Debug.Log(count);

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

    public void ClearPath()
    {
        enemyPath.positionCount = 0;
    }


    public void AdjustPath()
    {
        
    }
}
