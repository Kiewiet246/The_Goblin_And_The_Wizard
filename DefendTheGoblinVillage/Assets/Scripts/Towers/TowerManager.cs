using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerManager : MonoBehaviour
{
    [Header("create Towers")] [SerializeField]
    private Transform towerParents;
    [SerializeField] private TowerController.TowerType spawnTowerType;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private float adjustment = 1f;
    
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
        if (spawnPoint.structure == null)
        {
            Vector3 spawnPosition = new Vector3(spawnPoint.transform.position.x,
                (spawnPoint.transform.position.y + spawnPoint.tilesOnTOp.Count() + adjustment),
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
                        towerController.towerTile = spawnPoint;
                        towerController.CalculateRange();
                        spawnPoint.structureWeight = (int)towerController.towerType;
                        spawnPoint.structure = archer;
                    }

                    break;
                case TowerController.TowerType.CanonTower:
                    if (canonTowerPrefab != null)
                    {
                        GameObject canon = Instantiate(canonTowerPrefab, spawnPosition, Quaternion.identity,
                            towerParents);
                        TowerController towerController = canon.GetComponent<TowerController>();
                        towerController.gridManager = this.gridManager;
                        towerController.towerTile = spawnPoint;
                        towerController.CalculateRange();
                        spawnPoint.structureWeight = (int)towerController.towerType;
                        spawnPoint.structure = canon;
                    }

                    break;
                case TowerController.TowerType.WallTower:
                    if (wallTowerPrefab != null)
                    {
                        GameObject wall = Instantiate(wallTowerPrefab, spawnPosition, Quaternion.identity,
                            towerParents);
                        TowerController towerController = wall.GetComponent<TowerController>();
                        towerController.gridManager = this.gridManager;
                        towerController.towerTile = spawnPoint;
                        towerController.CalculateRange();
                        spawnPoint.structureWeight = (int)towerController.towerType;
                        spawnPoint.structure = wall;
                    }

                    break;
                case TowerController.TowerType.BalistaTower:
                    if (ballistaTowerPrefab != null)
                    {
                        GameObject ballista = Instantiate(ballistaTowerPrefab, spawnPosition, Quaternion.identity,
                            towerParents);
                        TowerController towerController = ballista.GetComponent<TowerController>();
                        towerController.gridManager = this.gridManager;
                        towerController.towerTile = spawnPoint;
                        towerController.CalculateRange();
                        spawnPoint.structureWeight = (int)towerController.towerType;
                        spawnPoint.structure = ballista;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
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
