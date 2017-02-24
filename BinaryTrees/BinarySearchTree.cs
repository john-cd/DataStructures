﻿#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;

#endregion Using directives

namespace Libs.DataStructures
{
    /// <summary>
    /// Represents a binary search tree.  A binary search tree is a binary tree whose nodes are arranged
    /// such that for any given node k, all nodes in k's left subtree have a value less than k, and all
    /// nodes in k's right subtree have a value greater than k.
    /// </summary>
    /// <typeparam name="T">The type of data stored in the binary tree nodes.</typeparam>
    public class BinarySearchTree<T> : ICollection<T>, IEnumerable<T>
    {
        #region Private Member Variables

        private BinaryTreeNode<T> root = null;
        private int count = 0;
        private IComparer<T> comparer = Comparer<T>.Default;    // used to compare node values when percolating down the tree

        #endregion Private Member Variables

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public BinarySearchTree()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparer"></param>
        public BinarySearchTree(IComparer<T> comparer)
        {
            this.comparer = comparer;
        }

        #endregion Constructors

        #region Public Methods

        #region Clear

        /// <summary>
        /// Removes the contents of the BST
        /// </summary>
        public void Clear()
        {
            root = null;
            count = 0;
        }

        #endregion Clear

        #region CopyTo

        /// <summary>
        /// Copies the contents of the BST to an appropriately-sized array of type T, using the Inorder
        /// traversal method.
        /// </summary>
        public void CopyTo(T[] array, int arrayIndex)
        {
            CopyTo(array, arrayIndex, TraversalMethod.InOrder);
        }

        /// <summary>
        /// Copies the contents of the BST to an appropriately-sized array of type T, using a specified
        /// traversal method.
        /// </summary>
        public void CopyTo(T[] array, int index, TraversalMethod traversalMethod)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            IEnumerable<T> enumProp = null;

            // Determine which Enumerator-returning property to use, based on the TraversalMethod input parameter
            switch (traversalMethod)
            {
                case TraversalMethod.PreOrder:
                    enumProp = Preorder;
                    break;

                case TraversalMethod.InOrder:
                    enumProp = InOrder;
                    break;

                case TraversalMethod.PostOrder:
                default:
                    enumProp = PostOrder;
                    break;
            }

            // dump the contents of the tree into the passed-in array
            int i = 0;
            foreach (T value in enumProp)
            {
                array[i + index] = value;
                i++;
            }
        }

        #endregion CopyTo

        #region Add

        /// <summary>
        /// Adds a new value to the BST.
        /// </summary>
        /// <param name="item">The data to insert into the BST.</param>
        /// <remarks>Adding a value already in the BST has no effect; that is, the SkipList is not
        /// altered, the Add() method simply exits.</remarks>
        public virtual void Add(T item)
        {
            // create a new Node instance
            BinaryTreeNode<T> n = new BinaryTreeNode<T>(item);
            int result;

            // now, insert n into the tree
            // trace down the tree until we hit a NULL
            BinaryTreeNode<T> current = root, parent = null;
            while (current != null)
            {
                result = comparer.Compare(current.Value, item);
                if (result == 0)
                    // they are equal - attempting to enter a duplicate - do nothing
                    return;
                else if (result > 0)
                {
                    // current.Value > data, must add n to current's left subtree
                    parent = current;
                    current = current.Left;
                }
                else if (result < 0)
                {
                    // current.Value < data, must add n to current's right subtree
                    parent = current;
                    current = current.Right;
                }
            }

            // We're ready to add the node!
            count++;
            if (parent == null)
                // the tree was empty, make n the root
                root = n;
            else
            {
                result = comparer.Compare(parent.Value, item);
                if (result > 0)
                    // parent.Value > data, therefore n must be added to the left subtree
                    parent.Left = n;
                else
                    // parent.Value < data, therefore n must be added to the right subtree
                    parent.Right = n;
            }
        }

        #endregion Add

        #region Contains

