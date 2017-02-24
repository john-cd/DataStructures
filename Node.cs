namespace Libs.DataStructures
{
    /// <summary>
    /// The Node&lt;T&gt; class represents the base concept of a Node for a tree or graph.  It contains
    /// a data item of type T, and a list of neighbors.
    /// </summary>
    /// <typeparam name="T">The type of data contained in the Node.</typeparam>
    /// <remarks>None of the classes in the Libs.DataStructures namespace use the Node class directly;
    /// they all derive from this class, adding necessary functionality specific to each data structure.</remarks>
    public class Node<T>
    {
        #region Private Member Variables

        private T data;
        private NodeCollection<T> neighbors = null;

        #endregion Private Member Variables

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public Node()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public Node(T data) : this(data, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="neighbors"></param>
        public Node(T data, NodeCollection<T> neighbors)
        {
            this.data = data;
            this.neighbors = neighbors;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public T Value
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected NodeCollection<T> Neighbors
        {
            get
            {
                return neighbors;
            }
            set
            {
                neighbors = value;
            }
        }

        #endregion Properties
    }
}