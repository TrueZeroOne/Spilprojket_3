using UnityEngine;

public class Collectible : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Player"))
		{
			GameObject.Find("CollectibleManager").GetComponent<AudioSource>().Play();
			Destroy(gameObject);
			CollectibleManager.collected++;
			print($"You collected: {CollectibleManager.collected}/{CollectibleManager.collectiblesTotal} ({CollectibleManager.GetCollectiblesLeft()} Left)");
		}
	}
}
