namespace Libs.DataStructures
{
    /// <summary>
    /// Represents a binary tree.  This class provides access to the Root of the tree.  The developer
    /// must manually create the binary tree by adding descendents to the root.
    /// </summary>
    /// <typeparam name="T">The type of data stored in the binary tree's nodes.</typeparam>
    public class BinaryTree<T>
    {
        #region Private Member Variables

        private BinaryTreeNode<T> root = null;

        #endregion Private Member Variables

        #region Public Methods

        /// <summary>
        /// Clears out the contents of the binary tree.
        /// </summary>
        public void Clear()
        {
            root = null;
        }

        #endregion Public Methods

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public BinaryTreeNode<T> Root
        {
            get
            {
                return root;
            }
            set
            {
                root = value;
            }
        }

        #endregion Public Properties
    }
}