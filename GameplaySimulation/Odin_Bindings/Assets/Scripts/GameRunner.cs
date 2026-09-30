using System.Collections.Generic;
using UnityEngine;

public class GameRunner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GameApi.Init();
    }

    void OnDestroy()
    {
        GameApi.Shutdown();
    }

    // Update is called once per frame
    void Update()
    {
        GameApi.Update(Time.deltaTime);

        var entities = GameApi.GetGameEntities();
        for (int i = 0; i < entities.len; i++)
        {
            ref var e = ref entities[i];
            var go = gameObjects[i];

            go.transform.position = e.position;
            go.transform.rotation = e.rotation;
            go.transform.localScale = e.scale;
        }
    }


    private static List<GameObject> gameObjects = new List<GameObject>();
    public static void RegisterGameObject(GameObject go)
    {
        gameObjects.Add(go);

        GameApi.RegisterEntity(new Entity
        {
            position = go.transform.position,
            rotation = go.transform.rotation,
            scale = go.transform.localScale,
        });
    }
}
