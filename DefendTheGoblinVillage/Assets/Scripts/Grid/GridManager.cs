using UnityEngine;



public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")] [SerializeField]
    private Vector2Int gridSize;
    [SerializeField] private float tileRadius;
    [SerializeField] private bool pointyTop = false;
    
    [SerializeField] private GameObject tilePrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void ClearGridEditor()
    {
        if (transform.childCount != 0)
        {
            Debug.Log("Kill the grid");
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }
    }

    private void ClearGrid()
    {
        if (transform.childCount != 0)
        {
            Debug.Log("Kill the grid");
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
        }
    }

    public void CreateGrid()
    {
        
        ClearGridEditor();
       // ClearGrid();
        
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
              //  GameObject hexTile = new GameObject("HexTile: " + x + "; " + y);
              Vector2Int coordinates = new Vector2Int(x, y);
              Vector3 gridPosition = GetPositionForHexFromCoordinate(coordinates);

              Quaternion rotation;

              if (pointyTop)
              {
                  rotation = Quaternion.Euler(90, 0, 0);
              }

              else
              {
                  rotation = Quaternion.Euler(90, 0, 90);
              }
              
              GameObject tile = Instantiate(tilePrefab, gridPosition, rotation, transform);
              tile.name = "Tile_" + coordinates.x + "_" + coordinates.y;
            }
        }
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
            xPos = (column * horizontalDist) + offset;
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
}
