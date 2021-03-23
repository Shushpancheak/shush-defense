using Runtime;

namespace Field
{
    public class GridPointerController : IController
    {
        private GridHolder m_GridHolder;

        public GridPointerController(GridHolder gridHolder)
        {
            m_GridHolder = gridHolder;
        }

        public void OnStart()
        {
        }

        public void OnStop()
        {
        }

        public void Tick()
        {
            m_GridHolder.RaycastInGrid();
        }
    }
}