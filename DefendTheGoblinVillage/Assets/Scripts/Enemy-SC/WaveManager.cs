using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Manager")] [SerializeField] private EnemyManager enemyManager;

    [Header("The Important Values")] [SerializeField]
    private int waitTimeBetweenWaves;
    [SerializeField] private float currentTime;
    [SerializeField] private int waveCounter = 0;
    [SerializeField] private int spawnIndividual = 0;
    [SerializeField] private bool isSpawningIndividuals;
    [SerializeField] private int spawnBunch;
    
    [Header("Waves")]
    [SerializeField] private Wave currentWave;
    [SerializeField] private Wave waveOne;

    [SerializeField] private int createOfEach;

    [Header("Enemy Prefabs")] [SerializeField]
    private GameObject fastEnemy, normalEnemy, shieldEnemy, wizardEnemy, clericEnemy;
    [SerializeField] private List<GameObject> allEnemies;

    [SerializeField] private Transform storeTheEnemies;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      CreateAllTheEnemies();
      currentWave = waveOne;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (isSpawningIndividuals)
        {
            CountDownIndividual();
        }
    }

    private void CountDownIndividual()
    {
        float difference = Time.time - currentTime;
        if (difference >= currentWave.parties[spawnBunch].spawnRateForIndividuals)
        {
            switch (currentWave.parties[spawnBunch].singleParty[spawnIndividual])
            {
                case EnemyContoller.EnemyType.Normal:
                    SpawnNormalEnemy();
                    break;
                case EnemyContoller.EnemyType.Fast:
                    SpawnFastEnemy();
                    break;
                case EnemyContoller.EnemyType.Shield:
                    SpawnShieldEnemy();
                    break;
                case EnemyContoller.EnemyType.Wizard:
                    SpawnWizardEnemy();
                    break;
                case EnemyContoller.EnemyType.Cleric:
                    SpawnClericEnemy();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            spawnIndividual += 1;
            if (spawnIndividual == currentWave.parties[spawnBunch].singleParty.Count)
            {
                isSpawningIndividuals = false;
                spawnIndividual = 0;
            }
            currentTime = Time.time;
        }
    }

    private void SpawnWave()
    {
        
    }

    private void CreateAllTheEnemies()
    {
        for (int i = 0; i < allEnemies.Count; i++)
        {
            for (int j = 0; j < createOfEach; j++)
            {
                GameObject leader = Instantiate(allEnemies[i], storeTheEnemies.position, Quaternion.identity, storeTheEnemies);
                LeaderScript leaderScript = leader.GetComponent<LeaderScript>();
                leaderScript.enemyCollider.enabled = false;
                leaderScript.SetEnemyMan(enemyManager);
                switch (leaderScript.enemyCont.enemyType)
                {
                    case EnemyContoller.EnemyType.Normal:
                        enemyManager.normalEnemiesInPool.Add(leaderScript);
                        break;
                    case EnemyContoller.EnemyType.Fast:
                        enemyManager.fastEnemiesInPool.Add(leaderScript);
                        break;
                    case EnemyContoller.EnemyType.Shield:
                        enemyManager.shieldEnemiesInPool.Add(leaderScript);
                        break;
                    case EnemyContoller.EnemyType.Wizard:
                        enemyManager.wizardEnemiesInPool.Add(leaderScript);
                        break;
                    case EnemyContoller.EnemyType.Cleric:
                        enemyManager.clericEnemiesInPool.Add(leaderScript);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                leader.SetActive(false);
            }
        }
    }

    private void SpawnNormalEnemy()
    {
        if (enemyManager.normalEnemiesInPool.Count == 0)
        {
            GameObject newLeader = Instantiate(normalEnemy);
            LeaderScript leaderScript = newLeader.GetComponent<LeaderScript>();
            leaderScript.SetEnemyMan(enemyManager);
            enemyManager.LeadersIsSpawnde(leaderScript);
        }
        else
        {
            LeaderScript leaderScript = enemyManager.normalEnemiesInPool[0];
            enemyManager.normalEnemiesInPool.RemoveAt(0);
            leaderScript.gameObject.SetActive(true);
            enemyManager.LeadersIsSpawnde(leaderScript);
        }
    }

    private void SpawnFastEnemy()
    {
        if (enemyManager.fastEnemiesInPool.Count == 0)
        {
            GameObject newLeader = Instantiate(fastEnemy);
            LeaderScript leaderScript = newLeader.GetComponent<LeaderScript>();
            leaderScript.SetEnemyMan(enemyManager);
            enemyManager.LeadersIsSpawnde(leaderScript);
        }
        else
        {
            LeaderScript leaderScript = enemyManager.fastEnemiesInPool[0];
            enemyManager.fastEnemiesInPool.RemoveAt(0);
            leaderScript.gameObject.SetActive(true);
            enemyManager.LeadersIsSpawnde(leaderScript);
        }
    }

    private void SpawnShieldEnemy()
    {
        if (enemyManager.shieldEnemiesInPool.Count == 0)
        {
            GameObject newLeader = Instantiate(shieldEnemy);
            LeaderScript leaderScript = newLeader.GetComponent<LeaderScript>();
            leaderScript.SetEnemyMan(enemyManager);
            enemyManager.LeadersIsSpawnde(leaderScript);
        }
        else
        {
            LeaderScript leaderScript = enemyManager.shieldEnemiesInPool[0];
            enemyManager.shieldEnemiesInPool.RemoveAt(0);
            leaderScript.gameObject.SetActive(true);
            enemyManager.LeadersIsSpawnde(leaderScript);
        }
    }

    private void SpawnWizardEnemy()
    {
        if (enemyManager.wizardEnemiesInPool.Count == 0)
        {
            GameObject newLeader = Instantiate(wizardEnemy);
            LeaderScript leaderScript = newLeader.GetComponent<LeaderScript>();
            leaderScript.SetEnemyMan(enemyManager);
            enemyManager.LeadersIsSpawnde(leaderScript);
        }
        else
        {
            LeaderScript leaderScript = enemyManager.wizardEnemiesInPool[0];
            enemyManager.wizardEnemiesInPool.RemoveAt(0);
            leaderScript.gameObject.SetActive(true);
            enemyManager.LeadersIsSpawnde(leaderScript);
        }
    }

    private void SpawnClericEnemy()
    {
        if (enemyManager.clericEnemiesInPool.Count == 0)
        {
            GameObject newLeader = Instantiate(clericEnemy);
            LeaderScript leaderScript = newLeader.GetComponent<LeaderScript>();
            leaderScript.SetEnemyMan(enemyManager);
            enemyManager.LeadersIsSpawnde(leaderScript);
        }
        else
        {
            LeaderScript leaderScript = enemyManager.clericEnemiesInPool[0];
            enemyManager.clericEnemiesInPool.RemoveAt(0);
            leaderScript.gameObject.SetActive(true);
            enemyManager.LeadersIsSpawnde(leaderScript);
        }
    }
}
