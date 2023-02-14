using UnityEngine;

public class Collectible : MonoBehaviour
{
	[SerializeField] private GameObject head;
	[SerializeField] private SpriteRenderer body;
	[SerializeField] private Sprite aliveBodySprite;
	[SerializeField] private Sprite deadBodySprite;
	private void Start() => body.sprite = aliveBodySprite;
	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Player"))
		{
			GameObject collectibleManager = GameObject.Find("CollectibleManager");
			collectibleManager.GetComponent<AudioSource>().Play();
			Destroy(head);
			body.sprite = deadBodySprite;
			CollectibleManager.collected++;
			print($"You collected: {CollectibleManager.collected}/{CollectibleManager.collectiblesTotal} ({CollectibleManager.GetCollectiblesLeft()} Left)");
			collectibleManager.GetComponent<CollectibleManager>().UpdateCollectibleUI();
		}
	}
}
