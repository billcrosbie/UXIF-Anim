using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAnimHelper : UIAnimHelper
{
	
    Toggle _toggle;


    protected override void Awake()
    {
        base.Awake();

        _toggle = GetComponent<Toggle>() != null ? GetComponent<Toggle>() : null;
    }

    public void SetAnimatorBoolFromToggle()
    {
        //Debug.Log("Called animator toggle");
		if (_toggle != null)
		{
			if (_animator != null)
			{
				//Debug.Log("Called animator toggle");
				SetBool(selectedParameter, _toggle.isOn);
			}
		}        
    }
}
