using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
#pragma warning disable 0649
	[SerializeField] GameObject enemyPrefab;
	[SerializeField] GameObject enemySpawns;
	[SerializeField] GameObject tripleShotPowerUp;
	[SerializeField] GameObject scatterShotPowerUp;
	[SerializeField] GameObject shieldPowerUp;
	[SerializeField] GameObject shipRepairPowerUp;
	[SerializeField] GameObject stdAmmoCrate;
#pragma warning restore
	int gameDifficulty;
	int currentWave;
	float difficultyStartWait;

	bool stopSpawning;
	List <GameObject> enemyPool;
	float spawnWait;
	float spawnStartTime;
	int enemySpawnedTally;
	int laserUpgradeLevel;
	int shipBuffLevel;
	float ammoCrateSpawnTimer;
	bool canSpawnAmmoCrate;

	public bool CanSpawnAmmoCrate
	{
		set
		{
			canSpawnAmmoCrate = value;
		}
	}

	void Start()
	{
		stopSpawning      = false;
		currentWave       = 0;
		enemySpawnedTally = 0;
		enemyPool         = new List <GameObject>();
		gameDifficulty = PlayerPrefs.GetInt ("GameDifficulty");
		
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
				Debug.LogError ("You are trying to set an unknown difficulty!!");
				break;
		}
		ammoCrateSpawnTimer = difficultyStartWait * 10;
	}

	public void StartSpawning()
	{
		StartCoroutine (SpawnEnemyRoutine());
		StartCoroutine (SpawnLaserUpgradePowerUpRoutine());
		StartCoroutine (SpawnShipBuffPowerUpRoutine());
		canSpawnAmmoCrate = true;
	}

	void Update()
	{
		if (!canSpawnAmmoCrate)
		{
			return;
		}
		StartCoroutine (StartAmmoCrateSpawnCheckRoutine());
		canSpawnAmmoCrate = false;
	}

	IEnumerator StartAmmoCrateSpawnCheckRoutine()
	{
		yield return new WaitForSeconds (ammoCrateSpawnTimer);
		GameObject ammoCrate = Instantiate (stdAmmoCrate, new Vector3 (Random.Range (-8, 8), Random.Range (-4.5f, -4.0f), 0), Quaternion.identity);
	}

	IEnumerator SpawnEnemyRoutine()
	{
		yield return new WaitForSeconds (2f);

		while (stopSpawning == false)
		{
			if (enemySpawnedTally >= 200)
			{
				currentWave = 4;
			}
			else if (enemySpawnedTally >= 150)
			{
				currentWave = 3;
			}
			else if (enemySpawnedTally >= 100)
			{
				currentWave = 2;
			}
			else if (enemySpawnedTally >= 50)
			{
				currentWave = 1;
			}
			else
			{
				currentWave = 0;
			}
			spawnWait           = difficultyStartWait - currentWave * 0.25f;
			ammoCrateSpawnTimer = spawnWait * 10;
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
				requestedEnemy = Instantiate (enemyPrefab, new Vector3 (Random.Range (-9.5f, 9.5f), 8f, 0), Quaternion.identity, enemySpawns.transform);
				enemyPool.Add (requestedEnemy);
			}

			requestedEnemy.transform.position = new Vector3 (Random.Range (-9.5f, 9.5f), 8f, 0);
			requestedEnemy.gameObject.SetActive (true);
			enemySpawnedTally += 1;
			yield return new WaitForSeconds (spawnWait);
		}
	}

	IEnumerator SpawnLaserUpgradePowerUpRoutine()
	{
		float randomStartSpawnTime = Random.Range (30, 61);
		yield return new WaitForSeconds (randomStartSpawnTime);
		StartCoroutine (LaserUpgradeLevelSetRoutine());
		while (stopSpawning == false)
		{
			switch (laserUpgradeLevel)
			{
				case 0:
					Instantiate (tripleShotPowerUp, new Vector3 (Random.Range (-9.5f, 9.5f), 7.2f, 0), Quaternion.identity);
					break;

				case 1:
					Instantiate (scatterShotPowerUp, new Vector3 (Random.Range (-9.5f, 9.5f), 7.2f, 0), Quaternion.identity);
					laserUpgradeLevel = 0;
					break;
			}
			float randomSpawnTime = Random.Range (30, 61);
			yield return new WaitForSeconds (randomSpawnTime);
		}
	}

	IEnumerator LaserUpgradeLevelSetRoutine()
	{
		float randomSpawnTime = Random.Range (60, 91);
		yield return new WaitForSeconds (randomSpawnTime);
		if (laserUpgradeLevel == 0)
		{
			laserUpgradeLevel = 1;
		}
	}

	IEnumerator SpawnShipBuffPowerUpRoutine()
	{
		float randomStartSpawnTime = Random.Range (40, 71);
		yield return new WaitForSeconds (randomStartSpawnTime);
		StartCoroutine (ShipBuffLevelSetRoutine());
		while (stopSpawning == false)
		{
			switch (shipBuffLevel)
			{
				case 0:
					Instantiate (shieldPowerUp, new Vector3 (Random.Range (-9, 9), 7.2f, 0), Quaternion.identity);
					break;

				case 1:
					Instantiate (shipRepairPowerUp, new Vector3 (Random.Range (-9, 9), 7.2f, 0), Quaternion.identity);
					shipBuffLevel = 0;
					break;
			}
			float randomSpawnTime = Random.Range (40, 71);
			yield return new WaitForSeconds (randomSpawnTime);
		}
	}

	IEnumerator ShipBuffLevelSetRoutine()
	{
		float randomSpawnTime = Random.Range (60, 91);
		yield return new WaitForSeconds (randomSpawnTime);
		if (shipBuffLevel == 0)
		{
			shipBuffLevel = 1;
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
