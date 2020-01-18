using UnityEngine;


public class Laser : MonoBehaviour
{
	TripleShot parent;
	[SerializeField] float speed = 8.0f;

	void Start()
	{
		parent = gameObject.GetComponentInParent <TripleShot>();
	}

	void Update()
	{
		transform.Translate (Vector3.up * (Time.deltaTime * speed));
		if (transform.position.y > 10.0f)
		{
			if (parent == null)
			{
				gameObject.SetActive (false);
				return;
			}

			parent.gameObject.SetActive (false);
		}
	}
}