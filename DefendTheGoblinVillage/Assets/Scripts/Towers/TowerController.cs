using System;
using UnityEngine;
using UnityEngine.Serialization;

public class TowerController : MonoBehaviour
{
    [Header("Range Values")]
    [SerializeField] private int range;
    [SerializeField] private float calRange;
    [SerializeField] private TileInfo towerTile;
    [SerializeField] private GridManager gridManager;

    [Header("Show Range")]
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private int steps;
    
    [Header("Range Types")] [SerializeField]
    private RangeType rangeType;
    
    public enum RangeType
    {
        SingleLine,
        Radius
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
            Debug.Log(nextBase);
            Vector3Int prevBase = towerTile.cubeCoordinates - new Vector3Int(0, 1,1);
            float distance = new float();
            
            if (gridManager.tiles.TryGetValue(nextBase, out TileInfo outTileN))
            {
                Debug.Log(outTileN.name);
                distance = Vector3.Distance(outTileN.transform.position, towerTile.transform.position);
            }
            else if (gridManager.tiles.TryGetValue(prevBase, out TileInfo outTileP))
            {
                Debug.Log(outTileP.name);
                distance = Vector3.Distance(outTileP.transform.position, towerTile.transform.position);
            }

            calRange = Mathf.RoundToInt(distance * range);
            Vector3 endPos = new Vector3();
            endPos = transform.position + (calRange * Vector3.forward);
            Debug.DrawLine(transform.position, endPos, Color.green, 1000f);
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
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion
}
