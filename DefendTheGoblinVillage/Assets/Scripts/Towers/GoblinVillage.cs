using UnityEngine;

public class GoblinVillage : MonoBehaviour
{
    [Header("Basic Stuff")] [SerializeField]
    private float villageHealth = 10f;
    [SerializeField] private ShowDamage showDamage;
    
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
        showDamage.FlashDamage();
        {
            if (villageHealth <= 0)
            {
                Debug.Log("Goblin village destroyed");
            }
        }
    }
}
