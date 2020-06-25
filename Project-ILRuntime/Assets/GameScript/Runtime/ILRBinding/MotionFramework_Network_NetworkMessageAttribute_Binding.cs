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
    unsafe class MotionFramework_Network_NetworkMessageAttribute_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(MotionFramework.Network.NetworkMessageAttribute);

            field = type.GetField("MsgID", flag);
            app.RegisterCLRFieldGetter(field, get_MsgID_0);
            app.RegisterCLRFieldSetter(field, set_MsgID_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_MsgID_0, AssignFromStack_MsgID_0);


        }



        static object get_MsgID_0(ref object o)
        {
            return ((MotionFramework.Network.NetworkMessageAttribute)o).MsgID;
        }

        static StackObject* CopyToStack_MsgID_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((MotionFramework.Network.NetworkMessageAttribute)o).MsgID;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_MsgID_0(ref object o, object v)
        {
            ((MotionFramework.Network.NetworkMessageAttribute)o).MsgID = (System.Int32)v;
        }

        static StackObject* AssignFromStack_MsgID_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @MsgID = ptr_of_this_method->Value;
            ((MotionFramework.Network.NetworkMessageAttribute)o).MsgID = @MsgID;
            return ptr_of_this_method;
        }



    }
}
