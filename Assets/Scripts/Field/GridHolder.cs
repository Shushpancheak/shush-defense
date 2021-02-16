using UnityEngine;

namespace Field
{
    public class GridHolder : MonoBehaviour
    {
        [SerializeField]
        private int m_GridWidth;
        [SerializeField]
        private int m_GridHeight;

        [SerializeField]
        private MovementAgent m_MovementAgent;

        [SerializeField]
        private float m_NodeSize;

        private Grid m_Grid;

        private Camera m_Camera;

        private Vector3 m_Offset;

        private void Start()
        {
            m_Grid = new Grid(m_GridWidth, m_GridHeight);
            m_Camera = Camera.main;

            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;
            
            // Default plane size is 10 x 10.
            transform.localScale = new Vector3(
                width * 0.1f, 
                1f,
                height * 0.1f);

            m_Offset = transform.position -
                       (new Vector3(width, 0f, height) * 0.5f);
        }

        private void Update()
        {
            if (m_Grid == null || m_Camera == null)
            {
                return;
            }

            Vector3 mousePosition = Input.mousePosition;

            Ray ray = m_Camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform != transform)
                {
                    return;
                }

                Vector3 hitPosition = hit.point;
                Vector3 difference = hitPosition - m_Offset;

                int x = (int) (difference.x / m_NodeSize);
                int y = (int) (difference.z / m_NodeSize);
                
                Debug.Log(x + " " + y);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(m_Offset, 0.1f);
        }
    }
}