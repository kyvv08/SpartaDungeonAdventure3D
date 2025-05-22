using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaserTrap : MonoBehaviour
{
    [SerializeField] private float raserRange;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LineRenderer lineRenderer;
    
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;

    [SerializeField] private float checkRate = .5f;
    [SerializeField] private float damage = 10f;
    private float lastCheck;
    void Awake()
    {
        targetLayer = LayerMask.GetMask("Player");
        lineRenderer = GetComponent<LineRenderer>();
        
        startPos = transform.Find("Start").position;
        endPos = transform.Find("End").position;
        lastCheck = 0f;
    }

    void Update()
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }
    
    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.forward*raserRange, Color.red);
        if (Time.time - lastCheck < checkRate)
        {
            return;
        }

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, raserRange, targetLayer))
        {
            if (hit.collider.CompareTag("Player"))
            {
                CharacterManager.Instance.Player.playerCondition.Damaged(damage);
                lastCheck = Time.time;
            }
        }
    }
}
