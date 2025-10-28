using System;
using UnityEngine;

public class EnemyContoller : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform target;
     
    
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
     //   MoveEnemy();
    }

    private void MoveEnemy()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            Vector3 movement = direction.normalized * (movementSpeed * Time.deltaTime);
            Vector3 endPoint = transform.position + movement;
        
            rb.MovePosition(endPoint);
        }
        
    }


    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
