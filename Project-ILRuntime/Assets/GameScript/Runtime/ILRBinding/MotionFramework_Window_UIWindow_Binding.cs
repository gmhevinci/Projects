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
    unsafe class MotionFramework_Window_UIWindow_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(MotionFramework.Window.UIWindow);

            field = type.GetField("TweenGrouper", flag);
            app.RegisterCLRFieldGetter(field, get_TweenGrouper_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_TweenGrouper_0, null);


        }



        static object get_TweenGrouper_0(ref object o)
        {
            return ((MotionFramework.Window.UIWindow)o).TweenGrouper;
        }

        static StackObject* CopyToStack_TweenGrouper_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((MotionFramework.Window.UIWindow)o).TweenGrouper;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
