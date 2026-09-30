using System.Collections.Generic;
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



    private static HandleMap<Entity> entities;
    private static List<GameObject> gameObjects;


    private static Handle AllocEntity(Entity entity)
    {
        Handle handle = entities.Add(entity);
        return handle;
    }


    private static void FreeEntity(Handle handle)
    {
        entities.Remove(handle);
    }

    public static void Init()
    {
        Debug.Log("EntitySystem.Init()");

        entities = new HandleMap<Entity>(4096);
        gameObjects = new List<GameObject>();
    }

    public static void Shutdown()
    {
        entities.Dispose();
        gameObjects = null;
    }


    public static Handle Spawn(GameObject prefab, float3 position = default, quaternion rotation = default)
    {
        GameObject go = GameObject.Instantiate(prefab, position, rotation);

        return RegisterGameObject(go);
    }


    public static Handle RegisterGameObject(GameObject go)
    {
        var handle = AllocEntity(new Entity
        {
            position = go.transform.position,
            rotation = go.transform.rotation,
            scale = go.transform.localScale,
        });

        var index = entities.GetIndex(handle);
        if (index >= gameObjects.Count)
        {
            gameObjects.Insert(index, go);
        }
        else
        {
            gameObjects[index] = go;
        }

        return handle;
    }


    public static void Update()
    {
        float dt = Time.deltaTime;

        // Systems like: Moving, Rotating, Targeting, Aiming, Following,...
        for (int i = 0, n = entities.Count; i < n; i++)
        {
            ref var entity = ref entities.elements.ElementAt(i);
            entity.rotation = Quaternion.AngleAxis((float)Time.time * 90, new Vector3(0, 1, 0));
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

    public static void SyncToGameObjects()
    {
        for (int i = 0, n = entities.Count; i < n; i++)
        {
            ref var entity = ref entities.elements.ElementAt(i);
            var gameObject = gameObjects[i];
            gameObject.transform.rotation = entity.rotation;
        }
    }
}