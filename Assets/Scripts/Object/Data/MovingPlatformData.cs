using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformType
{
    MovingX, MovingZ, Circular, MovingY
}

[CreateAssetMenu(fileName ="MovingPlatform", menuName = "New Moving Platform")]
public class MovingPlatformData : ScriptableObject
{
    [Header("MovingType")]
        public PlatformType type;
    [Header("Movement Speed")]
    public float moveSpeed;
    [Header("Rotation Speed")]
    public float rotationSpeed;

    [Header("Move Range")]
    public float moveRange;
    
    [Header("Move Circular")]
    public float radius;
    public float angularSpeed;
}
