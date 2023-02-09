using UnityEngine;
using UnityEngine.Serialization;

public class FallDamage : MonoBehaviour
{
	[FormerlySerializedAs("smallPlayerFallDamageDistance"),SerializeField] private float tinyPlayerFallDamageDistance = 1;
	[SerializeField] private float bigPlayerFallDamageDistance = 2;
	[SerializeField] private float raycastRange = 2;
	[SerializeField] private float fallDamage = 50;
	private PlayerMovement playerMovement;
	private Health health;
	private bool shouldTakeFallDamage;
	private float fallDistance;


	private Vector2 gizmosOrigin;
	private Vector2 gizmosSize;

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
		Vector3 localScale = playerTransform.localScale;
		Vector2 origin = new Vector2(position.x, position.y - localScale.y - (localScale.y / 4 + localScale.y / 4 + 0.02f) - raycastRange / 2);
		var size = new Vector2(localScale.x, localScale.y + raycastRange);
		gizmosOrigin = origin;
		gizmosSize = size;
		RaycastHit2D raycastHit = Physics2D.Raycast(origin, rayDirection * raycastRange);
		RaycastHit2D raycastHitBox = Physics2D.BoxCast(origin, size, 90, Vector2.down);

		if (raycastHitBox && !isGrounded)
		{
			if (raycastHitBox.distance >= tinyPlayerFallDamageDistance && !shouldTakeFallDamage)
			{
				shouldTakeFallDamage = true;
				fallDistance = raycastHitBox.distance;
			}
			else if (raycastHitBox.distance <= bigPlayerFallDamageDistance && shouldTakeFallDamage)
			{
				shouldTakeFallDamage = false;
			}
			print($"BoxRH Distance: {raycastHitBox.distance} ({raycastHitBox.collider.name}) | Big FDmg Distance: {bigPlayerFallDamageDistance} | Tiny FDmg Distance: {tinyPlayerFallDamageDistance} | Take Dmg: {shouldTakeFallDamage}");
		}
		else if (raycastHitBox && isGrounded && raycastHitBox.distance <= 0.2 && shouldTakeFallDamage)
		{
			print($"Take fall damage at this distance ({fallDistance})");
			shouldTakeFallDamage = false;
			health.TakeDamage(fallDamage);
		}
	}

	private void OnDrawGizmos()
	{
		Vector2 rayDirection = Vector2.down;

		Transform playerTransform = transform;		
		
		Vector3 position = playerTransform.position;
		Vector3 localScale = playerTransform.localScale;
		Vector2 origin = new Vector2(position.x, position.y - localScale.y - (localScale.y / 4 + localScale.y / 4) - raycastRange / 2);

		//Gizmos.color = Color.red;
		//Gizmos.DrawWireCube(origin, new Vector2(localScale.x, localScale.y + raycastRange));
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(gizmosOrigin, gizmosSize);
		if (TinyBig.sizeBig)
		{
			Gizmos.color = Color.green;
			var bigOrigin = new Vector2(origin.x, position.y - localScale.y);
			Gizmos.DrawRay(bigOrigin, rayDirection * bigPlayerFallDamageDistance);
		}
		else if (!TinyBig.sizeBig)
		{
			Gizmos.color = Color.yellow;
			var tinyOrigin = new Vector2(origin.x, position.y - localScale.y);
			Gizmos.DrawRay(tinyOrigin, rayDirection * tinyPlayerFallDamageDistance);
		}
	}
}
