    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;

    namespace Delegates.TreeTraversal
    {
       public static class Traversal
        {
            public static IEnumerable<int> GetBinaryTreeValues(BinaryTree<int> data)
            {
                return IterateSomething(
                    data,
                    x => new[] {x.Left, x.Right},
                    x => true,
                    x => new [] {x.Value}
                );
            }

            public static IEnumerable<Job> GetEndJobs(Job data)
            {
                return IterateSomething(
                    data,
                    x => x.Subjobs,
                    x => x.Subjobs.Count == 0,
                    x => new[] { x }
                );
            }

            public static IEnumerable<Product> GetProducts(ProductCategory data)
            {
                return IterateSomething(
                    data,
                    x => x.Categories,
                    x => true,
                    x => x.Products
                );
            }

            public static IEnumerable<TValue> IterateSomething<TValue, TGroup>(TGroup data, 
                Func<TGroup, IEnumerable<TGroup>> getGroup, 
                Func<TGroup, bool> getReturningConditions,
                Func<TGroup, IEnumerable<TValue>> getReturningData)
            {
                if (data == null) yield break;
                var groups = getGroup(data);

                foreach(var node in groups)
                    foreach (var element in IterateSomething(node, getGroup, getReturningConditions, getReturningData))
                        yield return element;

                foreach (var element in getReturningData(data))
                    if (getReturningConditions(data))
                        yield return element;
            }
        }
    }
