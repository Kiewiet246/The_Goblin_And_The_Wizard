using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TowerController : MonoBehaviour
{
    #region Range Variables
    [Header("Range Values")]
    [SerializeField] private int range;
    [SerializeField] private float calRange;
    public TileInfo towerTile;
    public GridManager gridManager;

    [Header("Show Range")]
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private int steps;
    [SerializeField] private List<TileInfo> tilesInRange;
    
    [Header("Range Types")] [SerializeField]
    private RangeType rangeType;
    
    public enum RangeType
    {
        SingleLine,
        Radius,
        None
    }
    #endregion

    [Header("Rotation Values")]
    [SerializeField] private float angle = 60f;

    public TowerType towerType;
    public enum TowerType
    {
        ArcherTower = 10,
        CanonTower = 15,
        WallTower = 20,
        BalistanTower =  5,
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // CalculateRange();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Calculate Range

    public void CalculateRange()
    {
        Debug.Log("Test");
        if (towerTile != null)
        {
            range = range + towerTile.height;
            Vector3Int towerBase = towerTile.cubeCoordinates;
            Vector3Int nextBase = towerTile.cubeCoordinates + new Vector3Int(0, 1, -1);
          //  Debug.Log(nextBase);
            Vector3Int prevBase = towerTile.cubeCoordinates - new Vector3Int(0, 1,1);
            float distance = new float();
            
            if (gridManager.tiles.TryGetValue(nextBase, out TileInfo outTileN))
            {
               // Debug.Log(outTileN.name);
                distance = Vector3.Distance(outTileN.transform.position, towerTile.transform.position);
            }
            else if (gridManager.tiles.TryGetValue(prevBase, out TileInfo outTileP))
            {
                //Debug.Log(outTileP.name);
                distance = Vector3.Distance(outTileP.transform.position, towerTile.transform.position);
            }

            calRange = Mathf.RoundToInt(distance * range);
            Vector3 endPos = new Vector3();
            endPos = transform.position + (calRange * Vector3.forward);
            //Debug.DrawLine(transform.position, endPos, Color.green, 1000f);
            UpdateCollider();
        }
        else
        {
            Debug.Log("No Tile Selected");
        }
    }

    private void UpdateCollider()
    {
        switch (rangeType)
        {
            case RangeType.SingleLine:
                break;
            case RangeType.Radius:
                sphereCollider.radius = calRange;
                FindTilesInRangeSphere();
               // ShowRange();
                break;
            case RangeType.None:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void FindTilesInRangeSphere()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, calRange);
        foreach (Collider tile in colliders)
        {
            if (tile.GetComponent<TileInfo>() != null)
            {
                tilesInRange.Add(tile.GetComponent<TileInfo>());
            }
            
        }
    }

    public void ShowRange()
    {
        for (int i = 0; i < tilesInRange.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, tilesInRange[i].transform.position);
            dist = Mathf.RoundToInt(dist);
            if (dist <= calRange)
            {
                tilesInRange[i].SetHighlight();
            }
        }
    }

    public void HideRange()
    {
        for (int i = 0; i < tilesInRange.Count; i++)
        {
            tilesInRange[i].DimHighlight();
        }
    }

    #endregion

    public void RotateTower()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y+angle, 0);
        if (rangeType == RangeType.SingleLine)
        {
            tilesInRange.Clear();
            UpdateCollider();
        }
    }
}
