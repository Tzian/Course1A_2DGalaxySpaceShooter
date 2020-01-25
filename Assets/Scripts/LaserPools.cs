using System.Collections.Generic;
using UnityEngine;


public class LaserPools : MonoBehaviour
{
#pragma warning disable 0649
	[SerializeField] GameObject laserPrefab;
	[SerializeField] GameObject tripleShotLaserPrefab;
	[SerializeField] GameObject scatterShotLaserPrefab;
	[SerializeField] GameObject enemyTwinLaserPrefab;
	[SerializeField] GameObject laserPool;
	[SerializeField] GameObject tripleShotLaserPool;
	[SerializeField] GameObject scatterShotLaserPool;
	[SerializeField] GameObject enemyTwinLaserPool;
#pragma warning restore
	List <GameObject> laserPoolList;
	List <GameObject> tripleShotLaserPoolList;
	List <GameObject> scatterShotLaserPoolList;
	List <GameObject> enemyTwinLaserPoolList;

	void Start()
	{
		laserPoolList = new List <GameObject>();
		for (int i = 0; i < 10; i++)
		{
			GameObject newLaser = Instantiate (laserPrefab, transform.position, Quaternion.identity, laserPool.transform);
			newLaser.gameObject.SetActive (false);
			laserPoolList.Add (newLaser);
		}

		tripleShotLaserPoolList = new List <GameObject>();
		for (int i = 0; i < 10; i++)
		{
			GameObject newTripleShot = Instantiate (tripleShotLaserPrefab, transform.position, Quaternion.identity, tripleShotLaserPool.transform);
			newTripleShot.gameObject.SetActive (false);
			tripleShotLaserPoolList.Add (newTripleShot);
		}

		scatterShotLaserPoolList = new List <GameObject>();
		for (int i = 0; i < 10; i++)
		{
			GameObject newScatterShot = Instantiate (scatterShotLaserPrefab, transform.position, Quaternion.identity, scatterShotLaserPool.transform);
			newScatterShot.gameObject.SetActive (false);
			scatterShotLaserPoolList.Add (newScatterShot);
		}

		enemyTwinLaserPoolList = new List <GameObject>();
		for (int i = 0; i < 10; i++)
		{
			GameObject enemyLaser = Instantiate (enemyTwinLaserPrefab, transform.position, Quaternion.identity, enemyTwinLaserPool.transform);
			enemyLaser.gameObject.SetActive (false);
			enemyTwinLaserPoolList.Add (enemyLaser);
		}
	}

	public GameObject FireSingleShotLaser()
	{
		foreach (GameObject laser in laserPoolList)
		{
			if (laser.activeInHierarchy == false)
			{
				return laser;
			}
		}

		GameObject newLaser = Instantiate (laserPrefab, transform.position, Quaternion.identity, laserPool.transform);
		newLaser.SetActive (false);
		laserPoolList.Add (newLaser);
		return newLaser;
	}

	public GameObject FireTripleShotLaser()
	{
		foreach (GameObject tripleShot in tripleShotLaserPoolList)
		{
			if (tripleShot.activeInHierarchy == false)
			{
				return tripleShot;
			}
		}

		GameObject newTripleShot = Instantiate (tripleShotLaserPrefab, transform.position, Quaternion.identity, tripleShotLaserPool.transform);
		newTripleShot.SetActive (false);
		tripleShotLaserPoolList.Add (newTripleShot);
		return newTripleShot;
	}

	public GameObject FireScatterShotLaser()
	{
		foreach (GameObject scatterShot in scatterShotLaserPoolList)
		{
			if (scatterShot.activeInHierarchy == false)
			{
				return scatterShot;
			}
		}

		GameObject newScatterShot = Instantiate (scatterShotLaserPrefab, transform.position, Quaternion.identity, scatterShotLaserPool.transform);
		newScatterShot.SetActive (false);
		scatterShotLaserPoolList.Add (newScatterShot);
		return newScatterShot;
	}

	public GameObject FireEnemyLaser()
	{
		foreach (GameObject enemyLaser in enemyTwinLaserPoolList)
		{
			if (enemyLaser.activeInHierarchy == false)
			{
				return enemyLaser;
			}
		}

		GameObject newEnemyLaser = Instantiate (enemyTwinLaserPrefab, transform.position, Quaternion.identity, enemyTwinLaserPool.transform);
		newEnemyLaser.SetActive (false);
		enemyTwinLaserPoolList.Add (newEnemyLaser);
		return newEnemyLaser;
	}
}