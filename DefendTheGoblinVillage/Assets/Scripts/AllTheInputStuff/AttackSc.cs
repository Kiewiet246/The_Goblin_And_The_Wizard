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
    }

    private void FigureOutWhatWithLeftClick()
    {
        if (!leftClick)
        {
            leftClick = true;
            buildingTowers.CastRayOnClick();
        }
    }

    private void FigureOutWhatWithRightClick()
    {
        
    }
}
