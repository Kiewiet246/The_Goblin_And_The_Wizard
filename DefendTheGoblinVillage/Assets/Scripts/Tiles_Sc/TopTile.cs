using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TopTile : MonoBehaviour
{
    [SerializeField] private GameObject grassTerrain, forestTerrain, swampTerrain, rockTerrain;

    [SerializeField] private GameObject currentTerrain;

    public void SetTerrainObjects(TileInfo.TerrainType terrainType)
    {
        int rotateBy = Random.Range(0, 7) * 60;
        transform.rotation = Quaternion.Euler(transform.parent.transform.eulerAngles.x, transform.parent.transform.eulerAngles.y ,transform.eulerAngles.z+ rotateBy);
       // transform.rotation = Quaternion.Euler(transform.parent.transform.eulerAngles.x, transform.parent.transform.eulerAngles.y ,transform.eulerAngles.z+ rotateBy);
        switch (terrainType)
        {
            case TileInfo.TerrainType.Normal:
                grassTerrain.SetActive(true);
                currentTerrain = grassTerrain;
                break;
            case TileInfo.TerrainType.Muddy:
              currentTerrain = grassTerrain;
                swampTerrain.SetActive(true);
                break;
            case TileInfo.TerrainType.Forest:
                currentTerrain = forestTerrain;
                forestTerrain.SetActive(true);
                break;
            case TileInfo.TerrainType.Stone:
                currentTerrain = swampTerrain;
                rockTerrain.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(terrainType), terrainType, null);
        }
        
    }

    public void SwitchTerrainOff()
    {
        currentTerrain.SetActive(false);
    }
}
