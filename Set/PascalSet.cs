#region Using directives

using System;
using System.Collections;

#endregion Using directives

namespace Libs.DataStructures
{
    /// <summary>
    ///
    /// </summary>
    public class PascalSet : ICloneable, ICollection, IEnumerable
    {
        // Private member variables
        private int lowerBound, upperBound;

        private BitArray data;

        #region Constructors

        /// <summary>
        /// Creates a new PascalSet instance with a specified lower and upper bound.
        /// </summary>
        /// <param name="lowerBound">The lower bound for the set.  Can be any legal character value.</param>
        /// <param name="upperBound">The upper bound for the set.  Can be any legal character value.</param>
        public PascalSet(char lowerBound, char upperBound) : this((int)lowerBound, (int)upperBound) { }

        /// <summary>
        /// Creates a new PascalSet instance with a specified lower and upper bound.
        /// </summary>
        /// <param name="lowerBound">The lower bound for the set.  Can be any legal integer value.</param>
        /// <param name="upperBound">The upper bound for the set.  Can be any legal integer value.</param>
        public PascalSet(int lowerBound, int upperBound)
        {
            // make sure lowerbound is less than or equal to upperbound
            if (lowerBound > upperBound)
                throw new ArgumentException("The set's lower bound cannot be greater than its upper bound.");

            this.lowerBound = lowerBound;
            this.upperBound = upperBound;

            // Create the BitArray
            data = new BitArray(upperBound - lowerBound + 1);
        }

