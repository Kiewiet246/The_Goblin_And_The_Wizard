using System;
using System.Collections.Generic;
using UnityEngine;


public class TowerManager : MonoBehaviour
{
    [Header("create Towers")] [SerializeField]
    private Transform towerParents;
    public TowerController.TowerType spawnTowerType;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private float adjustment = 0.75f;
    public List<TowerController> towers;
    
    [Header("Tower Prefabs")]
    [SerializeField] private GameObject archerTowerPrefab;
    [SerializeField] private GameObject canonTowerPrefab;
    [SerializeField] private GameObject wallTowerPrefab;
    [SerializeField] private GameObject ballistaTowerPrefab;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public TowerController CreateTower(TileInfo spawnPoint, Quaternion rotation)
    {
        if (spawnPoint.towerController == null)
        {
            Vector3 spawnPosition = new Vector3(spawnPoint.transform.position.x,
                ( spawnPoint.topTileTransform.transform.position.y + adjustment),
                spawnPoint.transform.position.z);
            switch (spawnTowerType)
            {
                case TowerController.TowerType.ArcherTower:
                    if (archerTowerPrefab != null)
                    {
                        GameObject archer = Instantiate(archerTowerPrefab, spawnPosition, rotation,
                            towerParents);
                        TowerController towerController = archer.GetComponent<TowerController>();
                        towerController.gridManager = this.gridManager;
                        towerController.towerManager = this;
                        towerController.towerTile = spawnPoint;
                        towerController.CalculateRange();
                        AssignSpellToTower(spawnPoint, towerController);
                        spawnPoint.towerController = towerController;
                        towers.Add(towerController);
                        towerController.gameObject.GetComponentInChildren<TowerMatController>().enabled = false;
                        return towerController;
                    }

                    break;
                case TowerController.TowerType.CanonTower:
                    if (canonTowerPrefab != null)
                    {
                        GameObject canon = Instantiate(canonTowerPrefab, spawnPosition, rotation,
                            towerParents);
                        TowerController towerController = canon.GetComponent<TowerController>();
                        towerController.gridManager = this.gridManager;
                        towerController.towerManager = this;
                        towerController.towerTile = spawnPoint;
                        towerController.CalculateRange();
                        AssignSpellToTower(spawnPoint, towerController);
                        spawnPoint.towerController = towerController;
                        towers.Add(towerController);
                        towerController.gameObject.GetComponentInChildren<TowerMatController>().enabled = false;
                        return towerController;
                    }

                    break;
                case TowerController.TowerType.WallTower:
                    if (wallTowerPrefab != null)
                    {
                        GameObject wall = Instantiate(wallTowerPrefab, spawnPosition, rotation,
                            towerParents);
                        TowerController towerController = wall.GetComponent<TowerController>();
                        towerController.gridManager = this.gridManager;
                        towerController.towerManager = this;
                        towerController.towerTile = spawnPoint;
                        towerController.CalculateRange();
                        AssignSpellToTower(spawnPoint, towerController);
                        spawnPoint.towerController = towerController;
                        towers.Add(towerController);
                        towerController.gameObject.GetComponentInChildren<TowerMatController>().enabled = false;
                        return towerController;
                    }

                    break;
                case TowerController.TowerType.BalistaTower:
                    if (ballistaTowerPrefab != null)
                    {
                        GameObject ballista = Instantiate(ballistaTowerPrefab, spawnPosition, rotation,
                            towerParents);
                        TowerController towerController = ballista.GetComponent<TowerController>();
                        towerController.gridManager = this.gridManager;
                        towerController.towerManager = this;
                        towerController.towerTile = spawnPoint;
                        towerController.CalculateRange();
                        AssignSpellToTower(spawnPoint, towerController);
                        spawnPoint.towerController = towerController;
                        towers.Add(towerController);
                        towerController.gameObject.GetComponentInChildren<TowerMatController>().enabled = false;
                        return towerController;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
          //enemyManager.CheckIfPathHasChanged(spawnPoint);
            
        }

        return null;
    }

    private void AssignSpellToTower(TileInfo spellInfo, TowerController towerController)
    {
        if (spellInfo.castedSpell != null)
        {
            towerController.spellTower = spellInfo.spellOnTile;
            spellInfo.castedSpell.hasTower = true;
        }
    }

    public void ATowerDied(TileInfo openTile)
    {
        enemyManager.AdjustPath();
    }
    
    #region ForButtons
    public void SetArcher()
    {
        spawnTowerType = TowerController.TowerType.ArcherTower;
    }

    public void SetCanon()
    {
        spawnTowerType = TowerController.TowerType.CanonTower;
    }

    public void SetWall()
    {
        spawnTowerType = TowerController.TowerType.WallTower;
    }

    public void SetBallista()
    {
        spawnTowerType = TowerController.TowerType.BalistaTower;
    }
    #endregion
}
