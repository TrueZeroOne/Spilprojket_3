using UnityEngine;
using UnityEngine.InputSystem;

public class SaveLoadBinds : MonoBehaviour
{
	public InputActionAsset actions;
	private void OnEnable()
	{
		string rebinds = PlayerPrefs.GetString("rebinds");
		if (!string.IsNullOrEmpty(rebinds)) actions.LoadBindingOverridesFromJson(rebinds);
	}

	private void OnDisable()
	{
		string rebinds = actions.SaveBindingOverridesAsJson();
		PlayerPrefs.SetString("rebinds", rebinds);
	}
}
