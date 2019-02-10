using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;


[CustomEditor(typeof(CanvasGroupController))]
public class CanvasGroupControllerInspector : Editor
{
	public override void OnInspectorGUI()
	{
		//Bbase.OnInspectorGUI();
		CanvasGroupController myBCG = (CanvasGroupController)target;
		if (myBCG != null)
		{

		}
		else
		{
			return;
		}

		DrawDefaultInspector();

		if(EditorApplication.isPlaying)
		{
			EditorGUILayout.BeginVertical();

			GUILayout.Label("This BCG owns: " + (myBCG.imageMatsInChildren != null ? myBCG.imageMatsInChildren.Count.ToString() : "0") + " Materials");

			EditorGUILayout.BeginVertical();
			if(myBCG.imagesInChildren != null)
			{
				GUILayout.Label("Images");
				foreach(Image i in myBCG.imagesInChildren)
				{
					GUILayout.Label(i.name + " : " + i.gameObject.name);
				}
			}
			EditorGUILayout.Space();
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical();
			GUILayout.Label("This BCG owns: " + (myBCG.canvasControllersInChildren != null ? myBCG.canvasControllersInChildren.Count.ToString() : "0") + " Canvas Group Controllers");
			if(myBCG.canvasControllersInChildren != null)
			{
				GUILayout.Label("Canvas Controllers");
				foreach(CanvasGroupController controller in myBCG.canvasControllersInChildren)
				{
					GUILayout.Label(controller.name);
				}
			}
			EditorGUILayout.Space();
			EditorGUILayout.EndVertical();


			EditorGUILayout.BeginVertical();
			GUILayout.Label("This BCG owns: " + (myBCG.textsInChildren != null ? myBCG.textsInChildren.Count.ToString() : "0") + " Texts");
			if(myBCG.textsInChildren != null)
			{
				GUILayout.Label("Texts");
				foreach(TextMeshProUGUI text in myBCG.textsInChildren)
				{
					GUILayout.Label(text.name);
				}
			}
			EditorGUILayout.Space();
			EditorGUILayout.EndVertical();


			EditorGUILayout.BeginVertical();
			GUILayout.Label("This BCG owns: " + (myBCG.threeDTextsInChildren != null ? myBCG.threeDTextsInChildren.Count.ToString() : "0") + " Texts");
			if(myBCG.threeDTextsInChildren != null)
			{
				GUILayout.Label("3D Texts");
				foreach(TextMeshPro text in myBCG.threeDTextsInChildren)
				{
					GUILayout.Label(text.name);
				}
			}
			EditorGUILayout.Space();
			EditorGUILayout.EndVertical();

			//GUILayout.Label("This BCG owns: " + (myBCG.textsInChildren != null ? myBCG.textsInChildren.Count.ToString() : "0") + " TMPro texts");
			//GUILayout.Label("This BCG owns: " + (myBCG.threeDTextsInChildren != null ? myBCG.threeDTextsInChildren.Count.ToString() : "0") + " TMPro 3D texts");
			EditorGUILayout.EndVertical();
		}
	}
}