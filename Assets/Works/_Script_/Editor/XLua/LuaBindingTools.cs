using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using XLua;

public static class LuaBindingTools
{
	// MotionFramework
	[LuaCallCSharp]
	public static List<Type> MotitonFramework_List
	{
		get
		{
			List<Type> result = new List<Type>();

			Type[] allTypes = Assembly.Load("MotionEngine").GetTypes();
			for (int i = 0; i < allTypes.Length; i++)
			{
				Type type = allTypes[i];
				if (string.IsNullOrEmpty(type.Namespace))
					continue;
				if (type.Namespace.Contains("MotionFramework"))
					result.Add(type);
			}

			allTypes = Assembly.Load("MotionGame").GetTypes();
			for (int i = 0; i < allTypes.Length; i++)
			{
				Type type = allTypes[i];
				if (string.IsNullOrEmpty(type.Namespace))
					continue;
				if (type.Namespace.Contains("MotionFramework"))
					result.Add(type);
			}

			Debug.Log($"[XLua] MotitonFramework binding count : {result.Count}");
			return result;
		}
	}

	// UGUI
	[LuaCallCSharp]
	public static List<Type> UnityEngine_UI_List
	{
		get
		{
			List<Type> result = new List<Type>();
			Type[] allTypes = Assembly.Load("UnityEngine.UI").GetTypes();
			for (int i = 0; i < allTypes.Length; i++)
			{
				Type type = allTypes[i];

				// 黑名单
				if (type.Name == "GraphicRebuildTracker")
					continue;

				if (type.IsPublic == false)
					continue;
				if (string.IsNullOrEmpty(type.Namespace))
					continue;
				if (type.Namespace.Contains("UnityEngine.UI"))
					result.Add(type);
			}

			Debug.Log($"[XLua] UnityEngine.UI binding count : {result.Count}");
			return result;
		}
	}

	// System
	[LuaCallCSharp]
	public static List<Type> System_List = new List<Type>()
	{
		typeof(System.Object),
		typeof(System.Array),
		typeof(System.DateTime),
		typeof(System.Collections.Generic.List<string>),
	};

	// UnityEngine
	[LuaCallCSharp]
	public static List<Type> Unity_Engine_List = new List<Type>()
	{
		typeof(UnityEngine.Object),
		typeof(UnityEngine.Debug),

		typeof(Application),
		typeof(PlayerPrefs),
		typeof(Time),
		typeof(Touch),
		typeof(Input),

		//数学
		typeof(Mathf),
		typeof(Matrix4x4),
		typeof(Vector2),
		typeof(Vector3),
		typeof(Vector4),
		typeof(Quaternion),

		//物理
		//typeof(Physics), //代码量巨大禁用，请写单独的接口类
		//typeof(Physics2D), //代码量巨大禁用，请写单独的接口类
		typeof(Ray),
		typeof(Ray2D),
		typeof(RaycastHit),
		typeof(RaycastHit2D),

		//组件
		typeof(Component),
		typeof(Behaviour),
		typeof(MonoBehaviour),
		typeof(GameObject),
		typeof(Transform),
		typeof(RectTransform),
		typeof(RectTransformUtility),

		//资源
		typeof(Resources),
		typeof(AssetBundle),
		typeof(AssetBundleManifest),
		typeof(AssetBundleRequest),
		typeof(Shader),
		typeof(Material),
		typeof(MaterialPropertyBlock),
		typeof(TextAsset),
		typeof(AudioClip),
		typeof(AudioSource),
		typeof(Texture),
		typeof(Texture2D),
		typeof(Sprite),

		//UI
		typeof(Canvas),
		typeof(CanvasGroup),
		typeof(CanvasRenderer),
		typeof(SpriteMask),
		typeof(SortingLayer),
		typeof(RenderTexture),
		typeof(RenderTextureFormat),
		typeof(RenderTextureReadWrite),
		typeof(Bounds),
		typeof(Rect),
		typeof(Color),
		typeof(ColorUtility),

		//动画
		typeof(Animator),
		typeof(Animation),
		typeof(AnimationCurve),
		typeof(AnimationClip),
		typeof(AnimationEvent),
		typeof(AnimationState),
		typeof(AnimationBlendMode),
		typeof(AnimationCullingType),
		typeof(AnimationPlayMode),
		typeof(Keyframe),

		//视频
		typeof(UnityEngine.Video.VideoPlayer),

		//渲染
		typeof(Camera),
		typeof(Screen),
		typeof(Renderer),
		typeof(MeshRenderer),
		typeof(SkinnedMeshRenderer),
		typeof(TrailRenderer),
		typeof(ParticleSystem),

		//U2D
		typeof(UnityEngine.U2D.SpriteAtlas),
	};

