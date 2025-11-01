using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerManager : MonoBehaviour
{
    [Header("create Towers")] [SerializeField]
    private Transform towerParents;
    [SerializeField] private TowerController.TowerType spawnTowerType;
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

    public void CreateTower(TileInfo spawnPoint)
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
                        GameObject archer = Instantiate(archerTowerPrefab, spawnPosition, Quaternion.identity,
                            towerParents);
                        TowerController towerController = archer.GetComponent<TowerController>();
                        towerController.gridManager = this.gridManager;
                        towerController.towerManager = this;
                        towerController.towerTile = spawnPoint;
                        towerController.CalculateRange();
                       spawnPoint.towerController = towerController;
                        towers.Add(towerController);
                    }

                    break;
                case TowerController.TowerType.CanonTower:
                    if (canonTowerPrefab != null)
                    {
                        GameObject canon = Instantiate(canonTowerPrefab, spawnPosition, Quaternion.identity,
                            towerParents);
                        TowerController towerController = canon.GetComponent<TowerController>();
                        towerController.gridManager = this.gridManager;
                        towerController.towerManager = this;
                        towerController.towerTile = spawnPoint;
                        towerController.CalculateRange();
                        
                        towers.Add(towerController);
                    }

                    break;
                case TowerController.TowerType.WallTower:
                    if (wallTowerPrefab != null)
                    {
                        GameObject wall = Instantiate(wallTowerPrefab, spawnPosition, Quaternion.identity,
                            towerParents);
                        TowerController towerController = wall.GetComponent<TowerController>();
                        towerController.gridManager = this.gridManager;
                        towerController.towerManager = this;
                        towerController.towerTile = spawnPoint;
                        towerController.CalculateRange();
                        
                        towers.Add(towerController);
                    }

                    break;
                case TowerController.TowerType.BalistaTower:
                    if (ballistaTowerPrefab != null)
                    {
                        GameObject ballista = Instantiate(ballistaTowerPrefab, spawnPosition, Quaternion.identity,
                            towerParents);
                        TowerController towerController = ballista.GetComponent<TowerController>();
                        towerController.gridManager = this.gridManager;
                        towerController.towerManager = this;
                        towerController.towerTile = spawnPoint;
                        towerController.CalculateRange();
                        
                        towers.Add(towerController);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            enemyManager.CheckIfPathHasChanged(spawnPoint);
        }
    }

    public void ATowerDied(TileInfo openTile)
    {
        enemyManager.CheckIfPathHasChanged(openTile);
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
