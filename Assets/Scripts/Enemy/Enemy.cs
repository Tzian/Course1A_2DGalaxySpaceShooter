﻿using System.Collections;
using UnityEngine;


public class Enemy : MonoBehaviour
{
	static readonly int OnEnemyDeath = Animator.StringToHash ("OnEnemyDeath");
	Animator animator;
	float canFire = -1;
	Collider2D enemyCollider;
	AudioSource explosionAudio;
	float fireRate;
	bool isColliding;
	LaserPools laserPools;
	Player player;
	[SerializeField] float speed = 3f;
	SpriteRenderer spriteRenderer;
	Sprite startingSprite;
	bool stopFiring;

	void Awake()
	{
		player = GameObject.Find ("Player").GetComponent <Player>();
		if (player == null)
		{
			Debug.LogError ("Player Not Found!!");
		}

		animator = GetComponent <Animator>();
		if (animator == null)
		{
			Debug.LogError ("Cannot get Animator!");
		}

		enemyCollider = GetComponent <Collider2D>();
		if (enemyCollider == null)
		{
			Debug.LogError ("Cannot get collider!");
		}

		explosionAudio = GameObject.Find ("AudioManager").GetComponent <AudioSource>();
		if (explosionAudio == null)
		{
			Debug.LogError ("Explosion Audio Source Not Found!!");
		}

		spriteRenderer = GetComponent <SpriteRenderer>();
		if (spriteRenderer == null)
		{
			Debug.LogError ("Sprite Renderer Not Found!!");
		}

		startingSprite = spriteRenderer.sprite;

		laserPools = GameObject.Find ("LaserPools").GetComponent <LaserPools>();
		if (laserPools == null)
		{
			Debug.LogError ("Laser Pools Not Found!!");
		}
	}

	void Update()
	{
		fireRate = Random.Range (2, 4f);
		transform.Translate (Vector3.down * (speed * Time.deltaTime));

		if (transform.position.y < -4f && gameObject.activeInHierarchy)
		{
			transform.position = new Vector3 (Random.Range (-9, 10), 7.2f, 0);
		}

		if (!(Time.time > canFire) || transform.position.y > 6f || transform.position.y < 0)
		{
			return;
		}

		EnemyFire();
	}

	void EnemyFire()
	{
		if (stopFiring)
		{
			return;
		}

		canFire = Time.time + fireRate;

		GameObject enemyLaser = laserPools.FireEnemyLaser();
		enemyLaser.SetActive (true);
		enemyLaser.transform.position = transform.position;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (isColliding || transform.position.y > 7f)
		{
			return;
		}

		if (other.CompareTag ("Player"))
		{
			stopFiring  = true;
			isColliding = true;
			if (player != null)
			{
				player.Damage();
			}

			if (gameObject.activeInHierarchy)
			{
				StartCoroutine (DeactivateEnemy());
			}
		}
		else if (other.CompareTag ("Laser"))
		{
			stopFiring  = true;
			isColliding = true;
			other.gameObject.SetActive (false);
			UpdateScore();
			if (gameObject.activeInHierarchy)
			{
				StartCoroutine (DeactivateEnemy());
			}
		}
	}

	IEnumerator DeactivateEnemy()
	{
		animator.SetTrigger (OnEnemyDeath);
		explosionAudio.Play();
		speed = 0;
		yield return new WaitForSeconds (2.37f);
		gameObject.SetActive (false);
		spriteRenderer.sprite = startingSprite;
		isColliding           = false;
		speed                 = 3f;
		stopFiring            = false;
	}

	void UpdateScore()
	{
		if (player != null)
		{
			player.UpdateScore (10);
		}
	}
}