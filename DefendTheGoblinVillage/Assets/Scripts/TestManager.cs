using UnityEngine;
using UnityEngine.Rendering;

public class TestManager : MonoBehaviour
{
    public GridManager gridManager;

    public EnemyManager enemyManager;

    public Vector3Int targetTower;

    public SetDestsScript setDestsScript;
    
    public TowerController towerController;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            gridManager.ClearGrid();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            gridManager.CreateGrid();
            setDestsScript.SetDestinations();
            enemyManager.ClearPath();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            gridManager.tiles.TryGetValue(targetTower, out TileInfo tile);
            TowerTesting(tile);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            setDestsScript.JustToCallStart();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            //gridManager.RandomLocations();
            enemyManager.SetPath();
            enemyManager.IncreaseWave();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            enemyManager.IncreaseWave();
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            setDestsScript.SetDestinations();
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            towerController.CalculateRange();
        }
    }



    public void TowerTesting(TileInfo tileInfo)
    {
        tileInfo.CallTower();
    }
}
