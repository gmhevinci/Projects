using System;
using System.Collections.Generic;
using System.Reflection;

namespace ILRuntime.Runtime.Generated
{
    class CLRBindings
    {

        internal static ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Vector2> s_UnityEngine_Vector2_Binding_Binder = null;
        internal static ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Vector3> s_UnityEngine_Vector3_Binding_Binder = null;
        internal static ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Quaternion> s_UnityEngine_Quaternion_Binding_Binder = null;

        /// <summary>
        /// Initialize the CLR binding, please invoke this AFTER CLR Redirection registration
        /// </summary>
        public static void Initialize(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            UnityEngine_GameObject_Binding.Register(app);
            UnityEngine_UI_Button_Binding.Register(app);
            UnityEngine_Events_UnityEvent_Binding.Register(app);
            MotionFramework_Resource_AssetReference_Binding.Register(app);
            MotionFramework_Resource_AssetOperationHandle_Binding.Register(app);
            MotionFramework_ModuleSingleton_1_NetworkManager_Binding.Register(app);
            MotionFramework_Network_NetworkManager_Binding.Register(app);
            UnityEngine_Vector3_Binding.Register(app);
            UnityEngine_Transform_Binding.Register(app);
            MotionFramework_AI_FiniteStateMachine_Binding.Register(app);
            System_String_Binding.Register(app);
            UnityEngine_Debug_Binding.Register(app);
            System_Type_Binding.Register(app);
            System_Reflection_MemberInfo_Binding.Register(app);
            MotionFramework_ModuleSingleton_1_ILRManager_Binding.Register(app);
            ILRManager_Binding.Register(app);
            System_Collections_Generic_List_1_Type_Binding.Register(app);
            System_NotImplementedException_Binding.Register(app);
            Google_Protobuf_ProtoPreconditions_Binding.Register(app);
            Google_Protobuf_CodedOutputStream_Binding.Register(app);
            Google_Protobuf_CodedInputStream_Binding.Register(app);
            MotionFramework_Network_DefaultNetworkPackage_Binding.Register(app);
            DoubleMap_2_Int32_Type_Binding.Register(app);
            System_Activator_Binding.Register(app);
            ProtobufHelper_Binding.Register(app);
            System_Object_Binding.Register(app);
            System_Attribute_Binding.Register(app);
            MotionFramework_Network_NetworkMessageAttribute_Binding.Register(app);
            System_Exception_Binding.Register(app);

            ILRuntime.CLR.TypeSystem.CLRType __clrType = null;
            __clrType = (ILRuntime.CLR.TypeSystem.CLRType)app.GetType (typeof(UnityEngine.Vector2));
            s_UnityEngine_Vector2_Binding_Binder = __clrType.ValueTypeBinder as ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Vector2>;
            __clrType = (ILRuntime.CLR.TypeSystem.CLRType)app.GetType (typeof(UnityEngine.Vector3));
            s_UnityEngine_Vector3_Binding_Binder = __clrType.ValueTypeBinder as ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Vector3>;
            __clrType = (ILRuntime.CLR.TypeSystem.CLRType)app.GetType (typeof(UnityEngine.Quaternion));
            s_UnityEngine_Quaternion_Binding_Binder = __clrType.ValueTypeBinder as ILRuntime.Runtime.Enviorment.ValueTypeBinder<UnityEngine.Quaternion>;
        }

        /// <summary>
        /// Release the CLR binding, please invoke this BEFORE ILRuntime Appdomain destroy
        /// </summary>
        public static void Shutdown(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            s_UnityEngine_Vector2_Binding_Binder = null;
            s_UnityEngine_Vector3_Binding_Binder = null;
            s_UnityEngine_Quaternion_Binding_Binder = null;
        }
    }
}
