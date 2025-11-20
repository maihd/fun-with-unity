using Minity.XLuaTools;
using UnityEngine;
using XLua;

[Hotfix]
public class LuaScriptCaller : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LuaEnvGuard.Environment.DoString("CS.UnityEngine.Debug.Log('Hello world from XLua')");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Hotfix]
    // [LuaCallCSharp]
    public void Greeting()
    {
        LuaEnvGuard.Environment.DoString("CS.UnityEngine.Debug.Log('Hello world from XLua that call LuaScriptCaller.Greeting()')");
    }
}
