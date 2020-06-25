using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;

public class CanvasWindowAdapter : CrossBindingAdaptor
{
	public override Type BaseCLRType
	{
		get
		{
			return typeof(global::CanvasWindow);
		}
	}

	public override Type AdaptorType
	{
		get
		{
			return typeof(Adapter);
		}
	}

	public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
	{
		return new Adapter(appdomain, instance);
	}

	public class Adapter : global::CanvasWindow, CrossBindingAdaptorType
	{
		ILTypeInstance instance;
		ILRuntime.Runtime.Enviorment.AppDomain appdomain;

		CrossBindingFunctionInfo<System.Int32> mget_Depth_0 = new CrossBindingFunctionInfo<System.Int32>("get_Depth");
		CrossBindingMethodInfo<System.Int32> mset_Depth_1 = new CrossBindingMethodInfo<System.Int32>("set_Depth");
		CrossBindingFunctionInfo<System.Boolean> mget_Visible_2 = new CrossBindingFunctionInfo<System.Boolean>("get_Visible");
		CrossBindingMethodInfo<System.Boolean> mset_Visible_3 = new CrossBindingMethodInfo<System.Boolean>("set_Visible");
		CrossBindingMethodInfo<UnityEngine.GameObject> mOnAssetLoad_4 = new CrossBindingMethodInfo<UnityEngine.GameObject>("OnAssetLoad");
		CrossBindingMethodInfo mOnCreate_5 = new CrossBindingMethodInfo("OnCreate");
		CrossBindingMethodInfo mOnDestroy_6 = new CrossBindingMethodInfo("OnDestroy");
		CrossBindingMethodInfo mOnRefresh_7 = new CrossBindingMethodInfo("OnRefresh");
		CrossBindingMethodInfo mOnUpdate_8 = new CrossBindingMethodInfo("OnUpdate");
		CrossBindingMethodInfo mOnSortDepth_9 = new CrossBindingMethodInfo("OnSortDepth");
		CrossBindingMethodInfo mOnSetVisible_10 = new CrossBindingMethodInfo("OnSetVisible");

		public Adapter()
		{

		}

		public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
		{
			this.appdomain = appdomain;
			this.instance = instance;
		}

		public ILTypeInstance ILInstance { get { return instance; } }

		protected override void OnAssetLoad(UnityEngine.GameObject go)
		{
			if (mOnAssetLoad_4.CheckShouldInvokeBase(this.instance))
				base.OnAssetLoad(go);
			else
				mOnAssetLoad_4.Invoke(this.instance, go);
		}

		public override void OnCreate()
		{
			mOnCreate_5.Invoke(this.instance);
		}

		public override void OnDestroy()
		{
			mOnDestroy_6.Invoke(this.instance);
		}

		public override void OnRefresh()
		{
			mOnRefresh_7.Invoke(this.instance);
		}

		public override void OnUpdate()
		{
			mOnUpdate_8.Invoke(this.instance);
		}

		public override void OnSortDepth()
		{
			if (mOnSortDepth_9.CheckShouldInvokeBase(this.instance))
				base.OnSortDepth();
			else
				mOnSortDepth_9.Invoke(this.instance);
		}

		public override void OnSetVisible()
		{
			if (mOnSetVisible_10.CheckShouldInvokeBase(this.instance))
				base.OnSetVisible();
			else
				mOnSetVisible_10.Invoke(this.instance);
		}

		public override System.Int32 Depth
		{
			get
			{
				if (mget_Depth_0.CheckShouldInvokeBase(this.instance))
					return base.Depth;
				else
					return mget_Depth_0.Invoke(this.instance);

			}
			set
			{
				if (mset_Depth_1.CheckShouldInvokeBase(this.instance))
					base.Depth = value;
				else
					mset_Depth_1.Invoke(this.instance, value);

			}
		}

		public override System.Boolean Visible
		{
			get
			{
				if (mget_Visible_2.CheckShouldInvokeBase(this.instance))
					return base.Visible;
				else
					return mget_Visible_2.Invoke(this.instance);

			}
			set
			{
				if (mset_Visible_3.CheckShouldInvokeBase(this.instance))
					base.Visible = value;
				else
					mset_Visible_3.Invoke(this.instance, value);

			}
		}

		public override string ToString()
		{
			IMethod m = appdomain.ObjectType.GetMethod("ToString", 0);
			m = instance.Type.GetVirtualMethod(m);
			if (m == null || m is ILMethod)
			{
				return instance.ToString();
			}
			else
				return instance.Type.FullName;
		}
	}
}