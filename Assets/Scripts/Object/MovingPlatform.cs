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
    private float angle;
    
    private void Start()
    {
        lastPosition = transform.position;
        if (data.type == PlatformType.Circular)
        {
            data.centerPoint = transform.position + (Vector3.left * data.radius);
            Debug.Log(data.centerPoint);
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
        float time = Time.time % timeReset;
        float move = Mathf.PingPong(time*data.moveSpeed, data.moveRange) - data.moveRange/ 2;
        switch (data.type)
        {
            case PlatformType.MovingX:
                transform.position = new Vector3(transform.position.x + move, transform.position.y, transform.position.z);
                break;
            case PlatformType.MovingZ:
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + move);
                break;
            case PlatformType.Circular:
                angle += data.angularSpeed * (Time.time % timeReset);
                float x = MathF.Cos(angle) * data.radius;
                float z = MathF.Sin(angle) * data.radius;
                
                transform.position = new Vector3(x,transform.position.y,z);
                break;
        }
    }
}
