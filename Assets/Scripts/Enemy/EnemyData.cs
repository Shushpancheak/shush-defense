using System.Collections;
using Assets;
using UnityEditor;
using UnityEngine;

namespace Enemy
{
    public class EnemyData
    {
        private EnemyView m_View;
        private float m_Health;

        public EnemyView View => m_View;

        public readonly EnemyAsset Asset;

        public EnemyData(EnemyAsset asset)
        {
            Asset = asset;
            m_Health = asset.StartHealth;
        }

        public void AttachView(EnemyView view)
        {
            m_View = view;
            m_View.AttachData(this);
        }

        public void GetDamage(float damage)
        {
            m_Health -= damage;
            if (m_Health < 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("died :(");
        }
    }
}