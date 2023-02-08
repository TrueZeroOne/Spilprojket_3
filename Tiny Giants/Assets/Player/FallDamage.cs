using UnityEngine;

public class FallDamage : MonoBehaviour
{
	[SerializeField] private float fallDamageDistance = 4;
	[SerializeField] private float raycastRange = 2;
	[SerializeField] private float fallDamage = 50;
	private PlayerMovement playerMovement;
	private Health health;
	private bool shouldTakeFallDamage;
	private float fallDistance;
	private void Awake()
	{
		playerMovement = GetComponent<PlayerMovement>();
		health = GetComponent<Health>();
	}
	private void Update()
	{
		bool isGrounded = playerMovement.grounded;
		Vector2 rayDirection = Vector2.down;

		Transform playerTransform = transform;		
		
		Vector3 position = playerTransform.position;
		Vector2 origin = new Vector2(position.x, position.y - playerTransform.localScale.y - 0.05f);

		RaycastHit2D raycastHit = Physics2D.Raycast(origin, rayDirection * raycastRange);

		if (raycastHit && !isGrounded)
		{
			if (raycastHit.distance >= fallDamageDistance && !shouldTakeFallDamage)
			{
				shouldTakeFallDamage = true;
				fallDistance = raycastHit.distance;
			}
		}
		else if (raycastHit && isGrounded && raycastHit.distance <= 0.2 && shouldTakeFallDamage)
		{
			print($"Take fall damage at this distance ({fallDistance})");
			shouldTakeFallDamage = false;
			health.TakeDamage(fallDamage);
		}
	}
}
