using ChenPipi.CodeExecutor.Editor;
using ChenPipi.CodeExecutor.Examples;
using UnityEditor;
using UnityEngine;

namespace ChenPipi.CodeExecutorCustomInjector.Editor
{

    /// <summary>
    /// 给 CodeExecutor 注入代码执行模式
    /// </summary>
    public static class CustomInjectorXLua
    {

        /// <summary>
        /// xLua 所在的程序集名称，置空则使用默认程序集 "Assembly-CSharp"
        /// </summary>
        public const string XLuaAssemblyName = "Assembly-CSharp";

        /// <summary>
        /// 注册 CodeExecutor 执行模式
        /// </summary>
        [CodeExecutorRegistration(1)]
        private static void Register()
        {
            // 禁用内置的 xLua 执行模式
            if (CodeExecutorManager.enableBuiltinExecModeXLua)
            {
                CodeExecutorManager.enableBuiltinExecModeXLua = false;
            }
            // 初始化 xLua Helper
            if (!ExecutionHelperXLua.isReady && !ExecutionHelperXLua.Init(XLuaAssemblyName))
            {
                return;
            }
            // 注册
            CodeExecutorManager.RegisterExecMode(new ExecutionMode
            {
                name = "xLua (Standalone)",
                executor = Executor,
            });
        }

        private static object[] Executor(string code)
        {
            object[] results = ExecutionHelperXLua.ExecuteCode(code);

            CodeExecutorManager.ShowNotification("已执行");

            return results;
        }

    }

}
