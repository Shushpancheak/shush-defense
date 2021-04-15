using UnityEngine;

namespace Turret
{
    public class TurretView : MonoBehaviour
    {
        [SerializeField]
        private Transform m_ProjectileOrigin;
        
        [SerializeField]
        private Transform m_Tower;

        [SerializeField]
        private Animator m_Animator;
        
        private TurretData m_Data;
        private static readonly int Shoot = Animator.StringToHash("Shoot");

        public TurretData Data => m_Data;

        public Transform ProjectileOrigin => m_ProjectileOrigin;
        
        public void AttachData(TurretData turretData)
        {
            m_Data = turretData;
            transform.position = m_Data.Node.Position;
        }
        
        public void TowerLookAt(Vector3 point)
        {
            point.y = m_Tower.position.y;
            m_Tower.LookAt(point);
        }

        public void AnimateShoot()
        {
            m_Animator.SetTrigger(Shoot);
        }
    }
}