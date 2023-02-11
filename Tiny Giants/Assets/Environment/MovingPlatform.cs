using System;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	[SerializeField] private MovingDirection moveDirection = MovingDirection.up;
	[SerializeField] private float movingSpeed = 1.11f;
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

	

	private void OnTriggerEnter2D(Collider2D col)
	{
		movePlatform = true;
		DetectPlayer(col);
	}

	private void OnTriggerExit2D(Collider2D col)
	{
		if (col.CompareTag("Player"))
        {
			movePlatform = false;
			DetectPlayer(col);
        }
		
	}
	private void OnTriggerStay2D(Collider2D col) => DetectPlayer(col);

	private void DetectPlayer(Collider2D other)
	{
		bool gotPlayer = other.TryGetComponent(out TinyBig tinyBig);
		if (gotPlayer)
			if (!movePlatform) rb.constraints = RigidbodyConstraints2D.FreezeAll;
			else
			{
				rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;

				ChangeDirection();
				MoveDirectionStates(other,tinyBig);
			}
	}

	private void MoveDirectionStates(Collider2D other,TinyBig tinyBig)
	{
		Rigidbody2D playerRB = other.GetComponent<Rigidbody2D>();
		Vector2 speed;

		Vector3 position = transform.position;
		float yPosition = position.y, xPosition = position.x;

		if (tinyBig.sizeBig && (yPosition <= maxPosition.y && direction == Vector2.up))
		{
			speed = direction * movingSpeed;
			rb.velocity = speed;
			rb.AddRelativeForce(speed);
		}
		else if (!tinyBig.sizeBig && (yPosition >= minPosition.y && oppositeDirection == Vector2.down))
		{
			speed = oppositeDirection * movingSpeed;
			rb.velocity = speed;
			rb.AddRelativeForce(speed);
			playerRB.velocity = new Vector2(playerRB.velocity.x, speed.y);
			playerRB.AddRelativeForce(speed);
		}
		/*else if (TinyBig.sizeBig && (xPosition <= maxPosition.x && direction == Vector2.left))
				{
					speed = direction * movingSpeed;
					rb.velocity = speed;
					rb.AddRelativeForce(speed);
				}
				else if (!TinyBig.sizeBig && (xPosition >= minPosition.x && oppositeDirection == Vector2.right))
				{
					speed = oppositeDirection * movingSpeed;
					rb.velocity = speed;
					rb.AddRelativeForce(speed);
				}*/
		else
		{
			speed = new Vector2(0, 0);
			rb.velocity = speed;
			rb.constraints = RigidbodyConstraints2D.FreezeAll;
			rb.AddRelativeForce(speed);
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
