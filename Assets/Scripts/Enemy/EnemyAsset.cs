using Enemy;
//using Runtime;
using UnityEngine;

namespace Assets
{
    [CreateAssetMenu(menuName = "Assets/Enemy Asset", fileName = "Enemy Asset")]
    public class EnemyAsset : ScriptableObject
    {
        public float StartHealth;
        public bool IsFlyingEnemy;
        public float Speed;
        public EnemyView ViewPrefab;
    }
}