        /// <summary>
        /// Creates a new PascalSet instance whose initial values are assigned from an integer array.
        /// </summary>
        /// <param name="lowerBound">The lower bound for the set.  Can be any legal integer value.</param>
        /// <param name="upperBound">The upper bound for the set.  Can be any legal integer value.</param>
        /// <param name="initialData">An integer array that is used as the initial values of the array.</param>
        public PascalSet(int lowerBound, int upperBound, int[] initialData)
        {
            if (initialData == null)
                throw new ArgumentNullException("initialData");
            // make sure lowerbound is less than or equal to upperbound
            if (lowerBound > upperBound)
                throw new ArgumentException("The set's lower bound cannot be greater than its upper bound.");

            this.lowerBound = lowerBound;
            this.upperBound = upperBound;

            // Create the BitArray
            data = new BitArray(upperBound - lowerBound + 1);

            // Populuate the BitArray with the passed-in initialData array.
            for (int i = 0; i < initialData.Length; i++)
            {
                int val = initialData[i];
                if (val >= this.lowerBound && val <= this.upperBound)
                    data.Set(val - this.lowerBound, true);
                else
                    throw new ArgumentException("Attempting to add an element with value " + val.ToString() + " that is outside of the set's universe.  Value must be between " + this.lowerBound.ToString() + " and " + this.upperBound.ToString());
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Determines if two PascalSets are "compatible."  Specifically, it checks to ensure that the PascalSets
        /// share the same lower and upper bounds.
        /// <returns><b>True</b> if the PascalSets share the same bounds, <b>False</b> otherwise.</returns>
        /// </summary>
        protected virtual bool AreSimilar(PascalSet set2)
        {
            if (set2 == null)
                throw new ArgumentNullException("set2");
            return this.lowerBound == set2.lowerBound && this.upperBound == set2.upperBound;
        }

        #region Union

        /// <summary>
        /// Unions a set of integers with the current PascalSet.
        /// <param name="list">An variable number of integers.</param>
        /// <returns>A new PascalSet, which is the union of the <b>this</b> PascalSet and the passed-in integers.</returns>
        /// </summary>
        public virtual PascalSet Union(params int[] list)
        {
            // create a deep copy of this
            PascalSet result = (PascalSet)Clone();

            // For each integer passed in, if it's within the bounds add it to the results's BitArray.
            for (int i = 0; i < list.Length; i++)
            {
                int val = list[i];
                if (val >= this.lowerBound && val <= this.upperBound)
                    result.data.Set(val - this.lowerBound, true);
                else
                    throw new ArgumentException("Attempting to add an element with value " + val.ToString() + " that is outside of the set's universe.  Value must be between " + this.lowerBound.ToString() + " and " + this.upperBound.ToString());
            }

            return result;		// return the new PascalSet
        }

        /// <summary>
        /// Unions a set of characters with the current PascalSet.
        /// </summary>
        /// <param name="list">A variable number of characters.</param>
        /// <returns>A new PascalSet, which is the union of the <b>this</b> PascalSet and the passed-in characters.</returns>
        public virtual PascalSet Union(params char[] list)
        {
            int[] intForm = new int[list.Length];
            Array.Copy(list, intForm, list.Length);
            return Union(intForm);
        }

        /// <summary>
        /// Unions a passed-in PascalSet with the current PascalSet.
        /// </summary>
        /// <param name="set2">A PascalSet.</param>
        /// <returns>A new PascalSet whose elements are the union of <b>s</b> and <b>this</b>.</returns>
        /// <remarks><b>s</b> and <b>this</b> must be "similar" PascalSets.</remarks>
        public virtual PascalSet Union(PascalSet set2)
        {
            if (!AreSimilar(set2))
                throw new ArgumentException("Attempting to union two dissimilar sets.  Union can only occur between two sets with the same universe.");

            // do a bit-wise OR to union together this.data and s.data
            PascalSet result = (PascalSet)Clone();
            result.data.Or(set2.data);

            return result;
        }

        /// <summary>
        /// Overloaded + operator for union...
        /// </summary>
        /// <param name="set1"></param>
        /// <param name="set2"></param>
        /// <returns></returns>
        public static PascalSet operator +(PascalSet set1, PascalSet set2)
        {
            return set1.Union(set2);
        }

        #endregion Union

        #region Intersection

        /// <summary>
        /// Intersects a set of integers with the current PascalSet.
        /// </summary>
        /// <param name="list">An variable number of integers.</param>
        /// <returns>A new PascalSet, which is the intersection of the <b>this</b> PascalSet and the passed-in integers.</returns>
        public virtual PascalSet Intersection(params int[] list)
        {
            PascalSet result = new PascalSet(this.lowerBound, this.upperBound);

            for (int i = 0; i < list.Length; i++)
            {
                // only add the element to result if its in this.data
                int val = list[i];
                if (val >= this.lowerBound && val <= this.upperBound)
                    if (this.data.Get(val - this.lowerBound))
                        result.data.Set(val - this.lowerBound, true);
            }

            return result;
        }

        /// <summary>
        /// Intersects a set of characters with the current PascalSet.
        /// </summary>
        /// <param name="list">A variable number of characters.</param>
        /// <returns>A new PascalSet, which is the intersection of the <b>this</b> PascalSet and the passed-in characters.</returns>
        public virtual PascalSet Intersection(params char[] list)
        {
            int[] intForm = new int[list.Length];
            Array.Copy(list, intForm, list.Length);
            return Intersection(intForm);
        }

        /// <summary>
        /// Intersects a passed-in PascalSet with the current PascalSet.
        /// </summary>
        /// <param name="set2">A PascalSet.</param>
        /// <returns>A new PascalSet whose elements are the intersection of <b>s</b> and <b>this</b>.</returns>
        /// <remarks><b>s</b> and <b>this</b> must be "similar" PascalSets.</remarks>
        public virtual PascalSet Intersection(PascalSet set2)
        {
            if (set2 == null)
                throw new ArgumentNullException("set2");
            if (!AreSimilar(set2))
                throw new ArgumentException("Attempting to intersect two dissimilar sets.  Intersection can only occur between two sets with the same universe.");

            // do a bit-wise AND to intersect this.data and s.data
            PascalSet result = (PascalSet)Clone();
            result.data.And(set2.data);

            return result;
        }

        /// <summary>
        /// Overloaded * operator for intersection
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static PascalSet operator *(PascalSet s, PascalSet t)
        {
            return s.Intersection(t);
        }

        #endregion Intersection

        #region Difference

        /// <summary>
        /// Set differences a set of integers with the current PascalSet.
        /// </summary>
        /// <param name="list">An variable number of integers.</param>
        /// <returns>A new PascalSet, which is the set difference of the <b>this</b> PascalSet and the passed-in integers.</returns>
        public virtual PascalSet Difference(params int[] list)
        {
            PascalSet result = new PascalSet(this.lowerBound, this.upperBound, list);
            return Difference(result);
        }

        /// <summary>
        /// Set differences a set of characters with the current PascalSet.
        /// </summary>
        /// <param name="list">A variable number of characters.</param>
        /// <returns>A new PascalSet, which is the set difference of the <b>this</b> PascalSet and the passed-in characters.</returns>
        public virtual PascalSet Difference(params char[] list)
        {
            int[] intForm = new int[list.Length];
            Array.Copy(list, intForm, list.Length);
            return Difference(intForm);
        }

        /// <summary>
        /// Set differences a passed-in PascalSet with the current PascalSet.
        /// </summary>
        /// <param name="set2">A PascalSet.</param>
        /// <returns>A new PascalSet whose elements are the set difference of <b>s</b> and <b>this</b>.</returns>
        /// <remarks><b>s</b> and <b>this</b> must be "similar" PascalSets.</remarks>
        public virtual PascalSet Difference(PascalSet set2)
        {
            if (set2 == null)
                throw new ArgumentNullException("set2");
            if (!AreSimilar(set2))
                throw new ArgumentException("Attempting to apply set difference to two dissimilar sets.  Set difference can only occur between two sets with the same universe.");

            // do a bit-wise XOR and then an AND to achieve set difference
            PascalSet result = (PascalSet)Clone();
            result.data.Xor(set2.data).And(this.data);

            return result;
        }

        /// <summary>
        /// Overloaded - operator for set difference
        /// </summary>
        /// <param name="set1"></param>
        /// <param name="set2"></param>
        /// <returns></returns>
        public static PascalSet operator -(PascalSet set1, PascalSet set2)
        {
            if (set1 == null)
                throw new ArgumentNullException("set1");
            if (set2 == null)
                throw new ArgumentNullException("set2");
            return set1.Difference(set2);
        }

        #endregion Difference

        #region Complement

        /// <summary>
        /// Complements a PascalSet.
        /// </summary>
        /// <returns>A new PascalSet that is the complement of <b>this</b>.</returns>
        public virtual PascalSet Complement()
        {
            PascalSet result = (PascalSet)Clone();
            result.data.Not();
            return result;
        }

        #endregion Complement

        #region Element Of

        /// <summary>
        /// Determines if a passed-in value is an element of the PascalSet.
        /// </summary>
        /// <param name="x">The integer to check if it exists in the set.</param>
        /// <returns><b>True</b> is <b>x</b> is in the set, <b>False</b> otherwise</returns>
        public virtual bool ContainsElement(int x)
        {
            if (x < lowerBound || x > upperBound)
                return false;

            return this.data.Get(x - lowerBound);
        }

        /// <summary>
        /// Determines if a passed-in value is an element of the PascalSet.
        /// </summary>
        /// <param name="x">The character to check if it exists in the set.</param>
        /// <returns><b>True</b> is <b>x</b> is in the set, <b>False</b> otherwise</returns>
        public virtual bool ContainsElement(char x)
        {
            return ContainsElement((int)x);
        }

        #endregion Element Of

        #region Subset

        /// <summary>
        /// Determins if this set is a subset of the integers passed-in.
        /// </summary>
        /// <param name="list">A variable number of integers.</param>
        /// <returns><b>True</b> if <b>this</b> is a subset of the passed-in integers; <b>False</b> otherwise.</returns>
        public virtual bool Subset(params int[] list)
        {
            PascalSet temp = new PascalSet(this.lowerBound, this.upperBound, list);
            return Subset(temp);
        }

        /// <summary>
        /// Determins if this set is a subset of the characters passed-in.
        /// </summary>
        /// <param name="list">A variable number of characters.</param>
        /// <returns><b>True</b> if <b>this</b> is a subset of the passed-in characters; <b>False</b> otherwise.</returns>
        public virtual bool Subset(params char[] list)
        {
            int[] intForm = new int[list.Length];
            Array.Copy(list, intForm, list.Length);
            return Subset(intForm);
        }

        /// <summary>
        /// Determins if this set is a subset of the passed-in PascalSet.
        /// </summary>
        /// <param name="set2">A PascalSet that is "similar" to <b>this</b>.</param>
        /// <returns><b>True</b> if <b>this</b> is a subset of <b>s</b>; <b>False</b> otherwise.</returns>
        public virtual bool Subset(PascalSet set2)
        {
            if (set2 == null)
                throw new ArgumentNullException("set2");
            if (!AreSimilar(set2))
                throw new ArgumentException("Attempting to compare two dissimilar sets.  Subset comparisons can only occur between two sets with the same universe.");

            // Get the BitArray's underlying array
            const int INT_SIZE = 32;
            int arraySize = (data.Length + INT_SIZE - 1) / INT_SIZE;
            int[] thisBits = new int[arraySize];
            int[] sBits = new int[arraySize];
            data.CopyTo(thisBits, 0);
            set2.data.CopyTo(sBits, 0);

            // now, enumerate through the int array elements
            for (int i = 0; i < thisBits.Length; i++)
            {
                // do a bitwise AND between thisBits[i] and sBits[i];
                int result = thisBits[i] & sBits[i];

                // see if result == thisBits[i] - if it doesn't, then not a subset
                if (result != thisBits[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Determins if this set is a proper subset of the integers passed-in.
        /// </summary>
        /// <param name="list">A variable number of integers.</param>
        /// <returns><b>True</b> if <b>this</b> is a proper subset of the passed-in integers; <b>False</b> otherwise.</returns>
        public virtual bool ProperSubset(params int[] list)
        {
            PascalSet temp = new PascalSet(this.lowerBound, this.upperBound, list);
            return ProperSubset(temp);
        }

        /// <summary>
        /// Determins if this set is a proper subset of the characters passed-in.
        /// </summary>
        /// <param name="list">A variable number of characters.</param>
        /// <returns><b>True</b> if <b>this</b> is a proper subset of the passed-in characters; <b>False</b> otherwise.</returns>
        public virtual bool ProperSubset(params char[] list)
        {
            int[] intForm = new int[list.Length];
            Array.Copy(list, intForm, list.Length);
            return ProperSubset(intForm);
        }

        /// <summary>
        /// Determins if this set is a proper subset of the passed-in PascalSet.
        /// </summary>
        /// <param name="set2">A PascalSet that is "similar" to <b>this</b>.</param>
        /// <returns><b>True</b> if <b>this</b> is a proper subset of <b>s</b>; <b>False</b> otherwise.</returns>
        public virtual bool ProperSubset(PascalSet set2)
        {
            if (set2 == null)
                throw new ArgumentNullException("set2");
            if (!AreSimilar(set2))
                throw new ArgumentException("Attempting to compare two dissimilar sets.  Subset comparisons can only occur between two sets with the same universe.");

            return Subset(set2) && !set2.Subset(this);
        }

        #endregion Subset

        #region Superset

        /// <summary>
        /// Determins if this set is a superset of the integers passed-in.
        /// </summary>
        /// <param name="list">A variable number of integers.</param>
        /// <returns><b>True</b> if <b>this</b> is a superset of the passed-in integers; <b>False</b> otherwise.</returns>
        public virtual bool Superset(params int[] list)
        {
            PascalSet temp = new PascalSet(this.lowerBound, this.upperBound, list);
            return Superset(temp);
        }

        /// <summary>
        /// Determins if this set is a superset of the characters passed-in.
        /// </summary>
        /// <param name="list">A variable number of characters.</param>
        /// <returns><b>True</b> if <b>this</b> is a superset of the passed-in characters; <b>False</b> otherwise.</returns>
        public virtual bool Superset(params char[] list)
        {
            int[] intForm = new int[list.Length];
            Array.Copy(list, intForm, list.Length);
            return Superset(intForm);
        }

        /// <summary>
        /// Determins if this set is a superset of the passed-in PascalSet.
        /// </summary>
        /// <param name="s">A PascalSet that is "similar" to <b>this</b>.</param>
        /// <returns><b>True</b> if <b>this</b> is a superset of <b>s</b>; <b>False</b> otherwise.</returns>
        public virtual bool Superset(PascalSet s)
        {
            if (!AreSimilar(s))
                throw new ArgumentException("Attempting to compare two dissimilar sets.  Superset comparisons can only occur between two sets with the same universe.");

            return s.Subset(this);
        }

        /// <summary>
        /// Determins if this set is a proper superset of the integers passed-in.
        /// </summary>
        /// <param name="list">A variable number of integers.</param>
        /// <returns><b>True</b> if <b>this</b> is a proper superset of the passed-in integers; <b>False</b> otherwise.</returns>
        public virtual bool ProperSuperset(params int[] list)
        {
            PascalSet temp = new PascalSet(this.lowerBound, this.upperBound, list);
            return ProperSuperset(temp);
        }

        /// <summary>
        /// Determins if this set is a proper superset of the characters passed-in.
        /// </summary>
        /// <param name="list">A variable number of characters.</param>
        /// <returns><b>True</b> if <b>this</b> is a proper superset of the passed-in characters; <b>False</b> otherwise.</returns>
        public virtual bool ProperSuperset(params char[] list)
        {
            int[] intForm = new int[list.Length];
            Array.Copy(list, intForm, list.Length);
            return ProperSuperset(intForm);
        }

        /// <summary>
        /// Determins if this set is a proper superset of the passed-in PascalSet.
        /// </summary>
        /// <param name="s">A PascalSet that is "similar" to <b>this</b>.</param>
        /// <returns><b>True</b> if <b>this</b> is a proper superset of <b>s</b>; <b>False</b> otherwise.</returns>
        public virtual bool ProperSuperset(PascalSet s)
        {
            if (!AreSimilar(s))
                throw new ArgumentException("Attempting to compare two dissimilar sets.  Superset comparisons can only occur between two sets with the same universe.");

            return Superset(s) && !s.Superset(this);
        }

        #endregion Superset

        #endregion Methods

        #region PascalSet Properties

        /// <summary>
        /// Returns the lower bound of the set.
        /// </summary>
        public virtual int LowerBound
        {
            get
            {
                return this.lowerBound;
            }
        }

        /// <summary>
        /// Returns the upper bound of the set.
        /// </summary>
        public virtual int UpperBound
        {
            get
            {
                return this.upperBound;
            }
        }

        #endregion PascalSet Properties

        #region ICloneable Members

        /// <summary>
        /// Clones the PascalSet, performing a deep copy.
        /// </summary>
        /// <returns>A new instance of a PascalSet, using a deep copy.</returns>
        public object Clone()
        {
            PascalSet p = new PascalSet(lowerBound, upperBound)
            {
                data = new BitArray(this.data)
            };
            return p;
        }

        #endregion ICloneable Members

        #region ICollection Members

        /// <summary>
        /// Returns a value indicating whether access to the ICollection is synchronized (thread-safe).
        /// </summary>
        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Provides the cardinality of the set.
        /// </summary>
        public int Count
        {
            get
            {
                int elements = 0;
                for (int i = 0; i < data.Length; i++)
                    if (data.Get(i)) elements++;

                return elements;
            }
        }

        /// <summary>
        /// Copies the elements of the ICollection to an Array, starting at a particular Array index.
        /// </summary>
        public void CopyTo(Array array, int index)
        {
            data.CopyTo(array, index);
        }

        /// <summary>
        /// Returns an object that can be used to synchronize access to the ICollection.
        /// </summary>
        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        #endregion ICollection Members

        #region IEnumerable Members

        /// <summary>
        /// Returns an IEnumerator to enumerate through the set.
        /// </summary>
        /// <returns>An IEnumerator instance.</returns>
        public IEnumerator GetEnumerator()
        {
            int totalElements = Count;
            int itemsReturned = 0;
            for (int i = 0; i < this.data.Length; i++)
            {
                if (itemsReturned >= totalElements)
                    break;
                else if (this.data.Get(i))
                    yield return i + this.lowerBound;
            }
        }

        #endregion IEnumerable Members
    }
}