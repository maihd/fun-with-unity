using System;
using System.Reflection;
using System.Runtime.InteropServices;

using UnityEngine;

public class Hellope : MonoBehaviour
{
#if UNITY_EDITOR_OSX
	const string LIB_PATH = "/Odin_Game/Out/Odin_Game.dylib";
#elif UNITY_EDITOR_LINUX
	const string LIB_PATH = "/Odin_Game/Out/Odin_Game.so";
#elif UNITY_EDITOR_WIN
    const string LIB_PATH = "/Odin_Game/Out/Odin_Game.dll";
#endif


    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate IntPtr HellopeFnPtr();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var libHandle = DylibHelper.OpenLibrary(Application.dataPath + LIB_PATH);
        if (libHandle == IntPtr.Zero)
        {
            Debug.LogError("Failed to load LibHandle");
        }

        var hellopeFn = DylibHelper.GetFnPtrFromNative<HellopeFnPtr>(libHandle, "hellope");
        if (hellopeFn != null)
        {
            hellopeFn();
        }
        else
        {
            Debug.LogError("Failed to get hellope function pointer!");
        }

        DylibHelper.CloseLibrary(libHandle);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
