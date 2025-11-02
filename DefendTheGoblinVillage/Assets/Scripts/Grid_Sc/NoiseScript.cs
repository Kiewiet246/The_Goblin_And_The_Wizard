using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;


public class NoiseScript : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    
    [SerializeField] private float width, height; //size of grid
    [SerializeField] private float scaleH = 1, scaleMaxH, scaleT = 1, scaleMaxT = 1f; //scale the perlin-noise
    [SerializeField] private float heightOffset, terrainOffset; //Randomises each generation on the perlin map
    
    [SerializeField] [Range(0,1)] private float lOneTh, lTwoTh, lThreeTh, lFourTh, lFiveTh;
    
    [SerializeField] [Range(0,1)] private float stoneTh, forrestTh, muddyTh;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void UpdateValues()
    {
        heightOffset = Random.Range(0, 1000000);
        terrainOffset = Random.Range(0, 1000000);

         scaleH = Random.Range(2, scaleMaxH);
         scaleT = Random.Range(2, scaleMaxT);
         //Debug.Log("H: " + scaleH);
        // Debug.Log("T: " + scaleT);
        
        width = gridManager.gridSize.x;
        height = gridManager.gridSize.y;
    }

    public void GenerateTerrain(int x, int y, TileInfo tile)
    {
        float coordX = (tile.cubeCoordinates.x + terrainOffset) / width * scaleT;
        float coordY = (tile.cubeCoordinates.y + terrainOffset) / width * scaleT;

        float noiseValue = Mathf.PerlinNoise(coordX, coordY);
        tile.noiseNumT = noiseValue;
        tile.terrainType = TileInfo.TerrainType.Normal;
        

        
        if (noiseValue <= stoneTh)
        {
            tile.terrainType = TileInfo.TerrainType.Stone;
        }
        else if (noiseValue <= forrestTh)
        {
            tile.terrainType = TileInfo.TerrainType.Forest;
        }
        
        else if (noiseValue <= muddyTh)
        {
            tile.terrainType = TileInfo.TerrainType.Muddy;
        }
    }

    public void GenerateHeight(int x, int y, TileInfo tile)
    {
        float coordX = (tile.cubeCoordinates.x + heightOffset) / width * scaleH;
        float coordY = (tile.cubeCoordinates.y + heightOffset) / width * scaleH;
        
        float noiseValue = Mathf.PerlinNoise(coordX, coordY);
        tile.noiseNumH = noiseValue;

        tile.height = 1;

        if (noiseValue <= lFiveTh)
        {
            tile.height = 5;
        }
        else if (noiseValue <= lFourTh)
        {
            tile.height = 4;
        }
        else if (noiseValue <= lThreeTh)
        {
            tile.height = 3;
        }
        else if (noiseValue <= lTwoTh)
        {
            tile.height = 2;
        }
        else
        {
            tile.height = 1;
        }
        
        tile.CreateTilesAbove();
        tile.SetTerrain();
    }
    
    
}
