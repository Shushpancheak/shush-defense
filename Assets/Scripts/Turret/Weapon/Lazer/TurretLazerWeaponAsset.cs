using Turret.Weapon.Projectile;
using UnityEngine;

namespace Turret.Weapon.Lazer
{
    [CreateAssetMenu(menuName = "Assets/Turret Lazer Weapon Asset", fileName = "Turret Lazer Weapon Asset")]
    public class TurretLazerWeaponAsset : TurretWeaponAssetBase
    {
        public LineRenderer LineRendererPrefab;
        public float MaxDistance;
        public float Damage;
        
        public override ITurretWeapon GetWeapon(TurretView view)
        {
            return new TurretLazerWeapon(this, view);
        }
    }
}