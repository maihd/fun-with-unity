using UnityEngine;

public class RegisterGameObject : MonoBehaviour
{
    void Start()
    {
        EntitySystem.RegisterGameObject(gameObject);
    }
}