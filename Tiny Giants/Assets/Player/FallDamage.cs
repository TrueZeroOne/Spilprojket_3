using UnityEngine;

public class FallDamage : MonoBehaviour
{
	[SerializeField] private float tinyPlayerFallDamageDistance = 6.8f;
	[SerializeField] private float bigPlayerFallDamageDistance = 7.8f;
	[SerializeField] private float raycastRange = 2;
	[SerializeField] private float fallDamage = 50;
	private PlayerMovement playerMovement;
	private Health health;
	private bool shouldTakeFallDamage;
	private float fallDistance;


	private Vector2 gizmosOrigin;
	private Vector2 gizmosSize;

	private bool smallFall;

	[SerializeField] private TinyBig tinyBig;

	private void Awake()
	{
		playerMovement = GetComponent<PlayerMovement>();
		health = GetComponent<Health>();
		if (tinyBig == null) tinyBig = GetComponent<TinyBig>();
	}
	private void Update()
	{
		bool isGrounded = playerMovement.grounded;
		Vector2 rayDirection = Vector2.down;

		Transform playerTransform = transform;		
		
		Vector3 position = playerTransform.position;
		Vector3 localScale = playerTransform.localScale;
		Vector2 origin = new Vector2(position.x, position.y - localScale.y - 0.11f);

		Vector2 size = new Vector2(localScale.x, 0.1f);
		
		gizmosOrigin = origin;
		gizmosSize = size;
		RaycastHit2D raycastHit = Physics2D.Raycast(origin, rayDirection * raycastRange);
		RaycastHit2D raycastHitBox = Physics2D.BoxCast(origin, size, 0, Vector2.down);
		if (raycastHitBox && !isGrounded)
		{
			if (!tinyBig.sizeBig)
			{
				smallFall = true;
				print(tinyPlayerFallDamageDistance - size.y);
				if (raycastHitBox.distance >= tinyPlayerFallDamageDistance - size.y && !shouldTakeFallDamage)
				{
					shouldTakeFallDamage = true;
					fallDistance = raycastHitBox.distance;
				}
			}
			else
			{
				print(bigPlayerFallDamageDistance - size.y);
				if (raycastHitBox.distance >= bigPlayerFallDamageDistance - size.y && !shouldTakeFallDamage)
				{
					shouldTakeFallDamage = true;
					fallDistance = raycastHitBox.distance;
				}
				else if (raycastHitBox.distance <= bigPlayerFallDamageDistance - size.y && shouldTakeFallDamage && smallFall)
				{
					smallFall = false;
					shouldTakeFallDamage = false;
				}
			}
			print($"BoxRH Distance: {raycastHitBox.distance} ({raycastHitBox.collider.name}) | Big FDmg Distance: {bigPlayerFallDamageDistance} | Tiny FDmg Distance: {tinyPlayerFallDamageDistance} | Take Dmg: {shouldTakeFallDamage}");
		}
		else if (raycastHitBox && isGrounded && raycastHitBox.distance <= 0.2 && shouldTakeFallDamage)
		{
			print($"Take fall damage at this distance ({fallDistance})");
			shouldTakeFallDamage = false;
			smallFall = false;
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
		if (tinyBig.sizeBig)
		{
			Gizmos.color = Color.green;
			var bigOrigin = new Vector2(origin.x, position.y - localScale.y);
			Gizmos.DrawRay(bigOrigin, rayDirection * bigPlayerFallDamageDistance);
		}
		else if (!tinyBig.sizeBig)
		{
			Gizmos.color = Color.yellow;
			var tinyOrigin = new Vector2(origin.x, position.y - localScale.y);
			Gizmos.DrawRay(tinyOrigin, rayDirection * tinyPlayerFallDamageDistance);
		}
	}
}
