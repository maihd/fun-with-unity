using System.Collections.Generic;
using System.Runtime.InteropServices;
using Mono.Cecil.Cil;
using Unity.Burst;
using Unity.Collections;

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


public struct EventQueue
{
    public Queue<Event> events;

    public EventQueue(int capacity)
    {
        events = new Queue<Event>(capacity);
    }
}


public static class EventRegistry
{
    static EventQueue queue = new EventQueue(4096);

    public static void Init(int eventCapacity)
    {
        queue = new EventQueue(eventCapacity);
    }

    public static void Shutdown()
    {
        // queue.events.Dispose();
        queue.events = default;
    }

    public static void QueueEvent(Event eventData)
    {
        queue.events.Enqueue(eventData);
    }

    public static Event? PollEvent()
    {
        Event data;
        return queue.events.TryDequeue(out data) ? data : null;
    }

    public static bool TryPollEvent(out Event data)
    {
        return queue.events.TryDequeue(out data);
    }
}