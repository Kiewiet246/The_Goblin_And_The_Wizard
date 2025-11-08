using UnityEngine;

public class Economy : MonoBehaviour
{
    [Header("Money")]
    public int money;
    
    [Header("Mana")]
    public int mana;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    public void AddMana(int amount)
    {
        mana += amount;
    }
}
