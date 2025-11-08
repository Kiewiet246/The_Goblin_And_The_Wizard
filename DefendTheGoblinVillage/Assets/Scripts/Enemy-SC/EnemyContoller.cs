using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContoller : MonoBehaviour
{
    [Header("Enemy Identity")]
    [SerializeField] private int enemyIdentity;
    [SerializeField] private EnemyType enemyType;

    [Header("Casting Spells")]
    [SerializeField] private float castingRange;
    [SerializeField] private float castRate;
    [SerializeField] private float currentTime;
    [SerializeField] private float castValue;

    [SerializeField] private LayerMask towerLayer;
    public bool canShoot;
    public enum EnemyType
    {
        Normal,
        Fast,
        Shield,
        Wizard,
        Cleric
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (canShoot)
        {
            CountDown();
        }
    }

    public void SpawnedAgained()
    {
        currentTime = Time.time;
    }

    private void CountDown()
    {
        float difference = Time.time - currentTime;
        if (difference >= castRate)
        {
            FigureOutWhoWeAre();
        }
    }

    private void FigureOutWhoWeAre()
    {
        currentTime = Time.time;
        switch (enemyType)
        {
            case EnemyType.Wizard:
                CastSpellOfDestruction();
                break;
            case EnemyType.Cleric:
                break;
        }
    }

    private void CastSpellOfDestruction()
    {
        List<TowerController> towersInRange = new List<TowerController>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, castingRange, towerLayer);
        if (colliders.Length == 0)
        {
            return;
        }
        
        foreach (Collider col in colliders)
        {
                if (col.GetComponent<TowerController>() != null)
                {
                    TowerController tower = col.GetComponent<TowerController>();
                    if (!towersInRange.Contains(tower))
                    {
                        towersInRange.Add(tower);
                    }
                }
                
                else if (col.GetComponentInParent<TowerController>())
                {
                    TowerController tower = col.GetComponentInParent<TowerController>();
                    if (!towersInRange.Contains(tower))
                    {
                        towersInRange.Add(tower);
                    }
                }
        }
        
        int randomeTower = UnityEngine.Random.Range(0, towersInRange.Count);
        Debug.Log(towersInRange[randomeTower].name);
        towersInRange[randomeTower].TakeDamage(castValue);
    }
}
