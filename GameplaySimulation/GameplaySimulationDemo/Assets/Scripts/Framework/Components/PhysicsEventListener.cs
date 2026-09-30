using Unity.VisualScripting;
using UnityEngine;

public class PhysicsEventListener : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        EventRegistry.QueueEvent(new Event
        {
            kind = EventKind.Physics,
            data =
            {
                physics =
                {
                    kind = PhysicsEventKind.TriggerEnter3D,
                    objectA = gameObject,
                    objectB = other.gameObject
                }
            }
        });
    }


    void OnTriggerExit(Collider other)
    {
        EventRegistry.QueueEvent(new Event
        {
            kind = EventKind.Physics,
            data =
            {
                physics =
                {
                    kind = PhysicsEventKind.TriggerExit3D,
                    objectA = gameObject,
                    objectB = other.gameObject
                }
            }
        });
    }


    void OnTriggerStay(Collider other)
    {
        EventRegistry.QueueEvent(new Event
        {
            kind = EventKind.Physics,
            data =
            {
                physics =
                {
                    kind = PhysicsEventKind.TriggerStay3D,
                    objectA = gameObject,
                    objectB = other.gameObject
                }
            }
        });
    }


    void OnCollisionEnter(Collision collision)
    {
        EventRegistry.QueueEvent(new Event
        {
            kind = EventKind.Physics, 
            data =
            {
                physics =
                {
                    kind = PhysicsEventKind.CollisionEnter3D,
                    objectA = gameObject,
                    objectB = collision.gameObject
                }
            }
        });
    }


    void OnCollisionExit(Collision collision)
    {
        EventRegistry.QueueEvent(new Event
        {
            kind = EventKind.Physics, 
            data =
            {
                physics =
                {
                    kind = PhysicsEventKind.CollisionExit3D,
                    objectA = gameObject,
                    objectB = collision.gameObject
                }
            }
        });
    }


    void OnCollisionStay(Collision collision)
    {
        EventRegistry.QueueEvent(new Event
        {
            kind = EventKind.Physics,
            data =
            {
                physics =
                {
                    kind = PhysicsEventKind.CollisionStay3D,
                    objectA = gameObject,
                    objectB = collision.gameObject
                }
            }
        });
    }
}
