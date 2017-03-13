using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Generics.BinaryTrees
{
    public class BinaryTree<T> : IEnumerable<T>
        where T : IComparable
    {
        public BinaryTree<T> Left { get; set; }
        public BinaryTree<T> Right { get; set; }
        public BinaryTree<T> Parent { get; set; }
        public T Value { get; set; }

        public void Add(T value)
        {
            if (Equals(Value, default(T)))
            {
                Value = value;
                return;
            }
            if (Value.CompareTo(value) >= 0)
            {
                if (Left == null) Left = new BinaryTree<T>();
                Add(value, Left, this);
            }
            else
            {
                if (Right == null) Right = new BinaryTree<T>();
                Add(value, Right, this);
            }
        }

        public void Add(T value, BinaryTree<T> node, BinaryTree<T> parent)
        {
            if (Equals(node.Value, default(T)))
            {
                node.Value = value;
                node.Parent = parent;
                return;
            }
            if (node.Value.CompareTo(value) >= 0)
            {
                if (node.Left == null) node.Left = new BinaryTree<T>();
                Add(value, node.Left, node);
            }
            else
            {
                if (node.Right == null) node.Right = new BinaryTree<T>();
                Add(value, node.Right, node);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new BinaryTreeEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class BinaryTree
    {
        public static BinaryTree<T> Create<T>(params T[] collection) where T : IComparable
        {
            var tree = new BinaryTree<T>();
            foreach (var node in collection)
            {
                tree.Add(node);
            }
            return tree;
        }
    }

    public class BinaryTreeEnumerator<T> : IEnumerator<T>
        where T : IComparable
    {
        private BinaryTree<T> mtree;
        private List<T> mlist;
        private int mindex;

        public BinaryTreeEnumerator(BinaryTree<T> mtree)
        {
            this.mtree = mtree;
            Current = default(T);
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (mlist == null)
            {
                mlist = new List<T>();
                foreach (var tree in DepthSearch(mtree))
                {
                    if(!Equals(tree.Value, default(T)))
                        mlist.Add(tree.Value);
                    mindex = -1;
                }
                mlist.Sort();
            }
            mindex++;
            if (mindex >= mlist.Count) return false;
            Current = mlist[mindex];
            return true;
        }

        public IEnumerable<BinaryTree<T>> DepthSearch(BinaryTree<T> startNode)
        {
            var visited = new HashSet<BinaryTree<T>>();
            var stack = new Stack<BinaryTree<T>>();
            stack.Push(startNode);
            while (stack.Count != 0)
            {
                var node = stack.Pop();
                if (visited.Contains(node)) continue;
                visited.Add(node);
                yield return node;
                if(node.Left != null)
                    stack.Push(node.Left);
                if(node.Right != null)
                    stack.Push(node.Right);
            }
        }

        public void Reset()
        {
        }

        public T Current { get; private set; }

        object IEnumerator.Current => Current;
    }
}
