using System;
using UnityEngine;

public class CastingSpells : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private SpellManager spellManager;
    [SerializeField] private BuildingTowers buildingTowers;
    
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
            buildingTowers.tileInfo = null;
            
        }
        Debug.Log("Casting Spell Ray");
        Vector2 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        hit = new RaycastHit();
        
        Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayer);

        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
            
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
            buildingTowers.tileInfo = null;
            
        }
        if (spellPrefab.activeSelf == true)
        {
            Vector3 spawnPosition = new Vector3(tileInfo.transform.position.x, tileInfo.topTileTransform.position.y + adjustHeight, tileInfo.transform.position.z);
            switch (spellManager.spell)
            {
                case SpellManager.SpellType.Fire:
                    Instantiate(firePrefab, spawnPosition, spellPrefab.transform.rotation, spellHolder);
                    break;
                case SpellManager.SpellType.Ice:
                    Instantiate(icePrefab, spawnPosition, spellPrefab.transform.rotation, spellHolder);
                    break;
                case SpellManager.SpellType.Poison:
                    Instantiate(poisonPrefab, spawnPosition, spellPrefab.transform.rotation, spellHolder);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            spellPrefab.SetActive(false);
        }
    }
}