        /// <summary>
        /// Returns a Boolean, indicating if a specified value is contained within the BST.
        /// </summary>
        /// <param name="item">The data to search for.</param>
        /// <returns>True if data is found in the BST; false otherwise.</returns>
        public bool Contains(T item)
        {
            // search the tree for a node that contains data
            BinaryTreeNode<T> current = root;
            int result;
            while (current != null)
            {
                result = comparer.Compare(current.Value, item);
                if (result == 0)
                    // we found data
                    return true;
                else if (result > 0)
                    // current.Value > data, search current's left subtree
                    current = current.Left;
                else if (result < 0)
                    // current.Value < data, search current's right subtree
                    current = current.Right;
            }

            return false;       // didn't find data
        }

        #endregion Contains

        #region Remove

        /// <summary>
        /// Attempts to remove the specified data element from the BST.
        /// </summary>
        /// <param name="item">The data to remove from the BST.</param>
        /// <returns>True if the element is found in the tree, and removed; false if the element is not
        /// found in the tree.</returns>
        public bool Remove(T item)
        {
            // first make sure there exist some items in this tree
            if (root == null)
                return false;       // no items to remove

            // Now, try to find data in the tree
            BinaryTreeNode<T> current = root, parent = null;
            int result = comparer.Compare(current.Value, item);
            while (result != 0)
            {
                if (result > 0)
                {
                    // current.Value > data, if data exists it's in the left subtree
                    parent = current;
                    current = current.Left;
                }
                else if (result < 0)
                {
                    // current.Value < data, if data exists it's in the right subtree
                    parent = current;
                    current = current.Right;
                }

                // If current == null, then we didn't find the item to remove
                if (current == null)
                    return false;
                else
                    result = comparer.Compare(current.Value, item);
            }

            // At this point, we've found the node to remove
            count--;

            // We now need to "rethread" the tree
            // CASE 1: If current has no right child, then current's left child becomes
            //         the node pointed to by the parent
            if (current.Right == null)
            {
                if (parent == null)
                    root = current.Left;
                else
                {
                    result = comparer.Compare(parent.Value, current.Value);
                    if (result > 0)
                        // parent.Value > current.Value, so make current's left child a left child of parent
                        parent.Left = current.Left;
                    else if (result < 0)
                        // parent.Value < current.Value, so make current's left child a right child of parent
                        parent.Right = current.Left;
                }
            }
            // CASE 2: If current's right child has no left child, then current's right child
            //         replaces current in the tree
            else if (current.Right.Left == null)
            {
                current.Right.Left = current.Left;

                if (parent == null)
                    root = current.Right;
                else
                {
                    result = comparer.Compare(parent.Value, current.Value);
                    if (result > 0)
                        // parent.Value > current.Value, so make current's right child a left child of parent
                        parent.Left = current.Right;
                    else if (result < 0)
                        // parent.Value < current.Value, so make current's right child a right child of parent
                        parent.Right = current.Right;
                }
            }
            // CASE 3: If current's right child has a left child, replace current with current's
            //          right child's left-most descendent
            else
            {
                // We first need to find the right node's left-most child
                BinaryTreeNode<T> leftmost = current.Right.Left, lmParent = current.Right;
                while (leftmost.Left != null)
                {
                    lmParent = leftmost;
                    leftmost = leftmost.Left;
                }

                // the parent's left subtree becomes the leftmost's right subtree
                lmParent.Left = leftmost.Right;

                // assign leftmost's left and right to current's left and right children
                leftmost.Left = current.Left;
                leftmost.Right = current.Right;

                if (parent == null)
                    root = leftmost;
                else
                {
                    result = comparer.Compare(parent.Value, current.Value);
                    if (result > 0)
                        // parent.Value > current.Value, so make leftmost a left child of parent
                        parent.Left = leftmost;
                    else if (result < 0)
                        // parent.Value < current.Value, so make leftmost a right child of parent
                        parent.Right = leftmost;
                }
            }

            // Clear out the values from current
            current.Left = current.Right = null;
            current = null;

            return true;
        }

        #endregion Remove

        #region GetEnumerator

        /// <summary>
        /// Enumerates the BST's contents using inorder traversal.
        /// </summary>
        /// <returns>An enumerator that provides inorder access to the BST's elements.</returns>
        public virtual IEnumerator<T> GetEnumerator()
        {
            return GetEnumerator(TraversalMethod.InOrder);
        }

