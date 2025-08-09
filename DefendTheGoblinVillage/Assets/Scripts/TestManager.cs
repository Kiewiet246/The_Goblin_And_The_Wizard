using UnityEngine;
using UnityEngine.Rendering;

public class TestManager : MonoBehaviour
{
    public GridManager gridManager;


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
    }



    public void TowerTesting(TileInfo tileInfo)
    {
        tileInfo.CallTower();
    }
}
