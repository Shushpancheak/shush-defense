using System.Collections.Generic;
using UnityEngine;
using Enemy;
using Field;
using Runtime;
using Turret.Weapon.Projectile;
using UnityEngine;

namespace Turret.Weapon.Field
{
    public class TurretFieldWeapon : ITurretWeapon
    {
        private TurretFieldWeaponAsset m_Asset;
        private TurretView m_View;

        private float m_MaxDistance;
        private HashSet<EnemyData> m_Enemies;
        private List<Node> m_NodesInRange;

        private float m_Damage;

        public TurretFieldWeapon(TurretFieldWeaponAsset asset, TurretView view)
        {
            m_Asset = asset;
            m_MaxDistance = m_Asset.MaxDistance;
            m_View = view;
            m_Damage = m_Asset.Damage;
            var position = m_View.transform.position;
            
            m_NodesInRange = Game.Player.Grid.GetNodesInCircle(position, m_MaxDistance);
            var field = Object.Instantiate(m_Asset.Field, position, Quaternion.identity);
            field.localScale.Set(m_MaxDistance, m_MaxDistance, m_MaxDistance);
        }

        public void TickShoot()
        {
            m_Enemies = EnemySearch.GetEnemies(Game.Player.Grid.GetNodesInCircle(m_View.transform.position, m_MaxDistance));
            if (m_Enemies == null)
            {
                return;
            }

            foreach (var enemy in m_Enemies) 
            {
                enemy.GetDamage(m_Damage);
            }
        }
    }

}