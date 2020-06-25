using System;
using System.Text;
using System.IO;
using UnityEditor;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;

public static class ILRuntimeCLRBinding
{
	[MenuItem("Tools/ILRuntime/Generate CLR Binding Code by Analysis")]
	static void GenerateCLRBindingByAnalysis()
	{
		// 先删除旧代码
		AssetDatabase.DeleteAsset(ILRDefine.StrMyBindingFolderPath);
		Directory.CreateDirectory(ILRDefine.StrMyBindingFolderPath);

		// 分析热更DLL来生成绑定代码
		ILRuntime.Runtime.Enviorment.AppDomain domain = new ILRuntime.Runtime.Enviorment.AppDomain();
		string dllFilePath = $"{ILRDefine.StrMyAssemblyFolderPath}/{ILRDefine.StrMyHotfixDLLFileName}.bytes";
		using (FileStream fs = new FileStream(dllFilePath, FileMode.Open, FileAccess.Read))
		{
			domain.LoadAssembly(fs);

			// Crossbind Adapter is needed to generate the correct binding code
			ILRRegister.Register(domain);

			// 生成所有绑定脚本
			ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(domain, ILRDefine.StrMyBindingFolderPath);
		}

		// 刷新目录
		AssetDatabase.Refresh();
	}

	[MenuItem("Tools/ILRuntime/Generate CLR Adapter Code")]
	static void GenerateCLRBindingAdapterCode()
	{
		// 先删除旧代码
		AssetDatabase.DeleteAsset(ILRDefine.StrMyAdapterFolderPath);
		Directory.CreateDirectory(ILRDefine.StrMyAdapterFolderPath);

		CreateBindingAdapterFile(typeof(CanvasWindow));
		CreateBindingAdapterFile(typeof(MotionFramework.Config.ConfigTable));
		CreateBindingAdapterFile(typeof(MotionFramework.Config.AssetConfig));
		CreateBindingAdapterFile(typeof(MotionFramework.Event.IEventMessage));
		CreateBindingAdapterFile(typeof(MotionFramework.AI.IFsmNode));
		CreateBindingAdapterFile(typeof(Google.Protobuf.IMessage));

		// 刷新目录
		AssetDatabase.Refresh();
	}

	static void CreateBindingAdapterFile(Type type)
	{
		string content = CrossBindingCodeGenerator.GenerateCrossBindingAdapterCode(type, type.Namespace);
		string filePath = $"{ILRDefine.StrMyAdapterFolderPath}/{type.Name}Adapter.cs";
		File.WriteAllText(filePath, content, Encoding.UTF8);
		UnityEngine.Debug.Log($"Create ILRuntime adapter file : {filePath}");
	}
}
