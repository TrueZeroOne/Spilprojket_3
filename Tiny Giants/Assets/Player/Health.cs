using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
	[SerializeField] private TMP_Text healthText;
	private const float maxHealth = 100;
	private float health = maxHealth;

	public float GetHealth() => health;

	private void UpdateHealthUI() => healthText.text = $"{GetHealth()}/{maxHealth} HP";

	public void SetHealth(float value) => health = value;

	public void TakeDamage(float value) => health -= value;

	public void Heal(float value) => health += value;

	private void Update()
	{
		UpdateHealthUI();
		if (GetHealth() <= 0) Die();
	}
	
	private static void Die() =>
		//Destroy(gameObject);
		print("Player Died");
}
