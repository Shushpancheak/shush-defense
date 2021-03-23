using System;
using Assets;
using UnityEngine;

namespace Runtime
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField]
        private AssetRoot m_AssetRoot;

        private void Awake()
        {
            Game.SetAssetRoot(m_AssetRoot);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Game.StartLevel(m_AssetRoot.Levels[0]);
            }
        }
    }
}