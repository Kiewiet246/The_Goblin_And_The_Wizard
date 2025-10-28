using UnityEngine;

public class ActivateEnemies : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<LeaderScript>().enemyCollider.enabled = true;
            Debug.Log(other.gameObject.name);
        }
    }
}
