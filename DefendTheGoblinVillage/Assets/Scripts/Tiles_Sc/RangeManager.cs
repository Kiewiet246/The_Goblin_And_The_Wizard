using System;
using System.Collections.Generic;
using UnityEngine;

public class RangeManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private TileInfo tileInfo;

    [SerializeField] private List<TileInfo> tileInfos;
    [SerializeField] private int range = 1;
    [SerializeField] private RangeType rangeType;
    public enum RangeType
    {
        Circle,
        Cone,
        ConeExtended,
        Single,
        Double,
        Star
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gridManager == null)
        {
            gridManager = transform.root.gameObject.GetComponent<GridManager>();
        }

        if (tileInfo == null)
        {
            tileInfo = transform.parent.gameObject.GetComponent<TileInfo>();
        }
        CalculateRange();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void CalculateRange()
    {
        if (tileInfo != null)
        {
            HideRange();
        }
        
        switch (rangeType)
        {
            case RangeType.Circle:
                CalculateCircleRange();
                break;
            case RangeType.Cone:
                CalculateConeRange();
                break;
            case RangeType.ConeExtended:
                break;
            case RangeType.Single:
                break;
            case RangeType.Double:
                break;
            case RangeType.Star:
                CalculateStarRange();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        ShowRange();
        
    }

    private void CalculateConeRange()
    {
        for (int x = 0; x <= range; x++)
        {
            for (int y = 0; y <= range; y++)
            {
                for (int z = 0; z <= range; z++)
                {
                    Vector3Int origin = tileInfo.cubeCoordinates;
                    Vector3Int firstDestination = origin + new Vector3Int(x, y, z);
                    Vector3Int secondDestination = origin + new Vector3Int(-x, y, z);

                    Vector3Int thirdDestination = origin + new Vector3Int(-x, -y, z);

                    if ((gridManager.tiles.TryGetValue(firstDestination, out TileInfo outTile)))
                    {
                        if (!tileInfos.Contains(outTile))
                        {
                            Debug.Log(outTile.name);
                            tileInfos.Add(outTile);
                        }
                    }

                    if (gridManager.tiles.TryGetValue(secondDestination, out TileInfo outTile2))
                    {
                        if (!tileInfos.Contains(outTile))
                        {
                            tileInfos.Add(outTile2);
                        }
                    }

                    if (gridManager.tiles.TryGetValue(thirdDestination, out TileInfo outTile3))
                    {
                        if (!tileInfos.Contains(outTile3))
                        {
                            tileInfos.Add(outTile3);
                        }
                    }
                }
            }

        }
    }

    private void CalculateCircleRange()
    {
        for (int x = 0; x <= range; x++)
        {
            for (int y = 0; y <= range; y++)
            {
                for (int z = 0; z <= range; z++)
                {
                    Vector3Int origin = tileInfo.cubeCoordinates;
                    Vector3Int firstDestination = origin + new Vector3Int(x, y, z);
                    Vector3Int secondDestination = origin + new Vector3Int(-x, y, z);
               
                    Vector3Int thirdDestination = origin + new Vector3Int(-x, -y, z);
                    Vector3Int fourthDestination = origin + new Vector3Int(-x, -y, -z);
                
                    Vector3Int fifthDestination = origin + new Vector3Int(x, -y, -z);
                    Vector3Int sixthDestination = origin + new Vector3Int(x, y, -z);
                    
                    if ((gridManager.tiles.TryGetValue(firstDestination, out TileInfo outTile)))
                    {
                        if (!tileInfos.Contains(outTile))
                        {
                            Debug.Log(outTile.name);
                            tileInfos.Add(outTile);
                        }
                    }
                    if (gridManager.tiles.TryGetValue(secondDestination, out TileInfo outTile2))
                    {
                        if (!tileInfos.Contains(outTile))
                        {
                            tileInfos.Add(outTile2);
                        }
                    }

                    if (gridManager.tiles.TryGetValue(thirdDestination, out TileInfo outTile3))
                    {
                        if (!tileInfos.Contains(outTile3))
                        {
                            tileInfos.Add(outTile3);
                        }
                    }

                    // if (gridManager.tiles.TryGetValue(fourthDestination, out TileInfo outTile4))
                    // {
                    //     if (!tileInfos.Contains(outTile4))
                    //     {
                    //         tileInfos.Add(outTile4);
                    //     }
                    // }
                    //
                    // if (gridManager.tiles.TryGetValue(fifthDestination, out TileInfo outTile5))
                    // {
                    //     if (!tileInfos.Contains(outTile5))
                    //     {
                    //         tileInfos.Add(outTile5);
                    //     }
                    // }
                    //
                    // if (gridManager.tiles.TryGetValue(sixthDestination, out TileInfo outTile6))
                    // {
                    //     if (!tileInfos.Contains(outTile6))
                    //     {
                    //         tileInfos.Add(outTile6);
                    //     }
                    // }
                }
            }
        }
    }

    private void CalculateConeRangeExtended()
    {
        
    }

    private void CalculateSingleRange()
    {
        
    }

    private void CalculateDoubleRange()
    {
        
    }
    
    private void CalculateStarRange()
    {
        Debug.Log("Calculate Circle Range");
        for (int x = 0; x <= range; x++)
        {
            for (int y = 0; y <= range; y++)
            {
                Vector3Int origin = tileInfo.cubeCoordinates;
                Vector3Int firstDestination = origin + new Vector3Int(x, -y, -x-y);
                Vector3Int secondDestination = origin + new Vector3Int(-x, y, -x-y);
               
                Vector3Int thirdDestination = origin + new Vector3Int(-x-y, y, -x);
                Vector3Int fourthDestination = origin + new Vector3Int(-x-y, -y, x);
                
                Vector3Int fifthDestination = origin + new Vector3Int(x, -x-y, -y);
                Vector3Int sixthDestination = origin + new Vector3Int(-x, -x-y, y);
                
                if ((gridManager.tiles.TryGetValue(firstDestination, out TileInfo outTile)))
                {
                    if (!tileInfos.Contains(outTile))
                    {
                        Debug.Log(outTile.name);
                        tileInfos.Add(outTile);
                    }
                }

                if (gridManager.tiles.TryGetValue(secondDestination, out TileInfo outTile2))
                {
                    if (!tileInfos.Contains(outTile))
                    {
                        tileInfos.Add(outTile2);
                    }
                }

                if (gridManager.tiles.TryGetValue(thirdDestination, out TileInfo outTile3))
                {
                    if (!tileInfos.Contains(outTile3))
                    {
                        tileInfos.Add(outTile3);
                    }
                }

                if (gridManager.tiles.TryGetValue(fourthDestination, out TileInfo outTile4))
                {
                    if (!tileInfos.Contains(outTile4))
                    {
                        tileInfos.Add(outTile4);
                    }
                }

                if (gridManager.tiles.TryGetValue(fifthDestination, out TileInfo outTile5))
                {
                    if (!tileInfos.Contains(outTile5))
                    {
                        tileInfos.Add(outTile5);
                    }
                }

                if (gridManager.tiles.TryGetValue(sixthDestination, out TileInfo outTile6))
                {
                    if (!tileInfos.Contains(outTile6))
                    {
                        tileInfos.Add(outTile6);
                    }
                }
            }
        }
    }
    
    
    private void ShowRange()
    {
        foreach (TileInfo tileInfo in tileInfos)
        {
            tileInfo.SetHighlight();
        }
    }

    private void HideRange()
    {
        foreach (TileInfo tileInfo in tileInfos)
        {
            tileInfo.DimHighlight();
        }
        
        tileInfos.Clear();
    }
    
}
