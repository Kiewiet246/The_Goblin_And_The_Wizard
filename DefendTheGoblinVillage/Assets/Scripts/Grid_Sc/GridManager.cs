using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.ProBuilder;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")] [SerializeField]
    public Vector2Int gridSize;
    [SerializeField] private float tileRadius;
    public bool pointyTop = false;
    [SerializeField] private GameObject tilePrefab;

    [Header("Noise")]
    [SerializeField] private NoiseScript noiseScript;
    
    [Header("Navigation")]
    public Dictionary<Vector3Int, TileInfo> tiles = new Dictionary<Vector3Int, TileInfo>();
    [SerializeField] private List<Vector3Int> cubeCords = new List<Vector3Int>();
    [SerializeField] private List<GameObject> cubeObjects = new List<GameObject>();
    [SerializeField] private Vector3Int[] possibleNeighbors;
    public TileInfo startTile, endTile;


    [SerializeField] private Slider xSlider;
    [SerializeField] private Slider ySlider;
    
    public TextMeshProUGUI xtext, ytext;
    
    [SerializeField] int MaxX, MaxY, MinX, MinY;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (xSlider != null && ySlider != null)
        {
            xSlider.value = gridSize.x;
            xSlider.maxValue = MaxX;
            xSlider.minValue = MinX;
        
            ySlider.value = gridSize.y;
            ySlider.maxValue = MaxY;
            ySlider.minValue = MinY;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (xSlider != null && ySlider != null)
        // {
        //     gridSize.x = (int)xSlider.value;
        //     xtext.text = "X: "+ gridSize.x.ToString();
        //
        //     gridSize.y = (int)ySlider.value;
        //     ytext.text = "Y: " + gridSize.y.ToString();
        // }
    }

    public void PointyTop()
    {
        pointyTop = true;
    }

    public void FlatTop()
    {
        pointyTop = false;
    }
    
    public void ClearGridEditor()
    {
        cubeCords.Clear();
        cubeObjects.Clear();
        tiles.Clear();
        
        if (transform.childCount != 0)
        {
          //  Debug.Log("Kill the grid");
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }
    }

    public void ClearGrid()
    {
        cubeCords.Clear();
        cubeObjects.Clear();
        tiles.Clear();
        
        if (transform.childCount != 0)
        {
            Debug.Log("Kill the grid");
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }

    public void CreateGrid()
    {
        tiles.Clear();
        cubeCords.Clear();
        cubeObjects.Clear();
        
       // ClearGrid();
        ClearGridEditor();
        
        noiseScript.UpdateValues();
        
        for (int x = 0; x <= gridSize.x; x++)
        {
            for (int y = 0; y <= gridSize.y; y++)
            {
              //  GameObject hexTile = new GameObject("HexTile: " + x + "; " + y);
              Vector2Int coordinates = new Vector2Int(x, y);
              Vector3 gridPosition = GetPositionForHexFromCoordinate(coordinates);

              Quaternion rotation;

              if (pointyTop)
              {
                  rotation = Quaternion.Euler(-90, 0, 0);
              }

              else
              {
                  rotation = Quaternion.Euler(-90, 0, 90);
              }
              
              GameObject tile = Instantiate(tilePrefab, gridPosition, rotation, transform);
              
              tile.transform.localScale *= tileRadius;
              tile.name = "Tile_" + coordinates.x + "_" + coordinates.y;
              
              TileInfo tileInfo = tile.GetComponent<TileInfo>();
              tileInfo.cubeCoordinates = GetCubeCoordinate(coordinates);
              
              noiseScript.GenerateTerrain(x, y, tileInfo);
              noiseScript.GenerateHeight(x,y, tileInfo);
              
              cubeObjects.Add(tile);
              cubeCords.Add(GetCubeCoordinate(coordinates));
            }
        }
        
        RegisterTiles();
        StartListingNeighbors();
    }

    private Vector3 GetPositionForHexFromCoordinate(Vector2Int coordinate)
    {
        int column = coordinate.x;
        int row = coordinate.y;
        float width;
        float height;
        float xPos;
        float yPos;
        bool shouldOffset;
        float horizontalDist;
        float verticalDist;
        float offset;
        float size = tileRadius;

        if (pointyTop)
        {
            shouldOffset = (row % 2 == 0);
            width = Mathf.Sqrt(3) * size;
            height = 2f * size;
            horizontalDist = width;
            verticalDist = height * (3f / 4f);

            offset = (shouldOffset) ? width / 2f : 0;
            xPos = (column * horizontalDist) - offset;
            yPos = (row * verticalDist);
        }

        else
        {
            shouldOffset = (column % 2 == 0);
            width = 2f * size;
            height = Mathf.Sqrt(3) * size;
            horizontalDist = width * (3f / 4f);
            verticalDist = height;

            offset = (shouldOffset) ? height / 2f : 0;
            xPos = (column * horizontalDist);
            yPos = (row * verticalDist) - offset;
        }

        return new Vector3(xPos, 0f, yPos);
    }

    private Vector3Int GetCubeCoordinate(Vector2Int coordinate)
    {
        var q = new int();
        var r = new int();
        if (pointyTop)
        {
            q = coordinate.x - (coordinate.y - (coordinate.y % 2))/2;
            r = coordinate.y;
        }
        else
        {
            q = coordinate.x;
            r = coordinate.y - (coordinate.x - (coordinate.x % 2))/2; 
        }
        
        return new Vector3Int(q, r, -q-r);
    }

    public void RegisterTiles()
    {
        foreach (Transform child in transform)
        {
            TileInfo tileInfo = child.GetComponent<TileInfo>();
            tiles.Add(tileInfo.cubeCoordinates, tileInfo);
        }
    }

    public void StartListingNeighbors()
    {
        foreach (Transform child in transform)
        {
            TileInfo tileInfo = child.GetComponent<TileInfo>();
            tileInfo.neighborTiles.Clear();
            FindNeighbors(tileInfo);
        }
        
       // FindPath();
    }
    
    public void FindNeighbors(TileInfo tileInfo)
    {
        foreach (Vector3Int neighbor in possibleNeighbors)
        {
            if (tiles.TryGetValue(tileInfo.cubeCoordinates + neighbor, out TileInfo neighborTileInfo))
            {
                if (tileInfo.canStep(tileInfo.tilesOnTOp.Count, neighborTileInfo.tilesOnTOp.Count))
                tileInfo.neighborTiles.Add(neighborTileInfo);
            }
        }
    }

    public void RandomLocations()
    {
        int randomStart = Random.Range(0, transform.childCount);
        startTile = transform.GetChild(randomStart).gameObject.GetComponent<TileInfo>();
        startTile.StartTile();
        int randomEnd = Random.Range(0, transform.childCount);
        endTile = transform.GetChild(randomEnd).gameObject.GetComponent<TileInfo>();
        endTile.StopTile();
        //FindPath();
       
       // StartListingNeighbors();
    }

    public Queue<TileInfo> FindPath(TileInfo start, TileInfo end)
    {
        //Debug.Log("Hello");
        Queue<TileInfo> thePath = new Queue<TileInfo>();
        thePath.Enqueue(start);
        foreach (Transform child in transform)
        {
            if (child != start.transform && child != end.transform)
            {
                TileInfo tileInfo = child.GetComponent<TileInfo>();
               // tileInfo.SetDefualtTiles();
            }
            
        }

        if (start != null && end != null)
        {
            Queue<TileInfo> highlightPath = Djikstra(start, end);
            //thePath = highlightPath;
          //  Debug.Log(thePath.Count);
            while (highlightPath.Count > 0)
            {
                TileInfo highlightTile = highlightPath.Dequeue();
                highlightTile.SetPathTile();
                thePath.Enqueue(highlightTile);
            }
            
            end.StopTile();
           // thePath.Enqueue(endTile);
        }
        
        return thePath;
    }

    public Queue<TileInfo> Floodview(TileInfo start, TileInfo goal)
    {
        Dictionary<TileInfo, TileInfo> nextTileToGoal = new Dictionary<TileInfo, TileInfo>();
        Queue<TileInfo> frontier = new Queue<TileInfo>();
        List<TileInfo> visited = new List<TileInfo>();
        frontier.Enqueue(goal);

        while (frontier.Count > 0)
        {
            TileInfo curTile = frontier.Dequeue();
            
            foreach (TileInfo neighbor in curTile.neighborTiles)
            {
                if (visited.Contains(neighbor) == false && frontier.Contains(neighbor) == false)
                {
                    frontier.Enqueue(neighbor);
                    nextTileToGoal[neighbor] = curTile;  
                }
            }
            
            visited.Add(curTile);
        }

        if (visited.Contains(start) == false)
        {
            Debug.Log("No path found");
            return null;
        }
        
        Queue<TileInfo> path = new Queue<TileInfo>();
        path.Enqueue(start);
        TileInfo curPathTile = start;

        while (curPathTile != goal)
        {
            curPathTile = nextTileToGoal[curPathTile];
            path.Enqueue(curPathTile);
        }
        return path;
    }

    public Queue<TileInfo> Djikstra(TileInfo start, TileInfo goal)
    {
        Dictionary<TileInfo, TileInfo> nextTileToGoal = new Dictionary<TileInfo, TileInfo>();
        Dictionary<TileInfo, int> travelCost = new Dictionary<TileInfo, int>();
        
        PriorityQueue<TileInfo> frontier = new PriorityQueue<TileInfo>();
        
        
        frontier.Enqueue(goal, 0);
        travelCost[goal] = 0;

        while (frontier.Count > 0)
        {
            TileInfo curTile = frontier.Dequeue();
            if (curTile == start)
            {
               // break;
            }
            foreach (TileInfo neighbor in curTile.neighborTiles)
            {
                int newCost = travelCost[curTile] + neighbor.GetCostValue();
                if (travelCost.ContainsKey(neighbor) == false || newCost < travelCost[neighbor])
                {
                    travelCost[neighbor] = newCost;
                    int priority = newCost;
                    
                    frontier.Enqueue(neighbor, priority);
                    nextTileToGoal[neighbor] = curTile;  
                }
            }
        }

        if (nextTileToGoal.ContainsKey(start) == false)
        {
            Debug.Log("No path found");
            return null;
        }
        
        Queue<TileInfo> path = new Queue<TileInfo>();
       // path.Enqueue(start);
        TileInfo curPathTile = start;

        while (curPathTile != goal)
        {
            curPathTile = nextTileToGoal[curPathTile];
            path.Enqueue(curPathTile);
        }
        return path;
    }

    // public void AssignStartLoc(TileInfo start)
    // {
    //     if (startTile != null)
    //     {
    //         startTile.SetDefualtTiles();
    //     }
    //     
    //     startTile = start;
    //     start.StartTile();
    //     if (endTile != null)
    //     {
    //       //  StartListingNeighbors();
    //         FindPath();
    //     }
    // }
    //
    // public void AssignGoalLoc(TileInfo goal)
    // {
    //     if (endTile != null)
    //     {
    //         endTile.SetDefualtTiles();
    //     }
    //     endTile = goal;
    //     endTile.StopTile();
    //     if (startTile != null)
    //     {
    //        // StartListingNeighbors();
    //         FindPath();
    //     }
    //     
    // }
}
