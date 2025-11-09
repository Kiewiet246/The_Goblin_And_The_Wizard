using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GoblinVillage goblinVillage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateHealthSlider();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthSlider()
    {
        healthSlider.value = goblinVillage.villageHealth;
    }
    
}