        /// <summary>
        /// Enumerates the BST's contents using a specified traversal method.
        /// </summary>
        /// <param name="traversalMethod">The type of traversal to perform.</param>
        /// <returns>An enumerator that provides access to the BST's elements using a specified traversal technique.</returns>
        public virtual IEnumerator<T> GetEnumerator(TraversalMethod traversalMethod)
        {
            // The traversal approaches are defined as public properties in the BST class...
            // This method simply returns the appropriate property.
            switch (traversalMethod)
            {
                case TraversalMethod.PreOrder:
                    return Preorder.GetEnumerator();

                case TraversalMethod.InOrder:
                    return InOrder.GetEnumerator();

                case TraversalMethod.PostOrder:
                default:
                    return PostOrder.GetEnumerator();
            }
        }

        #endregion GetEnumerator

        #endregion Public Methods

        #region Public Properties

        #region Enumerable Properties

        /// <summary>
        /// Provides enumeration through the BST using preorder traversal.
        /// </summary>
        public IEnumerable<T> Preorder
        {
            get
            {
                // A single stack is sufficient here - it simply maintains the correct
                // order with which to process the children.
                Stack<BinaryTreeNode<T>> toVisit = new Stack<BinaryTreeNode<T>>(Count);
                BinaryTreeNode<T> current = root;
                if (current != null) toVisit.Push(current);

                while (toVisit.Count != 0)
                {
                    // take the top item from the stack
                    current = toVisit.Pop();

                    // add the right and left children, if not null
                    if (current.Right != null) toVisit.Push(current.Right);
                    if (current.Left != null) toVisit.Push(current.Left);

                    // return the current node
                    yield return current.Value;
                }
            }
        }

        /// <summary>
        /// Provides enumeration through the BST using inorder traversal.
        /// </summary>
        public IEnumerable<T> InOrder
        {
            get
            {
                // A single stack is sufficient - this code was made available by Grant Richins:
                // http://blogs.msdn.com/grantri/archive/2004/04/08/110165.aspx
                Stack<BinaryTreeNode<T>> toVisit = new Stack<BinaryTreeNode<T>>(Count);
                for (BinaryTreeNode<T> current = root; current != null || toVisit.Count != 0; current = current.Right)
                {
                    // Get the left-most item in the subtree, remembering the path taken
                    while (current != null)
                    {
                        toVisit.Push(current);
                        current = current.Left;
                    }

                    current = toVisit.Pop();
                    yield return current.Value;
                }
            }
        }

        /// <summary>
        /// Provides enumeration through the BST using postorder traversal.
        /// </summary>
        public IEnumerable<T> PostOrder
        {
            get
            {
                // maintain two stacks, one of a list of nodes to visit,
                // and one of booleans, indicating if the note has been processed
                // or not.
                Stack<BinaryTreeNode<T>> toVisit = new Stack<BinaryTreeNode<T>>(Count);
                Stack<bool> hasBeenProcessed = new Stack<bool>(Count);
                BinaryTreeNode<T> current = root;
                if (current != null)
                {
                    toVisit.Push(current);
                    hasBeenProcessed.Push(false);
                    current = current.Left;
                }

                while (toVisit.Count != 0)
                {
                    if (current != null)
                    {
                        // add this node to the stack with a false processed value
                        toVisit.Push(current);
                        hasBeenProcessed.Push(false);
                        current = current.Left;
                    }
                    else
                    {
                        // see if the node on the stack has been processed
                        bool processed = hasBeenProcessed.Pop();
                        BinaryTreeNode<T> node = toVisit.Pop();
                        if (!processed)
                        {
                            // if it's not been processed, "recurse" down the right subtree
                            toVisit.Push(node);
                            hasBeenProcessed.Push(true);    // it's now been processed
                            current = node.Right;
                        }
                        else
                            yield return node.Value;
                    }
                }
            }
        }

        #endregion Enumerable Properties

        /// <summary>
        /// Returns the number of elements in the BST.
        /// </summary>
        public int Count
        {
            get
            {
                return count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        #endregion Public Properties

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator() as IEnumerator;
        }
    }
}