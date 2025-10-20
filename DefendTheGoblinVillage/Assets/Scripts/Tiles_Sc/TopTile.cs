using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TopTile : MonoBehaviour
{
    [SerializeField] private GameObject grassTerrain, forestTerrain, swampTerrain, rockTerrain;


    public void SetTerrainObjects(TileInfo.TerrainType terrainType)
    {
        int rotateBy = Random.Range(0, 7) * 60;
        
       // transform.rotation = Quaternion.Euler(transform.parent.transform.eulerAngles.x, transform.parent.transform.eulerAngles.y ,transform.eulerAngles.z+ rotateBy);
        switch (terrainType)
        {
            case TileInfo.TerrainType.Normal:
                grassTerrain.SetActive(true);
                break;
            case TileInfo.TerrainType.Muddy:
                break;
            case TileInfo.TerrainType.Forest:
                break;
            case TileInfo.TerrainType.Stone:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(terrainType), terrainType, null);
        }
        
    }
}
