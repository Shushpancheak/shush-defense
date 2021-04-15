namespace Enemy
{
    public abstract class MovementAgentBase : IMovementAgent
    {
        protected EnemyData m_EnemyData;

        protected MovementAgentBase(EnemyData enemyData)
        {
            m_EnemyData = enemyData;
        }

        public abstract void TickMovement();
        public abstract void Die();
    }
}