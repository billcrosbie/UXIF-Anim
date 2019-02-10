using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;



[CustomEditor(typeof(UIAnimHelper))]
public class UIAnimHelperInspector : Editor
{
	string m_selectedState;


	float[] columnWeights = new float[6]{4f, 2f, 2f, 1f, 1f, 1f};


	protected enum editorColumnType
	{
		_animatorObject,
		_stateSelector,
		_boolValueCheck,
		_parameterName,
		_removeButton
	}

	UIAnimHelper myUIAnimHelper
	{
		get
		{
			return target as UIAnimHelper;
		}
	}

	Animator animator
	{
		get
		{
			return myUIAnimHelper.GetComponent<Animator>();
		}
	}

	protected float GetWidthOfRectFromWeight(int columnIndex)
	{
		float weightSum = 0f;
		foreach(float weight in columnWeights)
		{
			weightSum += weight;
		}

		float adjustedScreenWidth = Screen.width - (columnWeights.Length * (EditorGUIUtility.singleLineHeight/2f));
		if(columnIndex < columnWeights.Length)
		{
			return adjustedScreenWidth * (columnWeights[columnIndex] / weightSum);
		}

		return 50f;
	}

	//Bbase.OnInspectorGUI();

	AnimHelperMessageData messageToRemove = null;

