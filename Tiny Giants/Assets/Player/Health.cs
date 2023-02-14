using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
	[SerializeField] private TMP_Text healthText;
	private const float maxHealth = 100;
	private static float health = maxHealth;

	public static float GetHealth() => health;

	private void UpdateHealthUI() => healthText.text = $"{GetHealth()}/{maxHealth} HP";

	public static void SetHealth(float value) => health = value;

	public static void TakeDamage(float value) => health -= value;

	public static void Heal(float value) => health += value;

	private void Update()
	{
		UpdateHealthUI();
		if (GetHealth() <= 0) Die();
	}
	
	private static void Die() =>
		//Destroy(gameObject);
		print("Player Died");
}
