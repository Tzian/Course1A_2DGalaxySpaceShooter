using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
#pragma warning disable 0649
	[SerializeField] GameObject playerShield;
	[SerializeField] GameObject thrusters;
	[SerializeField] GameObject[] engines;
	[SerializeField] AudioClip fuelRechargeAudioClip;
	[SerializeField] AudioClip powerUpGainedAudioClip;
	[SerializeField] AudioClip singleShotAudioClip;
	[SerializeField] AudioClip tripleShotAudioClip;
	[SerializeField] AudioClip scatterShotAudioClip;
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
	LaserPools laserPools;
	int score;

	[SerializeField] bool scatterShotEnabled;
	[SerializeField] bool tripleShotEnabled;
	[SerializeField] bool shieldEnabled;

	public int thrusterFuel;
	public bool fuelRecharging;
	public bool speedBoostEnabled;
	public int shieldStrength;
	SpriteRenderer shieldRenderer;

	bool thrustersOnCd;
	int stdAmmoCount;

	
	void Start()
	{
		score        = 0;
		thrusterFuel = 100;
		fuelRecharging = false;
		shieldStrength = 0;
		thrustersOnCd = false;
		stdAmmoCount = 15;
		
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
		shieldRenderer = playerShield.GetComponent <SpriteRenderer>();
	}

	void Update()
	{
		horizontalInput = Input.GetAxis ("Horizontal");
		verticalInput   = Input.GetAxis ("Vertical");

		ConstrainPlayer();
		
		if (Time.time > canFire)
		{
			if (Input.GetKeyDown (KeyCode.Space) || Input.GetMouseButtonDown (0))
			{
				if (stdAmmoCount > 0)
				{
					if (scatterShotEnabled)
					{
						SpawnScatterShotLaser();
					}
					else if (tripleShotEnabled)
					{
						SpawnTripleShotLaser();
					}
					else
					{
						SpawnSingleShotLaser();
					}
				}
				uiManager.UpdateStandardAmmoCount (stdAmmoCount);
			}
		}
		
		uiManager.UpdateThrusterFuel(thrusterFuel);

		if (Input.GetKeyDown (KeyCode.LeftShift) && fuelRecharging == false)
		{
			EnableSpeedBoost();
		}
		
		if (Input.GetKeyUp (KeyCode.LeftShift))
		{
			DisableSpeedBoost();
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

	public void Damage()
	{
		if (shieldEnabled)
		{
			shieldStrength -= 1;
			UpdateShieldStrengthImage();

			if (shieldStrength <= 0)
			{
				shieldEnabled = false;
				playerShield.SetActive (false);
			}
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

	public void StdAmmoPickedUp()
	{
		spawnManager.canSpawnAmmoCrate = true;
		stdAmmoCount = 15;
		uiManager.UpdateStandardAmmoCount (stdAmmoCount);
	}
	
#region shieldsAndRepair
	
	public void EnableShield()
	{
		AudioSource.PlayClipAtPoint (powerUpGainedAudioClip, transform.position);
		shieldStrength = 3;
		UpdateShieldStrengthImage();
		shieldEnabled = true;
		playerShield.SetActive (true);
	}

	public void RepairShip()
	{
		AudioSource.PlayClipAtPoint (powerUpGainedAudioClip, transform.position);
		if (lives == 3 || lives == 0)
		{
			return;
		}
		lives += 1;
		uiManager.UpdateLivesImage (lives);

		switch (lives)
		{
			case 2:
				engines[Random.Range (0, 2)].SetActive (false);
				break;

			case 3:
				if (engines[1].activeInHierarchy)
				{
					engines[1].SetActive (false);
				}
				else
				{
					engines[0].SetActive (false);
				}

				break;
		}
	}

	void UpdateShieldStrengthImage()
	{
		switch (shieldStrength)
		{
			case 3:
				shieldRenderer.color = new Color (1, 1, 1, 1);
				break;

			case 2:
				shieldRenderer.color = new Color (0.6f, 0.6f, 0.6f, 1);
				break;

			case 1:
				shieldRenderer.color = new Color (0.3f, 0.3f, 0.3f, 1);
				break;
		}
	}

#endregion
	
	
#region Thrusters

	void EnableSpeedBoost()
	{
		speedBoostEnabled = true;
		thrusters.SetActive (true);
		speed *= 2;
		StartCoroutine (DepleteThrusterFuel());
	}

	void DisableSpeedBoost()
	{
		if (speedBoostEnabled == false)
		{
			return;
		}
		speedBoostEnabled = false;
		thrusters.SetActive (false);
		speed /= 2;
		StopCoroutine (DepleteThrusterFuel());
	}

	IEnumerator DepleteThrusterFuel()
	{
		while (thrusters.activeInHierarchy)
		{
			yield return new WaitForSeconds (0.5f);
			thrusterFuel -= 10;
			if (thrusterFuel <= 0 && thrustersOnCd == false)
			{
				thrusterFuel  = 0;
				thrustersOnCd = true;
				yield return StartCoroutine (ThrusterFuelRechargeCd());
			}
		}
	}

	IEnumerator ThrusterFuelRechargeCd()
	{
		DisableSpeedBoost();
		fuelRecharging = true;

		yield return new WaitForSeconds (10f);
		yield return StartCoroutine (RechargeFuel());
	}

	IEnumerator RechargeFuel()
	{
		AudioSource.PlayClipAtPoint (fuelRechargeAudioClip, transform.position);
		while (thrusterFuel != 100)
		{
			yield return new WaitForSeconds (0.05f);
			thrusterFuel += 10;
		}
		fuelRecharging = false;
		thrustersOnCd  = false;
	}

#endregion

	
#region Lasers

	public void EnableTripleShot()
	{
		AudioSource.PlayClipAtPoint (powerUpGainedAudioClip, transform.position);
		tripleShotEnabled = true;
		StartCoroutine (TripleShotTimer());
	}

	IEnumerator TripleShotTimer()
	{
		yield return new WaitForSeconds (10f);
		tripleShotEnabled = false;
	}

	public void EnableScatterShot()
	{
		AudioSource.PlayClipAtPoint (powerUpGainedAudioClip, transform.position);
		scatterShotEnabled = true;
		StartCoroutine (ScatterShotTimer());
	}

	IEnumerator ScatterShotTimer()
	{
		yield return new WaitForSeconds (5f);
		scatterShotEnabled = false;
	}
	
	void SpawnSingleShotLaser()
	{
		canFire      =  Time.time + fireRate;
		stdAmmoCount -= 1;
		GameObject singleShotLaser = laserPools.FireSingleShotLaser();
		singleShotLaser.SetActive (true);
		singleShotLaser.transform.position = transform.position + laserSpawnOffset;
		AudioSource.PlayClipAtPoint (singleShotAudioClip, singleShotLaser.transform.position);
	}

	void SpawnTripleShotLaser()
	{
		canFire      =  Time.time + fireRate;
		stdAmmoCount -= 1;
		GameObject tripleShotLaser = laserPools.FireTripleShotLaser();
		tripleShotLaser.SetActive (true);
		tripleShotLaser.transform.position = transform.position;
		AudioSource.PlayClipAtPoint (tripleShotAudioClip, tripleShotLaser.transform.position);
	}

	void SpawnScatterShotLaser()
	{
		canFire      =  Time.time + fireRate;
		stdAmmoCount -= 1;
		GameObject fireScatterShotLaser = laserPools.FireScatterShotLaser();
		fireScatterShotLaser.SetActive (true);
		fireScatterShotLaser.transform.position = transform.position;
		AudioSource.PlayClipAtPoint (scatterShotAudioClip, fireScatterShotLaser.transform.position);
	}

#endregion


#region Player Movement

	void MovePlayer()
	{
		Vector3 direction = new Vector3 (horizontalInput, verticalInput, 0);
		transform.Translate (direction * (speed * Time.deltaTime));
	}

	void ConstrainPlayer()
	{
		Vector3 position = transform.position;
		transform.position = new Vector3 (position.x, Mathf.Clamp (position.y, -3.9f, 0), 0);

		if (position.x >= 10.2f)
		{
			transform.position = new Vector3 (-10.2f, position.y, 0);
		}
		else if (position.x <= -10.2f)
		{
			transform.position = new Vector3 (10.2f, position.y, 0);
		}
	}

#endregion

}