	// C#静态调用Lua的配置（包括事件的原型）[仅可以配delegate，interface]
	[CSharpCallLua]
	public static List<Type> CSharpCallLua = new List<Type>()
	{
		typeof(Func<double, double, double>),
		typeof(Action),
		typeof(Action<string>),
		typeof(Action<float>),
		typeof(Action<double>),
		typeof(Action<int>),
		typeof(Action<long>),
		typeof(Action<UnityEngine.GameObject>),
		typeof(Action<UnityEngine.GameObject, int>),
		typeof(Action<MotionFramework.Resource.AssetOperationHandle>),
		typeof(System.Collections.IEnumerator),
		typeof(UnityEngine.Events.UnityAction),
		typeof(UnityEngine.Events.UnityAction<string>),
		typeof(UnityEngine.Events.UnityAction<float>),
		typeof(UnityEngine.Events.UnityAction<double>),
		typeof(UnityEngine.Events.UnityAction<int>),
		typeof(UnityEngine.Events.UnityAction<long>),
		typeof(UnityEngine.Events.UnityAction<bool>),
		typeof(UnityEngine.Video.VideoPlayer.EventHandler),
	};

	//黑名单
	[BlackList]
	public static List<List<string>> BlackList = new List<List<string>>()
	{
		new List<string>(){"System.Xml.XmlNodeList", "ItemOf"},
		new List<string>(){"UnityEngine.WWW", "movie"},
    #if UNITY_WEBGL
		new List<string>(){"UnityEngine.WWW", "threadPriority"},
    #endif
        new List<string>(){"UnityEngine.Texture2D", "alphaIsTransparency"},
		new List<string>(){"UnityEngine.Security", "GetChainOfTrustValue"},
		new List<string>(){"UnityEngine.CanvasRenderer", "onRequestRebuild"},
		new List<string>(){"UnityEngine.Light", "areaSize"},
		new List<string>(){"UnityEngine.Light", "lightmapBakeType"},
		new List<string>(){"UnityEngine.WWW", "MovieTexture"},
		new List<string>(){"UnityEngine.WWW", "GetMovieTexture"},
		new List<string>(){"UnityEngine.AnimatorOverrideController", "PerformOverrideClipListCleanup"},
    #if !UNITY_WEBPLAYER
		new List<string>(){"UnityEngine.Application", "ExternalEval"},
    #endif
		new List<string>(){"UnityEngine.GameObject", "networkView"}, //4.6.2 not support
		new List<string>(){"UnityEngine.Component", "networkView"},  //4.6.2 not support
		new List<string>(){"System.IO.FileInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
		new List<string>(){"System.IO.FileInfo", "SetAccessControl", "System.Security.AccessControl.FileSecurity"},
		new List<string>(){"System.IO.DirectoryInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
		new List<string>(){"System.IO.DirectoryInfo", "SetAccessControl", "System.Security.AccessControl.DirectorySecurity"},
		new List<string>(){"System.IO.DirectoryInfo", "CreateSubdirectory", "System.String", "System.Security.AccessControl.DirectorySecurity"},
		new List<string>(){"System.IO.DirectoryInfo", "Create", "System.Security.AccessControl.DirectorySecurity"},
		new List<string>(){"UnityEngine.MonoBehaviour", "runInEditMode"},

		// 自定义黑名单
		new List<string>() { "UnityEngine.Input", "IsJoystickPreconfigured", "System.String"},
		new List<string>() { "UnityEngine.Texture", "imageContentsHash"},
		new List<string>() { "UnityEngine.UI.Graphic", "OnRebuildRequested"},
		new List<string>() { "UnityEngine.UI.Text", "OnRebuildRequested"},
		new List<string>() { "UnityEngine.UI.UIAtlas", "OnValidate"},
		new List<string>() { "UnityEngine.UI.UIAnimation", "EditotUpdate"},
	};
}