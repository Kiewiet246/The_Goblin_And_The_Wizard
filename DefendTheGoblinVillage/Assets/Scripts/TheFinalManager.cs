using UnityEngine;

public class TheFinalManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private SetDestsScript setDestsScript;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private WaveManager waveManager;

    void Awake()
    {
        CreateGrid();
        waveManager.isSpawningNextWave = true;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void CreateGrid()
    {
        gridManager.CreateGrid();
        setDestsScript.SetDestinations();
        enemyManager.ClearPath();
        enemyManager.SetPath();
    }
}
