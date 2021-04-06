using System.Collections.Generic;
using Enemy;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.Projectile.Bullet
{
    public class RocketProjectile : MonoBehaviour, IProjectile
    {
        // Starting speed of the rocket
        private float m_Speed = 0.1f;
        // Determines the rate at which the speed will increase.
        private float m_SpeedModifier = 0.05f;
        private float m_MaxSpeed = 5f;
        
        // Base damage, after explosion it dissipates linearly
        private float m_DamageBase = 10f;
        private float m_DamageRadius = 5f;
        
        private bool m_DidHit = false;
        public EnemyData m_TargetEnemy = null;
        private HashSet<EnemyData> m_HitEnemies = null;
        
        public void TickApproaching()
        {
            if (m_TargetEnemy == null)
            {
                Debug.Log("No target");
                Destroy(gameObject);
                return;
            }

            if (m_Speed >= m_MaxSpeed)
            {
                m_Speed = m_MaxSpeed;
            }
            m_Speed += m_Speed * m_SpeedModifier;
            var dir = (m_TargetEnemy.View.transform.position - transform.position).normalized;
            transform.Translate(dir * (m_Speed * Time.deltaTime), Space.World);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                EnemyView enemyView = other.GetComponent<EnemyView>();
                if (enemyView != null)
                {
                    m_DidHit = true;
                    m_HitEnemies = EnemySearch.GetEnemies(Game.Player.Grid.GetNodesInCircle(enemyView.transform.position, m_DamageRadius));
                }
            }
        }

        public bool DidHit()
        {
            return m_DidHit;
        }

        public void DestroyProjectile()
        {
            if (m_TargetEnemy == null || m_HitEnemies == null)
            {
                Debug.Log("No target");
                Destroy(gameObject);
                return;
            }
            var target_pos = m_TargetEnemy.View.transform.position;
            
            foreach (var enemy in m_HitEnemies)
            {
                enemy?.GetDamage(m_DamageBase * (1f - (target_pos - enemy.View.transform.position).magnitude / m_DamageRadius));
            }
            
            Destroy(gameObject);
        }
    }
}