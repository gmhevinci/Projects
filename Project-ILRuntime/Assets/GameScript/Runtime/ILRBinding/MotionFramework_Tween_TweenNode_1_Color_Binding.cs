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
    unsafe class MotionFramework_Tween_TweenNode_1_Color_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(MotionFramework.Tween.TweenNode<UnityEngine.Color>);
            args = new Type[]{typeof(MotionFramework.Tween.ETweenLoop), typeof(System.Int32)};
            method = type.GetMethod("SetLoop", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetLoop_0);


        }


        static StackObject* SetLoop_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @loopCount = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            MotionFramework.Tween.ETweenLoop @tweenLoop = (MotionFramework.Tween.ETweenLoop)typeof(MotionFramework.Tween.ETweenLoop).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            MotionFramework.Tween.TweenNode<UnityEngine.Color> instance_of_this_method = (MotionFramework.Tween.TweenNode<UnityEngine.Color>)typeof(MotionFramework.Tween.TweenNode<UnityEngine.Color>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.SetLoop(@tweenLoop, @loopCount);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
