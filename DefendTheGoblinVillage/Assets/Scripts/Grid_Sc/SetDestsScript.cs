using UnityEngine;
using UnityEngine.Serialization;
using Random = Unity.Mathematics.Random;

public class SetDestsScript : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;

    [FormerlySerializedAs("xAxis")] [Header("Start Variables")] [SerializeField]
    private int xAxistart;

    [FormerlySerializedAs("yAxis")] [Header("Start Variables")] [SerializeField]
    private int yAxisStart;

    [Header("Target Dest")]
    [SerializeField] private int xTarget, yTarget;
    
    
    [Header("Quadrants for Village")]
    [SerializeField] private int xTotalQuadrantsVil=1;
    [SerializeField] private int xTargetQuadrantsVil=1;
    [SerializeField] private int yTotalQuadrantsVil=1;
    [SerializeField] private int yTargetQuadrantsVil=1;
    [SerializeField] private bool randomYVil;
    [SerializeField] private bool randomXVil;

    [Header("guadrants for Cities")] [SerializeField]
    private int xTotalSectorsCit = 1;
    [SerializeField] private int yTotalSectorsCit = 1;
    [SerializeField] private int xTargetSectorsCit = 1;
    [SerializeField] private int yTargetSectorsCit = 1;
    [SerializeField] private bool randomCitY;
    [SerializeField] private bool randomCitX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RandomiseVillageQuadrants();
        RandomSectorsCits();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDestinations()
    {
        SetStartLoc();
        SetEndLoc();
       // gridManager.FindPath();
    }

    private void RandomSectorsCits()
    {
        if (randomCitX)
        {
            xTargetSectorsCit = UnityEngine.Random.Range(1, xTotalSectorsCit);
        }

        if (randomCitY)
        {
            yTargetSectorsCit = UnityEngine.Random.Range(1, yTotalSectorsCit);
        }
    }
    private void SetStartLoc()
    {
        float selectedSectorX = (float)xTargetSectorsCit / xTotalSectorsCit;
        int  topBracketX = Mathf.RoundToInt(selectedSectorX * gridManager.gridSize.x);
        int lowerNumX = xTargetSectorsCit - 1;
        float prevSectorX = (float)lowerNumX / xTotalSectorsCit;
        int bottomBracketX = Mathf.RoundToInt((prevSectorX * gridManager.gridSize.x));
        
        xAxistart = UnityEngine.Random.Range(bottomBracketX, topBracketX);
        
        float selectedSectorY = (float)yTargetSectorsCit / yTotalSectorsCit;
        int topBracketY = Mathf.RoundToInt(selectedSectorY * gridManager.gridSize.y);
        int lowerNumY = yTargetSectorsCit - 1;
        float prevSectorY = (float)lowerNumY / yTotalSectorsCit;
        int bottomBracketY = Mathf.RoundToInt((prevSectorY * gridManager.gridSize.y));
        
        yAxisStart = UnityEngine.Random.Range(bottomBracketY, topBracketY);
       
        var q = new int();
        var r = new int();
        if (gridManager.pointyTop)
        {
            q = xAxistart - (yAxisStart- (yAxisStart % 2))/2;
            r = yAxisStart;
        }
        else
        {
            q = xAxistart;
            r = yAxisStart - (xAxistart- (xAxistart % 2))/2; 
        }
        
        int zAxis = -q - r;
       // Debug.Log("Start: "+q + "," + r + "," +  zAxis);
        Vector3Int key = new Vector3Int(q, r, zAxis);

        if (gridManager.tiles.TryGetValue(key, out TileInfo tile))
        {
            gridManager.startTile = tile;
            tile.StartTile();
            //Debug.Log(tile);
        }
        else
        {
            Debug.Log("StartTile not found");
        }
    }

    private void RandomiseVillageQuadrants()
    {
        if (randomYVil)
        {
            yTargetQuadrantsVil = UnityEngine.Random.Range(1, yTotalQuadrantsVil);
        }

        if (randomXVil)
        {
            xTargetQuadrantsVil = UnityEngine.Random.Range(1, xTotalQuadrantsVil);
        }
    }
    
    private void SetEndLoc()
    {
        float selectedQuadrantX = (float)xTargetQuadrantsVil / xTotalQuadrantsVil;
        int xTopBracket= Mathf.RoundToInt(gridManager.gridSize.x * selectedQuadrantX);
        int lowerNumX = xTargetQuadrantsVil - 1;
        float prevQuadrantX = (float)lowerNumX/xTotalQuadrantsVil;
        int xBottomBracket = Mathf.RoundToInt(gridManager.gridSize.x * prevQuadrantX);
        
        xTarget = UnityEngine.Random.Range(xBottomBracket, xTopBracket);
       
       float selectedQuadrantY = (float)yTargetQuadrantsVil / yTotalQuadrantsVil;
       int yTopBracket = Mathf.RoundToInt(gridManager.gridSize.y * selectedQuadrantY);
       int lowerNumY = yTargetQuadrantsVil - 1;
       float prevQuadrantY = (float)lowerNumY / yTotalQuadrantsVil;
       int yBottomBracket = Mathf.RoundToInt(gridManager.gridSize.y * prevQuadrantY);
       
       yTarget = UnityEngine.Random.Range(yBottomBracket, yTopBracket);
        
        
        var q = new int();
        var r = new int();
        if (gridManager.pointyTop)
        {
            q = xTarget - (yTarget- (yTarget % 2))/2;
            r = yTarget;
        }
        else
        {
            q = xTarget;
            r = yTarget - (xTarget- (xTarget % 2))/2; 
        }
        int zTarget = -q - r;
        //Debug.Log("Target: "+xTarget + "," + yTarget+ "," +  zTarget);
        
        Vector3Int key = new Vector3Int(q, r, zTarget);
        
        if (gridManager.tiles.TryGetValue(key, out TileInfo tile))
        {
            gridManager.endTile = tile;
            tile.StopTile();
        }
        else
        {
            Debug.Log("EndTile not found");
        }
    }
}
