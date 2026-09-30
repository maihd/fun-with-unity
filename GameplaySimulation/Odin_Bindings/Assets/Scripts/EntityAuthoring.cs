using UnityEngine;

public class EntityAuthoring : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameRunner.RegisterGameObject(gameObject);
    }
}
