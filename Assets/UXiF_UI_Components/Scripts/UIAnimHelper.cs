using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIAnimHelper : MonoBehaviour
{
    public bool setAnimatorOnAtStart = true;
    public bool usesInstantTransitions = false;

	public static string hoverTrigger = "hover";
	public static string pressTrigger = "press";
	public static string releaseTrigger = "release";
	public static string idleTrigger = "idle";
	public static string onParameter = "on";
	public static string lockedParameter = "locked";
	public static string disabledParameter = "disabled";
	public static string attentionParameter = "attention";
	public static string readyParameter = "ready";
	public static string completeParameter = "complete";
	public static string instantParameter = "instant";
	public static string selectedParameter = "selected";

	protected string m_selectedState;

	AnimHelperMessageData _pendingData;

	#if UNITY_EDITOR
	Dictionary<string, bool> _lastSavedParams = new Dictionary<string, bool>();
	#endif

    protected Animator _animator;
	public Animator animator
	{
		get
		{
			return _animator;
		}
		set
		{
			_animator = value;
		}
	}
    [SerializeField]
    public List<AnimHelperMessageData> AnimatorMessagesToSend = new List<AnimHelperMessageData>();

	[SerializeField]
	public List<GameObjectMessageData> GameObjectMessagesToSend = new List<GameObjectMessageData>();

	[SerializeField]
	public List<AnimHelperMessageData> ParameterBindMessagesToSend = new List<AnimHelperMessageData>();

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>() != null ? GetComponent<Animator>() : null;
        if (_animator == null)
        {
            Destroy(this);
            return;
        }
        SetAnimatorBoolOnAtStart(setAnimatorOnAtStart);
        SetAnimatorInstantBoolAtStart(usesInstantTransitions);

		#if UNITY_EDITOR
		string[] parameters = _animator.parameters.Where( p => p.type == AnimatorControllerParameterType.Bool).Select( p => p.name).ToArray();
		foreach(string param in parameters)
		{
			_lastSavedParams.Add(param, _animator.GetBool(param));
		}

		#endif
    }

	protected virtual void Start()
	{

	}

    protected virtual void OnEnable()
    {		
        SetAnimatorBoolOnAtStart(setAnimatorOnAtStart);
    }

	public void TurnOn()
	{
		if (_animator != null)
		{
			SetBool(onParameter, true);
		}
	}

	public void TurnOff()
	{
		if (_animator != null)
		{
			SetBool(onParameter, false);
		}
	}

    public void SetAnimatorBoolTrue(string parameterName)
    {
        if (_animator != null)
        {            
			SetBool(parameterName, true);
        }
        
    }
    public void SetAnimatorBoolFalse(string parameterName)
    {
        if (_animator != null)
        {
			SetBool(parameterName, false);            
        }        
    }

    public void SetAnimatorDisabledState(bool value)
    {
        if (_animator != null)
        {
			SetBool(disabledParameter, value);            
        }
        
    }

    public void SetAnimatorBoolOnAtStart(bool value)
    {
        if(_animator != null)
        {
			SetBool(onParameter, value);            
        }
    }

    public void SetAnimatorInstantBoolAtStart(bool value)
    {
        if (_animator != null)
        {
			SetBool(instantParameter, value);            
        }
    }

	public void SetBool(string parameterName, bool value)
	{
		if(_animator != null)
		{
			_animator.SetBool(parameterName, value);
			CheckIfBoolChangeHasBoundMessages(parameterName, value);
		}
	}

	public void StateEntered(string state)
	{
		CheckForAndSendAnimatorMessages(state);
	}

	public void CheckForAndSendAnimatorMessages(string state)
	{
		if (AnimatorMessagesToSend != null)
		{
			foreach (AnimHelperMessageData message in AnimatorMessagesToSend)
			{
				if (message.stateNameToSendMessage == state)
				{
					if (message.animator != null)
					{
						message.animator.GetComponent<UIAnimHelper>().ReceiveAnimatorMessage(message, this);
						//message.animator.SetBool(message.paramter, message.boolValue);
					}
				}
			}
		}

		if(GameObjectMessagesToSend != null)
		{
			foreach(GameObjectMessageData message in GameObjectMessagesToSend)
			{
				if(message.stateNameToSendMessage == state)
				{
					if(message._gameObject != null)
					{
						message._gameObject.SetActive(message._setActiveValue);
					}
				}
			}
		}
	}

	public void ReceiveAnimatorMessage(AnimHelperMessageData message, UIAnimHelper messageSender)
	{
		if(message.delay == 0f)
		{
			SetBool(message.parameter, message.boolValue);
		}
		else
		{
			_pendingData = message;
			StartCoroutine("ApplyMessageAfterDelay");
		}
	}

	public void ReceiveParameterBindMessage(AnimHelperMessageData message, UIAnimHelper messageSender, bool sourceValue)
	{		
		SetBool(message.parameter, message.boolValue == false ? sourceValue : !sourceValue);
	}

	public void CheckIfBoolChangeHasBoundMessages(string parameterName, bool valueSet)
	{		
		foreach(AnimHelperMessageData message in ParameterBindMessagesToSend)
		{
			if(parameterName == message.sourceParamterBoundTo)
			{
				if(message.animator != null)
				{
					message.animator.GetComponent<UIAnimHelper>().ReceiveParameterBindMessage(message, this, valueSet);
				}
			}
		}
	}

	IEnumerator ApplyMessageAfterDelay()
	{
		yield return new WaitForSeconds(_pendingData.delay);
		SetBool(_pendingData.parameter, _pendingData.boolValue);
		_pendingData = null;
	}

	//helpful callbacks for scripts to be able make based on input events to avoid some of the edge cases of the Input system
	//as well as being able to easily support cross platform input (touch vs. click) while also avoiding some edge cases introduced there
	//ie mobile has no hover state
	public virtual void OnHoverIn()
	{
		_animator.SetTrigger(hoverTrigger);
	}

	public virtual void OnHoverOut()
	{
		_animator.SetTrigger(idleTrigger);
	}

	public virtual void OnPress()
	{
		_animator.SetTrigger(pressTrigger);
	}

	public virtual void OnRelease()
	{
		_animator.SetTrigger(idleTrigger);
	}

	public virtual void OnSelect()
	{

	}

	public virtual void OnDeselect()
	{

	}

	public virtual void OnClicked()
	{
		
	}

	//run a more costly check to make sure in editor we detect parameter changess manually done by the user flipping the bool checkbox
	//in the animator window
	#if UNITY_EDITOR
	public void EditorCheckForChangedBoolParams()
	{
		foreach(KeyValuePair<string, bool> param in _lastSavedParams)
		{
			if(_animator.GetBool(param.Key) != param.Value)
			{
				CheckIfBoolChangeHasBoundMessages(param.Key, _animator.GetBool(param.Key));
				UpdateLastSavedParam(param.Key, _animator.GetBool(param.Key));
				break;
			}
		}
	}

	public void UpdateLastSavedParam(string name, bool value)
	{
		_lastSavedParams[name] = value;
	}


	public void Update()
	{
		if(Application.isEditor && Application.isPlaying)
		{
			EditorCheckForChangedBoolParams();
		}
	}
	#endif
}
