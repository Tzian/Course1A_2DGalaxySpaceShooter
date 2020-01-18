using System.Collections.Generic;
using UnityEngine;


public class LaserPools : MonoBehaviour
{
#pragma warning disable 0649
	[SerializeField] GameObject laserPrefab;
	[SerializeField] GameObject tripleShotLaserPrefab;
	[SerializeField] GameObject enemyTwinLaserPrefab;
	[SerializeField] GameObject laserPool;
	[SerializeField] GameObject tripleShotLaserPool;
	[SerializeField] GameObject enemyTwinLaserPool;
#pragma warning restore
	List <GameObject> laserPoolList;
	List <GameObject> tripleShotLaserPoolList;
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

		enemyTwinLaserPoolList = new List <GameObject>();
		for (int i = 0; i < 10; i++)
		{
			GameObject enemyLaser = Instantiate (enemyTwinLaserPrefab, transform.position, Quaternion.identity, enemyTwinLaserPool.transform);
			enemyLaser.gameObject.SetActive (false);
			enemyTwinLaserPoolList.Add (enemyLaser);
		}
	}

	public GameObject FireLaser()
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