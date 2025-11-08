using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private TowerController towerController;
    [SerializeField] private float damage;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float lifetime=2;
    [SerializeField] private float currentTime;
    [SerializeField] private LeaderScript enemy;

    [SerializeField] private bool isAlive = false;
    [SerializeField] private Transform target;
    [SerializeField] private int hits;
    [SerializeField] private bool oneExplosion = false;
    [SerializeField] private LayerMask enemiesLayer;

    [Header("CanonBall Stuff")] [SerializeField]private float radius;

    public SpellManager.SpellType projectileSpell;
    [SerializeField] private float applyStack;
    [SerializeField] private float fireStack;
    [SerializeField] private float iceStack;
    [SerializeField] private float poisonStack;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damage = towerController.damage;
        //StartProjectile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignSpell(SpellManager.SpellType spellType)
    {
        projectileSpell = spellType;
        switch (spellType)
        {
            case SpellManager.SpellType.Normal:
                applyStack = 0;
                break;
            case SpellManager.SpellType.Fire:
                applyStack = fireStack;
                break;
            case SpellManager.SpellType.Ice:
                applyStack = iceStack;
                break;
            case SpellManager.SpellType.Poison:
                applyStack = poisonStack;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(spellType), spellType, null);
        }
    }

    void FixedUpdate()
    {
        if (isAlive)
        {
            transform.LookAt(target);
            float difference = Time.time - currentTime;
            if (difference >= lifetime)
            {
                EndProjectile();
            }
        }
    }

    public void StartProjectile(Transform target)
    {
        currentTime = Time.time;
        isAlive = true;
        this.target = target;
        oneExplosion = false;
    }

    private void EndProjectile()
    {
        isAlive = false;
        towerController.projectiles.Add(rb);
        gameObject.SetActive(false);
        enemy = null;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponent<LeaderScript>())
            {
                enemy = other.GetComponent<LeaderScript>();
               ArrowHitItsMark(enemy);
            }
        }
    }

    private void ArrowHitItsMark(LeaderScript leader)
    {
        if (leader != null)
        {
            enemy.TakeDamage(damage, applyStack, projectileSpell);
            EndProjectile();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!oneExplosion)
        {
            ExplodeTheProjectile();
        }
    }

    private void ExplodeTheProjectile()
    {
        oneExplosion = true;
        List<LeaderScript> leaders = new List<LeaderScript>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, enemiesLayer);

        foreach (Collider hit in colliders)
        {
            
            if (hit.GetComponent<LeaderScript>())
            {
                LeaderScript hitLeader = hit.GetComponent<LeaderScript>();
                if (!leaders.Contains(hitLeader))
                {
                    leaders.Add(hitLeader);
                }
            }
            else if (hit.GetComponentInParent<LeaderScript>())
            {
                LeaderScript hitLeader = hit.GetComponentInParent<LeaderScript>();
                if (!leaders.Contains(hitLeader))
                {
                    leaders.Add(hitLeader);
                }
            }
        }

        foreach (LeaderScript leader in leaders)
        {
            leader.TakeDamage(damage, applyStack, projectileSpell);
        }
        EndProjectile();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
