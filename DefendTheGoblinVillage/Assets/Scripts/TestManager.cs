using UnityEngine;
using UnityEngine.Rendering;

public class TestManager : MonoBehaviour
{
    public GridManager gridManager;

    public EnemyManager enemyManager;

    public Vector3Int targetTower;

    public SetDestsScript setDestsScript;
    
    public TowerController towerController;
    
    public TowerManager towerManager;
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

        if (Input.GetKeyDown(KeyCode.H))
        {
            towerController.HideRange();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            towerController.ShowRange();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            towerController.RotateTower();
        }
    }



    public void TowerTesting(TileInfo tileInfo)
    {
        towerManager.CreateTower(tileInfo);
    }
}
