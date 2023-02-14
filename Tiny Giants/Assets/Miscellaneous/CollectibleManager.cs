using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
	[SerializeField] private TMP_Text collectedText;
	private int collectiblesTotalPrivate;

	public void UpdateCollectibleUI() => collectedText.text = $"{collected}/{collectiblesTotal}";

	public int collectiblesTotal => collectiblesTotalPrivate;

	public int collected;

	private void Start() => FindAllCollectibles();

	private void FindAllCollectibles()
	{
		List<Collectible> collectibles = new List<Collectible>(FindObjectsOfType<Collectible>());
		collectibles.Clear();
		collectiblesTotalPrivate = 0;
		collectibles = new List<Collectible>(FindObjectsOfType<Collectible>());
		collectiblesTotalPrivate = collectibles.Count;
		UpdateCollectibleUI();
	}

	public float GetCollectiblesLeft() => collectiblesTotalPrivate - collected;
}
