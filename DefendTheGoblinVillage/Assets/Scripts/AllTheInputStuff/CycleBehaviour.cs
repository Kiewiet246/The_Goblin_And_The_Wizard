using System.Collections.Generic;
using UnityEngine;

public class CycleBehaviour : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InputController inputController;
    [SerializeField] private ModeControl modeControl;
    
    [Header("Manager Components")]
    [SerializeField] private TowerManager towerManager;
    
    [Header("Towers")] [SerializeField]
    private List<TowerController.TowerType> towerTypes;
    [SerializeField] private int currentTower = 0;

    [SerializeField] private bool oncePressed = false;
    void Start()
    {
        towerManager.spawnTowerType = towerTypes[0];
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
            }
        }
        else
        {
            oncePressed = false;
        }
    }

    private void ChangeSelectedTower()
    {
        Debug.Log("Hello" + currentTower);
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
    }
  
}
