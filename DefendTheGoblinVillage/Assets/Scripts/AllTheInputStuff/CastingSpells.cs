using System;
using UnityEngine;

public class CastingSpells : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private SpellManager spellManager;
    [SerializeField] private BuildingTowers buildingTowers;
    [SerializeField] private EnemyManager enemyManager;
    
    [Header("Spells")]
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private GameObject icePrefab;
    [SerializeField] private GameObject poisonPrefab;
    
    [Header("Raycasting Variables")]
    [SerializeField] RaycastHit hit = new RaycastHit();
    [SerializeField] private LayerMask tileLayer;
    public TileInfo tileInfo;
    public GameObject spellPrefab;
    [SerializeField] private float adjustHeight = 4;
    [SerializeField] private Transform spellHolder;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spellPrefab.SetActive(false);
    }

    public void CastSpellRay()
    {
        if (buildingTowers.preBuildTower.activeSelf == true)
        {
            buildingTowers.preTowerController.HideRange();
            buildingTowers.preTowerController.ClearTilesInRange();
            buildingTowers.preBuildTower.SetActive(false);
            buildingTowers.tileInfo.towerController = null;
            buildingTowers.tileInfo = null;
            
        }
        Vector2 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        hit = new RaycastHit();
        
        Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayer);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.GetComponentInParent<TileInfo>())
            {
                TileInfo compareTile = hit.collider.gameObject.GetComponentInParent<TileInfo>();
                if (tileInfo == compareTile)
                {
                    spellPrefab.SetActive(false);
                    tileInfo = null;
                    return;
                }
                
                tileInfo = compareTile;
                if (tileInfo != null)
                {
                    if (tileInfo.spellOnTile == SpellManager.SpellType.Normal)
                    {
                        spellPrefab.SetActive(true);
                        spellPrefab.transform.position = new Vector3(tileInfo.transform.position.x, tileInfo.topTileTransform.position.y + adjustHeight, tileInfo.transform.position.z);
                    }
                }
            }
        }
    }

    public void ConfirmRightClick()
    {
        if (buildingTowers.preBuildTower.activeSelf == true)
        {
            buildingTowers.preTowerController.HideRange();
            buildingTowers.preTowerController.ClearTilesInRange();
            buildingTowers.preBuildTower.SetActive(false);
            buildingTowers.tileInfo.towerController = null;
            buildingTowers.tileInfo = null;
            
        }
        if (spellPrefab.activeSelf == true)
        {
            Vector3 spawnPosition = new Vector3(tileInfo.transform.position.x, tileInfo.topTileTransform.position.y + adjustHeight, tileInfo.transform.position.z);
            switch (spellManager.spell)
            {
                case SpellManager.SpellType.Fire:
                    GameObject fireObject = Instantiate(firePrefab, spawnPosition, spellPrefab.transform.rotation, spellHolder);
                    CastedSpell fireSpell = fireObject.GetComponent<CastedSpell>();
                    fireSpell.stackDamage = spellManager.maxFireStacks;
                    fireSpell.enemyManager = enemyManager;
                    fireSpell.spellManager = spellManager;
                    fireSpell.spellTile = tileInfo;
                    tileInfo.spellOnTile = SpellManager.SpellType.Fire;
                    break;
                case SpellManager.SpellType.Ice:
                    GameObject iceObject = Instantiate(icePrefab, spawnPosition, spellPrefab.transform.rotation, spellHolder);
                    CastedSpell iceSpell = iceObject.GetComponent<CastedSpell>();
                    iceSpell.stackDamage = spellManager.maxIceStacks;
                    iceSpell.enemyManager = enemyManager;
                    iceSpell.spellManager = spellManager;
                    iceSpell.spellTile = tileInfo;
                    tileInfo.spellOnTile = SpellManager.SpellType.Ice;
                    break;
                case SpellManager.SpellType.Poison:
                    GameObject poisonObject = Instantiate(poisonPrefab, spawnPosition, spellPrefab.transform.rotation, spellHolder);
                    CastedSpell poisonSpell = poisonObject.GetComponent<CastedSpell>();
                    poisonSpell.stackDamage = spellManager.maxPoisonStacks;
                    poisonSpell.enemyManager = enemyManager;
                    poisonSpell.spellManager = spellManager;
                    poisonSpell.spellTile = tileInfo;
                    tileInfo.spellOnTile = SpellManager.SpellType.Poison;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            tileInfo = null;
            spellPrefab.SetActive(false);
        }
    }
}
