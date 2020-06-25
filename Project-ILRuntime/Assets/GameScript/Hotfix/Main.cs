
namespace Hotfix
{
	public static class Main
	{
		public static void Start()
		{
			HotfixLogger.Log("Hello ILRuntime World");
			Demo.Instance.Start();
		}
		public static void Update()
		{
			Demo.Instance.Update();
		}
		public static void LateUpdate()
		{
		}
		public static void UILanguage(string key)
		{
			throw new System.NotImplementedException();
		}
	}
}