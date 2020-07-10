using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class MotionFramework_Tween_TweenNode_1_Vector2_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(MotionFramework.Tween.TweenNode<UnityEngine.Vector2>);
            args = new Type[]{typeof(MotionFramework.Tween.TweenNode<UnityEngine.Vector2>.TweenLerpDelegate)};
            method = type.GetMethod("SetLerp", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetLerp_0);


        }


        static StackObject* SetLerp_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            MotionFramework.Tween.TweenNode<UnityEngine.Vector2>.TweenLerpDelegate @lerp = (MotionFramework.Tween.TweenNode<UnityEngine.Vector2>.TweenLerpDelegate)typeof(MotionFramework.Tween.TweenNode<UnityEngine.Vector2>.TweenLerpDelegate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            MotionFramework.Tween.TweenNode<UnityEngine.Vector2> instance_of_this_method = (MotionFramework.Tween.TweenNode<UnityEngine.Vector2>)typeof(MotionFramework.Tween.TweenNode<UnityEngine.Vector2>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.SetLerp(@lerp);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
