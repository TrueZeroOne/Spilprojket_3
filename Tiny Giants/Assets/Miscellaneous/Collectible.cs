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
			GetComponent<Collider2D>().enabled = false;
			GameObject collectibleManager = GameObject.Find("CollectibleManager");
			collectibleManager.GetComponent<AudioSource>().Play();
			Destroy(head);
			CollectibleManager cm = collectibleManager.GetComponent<CollectibleManager>();
			body.sprite = deadBodySprite;
			cm.collected++;
			print($"You collected: {cm.collected}/{cm.collectiblesTotal} ({cm.GetCollectiblesLeft()} Left)");
			collectibleManager.GetComponent<CollectibleManager>().UpdateCollectibleUI();
		}
	}
}
