using UnityEngine;

namespace Turret.Weapon.Projectile
{
    [CreateAssetMenu(menuName = "Assets/Turret Projectile Weapon Asset", fileName = "Turret Projectile Weapon Asset")]
    public class TurretProjectileWeaponAsset : TurretWeaponAssetBase
    {
        public float RateOfFire;
        public float MaxDistance;

        public ProjectileAssetBase ProjectileAsset;
        
        public override ITurretWeapon GetWeapon(TurretView view)
        {
            return new TurretProjectileWeapon(this, view);
        }
    }
}