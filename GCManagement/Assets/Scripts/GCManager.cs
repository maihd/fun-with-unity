using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Scripting;

public class GCManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // If you have no fps limit, we cannot calculate which remain time of a frame to do GC
        Application.targetFrameRate = 60;

    #if !UNITY_EDITOR
        GarbageCollector.GCMode = GarbageCollector.Mode.Manual;
    #endif
    }

    double start;

    // Update is called once per frame
    void LateUpdate()
    {
        var end = Time.realtimeSinceStartupAsDouble;

        var elapsed = end - start;
        var frameTime = (double)1.0 / (double)Application.targetFrameRate;
        var frameRemain = frameTime - (double)elapsed;

        var gcBudget = frameRemain > 0 ? (ulong)(frameRemain * 1000 * 1000 * 1000) : 0UL;
        if (gcBudget > 0)
        {
        #if !UNITY_EDITOR
            GarbageCollector.CollectIncremental(gcBudget);
        #endif
        }

        // var gcStep = 100UL;
        // while (gcBudget > 0 && GarbageCollector.CollectIncremental(gcStep))
        // {
        //     gcBudget -= gcStep;
        // }

        start = Time.realtimeSinceStartupAsDouble;

        // UnityEngine.Debug.Log(elapsed);
        // UnityEngine.Debug.Log(frameRemain);
    }
}
