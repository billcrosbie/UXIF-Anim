using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameObjectMessageData
{	
	public GameObject _gameObject;
	public bool _setActiveValue;
	public AnimHelperMessageData.AnimatorStates _stateToSendMessage;

	public string stateNameToSendMessage;
}
