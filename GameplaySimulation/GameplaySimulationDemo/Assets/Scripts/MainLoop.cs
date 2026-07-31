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
        var defaultLoop = PlayerLoop.GetDefaultPlayerLoop();
        var customUpdate = new PlayerLoopSystem()
        {
            updateDelegate = MainLoopUpdate,
            type = typeof(MainLoop)  
        };

        var finalLoopSystem = InsertSystemAfter<PreLateUpdate>(defaultLoop, customUpdate);
        PlayerLoop.SetPlayerLoop(finalLoopSystem);
    }

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

        for (int i = 0, n = eventBuffer.count; i < n; i++)
        {
            var eventData = eventBuffer.events[i];

            switch (eventData.kind)
            {
                case EventKind.Physics:
                    Debug.Log("Receiving physics event. Kind: " + eventData.data.physics.kind);
                    break;

                default:
                    break;
            }
        }

        eventBuffer.Clear();
    }
}
