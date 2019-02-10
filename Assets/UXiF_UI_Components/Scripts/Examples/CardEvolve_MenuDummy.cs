using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardEvolve_MenuDummy : MonoBehaviour
{
	public int numGold = 4000;
	public int numDiamond = 50;

	static int baseGoldCost = 3500;
	int goldCost = baseGoldCost;

	int goldIncrements = 100;

	int extraPowerAdded = 0;

	public Button confirmButton;
	CurrencyButton confirmButtonScript;
	public Button increasePowerButton;
	Animator increasePowerButtonAnimator;
	UIAnimHelper increasePowerButtonAnimHelper;
	public Button decreasePowerButton;
	Animator decreasePowerButtonAnimator;
	UIAnimHelper decreasePowerButtonAnimHelper;

	static string cardSelectedParameter = "cardSelected";
	static string resourceSelectedParameter = "resourceSelected";


	UIAnimHelper animHelper;

	public void PowerIncreased()
	{
		Debug.Log("HERE");
		decreasePowerButtonAnimHelper.SetBool(UIAnimHelper.disabledParameter, false);

		//if we can afford it
		if(goldCost + goldIncrements <= numGold)
		{
			extraPowerAdded ++;
			goldCost += goldIncrements;
		}
		else
		{
			increasePowerButtonAnimHelper.SetBool(UIAnimHelper.disabledParameter, true);
		}
		UpdateConfirmButton();
	}

	public void PowerDecreased()
	{
		increasePowerButtonAnimHelper.SetBool(UIAnimHelper.disabledParameter, false);
		if(extraPowerAdded != 0)
		{
			extraPowerAdded --;
		}
		if(extraPowerAdded == 0)
		{
			decreasePowerButtonAnimator.SetBool(UIAnimHelper.disabledParameter, true);
		}
		else
		{
			goldCost -= goldIncrements;
		}
		UpdateConfirmButton();
	}

	public void UpdateConfirmButton()
	{
		confirmButtonScript.UpdateLabel(goldCost.ToString());
	}

	public void CardSelected()
	{
		animHelper.SetAnimatorBoolTrue(cardSelectedParameter);
	}

	public void ChangeCardSelection()
	{
		animHelper.SetAnimatorBoolFalse(cardSelectedParameter);
	}

	public void ResourceSelected()
	{
		animHelper.SetAnimatorBoolTrue(resourceSelectedParameter);
	}

	public void ChangeResourceSelection()
	{
		animHelper.SetAnimatorBoolFalse(resourceSelectedParameter);
	}

	public void EvolveConfirmed()
	{
		
	}

	public void CloseButtonClicked()
	{
		TurnOff();
	}

	public void TurnOff()
	{
		animHelper.SetAnimatorBoolFalse(UIAnimHelper.onParameter);
	}

	void Awake()
	{		
		animHelper = GetComponent<UIAnimHelper>();
	}

	void Start ()
	{		
		confirmButtonScript = confirmButton.GetComponent<CurrencyButton>();
		increasePowerButtonAnimator = increasePowerButton.GetComponent<Animator>();
		increasePowerButtonAnimHelper = increasePowerButtonAnimator.GetComponent<UIAnimHelper>();
		decreasePowerButtonAnimator = decreasePowerButton.GetComponent<Animator>();
		decreasePowerButtonAnimHelper = decreasePowerButtonAnimator.GetComponent<UIAnimHelper>();
		UpdateConfirmButton();
	}
	

}
