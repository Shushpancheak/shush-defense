using System.Collections.Generic;
using Enemy;
using Field;
using Runtime;
using Turret.Weapon.Projectile;
using UnityEngine;

namespace Turret.Weapon.Lazer
{
    public class TurretLazerWeapon : ITurretWeapon
    {
        private TurretLazerWeaponAsset m_Asset;
        private LineRenderer m_LineRenderer;
        private TurretView m_View;

        private float m_MaxDistance;
        private EnemyData m_ClosestEnemy;
        private List<Node> m_NodesInRange;

        private float m_Damage;

        public TurretLazerWeapon(TurretLazerWeaponAsset asset, TurretView view)
        {
            m_Asset = asset;
            m_Damage = m_Asset.Damage;
            m_MaxDistance = m_Asset.MaxDistance;
            m_View = view;
            m_LineRenderer = Object.Instantiate(asset.LineRendererPrefab, m_View.transform);

            m_NodesInRange = Game.Player.Grid.GetNodesInCircle(m_View.transform.position, m_MaxDistance);
        }

        public void TickShoot()
        {
            m_ClosestEnemy = EnemySearch.GetClosestEnemy(m_View.transform.position, m_MaxDistance, m_NodesInRange);
            if (m_ClosestEnemy == null)
            {
                m_LineRenderer.gameObject.SetActive(false);
            }
            else
            {
                var position = m_ClosestEnemy.View.transform.position;
                m_View.TowerLookAt(position);

                m_LineRenderer.gameObject.SetActive(true);
                m_LineRenderer.SetPosition(0, m_View.ProjectileOrigin.position);
                m_LineRenderer.SetPosition(1, position);
                
                m_ClosestEnemy.GetDamage(m_Damage * Time.deltaTime);
            }
        }
    }

}