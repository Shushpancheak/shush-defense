using Field;
using Runtime;
using Turret;
using UnityEngine;
using Grid = Field.Grid;

namespace TurretSpawn
{
    public class TurretSpawnController : IController
    {
        private Grid m_Grid;
        private TurretMarket m_Market;

        public TurretSpawnController(Grid grid, TurretMarket market)
        {
            m_Grid = grid;
            m_Market = market;
        }
        
        public void OnStart()
        {
        }

        public void OnStop()
        {
        }

        public void Tick()
        {
            if (m_Grid.HasSelectedNode() && Input.GetMouseButtonDown(0))
            {
                Node selectedNode = m_Grid.GetSelectedNode();

                if (!m_Grid.CanOccupy(selectedNode))
                {
                    return;
                }

                SpawnTurret(m_Market.ChosenTurret, selectedNode);
            }
        }

        private void SpawnTurret(TurretAsset asset, Node node)
        {
            TurretView view = Object.Instantiate(asset.ViewPrefab);
            TurretData data = new TurretData(asset, node);

            data.AttachView(view);
            Game.Player.TurretSpawned(data);
            
            m_Grid.TryOccupyNode(node, true);
        }
    }
}