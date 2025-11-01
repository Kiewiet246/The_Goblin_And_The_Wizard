using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private TowerController towerController;
    [SerializeField] private int damage;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float lifetime=2;
    [SerializeField] private float currentTime;
    [SerializeField] private LeaderScript enemy;

    [SerializeField] private int hits;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damage = towerController.damage;
        StartProjectile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        float difference = Time.time - currentTime;
        if (difference >= lifetime)
        {
            towerController.projectiles.Add(rb);
            gameObject.SetActive(false);
        }
    }

    public void StartProjectile()
    {
        currentTime = Time.time;
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
}
