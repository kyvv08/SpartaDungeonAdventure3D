using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private MovingPlatformData data;
    
    public Vector3 velocity
    {
        get;private set;
    }
    
    private Vector3 lastPosition;
    private float timeReset = 10000f;
    private float angle = 0f;

    private Vector3 centerPoint;
    
    private void Start()
    {
        lastPosition = transform.position;
        if (data.type == PlatformType.Circular)
        { 
            centerPoint = transform.position + (Vector3.left * data.radius);
        }
    }

    void FixedUpdate()
    {
        Move();
        velocity = (transform.position - lastPosition)/Time.deltaTime;
        lastPosition = transform.position;
    }

    void Move()
    {
        float move = Mathf.PingPong(Time.time*data.moveSpeed, data.moveRange) - data.moveRange/ 2;
        switch (data.type)
        {
            case PlatformType.MovingX:
                transform.position = new Vector3(transform.position.x + move, transform.position.y, transform.position.z);
                break;
            case PlatformType.MovingZ:
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + move);
                break;
            case PlatformType.MovingY:
                transform.position = new Vector3(transform.position.x, transform.position.y + move, transform.position.z);
                break;
            case PlatformType.Circular:
                angle = data.angularSpeed * Time.time;
                
                float x = MathF.Cos(angle) * data.radius;
                float z = MathF.Sin(angle) * data.radius;
                
                transform.position = centerPoint + new Vector3(x,0,z);
                break;
        }
    }

    public bool IsMovingY()
    {
        if (data.type == PlatformType.MovingY)
        {
            return true;
        }

        return false;
        
    }
}
