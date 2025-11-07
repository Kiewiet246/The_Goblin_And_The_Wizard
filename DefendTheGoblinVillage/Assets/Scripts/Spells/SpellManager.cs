using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    [Header("Components and Managers")]

    [Header("Spells")] public SpellType spell;
    public enum SpellType
    {
        Normal,
        Fire,
        Ice,
        Poison,
    }
    
    [Header("Fire Variables")]
    [SerializeField] private float burnRate; //Rate at which burns happens
    [SerializeField] private int removeFireStacks;
    [SerializeField] private float burnDamage;
    [SerializeField] private List<LeaderScript> burningLeaders;
    [SerializeField] private bool isBurning = false;
    [SerializeField] private float burnTime;
    [SerializeField] private int maxFireStacks;
    
    [Header("Ice Variables")]
    [SerializeField] private float iceDamage;
    [SerializeField] private float frozenRate;
    [SerializeField] private int removeIceStacks;
    [SerializeField] private List<LeaderScript> frozenLeaders;
    [SerializeField] private bool isFrozen = false;
    [SerializeField] private float frozenTime;
    [SerializeField] private int maxIceStacks;
    
    [Header("Poison Variables")]
    [SerializeField] private float poisonDamage;
    [SerializeField] private float poisonDebuff;
    [SerializeField] private int removePoisonStacks;
    [SerializeField] private List<LeaderScript> poisonLeaders;
    [SerializeField] private bool isPoisoning = false;
    [SerializeField] private float poisonTime;
    [SerializeField] private float poisonRate;
    [SerializeField] private int maxPoisonStacks;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (isBurning)
        {
            BurnCountdown();
        }

        if (isFrozen)
        {
            IceCountdown();
        }

        if (isPoisoning)
        {
            PoisonCountdown();
        }
    }

    #region Fire
    private void BurnCountdown()
    {
        float difference = Time.time - burnTime;
        if (difference >= burnRate)
        {
           if (burningLeaders.Count > 0)
           {
               BurnTheLeaders();
           }
        }
    }

    private void BurnTheLeaders()
    {
        List<LeaderScript> leaders = burningLeaders;

        foreach (LeaderScript burningMan in burningLeaders)
        {
            burningMan.TakeDamage(burnDamage);
            burningMan.spellAflections.fireStack -= removeFireStacks;

            if (burningMan.spellAflections.fireStack <= 0)
            {
                burningLeaders.Remove(burningMan);
            }
        }

        if (burningLeaders.Count > 0)
        {
            burnTime = Time.time;
        }
        else if (burningLeaders.Count == 0)
        {
            isBurning = false;
        }
    }

    public void LeadersGotBurnt(LeaderScript leader, int burnAmount)
    {
        if (!burningLeaders.Contains(leader))
        {
            leader.spellAflections.fireStack += burnAmount;
            if (leader.spellAflections.fireStack >= maxFireStacks)
            {
                if (burningLeaders.Count == 0)
                {
                    isBurning = true;
                    burnTime = Time.time;
                }
                burningLeaders.Add(leader);
            }
        }
    }
    #endregion
    
    #region Ice

    private void IceCountdown()
    {
        float difference = Time.time - frozenTime;
        if (difference >= frozenRate)
        {
            if (frozenLeaders.Count > 0)
            {
                FreezeTheEnemies();
            }
        }
    }
    
    private void FreezeTheEnemies()
    {
        List<LeaderScript> leaders = frozenLeaders;

        foreach (LeaderScript frozen in frozenLeaders)
        {
            frozen.TakeDamage(iceDamage);
            frozen.spellAflections.iceStack -= removeIceStacks;
            if (frozen.spellAflections.iceStack <= 0)
            {
                frozenLeaders.Remove(frozen);
            }
        }

        if (frozenLeaders.Count > 0)
        {
            isFrozen = true;
            frozenTime = Time.time;
        }

        else if (frozenLeaders.Count == 0)
        {
             isFrozen = false;
        }
    }

    public void LeadersGotIced(LeaderScript leader, int iceAmount)
    {
        if (!frozenLeaders.Contains(leader))
        {
            leader.spellAflections.iceStack += iceAmount;
            if (leader.spellAflections.iceStack >= maxIceStacks)
            {
                if (frozenLeaders.Count == 0)
                {
                    isFrozen = true;
                    frozenTime = Time.time;
                }
                frozenLeaders.Add(leader);
            }
        }
    }
    
    #endregion
    
    #region Poison
    private void PoisonCountdown()
    {
        float difference = Time.time - poisonTime;
        if (difference >= poisonRate)
        {
            if (poisonLeaders.Count > 0)
            {
                PoisonTheEnemies();
            }
        }
    }
    
    private void PoisonTheEnemies()
    {
        List<LeaderScript> leaders = poisonLeaders;

        foreach (LeaderScript poisoned in poisonLeaders)
        {
            poisoned.TakeDamage(poisonDamage);
            poisoned.spellAflections.poisonStack -= removePoisonStacks;
            if (poisoned.spellAflections.poisonStack <= 0)
            {
                poisonLeaders.Remove(poisoned);
            }
        }

        if (poisonLeaders.Count > 0)
        {
            isPoisoning = true;
            poisonTime = Time.time;
        }

        else if (poisonLeaders.Count == 0)
        {
            isPoisoning = false;
        }
    }

    public void LeadersGotPoisoned(LeaderScript leader, int poisonAmount)
    {
        if (!poisonLeaders.Contains(leader))
        {
            leader.spellAflections.poisonStack += poisonAmount;
            if (leader.spellAflections.poisonStack >= maxPoisonStacks)
            {
                if (poisonLeaders.Count == 0)
                {
                    isPoisoning = true;
                    poisonTime = Time.time;
                }
                poisonLeaders.Add(leader);
            }
        }
    }
    #endregion
}
