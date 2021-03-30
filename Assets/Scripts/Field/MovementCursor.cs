using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Enemy;
using UnityEngine;

namespace Field
{
    public class MovementCursor : MonoBehaviour
    {
        private const float EPS = 0.0001f;
        
        [SerializeField] private int m_GridWidth;
        [SerializeField] private int m_GridHeight;

        // TODO change to appropriate MovementAgent
        [SerializeField] private GridMovementAgent m_MovementAgent;

        [SerializeField] private float m_NodeSize;

        [SerializeField] private GameObject m_Cursor;
        
        private Camera m_Camera;

        // local_x = 0, local_z = 0 Corner of the plane.
        private Vector3 m_Offset;

        // Returns (width, height)
        private ValueTuple<float, float> GetDimensions()
        {
            return new ValueTuple<float, float>(m_GridWidth * m_NodeSize, m_GridHeight * m_NodeSize);
        }
        
        private void RefreshPlaneScale()
        {
            var (width, height) = GetDimensions();

            // Default plane size is 10 x 10.
            transform.localScale = new Vector3(
                width * 0.1f,
                1f,
                height * 0.1f);

            m_Offset = transform.position -
                       (new Vector3(width, 0f, height) * 0.5f);
        }

        private void Start()
        {
            m_Camera = Camera.main;
            m_Cursor.layer = LayerMask.NameToLayer("Ignore Raycast");

            RefreshPlaneScale();
        }

        private void Update()
        {
            if (m_Camera is null)
            {
                return;
            }

            Vector3 mousePosition = Input.mousePosition;

            Ray ray = m_Camera.ScreenPointToRay(mousePosition);

            if (!Physics.Raycast(ray, out RaycastHit hit) || hit.transform != transform)
            {
                m_Cursor.SetActive(false);
                return;
            }

            m_Cursor.SetActive(true);

            Vector3 hitPosition = hit.point;
            Vector3 difference = hitPosition - m_Offset;

            int x = (int) (difference.x / m_NodeSize);
            int y = (int) (difference.z / m_NodeSize);

            var node_pos = m_Offset + new Vector3((x + .5f) * m_NodeSize, 0, (y + .5f) * m_NodeSize);
            m_Cursor.transform.position = node_pos;

            if (!Input.GetMouseButtonDown(0)) return;

            // TODO MovementAgent.setTarget
            // m_MovementAgent?.(node_pos);
        }

        private void OnValidate()
        {
            RefreshPlaneScale();
        }

        // axis: either 0 or 2 (x or z).
        private void DrawGizmosLinesAlongAxis(int axis)
        {
            if (m_NodeSize < EPS || m_GridHeight <= 0 || m_GridWidth <= 0 ||
                (axis != 0 && axis != 2))
            {
                return;
            }

            var opposite_axis = axis == 0 ? 2 : 0;
            
            var (width, height) = GetDimensions();

            var end_x = m_Offset.x + width;
            var end_z = m_Offset.z + height;
            var end = new Vector3(end_x, 0, end_z);
                
            // + EPS for an additional line on the border.
            var vec_end = m_Offset;
            vec_end[opposite_axis] = end[opposite_axis];
            for (var vec = m_Offset;
                vec[axis] < end[axis] + EPS;
                vec[axis] += m_NodeSize, vec_end[axis] += m_NodeSize)
            {
                Gizmos.DrawLine(vec, vec_end);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(m_Offset, 0.1f);
            
            Gizmos.color = Color.green;
            // Along x and z.
            DrawGizmosLinesAlongAxis(0);
            DrawGizmosLinesAlongAxis(2);
        }
    }
}