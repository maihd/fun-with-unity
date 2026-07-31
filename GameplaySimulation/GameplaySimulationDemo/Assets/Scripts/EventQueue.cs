using System.Collections.Generic;
using System.Runtime.InteropServices;
using Mono.Cecil.Cil;
using Unity.Burst;
using Unity.Collections;
using UnityEditor.PackageManager;
using UnityEngine;


public enum EventKind : uint
{
    None,
    Physics,
}


public enum PhysicsEventKind : uint
{
    TriggerEnter3D,
    TriggerExit3D,
    TriggerStay3D,
    CollisionEnter3D,
    CollisionExit3D,
    CollisionStay3D,
}


public struct PhysicsEventData
{
    public PhysicsEventKind kind;

    public GameObject       objectA;
    public GameObject       objectB;
}


[StructLayout(LayoutKind.Sequential)]
public struct Event
{
    public EventData data;
    public EventKind kind;
}


[StructLayout(LayoutKind.Explicit)]
public struct EventData
{
    [FieldOffset(0)]

    public PhysicsEventData physics;
}


public struct EventBuffer
{
    public Event[] events;

    public int count;
    public int capacity => events.Length;

    public EventBuffer(int capacity)
    {
        events = new Event[capacity];
        count = 0;
    }
    
    public void Add(Event eventData)
    {
        events[count] = eventData;
        count += 1;
    }

    public void Clear()
    {
        count = 0;
    }
}


public static class EventRegistry
{
    static EventBuffer[] queues = { new EventBuffer(4096), new EventBuffer(4096) };
    static int currentIndex = 0;

    public static ref EventBuffer Current => ref queues[currentIndex];

    // public static void Init(int eventCapacity)
    // {
    //     queue = new EventQueue(eventCapacity);
    // }

    // public static void Shutdown()
    // {
    //     // queue.events.Dispose();
    //     queue.events = default;
    // }

    public static void SwapBuffers()
    {
        currentIndex = (currentIndex + 1) % queues.Length;
    }

    public static void QueueEvent(Event eventData)
    {
        Current.Add(eventData);
    }
}