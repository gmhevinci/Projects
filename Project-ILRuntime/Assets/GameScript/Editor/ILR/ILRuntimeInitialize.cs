using System.IO;
using UnityEditor;
using UnityEngine;

public class ILRuntimeInitialize
{
	[InitializeOnLoadMethod]
	static void InitializeCopyAssemblyFiles()
	{
		// 创建目录
		if (Directory.Exists(ILRDefine.StrMyAssemblyDir) == false)
			Directory.CreateDirectory(ILRDefine.StrMyAssemblyDir);

		// Copy DLL
		string dllSource = Path.Combine(ILRDefine.StrScriptAssembliesDir, $"{ILRDefine.StrHotfixAssemblyName}.dll");
		string dllDest = Path.Combine(ILRDefine.StrMyAssemblyDir, $"{ILRDefine.StrMyHotfixDLLFileName}.bytes");
		AssetDatabase.DeleteAsset(dllDest);
		if (File.Exists(dllSource))
			File.Copy(dllSource, dllDest, true);

		// Copy PDB
		string pdbSource = Path.Combine(ILRDefine.StrScriptAssembliesDir, $"{ILRDefine.StrHotfixAssemblyName}.pdb");
		string pdbDest = Path.Combine(ILRDefine.StrMyAssemblyDir, $"{ILRDefine.StrMyHotfixPDBFileName}.bytes");
		AssetDatabase.DeleteAsset(pdbDest);
		if (File.Exists(pdbSource))
			File.Copy(pdbSource, pdbDest, true);

		Debug.Log("Copy hotfix assembly files done.");
		AssetDatabase.Refresh();
	}
}