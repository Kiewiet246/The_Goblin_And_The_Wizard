using System.Collections.Generic;
using UnityEngine;


public class TileInfo : MonoBehaviour
{
    public Vector3Int cubeCoordinates;
    
    public List<TileInfo> neighborTiles;
    
    [SerializeField]
    private List<Material> materials;

    [SerializeField] private bool isPath = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetDefualtTiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDefualtTiles()
    {
        isPath = false;
       gameObject.GetComponent<MeshRenderer>().material = materials[0]; 
    }

    public void SetPathTile()
    {
        isPath = true;
        gameObject.GetComponent<MeshRenderer>().material = materials[1];
    }
}
