using System.Collections.Generic;
using UnityEngine;

public class CycleBehaviour : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InputController inputController;
    [SerializeField] private ModeControl modeControl;
    [SerializeField] private BuildingTowers buildingTowers;
    [SerializeField] private CastingSpells castingSpells;
    [SerializeField] private UIManager uiManager;
    
    [Header("Manager Components")]
    [SerializeField] private TowerManager towerManager;
    [SerializeField] private SpellManager spellManager;
    
    [Header("Towers")] [SerializeField]
    private List<TowerController.TowerType> towerTypes;
    public TowerController.TowerType visualiseTowerType;
    [SerializeField] private int currentTower = 0;

    [Header("Spells")]
    [SerializeField] private List<SpellManager.SpellType> spellTypes;
    public SpellManager.SpellType visualiseSpell;
    [SerializeField] private int currentSpell = 0;
    
    [Header("Extra")]
    [SerializeField] private bool oncePressed = false;
    void Start()
    {
        towerManager.spawnTowerType = towerTypes[0];
        visualiseTowerType = towerManager.spawnTowerType;
        spellManager.spell = spellTypes[0];
        visualiseSpell = spellManager.spell;
        uiManager.UpdateCycleImage();
    }
    
    void FixedUpdate()
    {
        if (inputController.cycling)
        {
            if (!oncePressed)
            {
                oncePressed = true;
                if (modeControl.isBuildMode)
                {
                    ChangeSelectedTower();
                }

                else
                {
                    ChangeSelectedSpell();
                }
            }
        }
        else
        {
            oncePressed = false;
        }
    }

    private void ChangeSelectedTower()
    {
        
        currentTower += 1;
        if (currentTower < towerTypes.Count)
        {
            towerManager.spawnTowerType = towerTypes[currentTower];
            visualiseTowerType = towerManager.spawnTowerType;
        }
        
        else if (currentTower >= towerTypes.Count)
        {
            currentTower = 0;
            towerManager.spawnTowerType = towerTypes[0];
            visualiseTowerType = towerManager.spawnTowerType;
        }
        uiManager.UpdateCycleImage();

        if (buildingTowers.preBuildTower.activeSelf == true)
        {
            buildingTowers.tileInfo.towerController = null;
            buildingTowers.TileSelected();
        }
    }

    private void ChangeSelectedSpell()
    {
        currentSpell += 1;
        if (currentSpell < spellTypes.Count)
        {
            spellManager.spell = spellTypes[currentSpell];
            visualiseSpell = spellManager.spell;
        }
        
        else if (currentSpell >= spellTypes.Count)
        {
            currentSpell = 0;
            spellManager.spell = spellTypes[0];
            visualiseSpell = spellManager.spell;
        }
        uiManager.UpdateCycleImage();
        castingSpells.UpdateSprite();
    }
  
}
