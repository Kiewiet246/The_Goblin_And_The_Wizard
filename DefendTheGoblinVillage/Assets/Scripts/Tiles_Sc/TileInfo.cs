using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class TileInfo : MonoBehaviour
{
    [Header("Grid info")]
    public Vector3Int cubeCoordinates;
    public List<TileInfo> neighborTiles;
    public int costValue = 0;
    [SerializeField] [Range(0, 5)] private int heightRange;
    public int height;
    
    [SerializeField] private int stepRange = 1;
    [SerializeField]
    private List<Material> materials;
    [SerializeField] private bool isPath = false;
    
    [Header("Terrain")]
    public TerrainType terrainType;

    [SerializeField] private GameObject Tile;
    [SerializeField] private float adjustHeight = 1f;
    public enum TerrainType
    {
        Normal = 5,
        Muddy = 10,
        Forest = 15,
        Stone = 1000
    }

    [Header("Structure")]
    public StructureType structureType;
    public enum StructureType
    {
        normal = 0,
        normalTower = 10,
        wall = 20
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        costValue = 0;
        SetDefualtTiles();
        TileIsBorn();
        structureType = StructureType.normal;
    }

    private void TileIsBorn()
    {
        height = Random.Range(0, heightRange);
        CreateTilesAbove(height);

        int random = Random.Range(0, 101);

        if (random <= 40)
        {
            terrainType = TerrainType.Normal;
        }
        
        else if (random <= 60)
        {
            terrainType = TerrainType.Muddy;
        }
        else if (random <= 80)
        {
            terrainType = TerrainType.Forest;
        }
        
        else if (random <= 100)
        {
            terrainType = TerrainType.Stone;
        }
    }

    private void CreateTilesAbove(int i)
    {
        for (int j = 1; j <= height; j++)
        {
            Instantiate(Tile, new Vector3(transform.position.x,transform.position.y + (adjustHeight*j), transform.position.z), transform.rotation, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPath)
        {
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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public void SetDefualtTiles()
    {
       // isPath = false;
       gameObject.GetComponent<MeshRenderer>().material = materials[0]; 
       ChildrenMaterial(materials[0]);
    }

    private void SetMuddyTiles()
    {
        gameObject.GetComponent<MeshRenderer>().material = materials[2];
        ChildrenMaterial(materials[2]);
    }

    private void SetForestTiles()
    {
        gameObject.GetComponent<MeshRenderer>().material = materials[3];
        ChildrenMaterial(materials[3]);
    }

    private void SetStoneTiles()
    {
        gameObject.GetComponent<MeshRenderer>().material = materials[4];
        ChildrenMaterial(materials[4]);
    }

    public void SetPathTile()
    {
        isPath = true;
        gameObject.GetComponent<MeshRenderer>().material = materials[1];
        ChildrenMaterial(materials[1]);
    }

    private void ChildrenMaterial(Material mat)
    {
        if (transform.childCount > 0)
        {
            foreach (Transform child in transform )
            {
                child.gameObject.GetComponent<MeshRenderer>().material = mat;
            }
        }
    }

    public int GetCostValue()
    {
        costValue += (int)terrainType + (int)structureType + height;
        return costValue;
    }

    public bool canStep(int ownHeight, int neighborHeight)
    {
        int heightDiff = neighborHeight - ownHeight;
        if (heightDiff <= stepRange)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
}
