using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuDUmmy : MonoBehaviour
{
	public ToggleGroup tabs;
	Toggle[] tabToggles;

	// Use this for initialization
	void Start ()
	{
		tabToggles = tabs.GetComponentsInChildren<Toggle>();
		tabToggles[0].isOn = true;
		tabs.NotifyToggleOn(tabToggles[0]);
	}
	

}
