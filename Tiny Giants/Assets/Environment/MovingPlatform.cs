using System;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	[SerializeField] private MovingDirection moveDirection;
	[SerializeField] private float movingSpeed;
	[SerializeField] private Vector2 maxPosition, minPosition;

	private Rigidbody2D rb;
	private Vector2 direction;
	private Vector2 oppositeDirection;
	private bool movePlatform;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.constraints = RigidbodyConstraints2D.FreezeAll;
	}

	private void OnTriggerStay2D(Collider2D col) => DetectPlayer(col);

	private void OnTriggerEnter2D(Collider2D col)
	{
		movePlatform = true;
		DetectPlayer(col);
	}

	private void OnTriggerExit2D(Collider2D col)
	{
		movePlatform = false;
		DetectPlayer(col);
	}

	private void DetectPlayer(Collider2D other)
	{
		bool gotPlayer = other.TryGetComponent(out TinyBig tinyBig);
		if (gotPlayer)
			if (!movePlatform)
			{
				rb.constraints = RigidbodyConstraints2D.FreezeAll;
			}
			else
			{
				if (!transform.position.Equals(maxPosition) || !transform.position.Equals(minPosition))
				{
					rb.constraints = RigidbodyConstraints2D.FreezeRotation;
					Vector2 speed;
					if (tinyBig.sizeBig)
					{
						ChangeDirection();
						speed = direction * movingSpeed;
						rb.velocity = speed;
						rb.AddRelativeForce(speed);
					}
					else
					{
						ChangeDirection();
						speed = oppositeDirection * movingSpeed;
						rb.velocity = speed;
						rb.AddRelativeForce(speed);
					}
				}
			}
	}

	private void ChangeDirection()
	{
		switch (moveDirection)
		{
			case MovingDirection.up:
				direction = Vector2.up;
				oppositeDirection = Vector2.down;
				break;
			case MovingDirection.down:
				direction = Vector2.down;
				oppositeDirection = Vector2.up;
				break;
			case MovingDirection.left:
				direction = Vector2.left;
				oppositeDirection = Vector2.right;
				break;
			case MovingDirection.right:
				direction = Vector2.right;
				oppositeDirection = Vector2.left;
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
}

public enum MovingDirection
{
	up,
	down,
	left,
	right
}
