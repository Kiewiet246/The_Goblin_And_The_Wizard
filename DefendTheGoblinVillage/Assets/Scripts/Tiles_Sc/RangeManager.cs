using UnityEngine;

public class RangeManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private TileInfo tileInfo;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScriptsForRange(GridManager gridManager, TileInfo tileInfo)
    {
        this.gridManager = gridManager;
        this.tileInfo = tileInfo;
    }
}
