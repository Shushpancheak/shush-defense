using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace Field
{
    public readonly struct Connection
    {
        public Vector2Int coord { get; }
        public float weight { get; }

        public Connection(Vector2Int coord, float weight)
        {
            this.coord = coord;
            this.weight = weight;
        }
    }

    public class FlowFieldPathfinding
    {
        private Grid m_Grid;
        private Vector2Int m_Target;
        private Vector2Int m_Start;

        public FlowFieldPathfinding(Grid grid, Vector2Int target, Vector2Int start)
        {
            m_Grid = grid;
            m_Target = target;
            m_Start = start;
        }

        /// <summary>
        /// Checks whether 
        /// </summary>
        private bool CanTargetFindStart()
        {
            var visited = new bool[m_Grid.Width][];
            for (int index = 0; index < m_Grid.Width; index++)
            {
                visited[index] = new bool[m_Grid.Height];
            }

            Queue<Vector2Int> queue = new Queue<Vector2Int>();

            queue.Enqueue(m_Target);

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();

                if (current == m_Start)
                {
                    return true;
                }

                if (visited[current.x][current.y])
                {
                    continue;
                }

                visited[current.x][current.y] = true;

                foreach (var neighbour in GetNeighbours(current))
                {
                    if (!visited[neighbour.coord.x][neighbour.coord.y])
                    {
                        queue.Enqueue(neighbour.coord);
                    }
                }
            }

            return false;
        }

        public bool CanOccupy(Node node)
        {
            if (node.IsOccupied)
            {
                return false;
            }
            
            var availability = node.OccupationAvailability;
            
            if (availability != OccupationAvailability.Undefined)
            {
                return availability == OccupationAvailability.CanOccupy;
            }

            node.IsOccupied = true;
            var found_path = CanTargetFindStart();
            node.IsOccupied = false;

            if (found_path)
            {
                node.OccupationAvailability = OccupationAvailability.CanOccupy;
                return true;
            }
            else
            {
                node.OccupationAvailability = OccupationAvailability.CanNotOccupy;
                return false;
            }
        }
        
        public bool CanOccupy(Vector2Int coord)
        { 
            var node = m_Grid.GetNode(coord);
            return CanOccupy(node);
        }

        private void ResetNodes()
        {
            foreach (Node node in m_Grid.EnumerateAllNodes())
            {
                node.ResetWeight();
                node.OccupationAvailability = OccupationAvailability.CanOccupy;
            }

            m_Grid.GetNode(m_Start).OccupationAvailability = OccupationAvailability.CanNotOccupy;
            m_Grid.GetNode(m_Target).OccupationAvailability = OccupationAvailability.CanNotOccupy;
        }

        private void BuildPaths()
        {
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            
            queue.Enqueue(m_Target);
            m_Grid.GetNode(m_Target).PathWeight = 0f;
            m_Grid.GetNode(m_Target).NextNode = null;

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();
                Node currentNode = m_Grid.GetNode(current);

                foreach (var neighbour in GetNeighbours(current))
                {
                    float weightToTarget = currentNode.PathWeight + neighbour.weight;
                    Node neighbourNode = m_Grid.GetNode(neighbour.coord);
                    if (weightToTarget < neighbourNode.PathWeight)
                    {
                        neighbourNode.NextNode = currentNode;
                        neighbourNode.PathWeight = weightToTarget;
                        queue.Enqueue(neighbour.coord);
                    }
                }
            }
        }

        /// <summary>
        /// Sets Nodes' occupation availability to Undefined on path from start to target.
        /// </summary>
        private void SetPathUndefined()
        {
            Node target_node = m_Grid.GetNode(m_Target);
            for (Node cur = m_Grid.GetNode(m_Start).NextNode; cur != target_node; cur = cur.NextNode)
            {
                cur.OccupationAvailability = OccupationAvailability.Undefined;
            }
        }

        public void UpdateField()
        {
            ResetNodes();
            BuildPaths();
            SetPathUndefined();   
        }

        private bool WithinBounds(Vector2Int coord)
        {
            return coord.x >= 0 && coord.x < m_Grid.Width &&
                   coord.y >= 0 && coord.y < m_Grid.Height;
        }

        private bool IsOccupied(Vector2Int coord)
        {
            return m_Grid.GetNode(coord).IsOccupied;
        }

        private bool HasNonDiagonalNode(Vector2Int coord)
        {
            return WithinBounds(coord) && !IsOccupied(coord);
        }

        private class HasNodes
        {
            public bool Left;
            public bool Right;
            public bool Up;
            public bool Down;
            public bool DownRight;
            public bool DownLeft;
            public bool UpRight;
            public bool UpLeft;
        }

        private enum DiagonalOffset
        {
            None,
            DownRight,
            DownLeft,
            UpRight,
            UpLeft
        }

        private bool HasDiagonalNode(DiagonalOffset where, HasNodes has_nodes, Vector2Int coord)
        {
            switch (where)
            {
                case DiagonalOffset.DownRight:
                    return has_nodes.Down && has_nodes.Right && !IsOccupied(coord);
                case DiagonalOffset.DownLeft:
                    return has_nodes.Down && has_nodes.Left && !IsOccupied(coord);
                case DiagonalOffset.UpRight:
                    return has_nodes.Up && has_nodes.Right && !IsOccupied(coord);
                case DiagonalOffset.UpLeft:
                    return has_nodes.Up && has_nodes.Left && !IsOccupied(coord);
                default:
                    throw new ArgumentOutOfRangeException(nameof(@where), @where, null);
            }
        }

        private IEnumerable<Connection> GetNeighbours(Vector2Int coordinate)
        {
            Vector2Int rightCoordinate = coordinate + Vector2Int.right;
            Vector2Int leftCoordinate  = coordinate + Vector2Int.left;
            Vector2Int upCoordinate    = coordinate + Vector2Int.up;
            Vector2Int downCoordinate  = coordinate + Vector2Int.down;
            
            Vector2Int downrightCoordinate = coordinate + Vector2Int.down + Vector2Int.right;
            Vector2Int downleftCoordinate  = coordinate + Vector2Int.down + Vector2Int.left;
            Vector2Int uprightCoordinate   = coordinate + Vector2Int.up   + Vector2Int.right;
            Vector2Int upleftCoordinate    = coordinate + Vector2Int.up   + Vector2Int.left;

            HasNodes has_nodes = new HasNodes
            {
                Right = HasNonDiagonalNode(rightCoordinate),
                Left  = HasNonDiagonalNode(leftCoordinate),
                Up    = HasNonDiagonalNode(upCoordinate),
                Down  = HasNonDiagonalNode(downCoordinate),
                DownRight = false,
                DownLeft  = false,
                UpRight   = false,
                UpLeft    = false
            };

            has_nodes.DownRight = HasDiagonalNode(DiagonalOffset.DownRight, has_nodes, downrightCoordinate);
            has_nodes.DownLeft  = HasDiagonalNode(DiagonalOffset.DownLeft,  has_nodes, downleftCoordinate);
            has_nodes.UpRight   = HasDiagonalNode(DiagonalOffset.UpRight,   has_nodes, uprightCoordinate);
            has_nodes.UpLeft    = HasDiagonalNode(DiagonalOffset.UpLeft,    has_nodes, upleftCoordinate);

            float sqrt2 = Mathf.Pow(2, 1.0f / 2.0f);
            
            if (has_nodes.Right)
            {
                yield return new Connection(rightCoordinate, 1);
            }
            
            if (has_nodes.Left)
            {
                yield return new Connection(leftCoordinate, 1);
            }
            
            if (has_nodes.Up)
            {
                yield return new Connection(upCoordinate, 1);
            }
            
            if (has_nodes.Down)
            {
                yield return new Connection(downCoordinate, 1);
            }

            if (has_nodes.DownRight)
            {
                yield return new Connection(downrightCoordinate, sqrt2);
            }

            if (has_nodes.DownLeft)
            {
                yield return new Connection(downleftCoordinate, sqrt2);
            }
            
            if (has_nodes.UpRight)
            {
                yield return new Connection(uprightCoordinate, sqrt2);
            }

            if (has_nodes.UpLeft)
            {
                yield return new Connection(upleftCoordinate, sqrt2);
            }
        }
    }
}