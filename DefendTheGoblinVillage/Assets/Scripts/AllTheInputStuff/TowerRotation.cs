using UnityEngine;

public class TowerRotation : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InputController inputController;
    [SerializeField] private BuildingTowers buildingTowers;

    [Header("Settings")] [SerializeField] private bool clickedOnce = false;
    void FixedUpdate()
    {
        if (inputController.isRotatingtower)
        {
            if (!clickedOnce)
            {
                clickedOnce = true;
                RotateTheTower();
            }   
        }

        else
        {
            clickedOnce = false;
        }
    }

    private void RotateTheTower()
    {
        if (buildingTowers.towerController)
        {
            buildingTowers.towerController.RotateTower();
        }
    }
}
