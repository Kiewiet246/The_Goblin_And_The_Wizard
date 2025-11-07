using System;
using UnityEngine;
using UnityEngine.Serialization;

public class BuildingTowers : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TowerManager towerManager;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private CastingSpells castingSpells;
    public EnemyManager enemyManager;
    
    [Header("Building Tower Variables")]
    [SerializeField] RaycastHit hit = new RaycastHit();
    [SerializeField] private LayerMask tileLayer;
    public TileInfo tileInfo;
    public GameObject preBuildTower;
    [SerializeField] private GameObject archerTowerPrefab, archerGO, cannonTowerPrefab, canonGO, wallTowerPrefab, wallGO, balistaTowerPrefab, ballistGO;
    public TowerController preTowerController;
    [FormerlySerializedAs("towerType")] [SerializeField] private TowerController.TowerType desiredTowerType;
    [SerializeField] private float adjustment;
    public Quaternion rotation;
    [SerializeField] private LayerMask selectedLayer;
    
    [Header("Tower Information")]
    [SerializeField] private LayerMask towerLayer;
    public TowerController towerController;


    void Start()
    {
        archerGO = Instantiate(archerTowerPrefab, transform.position, Quaternion.identity, towerManager.transform);
        preBuildTower = archerGO;
        preTowerController = preBuildTower.GetComponent<TowerController>();
        enemyManager.futurePath.gameObject.SetActive(false);
        preBuildTower.SetActive(false);
        ApplyStuffToTowers(archerGO);
        wallGO =  Instantiate(wallTowerPrefab, transform.position, Quaternion.identity, towerManager.transform);
        ApplyStuffToTowers(wallGO);
        canonGO = Instantiate(cannonTowerPrefab, transform.position, Quaternion.identity, towerManager.transform);
        ApplyStuffToTowers(canonGO);
        ballistGO = Instantiate(balistaTowerPrefab, transform.position, Quaternion.identity, towerManager.transform);
        ApplyStuffToTowers(ballistGO);
    }

    private void ApplyStuffToTowers(GameObject towerPrefab)
    {
        towerPrefab.GetComponentInChildren<TowerMatController>().BuildTowerMat();
        LayerMask mask = 9 << selectedLayer;
        towerPrefab.GetComponentInChildren<TowerMatController>().gameObject.layer = mask;
       // towerPrefab.GetComponent<Collider>().isTrigger = true;
        TowerController tower = towerPrefab.GetComponent<TowerController>();
        tower.isAttackingTower = false;
        towerPrefab.SetActive(false);
    }
    
    public void CastRayOnClick()
    {
        if (castingSpells.spellPrefab.activeSelf)
        {
            castingSpells.spellPrefab.SetActive(false);
            castingSpells.tileInfo = null;
        }
        Vector2 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        
        Physics.Raycast(ray, out hit, Mathf.Infinity, towerLayer);
        if (hit.collider != null)
        {
            TowerSelected();
            return;
        }
        
        Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayer);
        if (hit.collider != null)
        {
            if (towerController != null)
            {
                towerController.HideRange();
                towerController = null;
            }
            preTowerController.HideRange();
            preTowerController.ClearTilesInRange();

            enemyManager.futurePath.gameObject.SetActive(false);
            preBuildTower.SetActive(false);
            if (tileInfo != null)
            {
                if (tileInfo == hit.collider.gameObject.GetComponentInParent<TileInfo>())
                {
                    enemyManager.futurePath.gameObject.SetActive(false);
                    preBuildTower.SetActive(false);
                    tileInfo.towerController = null;
                    tileInfo = null;
                    return;
                }

                if (tileInfo.towerController != null)
                {
                    if (tileInfo.towerController == preTowerController)
                    {
                        tileInfo.towerController = null;
                    }
                }
            }
            
            TileSelected();
        }
    }

    private void TowerSelected()
    {
        preTowerController.HideRange();
        preTowerController.ClearTilesInRange();
        enemyManager.futurePath.gameObject.SetActive(false);
        preBuildTower.SetActive(false);
        Debug.Log("Tower Selected");
        if (hit.collider.gameObject.GetComponentInParent<TowerController>() != null)
        {
            if (towerController != null)
            {
                towerController.HideRange();
                TowerController tower = hit.collider.gameObject.GetComponentInParent<TowerController>();
                if (tower == towerController)
                {
                    HidetowerRange();
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
                towerController = hit.collider.gameObject.GetComponentInParent<TowerController>();
                towerController.ShowRange();
            }
        } 
    }

    private void HidetowerRange()
    {
        towerController.HideRange();
        towerController = null;
    }

    public void TileSelected()
    {
        if (towerController != null)
        {
            HidetowerRange();
        }
        preTowerController.HideRange();
        preTowerController.ClearTilesInRange();
        enemyManager.futurePath.gameObject.SetActive(false);
        preBuildTower.SetActive(false);
        enemyManager.futurePath.gameObject.SetActive(false);
        
        if (hit.collider.gameObject.GetComponentInParent<TileInfo>() != null)
        {
             tileInfo = hit.collider.gameObject.GetComponentInParent<TileInfo>();

             if (tileInfo != enemyManager.saveStartTile && tileInfo != enemyManager.saveEndTile)
             {
                 if (tileInfo.towerController == null)
                 {
                     Vector3 spawnPosition = new Vector3(tileInfo.transform.position.x,
                         (tileInfo.topTileTransform.transform.position.y + adjustment),
                         tileInfo.transform.position.z);
                     desiredTowerType = towerManager.spawnTowerType;
                     switch (desiredTowerType)
                     {
                         case TowerController.TowerType.ArcherTower:
                             preBuildTower = archerGO;
                             break;
                         case TowerController.TowerType.CanonTower:
                             preBuildTower = canonGO;
                             break;
                         case TowerController.TowerType.WallTower:
                             preBuildTower = wallGO;
                             break;
                         case TowerController.TowerType.BalistaTower:
                             preBuildTower = ballistGO;
                             break;
                         default:
                             throw new ArgumentOutOfRangeException();
                     }
                     preBuildTower.SetActive(true);
                     preTowerController = preBuildTower.GetComponent<TowerController>();
                     preTowerController.gridManager = this.gridManager;
                     preBuildTower.transform.position = spawnPosition;
                     preTowerController.towerType = desiredTowerType;
                     preTowerController.towerTile = tileInfo;
                     preTowerController.CalculateRange();
                     preTowerController.ShowRange();
                     tileInfo.towerController = preTowerController;
                     enemyManager.CheckifPathWillChange(tileInfo);
                 }
             }
            
        }
    }
    

    public void TheRightClick()
    {
        if (castingSpells.spellPrefab.activeSelf)
        {
            castingSpells.spellPrefab.SetActive(false);
            castingSpells.tileInfo = null;
        }
        
        if (preBuildTower.activeSelf == true)
        {
            if (preBuildTower.GetComponentInChildren<TowerMatController>().canBuild)
            {
                if (tileInfo.towerController == preTowerController)
                {
                    tileInfo.towerController = null;
                }
                preBuildTower.SetActive(false);
                preTowerController.HideRange();
                preTowerController.ClearTilesInRange();
                towerController = towerManager.CreateTower(tileInfo, rotation);
                towerController.ShowRange();
                enemyManager.CheckIfPathHasChanged(tileInfo);
                enemyManager.futurePath.gameObject.SetActive(false);
                tileInfo = null;
            }
            
        }
    }
    
}
