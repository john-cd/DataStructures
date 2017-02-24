#region Using directives

using System.Collections.Generic;

#endregion Using directives

namespace Libs.DataStructures
{
    /// <summary>
    /// Represents a node in a graph.  A graph node contains some piece of data, along with a set of
    /// neighbors.  There can be an optional cost between a graph node and each of its neighbors.
    /// </summary>
    /// <typeparam name="T">The type of data stored in the graph node.</typeparam>
    public class GraphNode<T> : Node<T>
    {
        #region Private Member Variables

        private List<int> costs;        // the cost associated with each edge

        #endregion Private Member Variables

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public GraphNode() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public GraphNode(T value) : base(value)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="neighbors"></param>
        public GraphNode(T value, NodeCollection<T> neighbors) : base(value, neighbors)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Returns the set of neighbors for this graph node.
        /// </summary>
        new public NodeCollection<T> Neighbors
        {
            get
            {
                if (base.Neighbors == null)
                    base.Neighbors = new NodeCollection<T>();

                return base.Neighbors;
            }
        }

        /// <summary>
        /// Returns the set of costs for the edges eminating from this graph node.
        /// The k<sup>th</sup> cost (Cost[k]) represents the cost from the graph node to the node
        /// represented by its k<sup>th</sup> neighbor (Neighbors[k]).
        /// </summary>
        /// <value></value>
        public IList<int> Costs
        {
            get
            {
                if (costs == null)
                    costs = new List<int>();

                return costs;
            }
        }

        #endregion Properties
    }
}