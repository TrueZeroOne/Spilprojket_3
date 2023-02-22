using System;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	[SerializeField] private MovingDirection moveDirection = MovingDirection.up;
	[SerializeField] private float movingSpeed = 1.11f;
	[SerializeField] private Vector2 maxPosition, minPosition;
	[SerializeField] private AudioClip leafNoise;
	[SerializeField] private AnimationCurve animationSpeedCurve;

	[SerializeField] private Vector2 actualSpeed;

	private Rigidbody2D rb;
	private Vector2 direction;
	private Vector2 oppositeDirection;
	private bool movePlatform;
	private Animator anim;
	private static readonly int OnPlatform = Animator.StringToHash("onPlatform");
	private DateTime lastTouch;
	private TimeSpan ts;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.constraints = RigidbodyConstraints2D.FreezeAll;
	}

	private void Start()
	{
		lastTouch = DateTime.Now;
		anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		ts = DateTime.Now - lastTouch;
		if (col.CompareTag("Player"))
		{
			movePlatform = true;
			anim.SetBool(OnPlatform, movePlatform);
			DetectPlayer(col);
			GetComponent<AudioSource>().clip = leafNoise;
			if (!GetComponent<AudioSource>().isPlaying && ts.TotalMilliseconds > 100)
				GetComponent<AudioSource>().Play();
		}
	}

	private void OnTriggerExit2D(Collider2D col)
	{
		if (col.CompareTag("Player"))
        {
			movePlatform = false;
			anim.SetBool(OnPlatform, movePlatform);
			DetectPlayer(col);
			lastTouch = DateTime.Now;
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
				MoveDirectionStates(tinyBig);
			}
	}

	private void MoveDirectionStates(TinyBig tinyBig)
	{
		Vector3 position = transform.position;
		float yPosition = position.y, xPosition = position.x;

		if (tinyBig.sizeBig && yPosition <= maxPosition.y && direction == Vector2.up)
		{
			actualSpeed = direction * (movingSpeed * animationSpeedCurve.Evaluate(rb.velocity.y));
			rb.velocity = actualSpeed;
			rb.AddRelativeForce(actualSpeed);
		}
		else if (!tinyBig.sizeBig && yPosition >= minPosition.y && oppositeDirection == Vector2.down)
		{
			actualSpeed = oppositeDirection * (movingSpeed * animationSpeedCurve.Evaluate(+rb.velocity.y) * 4);
			rb.velocity = actualSpeed;
			rb.AddRelativeForce(actualSpeed);
		}
		else
		{
			actualSpeed = new Vector2(0, 0);
			rb.velocity = actualSpeed;
			rb.constraints = RigidbodyConstraints2D.FreezeAll;
			rb.AddRelativeForce(actualSpeed);
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
