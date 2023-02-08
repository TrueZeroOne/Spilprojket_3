using System;
using UnityEngine;

public class Health : MonoBehaviour
{
	[SerializeField] private float health = 100;

	public float GetHealth => health;

	public void SetHealth(float value) => health = value;

	public void TakeDamage(float value) => health -= value;

	public void Heal(float value) => health += value;

	private void Update()
	{
		if (GetHealth <= 0) Die();
	}
	
	private void Die()
	{
		//Destroy(gameObject);
		print("Player Died");
	}
}
