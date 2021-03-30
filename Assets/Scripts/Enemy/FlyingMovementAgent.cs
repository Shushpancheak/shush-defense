using Field;
using UnityEngine;
using Grid = Field.Grid;

namespace Enemy
{
    public class FlyingMovementAgent : MovementAgentBase
    {
        private float m_Speed;
        private Transform m_Transform;
        
        private const float TOLERANCE = 0.1f;

        private Node m_TargetNode;
        private Node m_StandingNode;

        private Grid m_Grid;

        public FlyingMovementAgent(EnemyData enemyData, float speed, Transform transform, Grid grid) : base(enemyData)
        {
            m_Speed = speed;
            m_Transform = transform;

            m_StandingNode = null;
            SetTargetNode(grid.GetTargetNode());
            m_Grid = grid;
        }

        public override void TickMovement()
        {
            if (m_TargetNode == null)
            {
                return;
            }

            Node now_standing_node = m_Grid.GetNodeAtPoint(m_Transform.position);

            if (m_StandingNode != now_standing_node)
            {
                // Means we moved from that node, so removing us from the list and
                // adding to the new one.
                m_StandingNode?.EnemyDatas.Remove(m_EnemyData);
                m_StandingNode = now_standing_node;
                m_StandingNode.EnemyDatas.Add(m_EnemyData);
            }

            Vector3 target = m_TargetNode.Position;
            var position = m_Transform.position;
            target.y = position.y; // Keep y for flying movement.
            
            float distance = (target - position).magnitude;
            if (distance < TOLERANCE)
            {
                m_TargetNode = m_TargetNode.NextNode;
                return;
            }
        
            Vector3 dir = (target - m_Transform.position).normalized;
            Vector3 delta = dir * (m_Speed * Time.deltaTime);
            m_Transform.Translate(delta);
        }

        private void SetTargetNode(Node node)
        {
            m_TargetNode = node;
        }
    }
}