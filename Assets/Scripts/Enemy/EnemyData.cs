using Assets;

namespace Enemy
{
    public class EnemyData
    {
        private EnemyView m_View;

        public EnemyView View => m_View;

        public readonly EnemyAsset Asset;

        public EnemyData(EnemyAsset asset)
        {
            Asset = asset;
        }

        public void AttachView(EnemyView view)
        {
            m_View = view;
            m_View.AttachData(this);
        }
    }
}