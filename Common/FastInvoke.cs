/**********************************************************************************
 * 程序说明：     反射方法类
 * 创建日期：     2009.9.20
 * 修改日期：     2013.07.15
 * 程序制作：     agui
 * 联系方式：     mailto:354990393@qq.com  
 * 版权申明：     http:
 * ********************************************************************************/

using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Common
{
    public delegate object FastInvokeHandler(object target, object[] paramters);
    /// <summary>
    /// 快速反射调用方法
    /// 1.(用Delegate调用方法)->
    /// private delegate void myDelegate(string str);
    /// private static myDelegate execPage = null;
    /// private static PlugNT.Shop.Pages.AppUtility app = null;
    /// if (execPage == null)
    /// {
    ///    //实体类
    ///    //execPage = new myDelegate(PlugNT.Shop.Pages.AppUtility.ExecutePage);
    ///    //app = new PlugNT.Shop.Pages.AppUtility();
    ///    //execPage = (myDelegate)Delegate.CreateDelegate(typeof(myDelegate), app, "ExecutePage"); 
    ///    //静态类
    ///     execPage = (myDelegate)Delegate.CreateDelegate(typeof(myDelegate), typeof(PlugNT.Shop.Pages.AppUtility), "ExecutePage");
    /// }
    /// execPage("TP_Default.aspx");
    /// 2.(用Emit调用方法)->
    /// </summary>
    public class FastInvoke
    {
        /// <summary>
        /// 调用方法(用Emit)
        /// Type t = typeof(Person);
        /// MethodInfo methodInfo = t.GetMethod("Say");
        /// Person person = new Person();
        /// FastInvokeHandler fastInvoker = FastInvoke.GetMethodInvoker(methodInfo);
        /// fastInvoker(person, param);
        /// 链接地址
        /// http://blog.163.com/dreamman_yx/blog/static/26526894201092825312949/
        /// http://www.cnblogs.com/heekui/archive/2007/01/10/616654.html
        /// http://hi.baidu.com/luchaoshuai/blog/item/6c8bf0a12b9a908346106444.html
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static FastInvokeHandler GetMethodInvoker(MethodInfo methodInfo)
        {
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new Type[] { typeof(object), typeof(object[]) }, methodInfo.DeclaringType.Module);
            ILGenerator il = dynamicMethod.GetILGenerator();
            ParameterInfo[] ps = methodInfo.GetParameters();
            Type[] paramTypes = new Type[ps.Length];
            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                    paramTypes[i] = ps[i].ParameterType.GetElementType();
                else
                    paramTypes[i] = ps[i].ParameterType;
            }
            LocalBuilder[] locals = new LocalBuilder[paramTypes.Length];
            for (int i = 0; i < paramTypes.Length; i++)
            {
                locals[i] = il.DeclareLocal(paramTypes[i], true);
            }
            for (int i = 0; i < paramTypes.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_1);
                EmitFastInt(il, i);
                il.Emit(OpCodes.Ldelem_Ref);
                EmitCastToReference(il, paramTypes[i]);
                il.Emit(OpCodes.Stloc, locals[i]);
            }
            if (!methodInfo.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                    il.Emit(OpCodes.Ldloca_S, locals[i]);
                else
                    il.Emit(OpCodes.Ldloc, locals[i]);
            }
            if (methodInfo.IsStatic)
                il.EmitCall(OpCodes.Call, methodInfo, null);
            else
                il.EmitCall(OpCodes.Callvirt, methodInfo, null);
            if (methodInfo.ReturnType == typeof(void))
                il.Emit(OpCodes.Ldnull);
            else
                EmitBoxIfNeeded(il, methodInfo.ReturnType);
            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                {
                    il.Emit(OpCodes.Ldarg_1);
                    EmitFastInt(il, i);
                    il.Emit(OpCodes.Ldloc, locals[i]);
                    if (locals[i].LocalType.IsValueType)
                        il.Emit(OpCodes.Box, locals[i].LocalType);
                    il.Emit(OpCodes.Stelem_Ref);
                }
            }
            il.Emit(OpCodes.Ret);
            FastInvokeHandler invoder = (FastInvokeHandler)dynamicMethod.CreateDelegate(typeof(FastInvokeHandler));
            return invoder;
        }
        private static void EmitCastToReference(ILGenerator il, System.Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, type);
            }
            else
            {
                il.Emit(OpCodes.Castclass, type);
            }
        }
        private static void EmitBoxIfNeeded(ILGenerator il, System.Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Box, type);
            }
        }
        private static void EmitFastInt(ILGenerator il, int value)
        {
            switch (value)
            {
                case -1:
                    il.Emit(OpCodes.Ldc_I4_M1);
                    return;
                case 0:
                    il.Emit(OpCodes.Ldc_I4_0);
                    return;
                case 1:
                    il.Emit(OpCodes.Ldc_I4_1);
                    return;
                case 2:
                    il.Emit(OpCodes.Ldc_I4_2);
                    return;
                case 3:
                    il.Emit(OpCodes.Ldc_I4_3);
                    return;
                case 4:
                    il.Emit(OpCodes.Ldc_I4_4);
                    return;
                case 5:
                    il.Emit(OpCodes.Ldc_I4_5);
                    return;
                case 6:
                    il.Emit(OpCodes.Ldc_I4_6);
                    return;
                case 7:
                    il.Emit(OpCodes.Ldc_I4_7);
                    return;
                case 8:
                    il.Emit(OpCodes.Ldc_I4_8);
                    return;
            }
            if (value > -129 && value < 128)
            {
                il.Emit(OpCodes.Ldc_I4_S, (SByte)value);
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4, value);
            }
        }
    }
}

