using UnityEngine;



public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")] [SerializeField]
    private Vector2Int gridSize;

    [SerializeField] private float tileRadius;
    [SerializeField] private bool pointyTop = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void ClearGrid()
    {
        if (transform.childCount != 0)
        {
            Debug.Log("Kill the grid");
        }
    }

    public void GreateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GameObject hexTile = new GameObject("HexTile: " + x + "; " + y);
            }
        }
    }
}
