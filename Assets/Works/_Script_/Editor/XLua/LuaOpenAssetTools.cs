using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class LuaOpenModeTools
{
	[UnityEditor.Callbacks.OnOpenAssetAttribute(1)]
	public static bool ClickOnce(int instanceID, int line)
	{
		return false;
	}

	[UnityEditor.Callbacks.OnOpenAssetAttribute(2)]
	public static bool ClickTwice(int instanceID, int line)
	{
		string path = AssetDatabase.GetAssetPath(EditorUtility.InstanceIDToObject(instanceID));
		string name = Application.dataPath + "/" + path.Replace("Assets/", "");

		if (name.EndsWith(".txt"))
		{
			// Sublime安装路径
			string sublimePath = "C:/Program Files/Sublime Text 3/sublime_text.exe";
			if(File.Exists(sublimePath))
			{
				System.Diagnostics.Process process = new System.Diagnostics.Process();
				System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
				startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
				startInfo.FileName = sublimePath;
				startInfo.Arguments = name;
				process.StartInfo = startInfo;
				process.Start();
				return true;
			}
			else
			{
				Debug.LogWarning($"Not found sublime : {sublimePath}");
			}
		}

		return false;
	}
}