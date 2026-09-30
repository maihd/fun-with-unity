using System;
using System.Reflection;
using System.Runtime.InteropServices;

using UnityEngine;

public struct Slice<T>
    where T : unmanaged
{
    public unsafe T* data;
    public long len;

    public unsafe Slice(T* data, long len)
    {
        this.data = data;
        this.len = len;
    }

    public unsafe Slice<T> this[int min, int max]
    {
        get => new Slice<T>(data + min, max - min);
    }

    public ref T this[int index]
    {
        get
        {
            unsafe
            {
                return ref data[index];
            }
        }
    }
}

public static class GameApi
{
#if UNITY_EDITOR_OSX
	const string LIB_PATH = "/Odin_Game/Out/Odin_Game.dylib";
#elif UNITY_EDITOR_LINUX
	const string LIB_PATH = "/Odin_Game/Out/Odin_Game.so";
#elif UNITY_EDITOR_WIN
    const string LIB_PATH = "/Odin_Game/Out/Odin_Game.dll";
#endif

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GameInitFn();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GameDeinitFn();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GameUpdateFn(float dt);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void RegisterEntityFn(Entity entity);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate Slice<Entity> GetGameEntitiesFn();

    private static IntPtr LibHandle;

    private static GameInitFn GameInitFnPtr;
    private static GameDeinitFn GameDeinitFnPtr;
    private static GameUpdateFn GameUpdateFnPtr;

    private static RegisterEntityFn RegisterEntityFnPtr;
    private static GetGameEntitiesFn GetGameEntitiesFnPtr;

    public static void Init()
    {
        LibHandle = DylibHelper.OpenLibrary(Application.dataPath + LIB_PATH);

        GameInitFnPtr = DylibHelper.GetFnPtrFromNative<GameInitFn>(LibHandle, "game_init");
        GameDeinitFnPtr = DylibHelper.GetFnPtrFromNative<GameDeinitFn>(LibHandle, "game_deinit");
        GameUpdateFnPtr = DylibHelper.GetFnPtrFromNative<GameUpdateFn>(LibHandle, "game_update");

        RegisterEntityFnPtr = DylibHelper.GetFnPtrFromNative<RegisterEntityFn>(LibHandle, "register_entity");
        GetGameEntitiesFnPtr = DylibHelper.GetFnPtrFromNative<GetGameEntitiesFn>(LibHandle, "get_game_entities");

        Debug.Assert(GameInitFnPtr != null);
        GameInitFnPtr();
    }

    public static void Shutdown()
    {
        if (LibHandle != IntPtr.Zero)
        {
            Debug.Assert(GameDeinitFnPtr != null);

            DylibHelper.CloseLibrary(LibHandle);
        }
    }

    public static void Update(float dt)
    {
        Debug.Assert(GameUpdateFnPtr != null);

        GameUpdateFnPtr(dt);
    }

    public static void RegisterEntity(Entity entity)
    {
        Debug.Assert(RegisterEntityFnPtr != null);

        RegisterEntityFnPtr(entity);
    }

    public static Slice<Entity> GetGameEntities()
    {
        Debug.Assert(GetGameEntitiesFnPtr != null);

        return GetGameEntitiesFnPtr();
    }
}
