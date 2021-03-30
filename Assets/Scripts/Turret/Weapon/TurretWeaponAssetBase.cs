using UnityEngine;

namespace Turret.Weapon
{
    public abstract class TurretWeaponAssetBase : ScriptableObject
    {
        public abstract ITurretWeapon GetWeapon(TurretView view);
    }
}