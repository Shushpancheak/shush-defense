using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class MovementAgent : MonoBehaviour
{
    private const float STOP_DISTANCE = 0.1f;
    
    [SerializeField]
    private float m_speed;
    [SerializeField]
    private Vector3 m_target;
    
    void Start()
    {
    }
    
    void Update()
    {
        float distance = (m_target - transform.position).magnitude;
        if (distance < STOP_DISTANCE)
        {
            return;
        }
        
        var dir = (m_target - transform.position).normalized;
        var move_vec = dir * (m_speed * Time.deltaTime);
        
        transform.Translate(move_vec);
    }
}
