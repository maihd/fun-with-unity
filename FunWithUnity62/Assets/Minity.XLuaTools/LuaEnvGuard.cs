using System;
using UnityEngine;
using XLua;

namespace Minity.XLuaTools
{
    public class LuaEnvGuard : MonoBehaviour
    {
        private static LuaEnvGuard _instance;

        public static LuaEnvGuard Instance
        {
            get
            {
                if (!_instance)
                {
                    var go = new GameObject($"[{typeof(LuaEnvGuard).Name}]", typeof(LuaEnvGuard));
                    go.SetActive(true);
                    DontDestroyOnLoad(go);
                    _instance = go.GetComponent<LuaEnvGuard>();
                }

                return _instance;
            }
        }

        public static readonly LuaEnv Environment = new();

        private const float GC_INTERVAL = 1f;

        private float lastGCTime;
        
        private void Update()
        {
            if (Time.unscaledTime - lastGCTime >= GC_INTERVAL)
            {
                lastGCTime = Time.unscaledTime;
                Environment.Tick();
            }
        }
    }
}
