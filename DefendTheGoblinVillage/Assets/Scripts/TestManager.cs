using UnityEngine;
using UnityEngine.Rendering;

public class TestManager : MonoBehaviour
{
    public GridManager gridManager;

    public EnemyManager enemyManager;

    public Vector3Int targetTower;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            gridManager.ClearGrid();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            gridManager.CreateGrid();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            gridManager.tiles.TryGetValue(targetTower, out TileInfo tile);
            TowerTesting(tile);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            gridManager.RandomLocations();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            gridManager.RandomLocations();
            enemyManager.SetPath();
        }
    }



    public void TowerTesting(TileInfo tileInfo)
    {
        tileInfo.CallTower();
    }
}
