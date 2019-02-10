using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencyButton : MonoBehaviour
{
	public TextMeshProUGUI label;
	public Image icon;

	public void UpdateLabel(string newLabel)
	{
		label.text = newLabel;
	}
}
