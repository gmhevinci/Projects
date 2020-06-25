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
    unsafe class MotionFramework_Network_NetworkManager_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(MotionFramework.Network.NetworkManager);
            args = new Type[]{};
            method = type.GetMethod("get_States", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_States_0);
            args = new Type[]{typeof(System.String), typeof(System.Int32)};
            method = type.GetMethod("ConnectServer", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ConnectServer_1);
            args = new Type[]{typeof(MotionFramework.Network.INetworkPackage)};
            method = type.GetMethod("SendMessage", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SendMessage_2);

            field = type.GetField("HotfixPackageCallback", flag);
            app.RegisterCLRFieldGetter(field, get_HotfixPackageCallback_0);
            app.RegisterCLRFieldSetter(field, set_HotfixPackageCallback_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_HotfixPackageCallback_0, AssignFromStack_HotfixPackageCallback_0);


        }


        static StackObject* get_States_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            MotionFramework.Network.NetworkManager instance_of_this_method = (MotionFramework.Network.NetworkManager)typeof(MotionFramework.Network.NetworkManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.States;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ConnectServer_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @port = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @host = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            MotionFramework.Network.NetworkManager instance_of_this_method = (MotionFramework.Network.NetworkManager)typeof(MotionFramework.Network.NetworkManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ConnectServer(@host, @port);

            return __ret;
        }

        static StackObject* SendMessage_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            MotionFramework.Network.INetworkPackage @package = (MotionFramework.Network.INetworkPackage)typeof(MotionFramework.Network.INetworkPackage).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            MotionFramework.Network.NetworkManager instance_of_this_method = (MotionFramework.Network.NetworkManager)typeof(MotionFramework.Network.NetworkManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SendMessage(@package);

            return __ret;
        }


        static object get_HotfixPackageCallback_0(ref object o)
        {
            return ((MotionFramework.Network.NetworkManager)o).HotfixPackageCallback;
        }

        static StackObject* CopyToStack_HotfixPackageCallback_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((MotionFramework.Network.NetworkManager)o).HotfixPackageCallback;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_HotfixPackageCallback_0(ref object o, object v)
        {
            ((MotionFramework.Network.NetworkManager)o).HotfixPackageCallback = (System.Action<MotionFramework.Network.INetworkPackage>)v;
        }

        static StackObject* AssignFromStack_HotfixPackageCallback_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<MotionFramework.Network.INetworkPackage> @HotfixPackageCallback = (System.Action<MotionFramework.Network.INetworkPackage>)typeof(System.Action<MotionFramework.Network.INetworkPackage>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((MotionFramework.Network.NetworkManager)o).HotfixPackageCallback = @HotfixPackageCallback;
            return ptr_of_this_method;
        }



    }
}
