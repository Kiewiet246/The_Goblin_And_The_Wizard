using System.Collections.Generic;
using UnityEngine;

public class CycleBehaviour : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InputController inputController;
    [SerializeField] private ModeControl modeControl;
    [SerializeField] private BuildingTowers buildingTowers;
    
    [Header("Manager Components")]
    [SerializeField] private TowerManager towerManager;
    [SerializeField] private SpellManager spellManager;
    
    [Header("Towers")] [SerializeField]
    private List<TowerController.TowerType> towerTypes;
    [SerializeField] private int currentTower = 0;

    [Header("Spells")]
    [SerializeField] private List<SpellManager.SpellType> spellTypes;
    [SerializeField] private int currentSpell = 0;
    
    [Header("Extra")]
    [SerializeField] private bool oncePressed = false;
    void Start()
    {
        towerManager.spawnTowerType = towerTypes[0];
        spellManager.spell = spellTypes[0];
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
        }
        
        else if (currentTower >= towerTypes.Count)
        {
            currentTower = 0;
            towerManager.spawnTowerType = towerTypes[0];
        }

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
        }
        
        else if (currentSpell >= spellTypes.Count)
        {
            currentSpell = 0;
            spellManager.spell = spellTypes[0];
        }
    }
  
}
