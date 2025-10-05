using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieSpawnController : MonoBehaviour
{
    public int initialZombiesPerWave = 5;
    public int currentZombiePerWave;

    public float spawnDelay = 0.5f; // Delay between spawning zombies

    public int currentWave = 0;
    public float waveCooldown = 10f;
    public bool inCooldown;
    public float cooldownCounter = 0; // We only use this for testing and UI;
    
    public List<Enemy> currentZombiesAlive;

    public GameObject zombiePrefab;

    public TextMeshProUGUI titleWaveOverUI;
    public TextMeshProUGUI cooldownCounterTitleUI;
    public TextMeshProUGUI currentWaveUI;

    private void Start()
    {
        currentZombiePerWave = initialZombiesPerWave;
        GlobalReferences.Instance.waveNumber = currentWave;
        StartNextWave();
    }

    private void StartNextWave()
    {
        currentZombiesAlive.Clear();
        currentWave++;
        GlobalReferences.Instance.waveNumber = currentWave;
        currentWaveUI.text = "Wave:" + currentWave.ToString("F0");
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentZombiePerWave; i++)
        {
            //Generate RandomOffset within a specified range
            Vector3 spawnOffset = new Vector3(Random.Range(-1f,1f), 0f, Random.Range(-1f,1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            //Instantiate the Zombie
            var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

            //Get Enemy Script
            Enemy enemyScript = zombie.GetComponent<Enemy>();

            // Track this zombie
            currentZombiesAlive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Update()
    {
        //Get All Dead Zombies
        List<Enemy> zombiesToRemove = new List<Enemy>();
        foreach (Enemy zombie in currentZombiesAlive)
        {
            if (zombie.isDead)
            {
                zombiesToRemove.Add(zombie);
            }
        }
   

    //Actually remove the zombie

        foreach (Enemy zombie in zombiesToRemove)
        {
            currentZombiesAlive.Remove(zombie);

        }
        zombiesToRemove.Clear();

        //Start cooldown if all zombies are dead
        if(currentZombiesAlive.Count <= 0 && inCooldown == false)
        {
            //Start Cooldown
            StartCoroutine(WaveCooldown());
        }
        
        if (inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
        }
        else
        {
            //Reset counter
            cooldownCounter = waveCooldown;
        }
    cooldownCounterTitleUI.text = cooldownCounter.ToString("F0");
   }

    private IEnumerator WaveCooldown()
    {
        inCooldown = true;
        titleWaveOverUI.gameObject.SetActive(true);
        cooldownCounterTitleUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(waveCooldown);

        inCooldown = false;
        titleWaveOverUI.gameObject.SetActive(false);
        cooldownCounterTitleUI.gameObject.SetActive(false);

        currentZombiePerWave *= 2; // multiply by two
        StartNextWave();

    }
}
