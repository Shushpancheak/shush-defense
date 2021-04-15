using UnityEngine;
using Grid = Field.Grid;

namespace Enemy
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField]
        private Animator m_Animator;
        
        private EnemyData m_Data;
        private IMovementAgent m_MovementAgent;
        private static readonly int Die = Animator.StringToHash("Die");

        public EnemyData Data => m_Data;
        public IMovementAgent MovementAgent => m_MovementAgent;

        public void AttachData(EnemyData data)
        {
            m_Data = data;
        }

        public void CreateMovementAgent(Grid grid)
        {
            if (m_Data.Asset.IsFlyingEnemy)
            {
                m_MovementAgent = new FlyingMovementAgent(m_Data, m_Data.Asset.Speed, transform, grid);
            }
            else
            {
                m_MovementAgent = new GridMovementAgent(m_Data, m_Data.Asset.Speed, transform, grid);
            }
        }

        public void AnimateDeath()
        {
            m_Animator.SetTrigger(Die);
        }
    }
}