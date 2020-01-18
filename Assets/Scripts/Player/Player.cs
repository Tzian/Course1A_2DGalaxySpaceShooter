using System.Collections;
using UnityEngine;


public class Player : MonoBehaviour
{
#pragma warning disable 0649
	[SerializeField] GameObject playerShield;
	[SerializeField] GameObject thrusters;
	[SerializeField] GameObject[] engines;
	[SerializeField] AudioClip laserAudioClip;
	[SerializeField] AudioClip powerUpClip;
#pragma warning restore
	[SerializeField] float speed = 3.5f;
	[SerializeField] float fireRate = 0.15f;
	[SerializeField] int lives = 3;

	float canFire = -1f;
	float horizontalInput;
	float verticalInput;

	readonly Vector3 laserSpawnOffset = new Vector3 (0.003f, 1.0f, 0);
	SpawnManager spawnManager;
	GameManager gameManager;
	UiManager uiManager;
	AudioSource audioSource;
	LaserPools laserPools;
	int score;

	[SerializeField] bool tripleShotEnabled;
	[SerializeField] bool shieldEnabled;

	void Start()
	{
		score        = 0;
		gameManager = GameObject.Find ("GameManager").GetComponent <GameManager>();
		if (gameManager == null)
		{
			Debug.LogError ("Game Manager is NULL");
		}
		
		spawnManager = GameObject.Find ("SpawnManager").GetComponent <SpawnManager>();
		if (spawnManager == null)
		{
			Debug.LogError ("Spawn Manager Not Found!!");
		}

		uiManager = GameObject.Find ("UiManager").GetComponent <UiManager>();
		if (uiManager == null)
		{
			Debug.LogError ("Ui Manager Not Found!!");
		}

		uiManager.UpdateLivesImage (lives);

		laserPools = GameObject.Find ("LaserPools").GetComponent <LaserPools>();
		if (laserPools == null)
		{
			Debug.LogError ("Laser Pools Not Found!!");
		}

		transform.position = new Vector3 (0, -3.5f, 0);

		audioSource = GetComponent <AudioSource>();
		if (audioSource == null)
		{
			Debug.LogError ("Audio Manager Not Found!!");
		}
	}

	void Update()
	{
		horizontalInput = Input.GetAxis ("Horizontal");
		verticalInput   = Input.GetAxis ("Vertical");

		ConstrainPlayer();

		if (!(Time.time > canFire))
		{
			return;
		}

		if (Input.GetKeyDown (KeyCode.Space) || Input.GetMouseButtonDown (0))
		{
			audioSource.clip = laserAudioClip;
			if (tripleShotEnabled)
			{
				SpawnTripleShot();
				audioSource.Play();
			}
			else
			{
				SpawnLaser();
				audioSource.Play();
			}
		}
	}

	void FixedUpdate()
	{
		MovePlayer();
	}

	public void UpdateScore (int value)
	{
		score += value;
		uiManager.UpdateScoreText (score);
		gameManager.UpdateScore (score);
	}

	public void EnableTripleShot()
	{
		tripleShotEnabled = true;
		audioSource.clip  = powerUpClip;
		audioSource.Play();
		StartCoroutine (TripleShotTimer());
	}

	IEnumerator TripleShotTimer()
	{
		yield return new WaitForSeconds (10f);
		tripleShotEnabled = false;
	}

	public void EnableSpeedBoost()
	{
		thrusters.SetActive (true);
		audioSource.clip = powerUpClip;
		audioSource.Play();
		speed *= 2;
		StartCoroutine (SpeedBoostTimer());
	}

	IEnumerator SpeedBoostTimer()
	{
		yield return new WaitForSeconds (10f);
		speed /= 2;
		thrusters.SetActive (false);
	}

	public void EnableShield()
	{
		shieldEnabled    = true;
		audioSource.clip = powerUpClip;
		audioSource.Play();
		playerShield.SetActive (true);
	}

	public void Damage()
	{
		if (shieldEnabled)
		{
			shieldEnabled = false;
			playerShield.SetActive (false);
			return;
		}

		lives -= 1;

		switch (lives)
		{
			case 2:
				engines[Random.Range (0, 2)].SetActive (true);
				break;

			case 1:
				if (engines[0].activeInHierarchy)
				{
					engines[1].SetActive (true);
				}
				else
				{
					engines[0].SetActive (true);
				}

				break;

			default:
			{
				if (lives <= 0)
				{
					gameManager.UpdateScore(score);
					
					spawnManager.OnPlayerDeath();
					Destroy (gameObject);
				}

				break;
			}
		}

		uiManager.UpdateLivesImage (lives);
	}

	void SpawnLaser()
	{
		canFire = Time.time + fireRate;

		GameObject laser = laserPools.FireLaser();
		laser.SetActive (true);
		laser.transform.position = transform.position + laserSpawnOffset;
	}

	void SpawnTripleShot()
	{
		canFire = Time.time + fireRate;

		GameObject tripleShotLaser = laserPools.FireTripleShotLaser();
		tripleShotLaser.SetActive (true);
		tripleShotLaser.transform.position = transform.position;
	}

	void MovePlayer()
	{
		Vector3 direction = new Vector3 (horizontalInput, verticalInput, 0);
		transform.Translate (direction * (speed * Time.deltaTime));
	}

	void ConstrainPlayer()
	{
		Vector3 position = transform.position;
		transform.position = new Vector3 (position.x, Mathf.Clamp (position.y, -3.9f, 0), 0);

		if (position.x >= 11.2f)
		{
			transform.position = new Vector3 (-11.2f, position.y, 0);
		}
		else if (position.x <= -11.2f)
		{
			transform.position = new Vector3 (11.2f, position.y, 0);
		}
	}
}
