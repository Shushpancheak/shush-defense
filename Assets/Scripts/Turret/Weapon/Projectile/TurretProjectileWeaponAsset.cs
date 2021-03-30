namespace Turret.Weapon.Projectile
{
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