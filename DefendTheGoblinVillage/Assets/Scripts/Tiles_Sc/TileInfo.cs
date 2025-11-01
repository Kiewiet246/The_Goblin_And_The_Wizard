using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class TileInfo : MonoBehaviour
{
    [FormerlySerializedAs("noiseNum")] public float noiseNumT, noiseNumH;
    [Header("Grid info")]
    public Vector3Int cubeCoordinates;
    public List<TileInfo> neighborTiles;
    public int costValue = 0;
    [SerializeField] [Range(0, 5)] private int heightRange;
    public int height;
    public List<GameObject> tilesOnTOp;
    
    [SerializeField] private int stepRange = 1;
    [SerializeField]
    private List<Material> materials;
    [SerializeField] private bool isPath = false;
    
    [Header("Terrain")]
    [SerializeField] private bool randomTerrain = false;
    public TerrainType terrainType;
    [SerializeField] private MeshRenderer terrainMesh;
    
    [Header("TIles on top")]
    [FormerlySerializedAs("Tile")] [SerializeField] private GameObject inbetweenTiles;
     [FormerlySerializedAs("topTileG")] [SerializeField] private GameObject topTilePrefab;
     public Transform topTileTransform;
     [SerializeField] private TopTile topTile;
    public float adjustHeight = 1f;
    public enum TerrainType
    {
        Normal = 5,
        Muddy = 30,
        Forest = 10,
        Stone = 90,
        Start = 0,
        End = 1
    }

    [Header("Structure")] public int structureWeight;
    public GameObject structure;
    
    [Header("HighLight")]
    public GameObject highLight;
    public Vector3 highLightPos;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        costValue = 0;
   
    }

    public void CreateTilesAbove()
    {
        RemoveTile();
        for (int j = 1; j <= height; j++)
        {
            if (j == height)
            {
                GameObject tileAdded = Instantiate(topTilePrefab, new Vector3(transform.position.x,transform.position.y + (adjustHeight*j*(transform.localScale.y/100f)), transform.position.z), transform.rotation, transform);
                tilesOnTOp.Add(tileAdded);
                topTile = tileAdded.GetComponent<TopTile>();
                topTileTransform = tileAdded.transform;
            }

            else
            {
                GameObject tileAdded = Instantiate(inbetweenTiles, new Vector3(transform.position.x,transform.position.y + (adjustHeight*j*(transform.localScale.y/100f)), transform.position.z), transform.rotation, transform);
                tilesOnTOp.Add(tileAdded);
            }
           
        }
        
    }

    public void RemoveTile()
    {
        if (tilesOnTOp.Count > 0)
        {
            GameObject tileToRemove = tilesOnTOp[tilesOnTOp.Count - 1];
            tilesOnTOp.Remove(tileToRemove);
            Destroy(tileToRemove);
        }
    }

    public void AddTile()
    {
        if (tilesOnTOp.Count < heightRange)
        {
            GameObject tileAdded =  Instantiate(inbetweenTiles, new Vector3(transform.position.x,transform.position.y + (adjustHeight*(tilesOnTOp.Count+1)), transform.position.z), transform.rotation, transform);
            tilesOnTOp.Add(tileAdded);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTerrain()
    {
        topTile.SetTerrainObjects(terrainType);
        switch (terrainType)
        {
            case TerrainType.Normal:
               
            SetDefualtTiles();
                break;
            case TerrainType.Muddy:
                
                SetMuddyTiles();
                break;
            case TerrainType.Forest:
                
                SetForestTiles();
                break;
            case TerrainType.Stone:
                
                SetStoneTiles();
                break;
            
        }
    }

    public void SetDefualtTiles()
    {
        isPath = false;
        terrainMesh.material = materials[0]; 
       ChildrenMaterial(materials[0]);
    }

    private void SetMuddyTiles()
    {
        
        terrainMesh.material = materials[2];
        ChildrenMaterial(materials[2]);
    }

    private void SetForestTiles()
    {
        
        terrainMesh.material = materials[3];
        ChildrenMaterial(materials[3]);
    }

    private void SetStoneTiles()
    {
        
        terrainMesh.material = materials[4];
        ChildrenMaterial(materials[4]);
    }

    public void SetPathTile()
    {
       // isPath = true;
        terrainMesh.material = materials[1];
        ChildrenMaterial(materials[1]);
    }

    private void ChildrenMaterial(Material mat)
    {
        if (transform.childCount > 0)
        {
            foreach (GameObject child in tilesOnTOp )
            {
                child.gameObject.GetComponent<MeshRenderer>().material = mat;
            }
        }
    }

    public int GetCostValue()
    {
        costValue = (int)terrainType + structureWeight + tilesOnTOp.Count;
        return costValue;
    }

    public bool canStep(int ownHeight, int neighborHeight)
    {
        int heightDiff = Math.Abs(neighborHeight - ownHeight);
        if (heightDiff <= stepRange)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    public void ChangeTheTerrain()
    {
        switch (terrainType)
        {
            case TerrainType.Normal:
                terrainType = TerrainType.Muddy;
                break;
            case TerrainType.Muddy:
                terrainType = TerrainType.Forest;
                break;
            case TerrainType.Forest:
                terrainType = TerrainType.Stone;
                break;
            case TerrainType.Stone:
                terrainType = TerrainType.Normal;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void StartTile()
    {
        isPath = true;
        terrainType = TerrainType.Start;
        topTile.SwitchTerrainOff();
        gameObject.GetComponent<MeshRenderer>().material = materials[5];
        ChildrenMaterial(materials[5]);
    }

    public void StopTile()
    {
        terrainType = TerrainType.End;
        topTile.SwitchTerrainOff();
        isPath = true;
        gameObject.GetComponent<MeshRenderer>().material = materials[6];
        ChildrenMaterial(materials[6]);
    }

    public void SetHighLightPosition()
    {
        if (tilesOnTOp.Count > 0)
        {
          highLightPos = highLight.transform.position = new Vector3(transform.position.x, transform.position.y + tilesOnTOp.Last().transform.position.y+ 0.5f, transform.position.z);
        }

        else
        {
            highLightPos = highLight.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        }
    }

    public void SetHighlight()
    {
        highLight.SetActive(true);
    }

    public void DimHighlight()
    {
        highLight.SetActive(false);
       // highLight.transform.position = highLightPos;
    }
}
