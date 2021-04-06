using Enemy;
using UnityEngine;

namespace Turret.Weapon.Projectile.Bullet
{
    [CreateAssetMenu(menuName = "Assets/Rocket Projectile Asset", fileName = "Rocket Projectile Asset")]
    public class RocketProjectileAsset : ProjectileAssetBase
    {
        [SerializeField]
        private RocketProjectile m_RocketPrefab;
        
        public override IProjectile CreateProjectile(Vector3 origin, Vector3 originForward, EnemyData enemyData)
        {
            var obj = Instantiate(m_RocketPrefab, origin, Quaternion.LookRotation(originForward, Vector3.up));
            obj.m_TargetEnemy = enemyData;
            return obj;
        }
    }
}