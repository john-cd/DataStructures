#region Using directives

using System.Collections.ObjectModel;

#endregion Using directives

namespace Libs.DataStructures
{
    /// <summary>
    /// Represents a collection of Node&lt;T&gt; instances.
    /// </summary>
    /// <typeparam name="T">The type of data held in the Node instances referenced by this class.</typeparam>
    public class NodeCollection<T> : Collection<Node<T>>
    {
        #region Constructors

        /// <summary>
        ///
        /// </summary>
        public NodeCollection() : base()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="initialSize"></param>
        public NodeCollection(int initialSize)
        {
            // Add the specified number of items
            for (int i = 0; i < initialSize; i++)
                base.Items.Add(default(Node<T>));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Searches the NodeCollection for a Node containing a particular value.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <returns>The Node in the NodeCollection, if it exists; null otherwise.</returns>
        public Node<T> FindByValue(T value)
        {
            // search the list for the value
            foreach (Node<T> node in Items)
                if (node.Value.Equals(value))
                    return node;

            // if we reached here, we didn't find a matching node
            return null;
        }

        #endregion Methods
    }
}