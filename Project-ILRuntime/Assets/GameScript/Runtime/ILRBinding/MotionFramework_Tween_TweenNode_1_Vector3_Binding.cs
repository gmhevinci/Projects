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
    unsafe class MotionFramework_Tween_TweenNode_1_Vector3_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(MotionFramework.Tween.TweenNode<UnityEngine.Vector3>);
            args = new Type[]{typeof(MotionFramework.Tween.TweenNode<UnityEngine.Vector3>.TweenEaseDelegate)};
            method = type.GetMethod("SetEase", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetEase_0);
            args = new Type[]{typeof(System.Action)};
            method = type.GetMethod("SetDispose", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetDispose_1);


        }


        static StackObject* SetEase_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            MotionFramework.Tween.TweenNode<UnityEngine.Vector3>.TweenEaseDelegate @ease = (MotionFramework.Tween.TweenNode<UnityEngine.Vector3>.TweenEaseDelegate)typeof(MotionFramework.Tween.TweenNode<UnityEngine.Vector3>.TweenEaseDelegate).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            MotionFramework.Tween.TweenNode<UnityEngine.Vector3> instance_of_this_method = (MotionFramework.Tween.TweenNode<UnityEngine.Vector3>)typeof(MotionFramework.Tween.TweenNode<UnityEngine.Vector3>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.SetEase(@ease);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SetDispose_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @onDispose = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            MotionFramework.Tween.TweenNode<UnityEngine.Vector3> instance_of_this_method = (MotionFramework.Tween.TweenNode<UnityEngine.Vector3>)typeof(MotionFramework.Tween.TweenNode<UnityEngine.Vector3>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.SetDispose(@onDispose);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
