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

	private void FindAllCollectibles()
	{
		Collectible[] collectibles = FindObjectsOfType<Collectible>();
		collectiblesTotalPrivate = collectibles.Length;
		UpdateCollectibleUI();
	}

	public static float GetCollectiblesLeft() => collectiblesTotalPrivate - collected;
}
