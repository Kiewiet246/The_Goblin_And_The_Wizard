using UnityEngine;
using UnityEngine.InputSystem;

public class AttackSc : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InputController inputController;
    [SerializeField] private ModeControl control;
    [SerializeField] private BuildingTowers buildingTowers;

    [Header("Managers")]
    [SerializeField] private TowerManager towerManager;

    [Header("Left Click")]
    [SerializeField] private bool leftClick = false;
    
    [Header("Right Click")]
    [SerializeField] private bool rightClick = false;
    void FixedUpdate()
    {
        if (inputController.isLeftClicked)
        {
            FigureOutWhatWithLeftClick();
        }
        else
        {
            leftClick = false;
        }

        if (inputController.isRightClicked)
        {
            FigureOutWhatWithRightClick();
        }
        else
        {
            rightClick = false;
        }
    }

    private void FigureOutWhatWithLeftClick()
    {
        if (!leftClick)
        {
            leftClick = true;
            if (control.isBuildMode)
            {
                buildingTowers.CastRayOnClick();
            }
            else
            {
                
            }
            
        }
    }

    private void FigureOutWhatWithRightClick()
    {
        if (!rightClick)
        {
            rightClick = true;
            if (control.isBuildMode)
            {
                buildingTowers.TheRightClick();
            }
            else
            {
                
            }
        }
    }
}
