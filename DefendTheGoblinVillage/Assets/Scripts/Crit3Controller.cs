using UnityEngine;

public class Crit3Controller : MonoBehaviour
{
    public GridManager gridManager;

    public EnemyManager enemyManager;

    public Vector3Int targetTower;

    public SetDestsScript setDestsScript;
    
    public TowerController towerController;
    
    public TowerManager towerManager;

    [SerializeField] private int layerTiles = 6;
    [SerializeField] private int layerTowers;
    
    void Awake()
    {
       // RecreateGrid();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void RecreateGrid()
    {
        gridManager.CreateGrid();
        setDestsScript.SetDestinations();
        enemyManager.ClearPath();
        enemyManager.SetPath();
    }

    // public void NextWave()
    // {
    //     enemyManager.IncreaseWave();
    // }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           // LeftMouseButton();
        }

        if (Input.GetMouseButtonDown(1))
        {
         //  RightMouseButton();
        }

        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     RotateTheTower();
        // }
        
        // if (Input.GetKeyDown(KeyCode.D))
        // {
        //     DestroyTheTower();
        // }
    }
    
    private void RightMouseButton()
    {
        Vector2 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        LayerMask mask = 1 << layerTowers;
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(ray, out hit, 10000 , mask);
        
        if (hit.collider != null)
        {
                if (hit.collider.gameObject.GetComponent<TowerController>() != null)
                {
                    if (towerController != null)
                    {
                        towerController.HideRange();
                        TowerController tower = hit.collider.gameObject.GetComponent<TowerController>();
                        if (tower == towerController)
                        {
                            towerController.HideRange();
                            towerController = null;
                        }
                    
                        else if (tower != towerController)
                        {
                            //tower.HideRange();
                            towerController = tower;
                            towerController.ShowRange();
                        }
                    }
                    else
                    {
                        towerController = hit.collider.gameObject.GetComponent<TowerController>();
                        towerController.ShowRange();
                    }
                } 
        }
    }

    public void DestroyTheTower()
    {
        if (towerController != null)
        {
            towerController.HideRange();
            towerController.TakeDamage(towerController.health);
        }
    }

    public void RotateTheTower()
    {
        if (towerController != null)
        {
            towerController.RotateTower();
        }
    }
    
    private void LeftMouseButton()
    {
        Vector2 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit = new RaycastHit();
        LayerMask mask = 1 << layerTiles;
        Physics.Raycast(ray, out hit, 10000, mask);

        
        if (hit.collider != null)
        {
            
            // if (hit.collider.gameObject.GetComponentInParent<TileInfo>() != null)
            // {
            //     TileInfo tileInfo = hit.collider.gameObject.GetComponentInParent<TileInfo>();
            //     towerManager.CreateTower(tileInfo);
            // }
            //
            // else if (hit.collider.gameObject.GetComponent<TileInfo>() != null)
            // {
            //     TileInfo tileInfo = hit.collider.gameObject.GetComponent<TileInfo>();
            //     towerManager.CreateTower(tileInfo);
            // }
        }
    }
}
