using System;
using System.Collections.Generic;
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

    [Header("CanonBall Stuff")] [SerializeField]private float radius;
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
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponent<LeaderScript>())
            {
                enemy = other.GetComponent<LeaderScript>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    towerController.projectiles.Add(rb);
                    gameObject.SetActive(false);
                    enemy = null;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!oneExplosion)
        {
            oneExplosion = true;
            Debug.Log("Explode");
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

            foreach (Collider hit in colliders)
            {
                if (hit.GetComponent<LeaderScript>())
                {
                    hit.GetComponent<LeaderScript>().TakeDamage(damage);
                }
            }
            EndProjectile();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
