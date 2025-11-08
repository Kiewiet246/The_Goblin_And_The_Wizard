using UnityEngine;

public class ModeControl : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InputController inputController;
    [SerializeField] private BuildingTowers buildingTowers;
    [SerializeField] private CastingSpells castingSpells;

    [Header("Mode Variables")] [SerializeField]
    private bool oncePressed;

    public bool isBuildMode = true;

    void FixedUpdate()
    {
        if (inputController.changePhase)
        {
            if (!oncePressed)
            {
                oncePressed = true;
                ChangeMode();
            }
        }
        else
        {
            oncePressed = false;
        }
    }

    private void ChangeMode()
    {
        if (isBuildMode)
        {
            isBuildMode = false;
            if (buildingTowers.preBuildTower.activeSelf)
            {
                buildingTowers.preBuildTower.SetActive(false);
                buildingTowers.preTowerController.HideRange();
                buildingTowers.preTowerController.ClearTilesInRange();
                buildingTowers.enemyManager.futurePath.gameObject.SetActive(false);
                castingSpells.tileInfo = buildingTowers.tileInfo;
                castingSpells.SetSpellOntile();
            }
        }

        else
        {
            isBuildMode = true;
            if (castingSpells.spellPrefab.activeSelf)
            {
                castingSpells.spellPrefab.SetActive(false);
                buildingTowers.tileInfo = castingSpells.tileInfo;
                buildingTowers.SetTowerOnTile();
            }
        }
    }
}
