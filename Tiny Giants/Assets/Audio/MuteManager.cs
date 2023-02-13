using TMPro;
using UnityEngine;
using UnityEngine.Audio;
public class MuteManager : MonoBehaviour
{
	[SerializeField] private AudioMixer audioMixer;
	[SerializeField] private string mutedStringIcon, unmutedStringIcon;
	[SerializeField] private TMP_Text muteIconText;
	private bool shouldBeMuted;
	public void MuteOrUnmute()
	{
		shouldBeMuted = !shouldBeMuted;
		float volumeValue = shouldBeMuted ? 0f : -80f;
		int shouldBeMutedInt = shouldBeMuted ? 0 : 1;
		muteIconText.text = shouldBeMuted ? unmutedStringIcon : mutedStringIcon;
		audioMixer.SetFloat("Volume", volumeValue);
		PlayerPrefs.SetInt("muted", shouldBeMutedInt);
	}                                 
}
