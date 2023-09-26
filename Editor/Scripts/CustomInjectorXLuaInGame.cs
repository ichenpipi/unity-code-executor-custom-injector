using ChenPipi.CodeExecutor.Editor;
using ChenPipi.CodeExecutor.Examples;
using UnityEditor;
using UnityEngine;

namespace ChenPipi.CodeExecutorCustomInjector.Editor
{

    /// <summary>
    /// 给 CodeExecutor 注入代码执行模式
    /// </summary>
    public static class CustomInjectorXLuaInGame
    {

        /// <summary>
        /// 注册 CodeExecutor 执行模式
        /// </summary>
        [CodeExecutorRegistration(2)]
        private static void Register()
        {
            // 初始化 xLua Helper
            if (!ExecutionHelperXLua.isReady && !ExecutionHelperXLua.Init(CustomInjectorXLua.XLuaAssemblyName))
            {
                return;
            }
            // 注册
            CodeExecutorManager.RegisterExecMode(new ExecutionMode
            {
                name = "xLua (InGame)",
                executor = Executor,
            });
        }

        private static object[] Executor(string code)
        {
            object luaEnv = GetLuaEnv();
            if (luaEnv == null)
            {
                if (!CodeExecutorManager.ShowNotification("请先运行游戏"))
                {
                    EditorUtility.DisplayDialog("Code Executor", "请先运行游戏！", "OK");
                }
                return null;
            }

            object[] results = ExecutionHelperXLua.ExecuteCode(code, luaEnv);

            CodeExecutorManager.ShowNotification("已执行");

            return results;
        }

        private static object GetLuaEnv()
        {
            // Return your LuaEnv object
            if (LuaManager.Instance != null)
            {
                return LuaManager.Instance.luaEnv;
            }
            return null;
        }

    }

}
