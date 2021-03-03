using Field;
using UnityEngine;

namespace Unit
{
    public class GridMovementAgent : MonoBehaviour
    {
        [SerializeField]
        private float m_Speed;

        private const float TOLERANCE = 0.1f;

        private Node m_TargetNode;

        private void Update()
        {
            if (m_TargetNode == null)
            {
                return;
            }

            Vector3 target = m_TargetNode.Position;
            
            float distance = (target - transform.position).magnitude;
            if (distance < TOLERANCE)
            {
                m_TargetNode = m_TargetNode.NextNode;
                return;
            }
        
            Vector3 dir = (target - transform.position).normalized;
            Vector3 delta = dir * (m_Speed * Time.deltaTime);
            transform.Translate(delta);
        }

        public void SetStartNode(Node node)
        {
            m_TargetNode = node;
        }
    }
}