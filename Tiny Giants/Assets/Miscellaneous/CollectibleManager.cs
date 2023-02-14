using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
	[SerializeField] private TMP_Text collectedText;
	private static int collectiblesTotalPrivate;

	public void UpdateCollectibleUI() => collectedText.text = $"{collected}/{collectiblesTotal}";

	public static int collectiblesTotal => collectiblesTotalPrivate;

	public static int collected;

	private void Awake() => FindAllCollectibles();

	private void Start() => FindAllCollectibles();

	private void OnEnable() => FindAllCollectibles();

	private void FindAllCollectibles()
	{
		List<Collectible> collectibles = new List<Collectible>(FindObjectsOfType<Collectible>());
		collectibles.Clear();
		collectibles = new List<Collectible>(FindObjectsOfType<Collectible>());
		collectiblesTotalPrivate = collectibles.Count;
		UpdateCollectibleUI();
	}

	public static float GetCollectiblesLeft() => collectiblesTotalPrivate - collected;
}
