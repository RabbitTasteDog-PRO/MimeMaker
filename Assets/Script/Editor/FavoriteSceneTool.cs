using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class FavoriteSceneTool : EditorWindow 
{
	public string key = "SCENES";

	[ MenuItem ( "Dev/FavoriteSceneTool" ) ]
	static public void UseFavoriteSceneTool ()
	{
		EditorWindow.GetWindow (  typeof ( FavoriteSceneTool ) );
	}

	void OnGUI ()
	{
		Color orgColor = GUI.color;

		EditorGUILayout.Space ();
		EditorGUIUtility.LookLikeControls();
		
		EditorGUILayout.BeginVertical ();

		DrawSceneNames ();

		GUI.color = Color.green;

		EditorGUILayout.Space ();
		if (GUILayout.Button ("Add Current Scene"))
		{
			AddCurrentScene ();
		}

		if (GUILayout.Button ("Save Project"))
		{
			AssetDatabase.SaveAssets ();
		}
		GUI.color = orgColor;


		EditorGUILayout.EndVertical ();
	}

	void OnHierarchyChange() 
	{
		Repaint ();
	}

	private void DrawSceneNames ()
	{
		string[] scenes = GetSceneNames ();

		Color orgColor = GUI.color;

		foreach (string name in scenes)
		{
			EditorGUILayout.BeginHorizontal ();

			if (name.Equals (EditorApplication.currentScene))
			{
				GUI.color = Color.green;
				EditorGUILayout.TextField (name);
				GUI.color = orgColor;
			}
			else
			{
				EditorGUILayout.TextField (name);
			}

			if (GUILayout.Button ("Open"))
			{
				//EditorApplication.SaveScene ();
				EditorApplication.OpenScene (name);
			}

			if (GUILayout.Button ("X"))
			{
				DeleteScene (name);
			}

			EditorGUILayout.EndHorizontal ();
		}
	}

	private void DeleteScene (string deletedName)
	{
		string[] scenes = GetSceneNames ();

		List<string> newsScenes = new List<string> ();

		bool isExist = false;

		foreach (string name in scenes)
		{
			if (name.Equals (deletedName))
			{
				continue;
			}

			if (name.Equals ("") || name == null)
			{
				continue;
			}

			newsScenes.Add (name);
		}

		int i = 0;

		string value = "";

		foreach (string name in newsScenes)
		{
			if (i != 0)
			{
				value += ",";
			}

			i++;

			value += name;
		}

		EditorPrefs.SetString (key, value);
	}

	public string[] GetSceneNames ()
	{
		string value = ""; 

		if (EditorPrefs.HasKey (key))
		{
			value = EditorPrefs.GetString (key);
		}
		else
		{
		}

		string currentSceneName = EditorApplication.currentScene;

		string[] scenes = value.Split (',');

		return scenes;
	}

	private void AddCurrentScene ()
	{
		string currentSceneName = EditorApplication.currentScene;

		string[] scenes = GetSceneNames ();

		List<string> newsScenes = new List<string> ();

		bool isExist = false;

		foreach (string name in scenes)
		{
			if (name.Equals (currentSceneName))
			{
				isExist = true;
				continue;
			}

			if (name.Equals ("") || name == null)
			{
				continue;
			}

			newsScenes.Add (name);
		}

		if (isExist)
		{
			return;
		}

		newsScenes.Add (currentSceneName);

		newsScenes.Sort ();

		int i = 0;

		string value = "";

		foreach (string name in newsScenes)
		{
			if (i != 0)
			{
				value += ",";
			}

			i++;

			value += name;
		}

		EditorPrefs.SetString (key, value);
	}
}



