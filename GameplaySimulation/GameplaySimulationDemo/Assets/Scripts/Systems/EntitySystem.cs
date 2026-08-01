using Unity.Collections;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public static unsafe class EntitySystem
{
    private struct Entry
    {
        public Entry* next;
    }


    private static HandleMap<Entity> entitiesMap;

    private static HandleMap<Entity> entities;


    private static Handle AllocEntity(Entity entity)
    {
        Handle handle = entities.Add(entity);
        return handle;
    }


    private static void FreeEntity(Handle handle)
    {
        entities.Remove(handle);
    }


    public static Handle Spawn(GameObject prefab, float3 position = default, quaternion rotation = default)
    {
        GameObject go = GameObject.Instantiate(prefab, position, rotation);

        return AllocEntity(new Entity
        {
            position = position,
            rotation = rotation,
            scale = go.transform.localScale,
        });
    }


    public static void Update()
    {
        // Systems like: Moving, Rotating, Targeting, Aiming, Following,...
        for (int i = 0, n = entities.Count; i < n; i++)
        {
            ref var entity = ref entities.elements.ElementAt(i);
        }
    }


    public static void HandleEvents(EventBuffer events)
    {
        for (int i = 0; i < events.count; i++)
        {
            var eventData = events[i];
            switch (eventData.kind)
            {


                default:
                    break;
            }
        }
    }
}