    public override void OnInspectorGUI()
    {
		
		UIAnimHelper helper = myUIAnimHelper;
        if (myUIAnimHelper != null)
        {
            
        }
        else
        {
            return;
        }

        //DrawDefaultInspector();

		EditorGUILayout.BeginVertical();
		GUILayout.Label("STATE BASED ANIMATOR MESSAGES");
		EditorGUILayout.EndVertical();


		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("use instant transitions");
		helper.usesInstantTransitions = EditorGUILayout.Toggle(helper.usesInstantTransitions);
		GUILayout.Label("turn on at start");
		helper.setAnimatorOnAtStart = EditorGUILayout.Toggle(helper.setAnimatorOnAtStart);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();

		string[] states = animator.GetBehaviours<UIAnimHelper_AnimatorStateBehaviour>().Select ( sms => sms.state).ToArray();

		string[] myParameters = animator.gameObject.activeInHierarchy ? animator.parameters.Where( p => p.type == AnimatorControllerParameterType.Bool).Select( p => p.name).ToArray() : new string[0];


        EditorGUILayout.BeginVertical();
        foreach (AnimHelperMessageData message in myUIAnimHelper.AnimatorMessagesToSend)
        {
			EditorGUILayout.BeginVertical();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("send to", GUILayout.Width(GetWidthOfRectFromWeight(0)));
			GUILayout.Label("from state", GUILayout.Width(GetWidthOfRectFromWeight(1)));
			GUILayout.Label("parameter", GUILayout.Width(GetWidthOfRectFromWeight(2)));
			GUILayout.Label("value", GUILayout.Width(GetWidthOfRectFromWeight(3)));
			GUILayout.Label("delay", GUILayout.Width(GetWidthOfRectFromWeight(4)));
			//GUILayout.FlexibleSpace();

			EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
			message.animator = (Animator)EditorGUILayout.ObjectField((Object)message.animator,typeof(Animator), true, GUILayout.Width(GetWidthOfRectFromWeight(0)));      
            

			if (states.Length == 0)
			{
				EditorGUILayout.LabelField("No animator states found");
			}
			else
			{
				int selectedIndex = System.Array.IndexOf(states, message.stateNameToSendMessage);
				if (selectedIndex == -1)
				{
					selectedIndex = 0; // make sure we select something by default
				}
				selectedIndex = EditorGUILayout.Popup(selectedIndex, states, GUILayout.Width(GetWidthOfRectFromWeight(1)));
				message.stateNameToSendMessage = states[selectedIndex];
				//m_selectedState = states[selectedIndex];
			}

			string[] parameters = message.animator != null ? message.animator.parameters.Where( p => p.type == AnimatorControllerParameterType.Bool).Select( p => p.name).ToArray() : new string[0];
			// get the names of all bool parameters
			if(message.animator == null)
			{
				EditorGUILayout.LabelField("Assign an animator first!!", GUILayout.Width(GetWidthOfRectFromWeight(2)));
			}

			else if (parameters.Length == 0)
			{				
				EditorGUILayout.LabelField("No bool parameters found", GUILayout.Width(GetWidthOfRectFromWeight(2)));
				EditorUtility.SetDirty(target);
				EditorUtility.SetDirty(message.animator.gameObject);
				//Debug.Log(message.animator);
				//Debug.Log(message.animator.runtimeAnimatorController);
				//string[] test = message.animator.parameters.Where( p => p.type == AnimatorControllerParameterType.Bool).Select( p => p.name).ToArray();
				//Debug.Log(test.Length);
				//string[] test = AnimHelperMessageData.GetParameters(message.animator).Where( p => p.type == AnimatorControllerParameterType.Bool).Select( p => p.name).ToArray();
				//Debug.Log(test.Length);
			}
			else
			{
				int selectedIndex = System.Array.IndexOf(parameters, message.parameter);
				if (selectedIndex == -1)
				{
					selectedIndex = 0; // make sure we select something by default
				}
				selectedIndex = EditorGUILayout.Popup(selectedIndex, parameters, GUILayout.Width(GetWidthOfRectFromWeight(2)));
				message.parameter = parameters[selectedIndex];
			}


			message.boolValue = EditorGUILayout.Toggle(message.boolValue, GUILayout.Width(GetWidthOfRectFromWeight(3)));
			message.delay = EditorGUILayout.FloatField(message.delay, GUILayout.Width(GetWidthOfRectFromWeight(4)));

			if (GUILayout.Button("-", GUILayout.Width(GetWidthOfRectFromWeight(5))))
			{
				messageToRemove = message;                
				//DestroyImmediate(message);
			}

            EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();

            //EditorGUILayout.Space();
            EditorUtility.SetDirty(target);
        }
        EditorGUILayout.EndVertical();

		if(messageToRemove != null)
		{
			myUIAnimHelper.AnimatorMessagesToSend.Remove(messageToRemove);
			messageToRemove = null;
		}

        if (GUILayout.Button("Add Animator State Message"))
        {
            myUIAnimHelper.AnimatorMessagesToSend.Add(new AnimHelperMessageData());
        }



		//BOOL BINDING SECTION



		EditorGUILayout.BeginVertical();
		EditorGUILayout.Space();
		GUILayout.Label("PARAMETER BINDING MESSAGES");
		EditorGUILayout.EndVertical();


		EditorGUILayout.BeginVertical();
		foreach (AnimHelperMessageData message in myUIAnimHelper.ParameterBindMessagesToSend)
		{
			EditorGUILayout.BeginVertical();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("send to", GUILayout.Width(GetWidthOfRectFromWeight(0)));
			GUILayout.Label("source parameter", GUILayout.Width(GetWidthOfRectFromWeight(1)));
			GUILayout.Label("target parameter", GUILayout.Width(GetWidthOfRectFromWeight(2)));
			GUILayout.Label("Invert?", GUILayout.Width(GetWidthOfRectFromWeight(3)));
			//GUILayout.Label("delay", GUILayout.Width(GetWidthOfRectFromWeight(4)));
			//GUILayout.FlexibleSpace();

			EditorGUILayout.EndHorizontal();


			EditorGUILayout.BeginHorizontal();
			message.animator = (Animator)EditorGUILayout.ObjectField((Object)message.animator,typeof(Animator), true, GUILayout.Width(GetWidthOfRectFromWeight(0)));      

			if(myParameters.Length == 0)
			{
				EditorGUILayout.LabelField("No parameters found!");
			}
			else
			{
				int selectedIndex = System.Array.IndexOf(myParameters, message.sourceParamterBoundTo);
				if (selectedIndex == -1)
				{
					selectedIndex = 0; // make sure we select something by default
				}
				selectedIndex = EditorGUILayout.Popup(selectedIndex, myParameters, GUILayout.Width(GetWidthOfRectFromWeight(1)));
				message.sourceParamterBoundTo = myParameters[selectedIndex];
				//m_selectedState = states[selectedIndex];
			}

			string[] parameters = message.animator != null ? message.animator.parameters.Where( p => p.type == AnimatorControllerParameterType.Bool).Select( p => p.name).ToArray() : new string[0];
			// get the names of all bool parameters
			if(message.animator == null)
			{
				EditorGUILayout.LabelField("Assign an animator first!!", GUILayout.Width(GetWidthOfRectFromWeight(2)));
			}

			else if (parameters.Length == 0)
			{				
				EditorGUILayout.LabelField("No bool parameters found", GUILayout.Width(GetWidthOfRectFromWeight(2)));
				EditorUtility.SetDirty(target);
				EditorUtility.SetDirty(message.animator.gameObject);
				//Debug.Log(message.animator);
				//Debug.Log(message.animator.runtimeAnimatorController);
				//string[] test = message.animator.parameters.Where( p => p.type == AnimatorControllerParameterType.Bool).Select( p => p.name).ToArray();
				//Debug.Log(test.Length);
				//string[] test = AnimHelperMessageData.GetParameters(message.animator).Where( p => p.type == AnimatorControllerParameterType.Bool).Select( p => p.name).ToArray();
				//Debug.Log(test.Length);
			}
			else
			{
				int selectedIndex = System.Array.IndexOf(parameters, message.parameter);
				if (selectedIndex == -1)
				{
					selectedIndex = 0; // make sure we select something by default
				}
				selectedIndex = EditorGUILayout.Popup(selectedIndex, parameters, GUILayout.Width(GetWidthOfRectFromWeight(2)));
				message.parameter = parameters[selectedIndex];
			}


			message.boolValue = EditorGUILayout.Toggle(message.boolValue, GUILayout.Width(GetWidthOfRectFromWeight(3)));
			//message.delay = EditorGUILayout.FloatField(message.delay, GUILayout.Width(GetWidthOfRectFromWeight(4)));

			if (GUILayout.Button("-", GUILayout.Width(GetWidthOfRectFromWeight(5))))
			{
				messageToRemove = message;                
				//DestroyImmediate(message);
			}

			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();

			EditorGUILayout.Space();
			EditorUtility.SetDirty(target);
		}
		EditorGUILayout.EndVertical();

		if(messageToRemove != null)
		{
			myUIAnimHelper.ParameterBindMessagesToSend.Remove(messageToRemove);
			messageToRemove = null;
		}


		if (GUILayout.Button("Add Parameter Binding Message"))
		{
			myUIAnimHelper.ParameterBindMessagesToSend.Add(new AnimHelperMessageData());
		}

       //if (EditorGUI.EndChangeCheck())
       //{
       //    EditorUtility.SetDirty(target);            
       //}
    }

	/// <summary>
	/// Draws a dropdown menu to select the parent state.
	/// </summary>
	void DrawSelectStateGUI()
	{
		// get the names of all states 
		string[] states = myUIAnimHelper.animator.GetBehaviours<UIAnimHelper_AnimatorStateBehaviour>().Select ( sms => sms.state).ToArray();
		if (states.Length == 0)
		{
			EditorGUILayout.LabelField("No animator states found");
		}
		else
		{
			int selectedIndex = System.Array.IndexOf(states, m_selectedState);
			if (selectedIndex == -1)
			{
				selectedIndex = 0; // make sure we select something by default
			}
			selectedIndex = EditorGUILayout.Popup("Select state", selectedIndex, states);
			m_selectedState = states[selectedIndex];
		}
	}
}
