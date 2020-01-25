using UnityEngine;


[RequireComponent (typeof(AudioSource))]
public class ExplosionTimer : MonoBehaviour
{
#pragma warning disable 0649
	[SerializeField] AudioClip audioClip;
#pragma warning restore

	void Start()
	{
		AudioSource.PlayClipAtPoint (audioClip, transform.position);
		Destroy (gameObject, 2.5f);
	}
}