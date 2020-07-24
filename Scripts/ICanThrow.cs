using UnityEngine;

public interface ICanThrow
{
    Transform Transform { get; }
    Rigidbody2D RigidBody { get; }
}