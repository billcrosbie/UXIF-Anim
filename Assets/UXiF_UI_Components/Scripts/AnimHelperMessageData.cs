using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimHelperMessageData
{
    //[SerializeField]
	//TODO delete these once I verify all the new stuff is working that gets states generically by name and we no longer need this enum
    public enum AnimatorStates
    {
        _On,
        _TurnOn,
        _TurnOff,
        _Off,
        _Selected,
        _Deselected,
        _Disabled,
        _Enabled,
        _Attention,
        _NotAttention,
		_Locked,
		_Unlocked
    }
    //[SerializeField]
    public AnimatorStates _stateToSendMessage;

	//this is the new generic method of storing and comparing states
	public string stateNameToSendMessage;

    //[SerializeField]
    //protected Animator _animator;
	[SerializeField]
    public Animator animator;
    //{
    //    get
    //    {
    //        return _animator;
    //    }
    //    set
    //    {
    //        _animator = value;
    //    }
    //}
    //[SerializeField]
    //protected string _parameter;
	[SerializeField]
    public string parameter;
    //{
    //    get
    //    {
    //        return _parameter;
    //    }
    //    set
    //    {
    //        _parameter = value;
    //    }
    //}
    //[SerializeField]
    //protected bool _boolValue;
	[SerializeField]
    public bool boolValue;
    //{
    //    get
    //    {
    //        return _boolValue;
    //    }
    //    set
    //    {
    //        _boolValue = value;
    //    }
    //}
	[SerializeField]
	public float delay;

	//the name of the parameter we are bound to on the source controller (that sent us this message)
	[SerializeField]
	public string sourceParamterBoundTo;

}
