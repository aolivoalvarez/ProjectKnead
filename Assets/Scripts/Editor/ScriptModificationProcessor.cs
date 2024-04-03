/*-----------------------------------------
Creation Date: 13 March 2024
Author: theco
Description: An editor script that, when a new script is created, replaces the custom keys #DATETIME#, #DEVELOPER#, and #PROJECTNAME# with their respective values.
-----------------------------------------*/

using UnityEditor;
using UnityEngine;

public class ScriptModificationProcessor : AssetModificationProcessor
{
	public static void OnWillCreateAsset (string path)
	{
		path = path.Replace(".meta", "");
		int index = path.LastIndexOf(".");
		if (index == -1) return;
		string file = path.Substring(index);

		if (file != ".cs" && file != ".js" && file != ".boo") return;

		index = Application.dataPath.LastIndexOf("Assets");
		path = Application.dataPath.Substring(0, index) + path;
		file = System.IO.File.ReadAllText(path);

		file = file.Replace("#DATETIME#", System.DateTime.Now.ToString());
		file = file.Replace("#DEVELOPER#", System.Environment.UserName);
		file = file.Replace("#PROJECTNAME#", PlayerSettings.productName);

		System.IO.File.WriteAllText(path, file);
		AssetDatabase.Refresh();
	}
}
