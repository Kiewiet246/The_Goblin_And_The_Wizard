using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GoblinVillage goblinVillage;


    [Header("Mode Elements")]
    [SerializeField] private ModeControl modeControl;
    [SerializeField] private Image imageMode;
    [SerializeField] private Sprite castingSprite, buildingSprite;
    
    [Header("Cycle Elements")]
    [SerializeField] private CycleBehaviour cycleBehaviour;
    [SerializeField] private Image imageCycle;

    [SerializeField] private Sprite archerSprite, canonSprite, wallSprite, blunderSprite;
    [SerializeField] private Sprite fireballSprite, iceSprite, poisonSprite;
    
    [Header("Currency Elements")]
    [SerializeField] private Economy economy;
    [SerializeField] private TextMeshProUGUI scrapText;
    [SerializeField] private TextMeshProUGUI manaText;
    
    [Header("WaveManager Elements")]
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private TextMeshProUGUI waveText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateHealthSlider();
        UpdateModeImage();
      //  UpdateCycleImage();
    }

    // Update is called once per frame
    void Update()
    {
        scrapText.text = economy.money.ToString();
        manaText.text = economy.mana.ToString();
        waveText.text = "Current Wave: " + waveManager.waveCounter.ToString();
    }

    public void UpdateHealthSlider()
    {
        healthSlider.value = goblinVillage.villageHealth;
    }

    public void UpdateModeImage()
    {
        if (modeControl.isBuildMode)
        {
            imageMode.sprite = buildingSprite;
        }
        else
        {
            imageMode.sprite = castingSprite;
        }
    }
    public void UpdateCycleImage()
    {
        if (modeControl.isBuildMode)
        {
            switch (cycleBehaviour.visualiseTowerType)
            {
                case TowerController.TowerType.ArcherTower:
                    imageCycle.sprite = archerSprite;
                    break;
                case TowerController.TowerType.CanonTower:
                    imageCycle.sprite = canonSprite;
                    break;
                case TowerController.TowerType.WallTower:
                    imageCycle.sprite = wallSprite;
                    break;
                case TowerController.TowerType.BalistaTower:
                    imageCycle.sprite = blunderSprite;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        else
        {
            switch (cycleBehaviour.visualiseSpell)
            {
                case SpellManager.SpellType.Fire:
                    imageCycle.sprite = fireballSprite;
                    break;
                case SpellManager.SpellType.Ice:
                    imageCycle.sprite = iceSprite;
                    break;
                case SpellManager.SpellType.Poison:
                    imageCycle.sprite = poisonSprite;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
}
