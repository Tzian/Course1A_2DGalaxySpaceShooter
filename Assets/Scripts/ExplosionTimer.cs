using UnityEngine;


public class ExplosionTimer : MonoBehaviour
{
	void Start()
	{
		Destroy (gameObject, 2.5f);
	}
}
