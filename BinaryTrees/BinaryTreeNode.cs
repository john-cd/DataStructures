namespace Libs.DataStructures
{
    /// <summary>
    /// The BinaryTreeNode class represents a node in a binary tree, or a binary search tree.
    /// It has precisely two neighbors, which can be accessed via the Left and Right properties.
    /// </summary>
    /// <typeparam name="T">The type of data stored in the binary tree node.</typeparam>
    public class BinaryTreeNode<T> : Node<T>
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public BinaryTreeNode() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public BinaryTreeNode(T data) : base(data, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public BinaryTreeNode(T data, BinaryTreeNode<T> left, BinaryTreeNode<T> right)
        {
            base.Value = data;
            NodeCollection<T> children = new NodeCollection<T>(2);
            children[0] = left;
            children[1] = right;

            base.Neighbors = children;
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public BinaryTreeNode<T> Left
        {
            get
            {
                if (base.Neighbors == null)
                    return null;
                else
                    return (BinaryTreeNode<T>)base.Neighbors[0];
            }
            set
            {
                if (base.Neighbors == null)
                    base.Neighbors = new NodeCollection<T>(2);

                base.Neighbors[0] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public BinaryTreeNode<T> Right
        {
            get
            {
                if (base.Neighbors == null)
                    return null;
                else
                    return (BinaryTreeNode<T>)base.Neighbors[1];
            }
            set
            {
                if (base.Neighbors == null)
                    base.Neighbors = new NodeCollection<T>(2);

                base.Neighbors[1] = value;
            }
        }

        #endregion Public Properties
    }
}