using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
	private static int collectiblesTotalPrivate;
	public static int collectiblesTotal => collectiblesTotalPrivate;

	public static int collected;
	private void Awake()
	{
		Collectible[] collectibles = FindObjectsOfType<Collectible>();
		collectiblesTotalPrivate = collectibles.Length;
	}
	public static float GetCollectiblesLeft() => collectiblesTotalPrivate - collected;
}
