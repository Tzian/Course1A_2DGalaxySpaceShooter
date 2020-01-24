using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
#pragma warning disable 0649
	[SerializeField] GameObject enemyPrefab;
	[SerializeField] GameObject enemySpawns;
	[SerializeField] GameObject[] stdPowerUps;
	[SerializeField] GameObject shipRepairPowerUp;
	[SerializeField] GameObject scatterShotPowerUp;
#pragma warning restore
	public int gameDifficulty;
	public int currentWave;
	public float difficultyStartWait;
	
	bool stopSpawning;
	List <GameObject> enemyPool;
	float spawnWait;
	float spawnStartTime;
	int enemySpawnedTally;
	
	void Start()
	{
		stopSpawning = false;
		currentWave = 0;
		enemySpawnedTally = 0;
		enemyPool    = new List <GameObject>();
		for (int i = 0; i < 5; i++)
		{
			GameObject newEnemy = Instantiate (enemyPrefab, new Vector3 (Random.Range (-9, 9), 8f, 0), Quaternion.identity, enemySpawns.transform);
			newEnemy.gameObject.SetActive (false);
			enemyPool.Add (newEnemy);
		}
		switch (gameDifficulty)
		{
			case 0:
				difficultyStartWait = 4.5f;
				break;
			case 1:
				difficultyStartWait = 3.5f;
				break;
			case 2:
				difficultyStartWait = 2.5f;
				break;
			default:
				Debug.LogError("You are trying to set an unknown difficulty!!");
				break;
		}
	}

	public void StartSpawning()
	{
		StartCoroutine (SpawnEnemyRoutine());
		StartCoroutine (SpawnPowerUpRoutine());
		StartCoroutine (SpawnSpecialPowerUpRoutine());
	}

	IEnumerator SpawnEnemyRoutine()
	{
		yield return new WaitForSeconds (2f);

		if (enemySpawnedTally >= 400)
		{
			currentWave = 4;
		}
		else if (enemySpawnedTally >= 300)
		{
			currentWave = 3;
		}
		else if (enemySpawnedTally >= 200)
		{
			currentWave = 2;
		}
		else if (enemySpawnedTally >= 100)
		{
			currentWave = 1;
		}
		else
		{
			currentWave = 0;
		}
		while (stopSpawning == false)
		{
			spawnWait = difficultyStartWait - currentWave * 0.5f;

			GameObject requestedEnemy = null;
			foreach (GameObject enemy in enemyPool)
			{
				switch (enemy.gameObject.activeInHierarchy)
				{
					case false:
						requestedEnemy = enemy;
						break;

					case true:
						break;
				}
			}

			if (requestedEnemy == null)
			{
				requestedEnemy = Instantiate (enemyPrefab, new Vector3 (Random.Range (-9, 9), 8f, 0), Quaternion.identity, enemySpawns.transform);
				enemyPool.Add (requestedEnemy);
			}

			requestedEnemy.transform.position = new Vector3 (Random.Range (-9, 9), 8f, 0);
			requestedEnemy.gameObject.SetActive (true);
			enemySpawnedTally += 1;
			yield return new WaitForSeconds (spawnWait);
		}
	}

	IEnumerator SpawnPowerUpRoutine()
	{
		yield return new WaitForSeconds (10f);
		while (stopSpawning == false)
		{
			int randomPowerUpId = Random.Range (0, stdPowerUps.Length);
			Instantiate (stdPowerUps[randomPowerUpId], new Vector3 (Random.Range (-9, 9), 7.2f, 0), Quaternion.identity);
			float randomSpawnTime = Random.Range (5, 11);
			yield return new WaitForSeconds (randomSpawnTime);
		}
		
	}

	IEnumerator SpawnSpecialPowerUpRoutine()
	{
		yield return new WaitForSeconds (30f);
		while (stopSpawning == false)
		{
			int randomSpecialPowerUpId = Random.Range (0, 2);
			if (randomSpecialPowerUpId == 0)
			{
				Instantiate (shipRepairPowerUp, new Vector3 (Random.Range (-9, 9), 7.2f, 0), Quaternion.identity);
			}
			else
			{
				Instantiate (scatterShotPowerUp, new Vector3 (Random.Range (-9, 9), 7.2f, 0), Quaternion.identity);
			}
			float randomSpawnTime = Random.Range (20, 31);
			yield return new WaitForSeconds (randomSpawnTime);
		}
	}

	public void OnPlayerDeath()
	{
		stopSpawning = true;
		foreach (GameObject enemy in enemyPool)
		{
			enemy.gameObject.SetActive (false);
		}
	}
}
