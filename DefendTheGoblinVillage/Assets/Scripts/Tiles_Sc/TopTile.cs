using System;
using UnityEngine;

public class TopTile : MonoBehaviour
{
    [SerializeField] private GameObject grassTerrain, forestTerrain, swampTerrain, rockTerrain;


    public void SetTerrainObjects(TileInfo.TerrainType terrainType)
    {
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
