﻿
public class ILRDefine
{
	/// <summary>
	/// DLL编译的临时存储目录
	/// </summary>
	public const string StrAssemblyTemperDir = "Temp/Assembly";

	/// <summary>
	/// Unity编译的DLL存储目录
	/// </summary>
	public const string StrScriptAssembliesDir = "Library/ScriptAssemblies";

	/// <summary>
	/// 我们设定的DLL存储目录
	/// </summary>
	public const string StrMyAssemblyDir = GameDefine.AssetRootPath + "/Assembly";

	/// <summary>
	/// 热更程序集的名称
	/// </summary>
	public const string StrHotfixAssemblyName = "Hotfix";

	/// <summary>
	/// 我们设定的热更DLL文件名称
	/// </summary>
	public const string StrMyHotfixDLLFileName = "HotfixDLL";

	/// <summary>
	/// 我们设定的热更PDB文件名称
	/// </summary>
	public const string StrMyHotfixPDBFileName = "HotfixPDB";

	/// <summary>
	/// 我们设定的自动生成的绑定脚本夹路径
	/// </summary>
	public const string StrMyBindingFolderPath = "Assets/GameScript/Runtime/ILRBinding";

	/// <summary>
	/// 我们设定的自动生成的适配脚本夹路径
	/// </summary>
	public const string StrMyAdapterFolderPath = "Assets/GameScript/Runtime/Temper";
}