using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private List<Transform> pathTiles;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPath()
    {
        Debug.Log("Set path");
        Queue<TileInfo> createPath = gridManager.FindPath();
        int count = createPath.Count;
       // Debug.Log(count);

        for (int i = 0; i < count; i++)
        {
            Transform pathTile = createPath.Dequeue().transform;
            pathTiles.Add(pathTile);
        }
    }
}
