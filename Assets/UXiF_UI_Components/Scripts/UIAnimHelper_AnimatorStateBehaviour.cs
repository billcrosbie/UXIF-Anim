using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach this to any animator state you want to receive OnState"" calls from to feed into the animation messenger system driven by UIAnimHelper
/// Give the state a logical name in the public string field, When selecting states to message from in the custom inspector
/// you will see the name you enter as an option there
/// </summary>
public class UIAnimHelper_AnimatorStateBehaviour : StateMachineBehaviour
{
	public string state;
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{		
		UIAnimHelper controller = animator.GetComponent<UIAnimHelper>();
		if(controller != null)
		{
			controller.StateEntered(state);
		}
	}
	//currently don't use/need this, but if you find a need for it, you could easily hook into it
	//right now all messages are fired from OnStateEnter
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		//animator.GetComponent<UIAnimHelper>().StateExited_ATTENTION();
	}
}
