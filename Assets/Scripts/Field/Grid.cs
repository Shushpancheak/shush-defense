using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Field
{
    public class Grid
    {
        private Node[,] m_Nodes;

        private int m_Width;
        private int m_Height;
        private Vector3 m_Offset;

        private float m_NodeSize;
        
        private Vector2Int m_StartCoordinate;
        private Vector2Int m_TargetCoordinate;
        
        private Node m_SelectedNode = null;

        private FlowFieldPathfinding m_Pathfinding;

        public int Width => m_Width;
        public int Height => m_Height;

        public Grid(int width, int height, Vector3 offset, float nodeSize, Vector2Int target, Vector2Int start)
        {
            m_Width = width;
            m_Height = height;
            m_Offset = offset;
            m_NodeSize = nodeSize;
            
            m_Nodes = new Node[m_Width, m_Height];

            for (int i = 0; i < m_Nodes.GetLength(0); i++)
            {
                for (int j = 0; j < m_Nodes.GetLength(1); j++)
                {
                    m_Nodes[i, j] = new Node(offset + new Vector3(i + .5f, 0, j + .5f) * nodeSize);
                }
            }

            m_StartCoordinate = start;
            m_TargetCoordinate = target;
            
            m_Pathfinding = new FlowFieldPathfinding(this, target, start);
            
            m_Pathfinding.UpdateField();
        }

        Vector2Int PointToVec2I(Vector3 point)
        {
            Vector3 difference = point - m_Offset;

            int x = (int) (difference.x / m_NodeSize);
            int y = (int) (difference.z / m_NodeSize);
            
            return new Vector2Int(x, y);
        }
        
        public Node GetNodeAtPoint(Vector3 point)
        {
            var v = PointToVec2I(point);
            return m_Nodes[v.x, v.y];
        }

        private bool InCircle(Vector3 point, Vector3 center, float radius)
        {
            return (point - center).magnitude <= radius;
        }

        private bool InGrid(Vector3 point)
        {
            var v = PointToVec2I(point);
            return v.x >= 0 && v.y >= 0 && v.x < m_Width && v.y < m_Height;
        }
        
        public List<Node> GetNodesInCircle(Vector3 point, float radius)
        {
            float node_size_sqr = m_NodeSize * Mathf.Sqrt(2);
            List<Node> result = new List<Node>();
            
            for (float cur_x = point.x - radius; cur_x <= point.x + radius; cur_x += m_NodeSize)
            {
                for (float cur_z = point.z - radius; cur_z <= point.z + radius; cur_z += m_NodeSize)
                {
                    Vector3 cur_point = new Vector3(cur_x, point.y, cur_z);
                    if (InCircle(cur_point, point, radius) && InGrid(cur_point))
                    {
                        result.Add(GetNodeAtPoint(cur_point));
                    }
                }
            }

            return result;
        }
        
        public Node GetStartNode()
        {
            return GetNode(m_StartCoordinate);
        }

        public Node GetTargetNode()
        {
            return GetNode(m_TargetCoordinate);
        }
        
        public void SelectCoordinate(Vector2Int coordinate)
        {
            m_SelectedNode = GetNode(coordinate);
        }

        public void UnselectNode()
        {
            m_SelectedNode = null;
        }

        public bool HasSelectedNode()
        {
            return m_SelectedNode != null;
        }

        public Node GetSelectedNode()
        {
            return m_SelectedNode;
        }

        public bool CanOccupy(Vector2Int coord)
        {
            return m_Pathfinding.CanOccupy(coord);
        }
        
        public bool CanOccupy(Node node)
        {
            return m_Pathfinding.CanOccupy(node);
        }

        /// <summary>
        /// Checks if a node could be occupied and occupies if it can do so.
        /// </summary>
        /// <param name="node"> The node to be occupied </param>
        /// <param name="occupy"> Bool telling whether to occupy or de-occupy the node </param>
        /// <returns> True if the de-/occupation has been completed, false otherwise </returns>
        public bool TryOccupyNode(Node node, bool occupy)
        {
            if (!occupy)
            {
                node.IsOccupied = false;
                UpdatePathfinding();
                return true;
            }
            
            if (!m_Pathfinding.CanOccupy(node)) return false;
            
            node.IsOccupied = true;
            UpdatePathfinding();
            
            return true;
        }
        
        /// <summary>
        /// Checks if a node could be occupied and occupies if it can do so.
        /// </summary>
        /// <param name="coordinate"> The node to be occupied </param>
        /// <param name="occupy"> Bool telling whether to occupy or de-occupy the node </param>
        /// <returns> True if the de-/occupation has been completed, false otherwise </returns>
        public bool TryOccupyNode(Vector2Int coordinate, bool occupy)
        {
            Node node = GetNode(coordinate);
            return TryOccupyNode(node, occupy);
        }
        
        public Node GetNode(Vector2Int coordinate)
        {
            return GetNode(coordinate.x, coordinate.y);
        }

        public Node GetNode(int i, int j)
        {
            if (i < 0 || i >= m_Width)
            {
                return null;
            }

            if (j < 0 || j >= m_Height)
            {
                return null;
            }
            
            return m_Nodes[i, j];
        }

        public IEnumerable<Node> EnumerateAllNodes()
        {
            for (int i = 0; i < m_Width; i++)
            {
                for (int j = 0; j < m_Height; j++)
                {
                    yield return GetNode(i, j);
                }
            }
        }

        public void UpdatePathfinding()
        {
            m_Pathfinding.UpdateField();
        }
    }
}