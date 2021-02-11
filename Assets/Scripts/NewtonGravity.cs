using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NewtonGravity : MonoBehaviour
{
    private const float MIN_DISTANCE = 0.1f;

    [SerializeField] private Vector3 m_pos_start   = new Vector3(0, 5, 0);
    [SerializeField] private Vector3 m_velocity    = new Vector3(5, 0, 0);
    
    [SerializeField] private Vector3 m_pos_target  = new Vector3(0, 0, 0);
    [SerializeField] private float   m_mass_target = 700f;

    [SerializeField] private float   G = 6.67f * 1e-2f;
    
    private float G_MASS_TARGET = 0;

    void Start()
    {
        G_MASS_TARGET = G * m_mass_target;
        transform.position = m_pos_start;
    }

    void FixedUpdate()
    {
        // Compute acceleration:
        // a = G * M * (R - r) / ((R - r)^2 * |R - r|)
        
        // R - r
        var delta = m_pos_target - transform.position;
        var delta_magnitude = delta.magnitude;

        if (delta_magnitude < MIN_DISTANCE)
        {
            return;
        }
        
        var acceleration = delta * (G_MASS_TARGET / Mathf.Pow(delta_magnitude, 3));
    
        transform.Translate(m_velocity * Time.fixedDeltaTime + acceleration * Mathf.Pow(Time.fixedDeltaTime, 2) / 2);
        
        m_velocity += acceleration * Time.fixedDeltaTime;
    }
}
