using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WaveManager : MonoBehaviour
{
    [Header("Manager")] [SerializeField] private EnemyManager enemyManager;

    [Header("The Important Values")] [SerializeField]
    private int waitTimeBetweenWaves;
    [SerializeField] private float currentTime;
    public int waveCounter = 0;
    [SerializeField] private int identifySpawnIndividual = 0;
    [SerializeField] private bool isSpawningIndividuals = false;
    [SerializeField] private bool isSpawningNextBunch = false;
    [SerializeField] private bool isSpawningNextWave = false;
    [SerializeField] private int identifySpawnBunch;
    [SerializeField] private int bunchRepeatsCount;
    [SerializeField] private int listRepeatsCount;
    
    [Header("Waves")]
    public Wave currentWave;
    [SerializeField] private Wave waveOne, waveTwo, waveThree, waveFour, waveFive;

    [SerializeField] private int createOfEach;

    [Header("Enemy Prefabs")] [SerializeField]
    private GameObject fastEnemy, normalEnemy, shieldEnemy, wizardEnemy, clericEnemy;
    [SerializeField] private List<GameObject> allEnemies;

    [SerializeField] private Transform storeTheEnemies;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      CreateAllTheEnemies();
      //currentWave = waveOne;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (isSpawningNextWave)
        {
            CountDownWave();
        }
        else if (isSpawningNextBunch)
        {
            CountDownBunch();
        }
        else if (isSpawningIndividuals)
        {
            CountDownIndividual();
        }
    }

    private void CountDownWave()
    {
        float difference = Time.time - currentTime;
        if (difference > waitTimeBetweenWaves)
        {
            NextWave();
        }
    }

    private void CountDownBunch()
    {
        float difference = Time.time - currentTime;
        if (difference >= currentWave.spawnRatesBetweenParty)
        {
            Debug.Log("Hello");
            isSpawningNextBunch = false;
            isSpawningIndividuals = true;
        }
    }

    private void CountDownIndividual()
    {
        float difference = Time.time - currentTime;
        if (difference >= currentWave.parties[identifySpawnBunch].spawnRateForIndividuals)
        {
           SpawnIndividual();
        }
    }

    private void SpawnIndividual()
    {
        switch (currentWave.parties[identifySpawnBunch].singleParty[identifySpawnIndividual])
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
        
        identifySpawnIndividual += 1;
        if (identifySpawnIndividual >= currentWave.parties[identifySpawnBunch].singleParty.Count)
        {
           // Debug.Log("Gone Through the party");
            identifySpawnIndividual = 0;
            bunchRepeatsCount += 1;
            if (bunchRepeatsCount >= currentWave.parties[identifySpawnBunch].repeatAmountForParty)
            {
               // Debug.Log("Did the party Multiple times");
                bunchRepeatsCount = 0;
                currentTime = Time.time;
                isSpawningNextBunch = true;
                isSpawningIndividuals = false;
                identifySpawnBunch += 1;
                if (identifySpawnBunch >= currentWave.parties.Count)
                {
                    //Debug.Log("BottleNeck?");
                    identifySpawnBunch = 0;
                    listRepeatsCount += 1;
                    if (listRepeatsCount >= currentWave.repeatThroughList)
                    {
                        listRepeatsCount = 0;
                        isSpawningNextBunch = false;
                        isSpawningIndividuals = false;
                        isSpawningNextWave = true;
                       // Debug.Log("EndWave");
                        return;
                    }
                }
                currentTime = Time.time;
                return;
            }
        }
        currentTime = Time.time;
    }

    private void NextWave()
    {
        currentTime = Time.time;
        isSpawningNextWave = false;
        isSpawningNextBunch = false;
        isSpawningIndividuals = true;
        currentTime = Time.time;
        waveCounter += 1;
        switch (waveCounter)
        {
            case(1):
                currentWave = waveOne;
                break;
            case(2):
                currentWave = waveTwo;
                break;
            case (3):
                currentWave = waveThree;
                break;
            case (4):
                currentWave = waveFour;
                break;
            case (5):
                currentWave = waveFive;
                break;
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
