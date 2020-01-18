using UnityEngine;


public class EnemyLaser : MonoBehaviour
{
	bool isColliding;
	EnemyTwinLasers parent;
	[SerializeField] float speed = 8.0f;

	void Start()
	{
		parent = gameObject.GetComponentInParent <EnemyTwinLasers>();
	}

	void Update()
	{
		transform.Translate (Vector3.down * (Time.deltaTime * speed));

		if (transform.position.y < -6f)
		{
			if (parent == null)
			{
				gameObject.SetActive (false);
				return;
			}

			parent.gameObject.SetActive (false);
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (isColliding)
		{
			return;
		}

		if (other.CompareTag ("Player"))
		{
			isColliding = true;
			Player player = other.gameObject.GetComponent <Player>();
			if (player != null)
			{
				player.Damage();
			}

			if (gameObject.activeInHierarchy)
			{
				gameObject.SetActive (false);
			}
		}
	}
}