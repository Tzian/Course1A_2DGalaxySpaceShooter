using UnityEngine;


public class PowerUp : MonoBehaviour
{
#pragma warning disable 0649

	// 0 = triple shot, 1 = scatter shot, 2 = shields, 3 = health +1 
	[SerializeField] int powerUpId;
#pragma warning restore
	[SerializeField] float speed = 3;

	void Update()
	{
		transform.Translate (Vector3.down * (speed * Time.deltaTime));

		if (transform.position.y < -6f)
		{
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (!other.CompareTag ("Player"))
		{
			return;
		}

		Player player = other.transform.GetComponent <Player>();
		if (player != null)
		{
			switch (powerUpId)
			{
				case 0:
					player.EnableTripleShot();
					break;

				case 1:
					player.EnableScatterShot();
					break;

				case 2:
					player.EnableShield();
					break;

				case 3:
					player.RepairShip();
					break;
				
				default:
					Debug.Log ("Trying to access a Power Up ID that does not exist!");
					break;
			}
		}

		Destroy (gameObject);
	}
}
