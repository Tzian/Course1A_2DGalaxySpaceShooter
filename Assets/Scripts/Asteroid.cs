using System.Collections;
using UnityEngine;


public class Asteroid : MonoBehaviour
{
#pragma warning disable 0649
	[SerializeField] GameObject explosion;
#pragma warning restore
	SpawnManager spawnManager;
	float rotateSpeed;
	bool isColliding;
	int randomDir;

	void Awake()
	{
		spawnManager = GameObject.Find ("SpawnManager").GetComponent <SpawnManager>();
		if (spawnManager == null)
		{
			Debug.LogError ("Spawn Manager Not Found!!");
		}

		rotateSpeed = Random.Range (5, 10f);
		randomDir   = Random.Range (0, 2);
		isColliding = false;
	}

	void Update()
	{
		if (randomDir == 0)
		{
			transform.Rotate (Vector3.forward * (rotateSpeed * Time.deltaTime));
		}
		else
		{
			transform.Rotate (Vector3.back * (rotateSpeed * Time.deltaTime));
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (!other.CompareTag ("Laser") || isColliding)
		{
			return;
		}
		isColliding = true;
		other.gameObject.SetActive (false);

		Instantiate (explosion, transform.position, Quaternion.identity);
		StartCoroutine (DelayDeactivate());
		spawnManager.StartSpawning();
	}

	IEnumerator DelayDeactivate()
	{
		yield return new WaitForSeconds (0.3f);
		gameObject.SetActive (false);
	}
}