 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Levels : MonoBehaviour {

   public  TextMeshProUGUI levelText; // Text displaying level number at the start of the wave


    
    public EnemyWave[] Waves; // array of enemySpawns (class below) holding enemy prefab, where to spawn, where to jump to, and time to wait before spawning next enemy in the array

    public GameObject[] currentWaveEnemies = new GameObject[0]; // array holding all the currently active enemies

    
    int currentLevel = 0;
    bool currentWaveSpawned = false;
    

    public GameController gc;


    AudioSource levelStartSound;

	// Use this for initialization
	void Start () {
        levelStartSound = GetComponent<AudioSource>();
        startNextLevel();
        
	}
	
	// Update is called once per frame
	void Update () {
        if (currentWaveSpawned)
        {
            if (CheckIfAllEnemiesDead())
            {
                if (currentLevel < 10)
                {
                    startNextLevel();
                }
                else {
                    gc.YouWin();
                }

            }
        }
		
	}
    //Starts the Level1 Coroutine
    public void startNextLevel()
    {
        levelStartSound.Play();
        currentWaveSpawned = false;
        currentLevel += 1;
        StartCoroutine(NextLevel());
    }
    
    // shows the player the level number and then spawns the wave of enemies for next level
    IEnumerator NextLevel()
    {
        levelText.text = "Level " + currentLevel.ToString();
        while (levelText.color.a < .8) { levelText.color = new Color(levelText.color.r, levelText.color.g, levelText.color.b, levelText.color.a + (Time.deltaTime)); yield return null; } // slowly make level text opaque
        yield return new WaitForSeconds(3); 
        while (levelText.color.a > 0) {  levelText.color = new Color(levelText.color.r, levelText.color.g, levelText.color.b, levelText.color.a - (Time.deltaTime)); yield return null; } // slowly make level text transparent
        StartCoroutine(SpawnEnemies(Waves[currentLevel - 1])); 
    }
  
    // Spawns all enemies for the given level array
    IEnumerator SpawnEnemies(EnemyWave enemies)
    {
        currentWaveEnemies = new GameObject[enemies.listOfEnemies.Length]; // clears the array of current enemies and redefines the array to have the length of the new wave

        for (int i = 0; i < enemies.listOfEnemies.Length ; i++) // for every enemy in the enemies array
        {

            Transform spawnPosition = new GameObject().transform;
            spawnPosition.position = enemies.listOfEnemies[i].spawn; // make a new transform with the enemy spawn position
            currentWaveEnemies[i] = Instantiate(enemies.listOfEnemies[i].enemy, spawnPosition.position, Quaternion.identity); // create the enemy with the spawn position
            currentWaveEnemies[i].GetComponentInChildren<WarpIn>().warpInLookingAt = enemies.listOfEnemies[i].lookAt; // set the warp target of the enemy
            currentWaveEnemies[i].SetActive(false); // set the enemy to inactive
            Destroy(spawnPosition.gameObject); // destroy the spawnPosition gameobject so it doenst clutter the heirarchy
            
        }
        for (int i = 0; i < enemies.listOfEnemies.Length; i++) // for every enemy in the enemies array
        {
            currentWaveEnemies[i].SetActive(true); 
            yield return new WaitForSeconds(enemies.listOfEnemies[i].timeToNextSpawn); // wait to spawn the next enemy until the specified timer
        }
        currentWaveSpawned = true;
        yield return null;
    }

    bool CheckIfAllEnemiesDead()
    {
        bool enemiesDead = true;

        for (int i = 0; i< currentWaveEnemies.Length; i++)
        {
            if (currentWaveEnemies[i] != null)
            {
                enemiesDead = false;
            }
        }

        return enemiesDead;
    }

    void YouWin()
    {

    }

}
[System.Serializable]
public class EnemySpawn
{
    public GameObject enemy;
    public Vector3 spawn, lookAt;
    public float timeToNextSpawn;
}
[System.Serializable]
public class EnemyWave
{
    public EnemySpawn[] listOfEnemies;
}