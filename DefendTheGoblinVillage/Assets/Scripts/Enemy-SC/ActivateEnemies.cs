using UnityEngine;

public class ActivateEnemies : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            LeaderScript leaderScript = other.gameObject.GetComponent<LeaderScript>();
            leaderScript.enemyCollider.enabled = true;
            leaderScript.ResetHealth();
            //Debug.Log(other.gameObject.name);
        }
    }
}
