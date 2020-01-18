using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
#pragma warning disable 0649
	[SerializeField] GameObject enemyPrefab;
	[SerializeField] GameObject enemySpawns;
	[SerializeField] GameObject[] powerUps;
#pragma warning restore
	bool stopSpawning;
	List <GameObject> enemyPool;
	float spawnWait = 4f;
	float spawnStartTime;
	int gameDuration;
	
	void Start()
	{
		stopSpawning = false;
		gameDuration = 0;
		enemyPool    = new List <GameObject>();
		for (int i = 0; i < 5; i++)
		{
			GameObject newEnemy = Instantiate (enemyPrefab, new Vector3 (Random.Range (-9, 9), 8f, 0), Quaternion.identity, enemySpawns.transform);
			newEnemy.gameObject.SetActive (false);
			enemyPool.Add (newEnemy);
		}
	}

	public void StartSpawning()
	{
		StartCoroutine (GameDurationTimerRoutine());
		StartCoroutine (SpawnEnemyRoutine());
		StartCoroutine (SpawnPowerUpRoutine());
	}

	IEnumerator GameDurationTimerRoutine()
	{
		while (stopSpawning == false)
		{
			yield return new WaitForSeconds(60f);
			gameDuration += 1;
		}
	}
	
	IEnumerator SpawnEnemyRoutine()
	{
		yield return new WaitForSeconds (2f);

		while (stopSpawning == false)
		{
			switch (gameDuration)
			{
				case 1:
					spawnWait = 3.5f;
					break;
				case 2:
					spawnWait = 3f;
					break;
				case 3:
					spawnWait = 2.5f;
					break;
				case 4:
					spawnWait = 2f;
					break;
				case 6:
					spawnWait = 1.5f;
					break;
				case 8:
					spawnWait = 1f;
					break;
				case 10:
					spawnWait = 0.5f;
					break;
			}

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
			yield return new WaitForSeconds (spawnWait);
		}
	}

	IEnumerator SpawnPowerUpRoutine()
	{
		yield return new WaitForSeconds (10f);
		while (stopSpawning == false)
		{
			int randomPowerUpId = Random.Range (0, powerUps.Length);
			Instantiate (powerUps[randomPowerUpId], new Vector3 (Random.Range (-9, 9), 7.2f, 0), Quaternion.identity);
			float randomSpawnTime = Random.Range (10, 16);
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
