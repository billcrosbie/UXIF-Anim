using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Optional inherited class to support custom functions for more menu specific operations / functions
/// clearly not currently used much, but have had a need for something similar in past projects. Here to customize as needed
/// </summary>
public class MenuAnimHelper : UIAnimHelper
{
	protected override void Awake()
	{
		base.Awake();		
	}

	IEnumerator ShowPopup()
	{
		yield return new WaitForEndOfFrame ();
		SetAnimatorBoolOnAtStart (true);
	}

}
