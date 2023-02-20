using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[Header("Audio"), SerializeField] private AudioClip cantSize;
	[SerializeField] private AudioClip changeSize, rChangeSize, jumpAudio, walkAudio;

	public AudioSource playerAudio, playerWalkAudio;
	private PlayerMovement playerMovement;
	private Animator animator;

	private static readonly int SizeAnimID = Animator.StringToHash("size");
	private static readonly int SpeedAnimID = Animator.StringToHash("speed");
	private static readonly int JumpSpeedAnimID = Animator.StringToHash("jumpSpeed");
	private static readonly int IsGrabbedAnimID = Animator.StringToHash("isGrabbed");
	private static readonly int OnPlatformAnimID = Animator.StringToHash("onPlatform");
	private void Start()
	{
		playerMovement = GetComponent<PlayerMovement>();
		animator = GetComponent<Animator>();
		playerWalkAudio.clip = walkAudio;
	}
	private void Update()
	{
		if ((playerMovement.grounded && animator.GetFloat(SpeedAnimID) > 0.9)  || (animator.GetFloat(SpeedAnimID) < -0.9 && playerMovement.grounded))
		{
			if (!playerWalkAudio.isPlaying) playerWalkAudio.Play();
		}
		else playerWalkAudio.Stop();
		if (animator.GetFloat(JumpSpeedAnimID) > 0.01 && !animator.GetBool(OnPlatformAnimID)) PlayAudio(jumpAudio);
	}
	public void PlayCannotGrow() => PlayAudio(cantSize);
	public void PlayGrow() => PlayAudio(changeSize);
	public void PlayShrink() => PlayAudio(rChangeSize);
	private void PlayAudio(AudioClip clip) => StartCoroutine(PlayAudioClipOnce(clip));
	private IEnumerator PlayAudioClipOnce(AudioClip clip)
	{
		if (playerAudio.isPlaying) yield break;
		playerAudio.clip = clip;
		playerAudio.Play();
		Debug.Log($"{clip.name} Was PLAYED");
		yield return new WaitForSeconds(playerAudio.clip.length);
		playerAudio.Stop();
	}
}
