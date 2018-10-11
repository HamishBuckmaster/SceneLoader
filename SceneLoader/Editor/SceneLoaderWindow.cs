using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections;
using System.IO;

class SceneLoaderWindow : EditorWindow {
	
	static EditorWindow editorWindow, deletePopUp;
	static string[] filesInScenesFolder;
	static Texture2D titleTexture;
	static FileInfo[] fileInfo;
	static Vector2 scrollPosition;
	static string fileToShit;

	string stringToEdit = "New Scene";

	[MenuItem ("Window/SceneLoader")]
	public static void  ShowWindow (){
		editorWindow = EditorWindow.GetWindow(typeof(SceneLoaderWindow));
		editorWindow.titleContent = new GUIContent("Scene Loader");

		CheckForSceneExistence();
		
		titleTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/SceneLoader/Textures/SceneLoaderTitleFile.png");
		
		editorWindow.minSize = new Vector2(titleTexture.width, 240f);
		editorWindow.Focus();
	}

	void OnGUI () {
		GUILayout.Space(2f);

		GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
				GUILayout.Label(titleTexture);
			GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		if(titleTexture != null || editorWindow != null) {
			EditorGUI.DrawRect(new Rect(0, titleTexture.height, editorWindow.position.width, 2f), Color.white);
		} else {
			ShowWindow();
		}

		GUILayout.Space(6f);


		if(GUILayout.Button("Refresh Scenes")){
			CheckForSceneExistence();
		}
		GUILayout.Space(6f);
		stringToEdit = GUILayout.TextField(stringToEdit, 25);
		if(GUILayout.Button("Create New Scene")){
			CreateScene(stringToEdit);
		}
		GUILayout.Space(12f);

		GUILayout.Label("Scenes in project:");

		scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false);
		for(int i = 0; i < fileInfo.GetLength(0); i++){
			GUILayout.BeginHorizontal();
			//string fileName = fileInfo[i].Split(new string[] {"/", "."}, System.StringSplitOptions.RemoveEmptyEntries)[2].ToString();
			string fileName = fileInfo[i].Name.Split(new string[] {"."}, System.StringSplitOptions.RemoveEmptyEntries)[0].ToString();

			if(GUILayout.Button(fileName, GUILayout.Height(21))){
				if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
					EditorSceneManager.OpenScene(fileInfo[i].FullName);
			}
			// Delete Function
			if(GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Trash"),  GUILayout.Width(35))){
				if(EditorUtility.DisplayDialog("SceneLoader Delete Prompt", "Are you sure you want to delete " + fileName + " scene?", "Yes", "No")){
					File.Delete(fileInfo[i].FullName);
					CheckForSceneExistence();
				}
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();

		GUILayout.Space(12f);

		GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
				GUILayout.Label("Thanks for using Scene Loader.");
			GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
				GUILayout.Label("Originally Created by: www.cryzen.com\r\nModified\\Updated by: Hamish Buckmaster");
			GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}
	
	static void CheckForSceneExistence(){
		fileInfo = (new DirectoryInfo(Application.dataPath)).GetFiles("*.unity", SearchOption.AllDirectories);
	}

	static void CreateScene(string sceneName)
	{
		try {
		Debug.Log("Creating Scene: " + sceneName);
		Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
		EditorSceneManager.SaveScene(newScene, Application.dataPath + @"/Scenes/" + sceneName + ".unity");
		CheckForSceneExistence();
		}
		catch
		{
			EditorUtility.DisplayDialog("Error!", "An Error occured", "ok");
		}
	}
}
