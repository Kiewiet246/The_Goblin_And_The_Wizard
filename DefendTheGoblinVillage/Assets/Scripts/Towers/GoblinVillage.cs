using UnityEngine;

public class GoblinVillage : MonoBehaviour
{
    [Header("Basic Stuff")] public float villageHealth = 10f;
    [SerializeField] private ShowDamage showDamage;
    [SerializeField] private UIManager uIManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        villageHealth -= damage;
        uIManager.UpdateHealthSlider();
        showDamage.FlashDamage();
        {
            if (villageHealth <= 0)
            {
                Debug.Log("Goblin village destroyed");
            }
        }
    }
}
