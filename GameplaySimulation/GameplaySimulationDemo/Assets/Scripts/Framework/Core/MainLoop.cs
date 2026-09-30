using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public static class MainLoop
{

    [RuntimeInitializeOnLoadMethod]
    public static void AppStart()
    {
        Debug.Log("AppStart()");

        var defaultLoop = PlayerLoop.GetDefaultPlayerLoop();
        var customUpdate = new PlayerLoopSystem()
        {
            updateDelegate = MainLoopUpdate,
            type = typeof(MainLoop)
        };

        var finalLoopSystem = InsertSystemAfter<PreLateUpdate>(defaultLoop, customUpdate);
        PlayerLoop.SetPlayerLoop(finalLoopSystem);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif

        EntitySystem.Init();
    }

#if UNITY_EDITOR
    private static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
    {
        // Triggered right when the stop button is pressed before exiting Play Mode
        if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode)
        {
            EntitySystem.Shutdown();

            // Reset the player loop completely back to Unity's default configuration
            PlayerLoopSystem defaultLoop = PlayerLoop.GetDefaultPlayerLoop();
            PlayerLoop.SetPlayerLoop(defaultLoop);

            // Unsubscribe to avoid memory leaks or duplicate hooks upon re-entering Play Mode
            UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

            Debug.Log("Custom Player Loop successfully removed on Play Mode exit.");
        }
    }
#endif


    private static PlayerLoopSystem InsertSystemAfter<T>(in PlayerLoopSystem loopSystem, PlayerLoopSystem newSystem) where T : struct
    {
        PlayerLoopSystem newPlayerLoop = new()
        {
            loopConditionFunction = loopSystem.loopConditionFunction,
            type = loopSystem.type,
            updateDelegate = loopSystem.updateDelegate,
            updateFunction = loopSystem.updateFunction
        };
        List<PlayerLoopSystem> newSubSystemList = new();

        if (loopSystem.subSystemList != null)
        {
            for (var i = 0; i < loopSystem.subSystemList.Length; i++)
            {
                newSubSystemList.Add(loopSystem.subSystemList[i]);
                if (loopSystem.subSystemList[i].type == typeof(T))
                {
                    newSubSystemList.Add(newSystem);
                }
            }
        }

        newPlayerLoop.subSystemList = newSubSystemList.ToArray();
        return newPlayerLoop;
    }


    private static void MainLoopUpdate()
    {
        // Debug.Log("MainLoop running...");

        ref var eventBuffer = ref EventRegistry.Current;
        EventRegistry.SwapBuffers();

        // Add more handle here
        EntitySystem.HandleEvents(eventBuffer);
        PhysicsSystem.HandleEvents(eventBuffer);

        eventBuffer.Clear();

        // Handle update
        EntitySystem.Update();

        // Handle command to notify other systems

        // Sync to Unity
        EntitySystem.SyncToGameObjects();
    }
}
