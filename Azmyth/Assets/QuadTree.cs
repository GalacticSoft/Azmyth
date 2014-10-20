using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Drawing;

namespace Azmyth.Assets
{
    public class QuadTree<T> where T : class, IHasRect
    {
        private readonly bool sort;
        private readonly SizeF minLeafSize;
        private readonly int maxObjectsPerLeaf;
        private QuadNode root = null;
        private Dictionary<T, QuadNode> objectToNodeLookup = new Dictionary<T, QuadNode>();
        private Dictionary<T, int> objectSortOrder = new Dictionary<T, int>();
        public QuadNode Root { get { return root; } }
        private object syncLock = new object();
        private int objectSortId = 0;

        public QuadTree(Size minLeafSize, int maxObjectsPerLeaf)
        {
            this.minLeafSize = minLeafSize;
            this.maxObjectsPerLeaf = maxObjectsPerLeaf;
        }

        public int GetSortOrder(T quadObject)
        {
            lock (objectSortOrder)
            {
                if (!objectSortOrder.ContainsKey(quadObject))
                    return -1;
                else
                {
                    return objectSortOrder[quadObject];
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="minLeafSize">The smallest size a leaf will split into</param>
        /// <param name="maxObjectsPerLeaf">Maximum number of objects per leaf before it forces a split into sub quadrants</param>
        /// <param name="sort">Whether or not queries will return objects in the order in which they were added</param>
        public QuadTree(Size minLeafSize, int maxObjectsPerLeaf, bool sort)
            : this(minLeafSize, maxObjectsPerLeaf)
        {
            this.sort = sort;
        }

        public void Insert(T quadObject)
        {
            lock (syncLock)
            {
                if (sort & !objectSortOrder.ContainsKey(quadObject))
                {
                    objectSortOrder.Add(quadObject, objectSortId++);
                }

                RectangleF bounds = quadObject.Bounds;
                if (root == null)
                {
                    var rootSize = new SizeF((float)Math.Ceiling(bounds.Width / minLeafSize.Width),
                                            (float)Math.Ceiling(bounds.Height / minLeafSize.Height));
                    float multiplier = Math.Max(rootSize.Width, rootSize.Height);
                    rootSize = new SizeF(minLeafSize.Width * multiplier, minLeafSize.Height * multiplier);
                    var center = new PointF(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);
                    var rootOrigin = new PointF(center.X - rootSize.Width / 2, center.Y - rootSize.Height / 2);
                    root = new QuadNode(new RectangleF(rootOrigin, rootSize));
                }

                while (!root.Bounds.Contains(bounds))
                {
                    ExpandRoot(bounds);
                }

                InsertNodeObject(root, quadObject);
            }
        }

        public List<T> Query(RectangleF bounds)
        {
            lock (syncLock)
            {
                List<T> results = new List<T>();
                if (root != null)
                    Query(bounds, root, results);
                if (sort)
                    results.Sort((a, b) => { return objectSortOrder[a].CompareTo(objectSortOrder[b]); });
                return results;
            }
        }

        private void Query(RectangleF bounds, QuadNode node, List<T> results)
        {
            lock (syncLock)
            {
                if (node == null) return;

                if (bounds.IntersectsWith(node.Bounds))
                {
                    foreach (T quadObject in node.Objects)
                    {
                        if (bounds.IntersectsWith(quadObject.Bounds))
                            results.Add(quadObject);
                    }

                    foreach (QuadNode childNode in node.Nodes)
                    {
                        Query(bounds, childNode, results);
                    }
                }
            }
        }

        private void ExpandRoot(RectangleF newChildBounds)
        {
            lock (syncLock)
            {
                bool isNorth = root.Bounds.Y < newChildBounds.Y;
                bool isWest = root.Bounds.X < newChildBounds.X;

                QuadDirection rootDirection;
                if (isNorth)
                {
                    rootDirection = isWest ? QuadDirection.NW : QuadDirection.NE;
                }
                else
                {
                    rootDirection = isWest ? QuadDirection.SW : QuadDirection.SE;
                }

                float newX = (rootDirection == QuadDirection.NW || rootDirection == QuadDirection.SW)
                                  ? root.Bounds.X
                                  : root.Bounds.X - root.Bounds.Width;
                float newY = (rootDirection == QuadDirection.NW || rootDirection == QuadDirection.NE)
                                  ? root.Bounds.Y
                                  : root.Bounds.Y - root.Bounds.Height;
                RectangleF newRootBounds = new RectangleF(newX, newY, root.Bounds.Width * 2, root.Bounds.Height * 2);
                QuadNode newRoot = new QuadNode(newRootBounds);
                SetupChildNodes(newRoot);
                newRoot[rootDirection] = root;
                root = newRoot;
            }
        }

        private void InsertNodeObject(QuadNode node, T quadObject)
        {
            lock (syncLock)
            {
                if (!node.Bounds.Contains(quadObject.Bounds))
                    throw new Exception("This should not happen, child does not fit within node bounds");

                if (!node.HasChildNodes() && node.Objects.Count + 1 > maxObjectsPerLeaf)
                {
                    SetupChildNodes(node);

                    List<T> childObjects = new List<T>(node.Objects);
                    List<T> childrenToRelocate = new List<T>();

                    foreach (T childObject in childObjects)
                    {
                        foreach (QuadNode childNode in node.Nodes)
                        {
                            if (childNode == null)
                                continue;

                            if (childNode.Bounds.Contains(childObject.Bounds))
                            {
                                childrenToRelocate.Add(childObject);
                            }
                        }
                    }

                    foreach (T childObject in childrenToRelocate)
                    {
                        RemoveQuadObjectFromNode(childObject);
                        InsertNodeObject(node, childObject);
                    }
                }

                foreach (QuadNode childNode in node.Nodes)
                {
                    if (childNode != null)
                    {
                        if (childNode.Bounds.Contains(quadObject.Bounds))
                        {
                            InsertNodeObject(childNode, quadObject);
                            return;
                        }
                    }
                }

                AddQuadObjectToNode(node, quadObject);
            }
        }

        private void ClearQuadObjectsFromNode(QuadNode node)
        {
            lock (syncLock)
            {
                List<T> quadObjects = new List<T>(node.Objects);
                foreach (T quadObject in quadObjects)
                {
                    RemoveQuadObjectFromNode(quadObject);
                }
            }
        }

        private void RemoveQuadObjectFromNode(T quadObject)
        {
            lock (syncLock)
            {
                QuadNode node = objectToNodeLookup[quadObject];
                node.quadObjects.Remove(quadObject);
                objectToNodeLookup.Remove(quadObject);
                quadObject.BoundsChanged -= new EventHandler(quadObject_BoundsChanged);
            }
        }

        private void AddQuadObjectToNode(QuadNode node, T quadObject)
        {
            lock (syncLock)
            {
                node.quadObjects.Add(quadObject);
                objectToNodeLookup.Add(quadObject, node);
                quadObject.BoundsChanged += new EventHandler(quadObject_BoundsChanged);
            }
        }

        void quadObject_BoundsChanged(object sender, EventArgs e)
        {
            lock (syncLock)
            {
                T quadObject = sender as T;
                if (quadObject != null)
                {
                    QuadNode node = objectToNodeLookup[quadObject];
                    if (!node.Bounds.Contains(quadObject.Bounds) || node.HasChildNodes())
                    {
                        RemoveQuadObjectFromNode(quadObject);
                        Insert(quadObject);
                        if (node.Parent != null)
                        {
                            CheckChildNodes(node.Parent);
                        }
                    }
                }
            }
        }

        private void SetupChildNodes(QuadNode node)
        {
            lock (syncLock)
            {
                if (minLeafSize.Width <= node.Bounds.Width / 2 && minLeafSize.Height <= node.Bounds.Height / 2)
                {
                    node[QuadDirection.NW] = new QuadNode(node.Bounds.X, node.Bounds.Y, node.Bounds.Width / 2,
                                                      node.Bounds.Height / 2);
                    node[QuadDirection.NE] = new QuadNode(node.Bounds.X + node.Bounds.Width / 2, node.Bounds.Y,
                                                      node.Bounds.Width / 2,
                                                      node.Bounds.Height / 2);
                    node[QuadDirection.SW] = new QuadNode(node.Bounds.X, node.Bounds.Y + node.Bounds.Height / 2,
                                                      node.Bounds.Width / 2,
                                                      node.Bounds.Height / 2);
                    node[QuadDirection.SE] = new QuadNode(node.Bounds.X + node.Bounds.Width / 2,
                                                      node.Bounds.Y + node.Bounds.Height / 2,
                                                      node.Bounds.Width / 2, node.Bounds.Height / 2);

                }
            }
        }

        public void Remove(T quadObject)
        {
            lock (syncLock)
            {
                if (sort && objectSortOrder.ContainsKey(quadObject))
                {
                    objectSortOrder.Remove(quadObject);
                }

                if (!objectToNodeLookup.ContainsKey(quadObject))
                    throw new KeyNotFoundException("QuadObject not found in dictionary for removal");

                QuadNode containingNode = objectToNodeLookup[quadObject];
                RemoveQuadObjectFromNode(quadObject);

                if (containingNode.Parent != null)
                    CheckChildNodes(containingNode.Parent);
            }
        }



        private void CheckChildNodes(QuadNode node)
        {
            lock (syncLock)
            {
                if (GetQuadObjectCount(node) <= maxObjectsPerLeaf)
                {
                    // Move child objects into this node, and delete sub nodes
                    List<T> subChildObjects = GetChildObjects(node);
                    foreach (T childObject in subChildObjects)
                    {
                        if (!node.Objects.Contains(childObject))
                        {
                            RemoveQuadObjectFromNode(childObject);
                            AddQuadObjectToNode(node, childObject);
                        }
                    }
                    if (node[QuadDirection.NW] != null)
                    {
                        node[QuadDirection.NW].Parent = null;
                        node[QuadDirection.NW] = null;
                    }
                    if (node[QuadDirection.NE] != null)
                    {
                        node[QuadDirection.NE].Parent = null;
                        node[QuadDirection.NE] = null;
                    }
                    if (node[QuadDirection.SW] != null)
                    {
                        node[QuadDirection.SW].Parent = null;
                        node[QuadDirection.SW] = null;
                    }
                    if (node[QuadDirection.SE] != null)
                    {
                        node[QuadDirection.SE].Parent = null;
                        node[QuadDirection.SE] = null;
                    }

                    if (node.Parent != null)
                        CheckChildNodes(node.Parent);
                    else
                    {
                        // Its the root node, see if we're down to one quadrant, with none in local storage - if so, ditch the other three
                        int numQuadrantsWithObjects = 0;
                        QuadNode nodeWithObjects = null;
                        foreach (QuadNode childNode in node.Nodes)
                        {
                            if (childNode != null && GetQuadObjectCount(childNode) > 0)
                            {
                                numQuadrantsWithObjects++;
                                nodeWithObjects = childNode;
                                if (numQuadrantsWithObjects > 1) break;
                            }
                        }
                        if (numQuadrantsWithObjects == 1)
                        {
                            foreach (QuadNode childNode in node.Nodes)
                            {
                                if (childNode != nodeWithObjects)
                                    childNode.Parent = null;
                            }
                            root = nodeWithObjects;
                        }
                    }
                }
            }
        }


        private List<T> GetChildObjects(QuadNode node)
        {
            lock (syncLock)
            {
                List<T> results = new List<T>();
                results.AddRange(node.quadObjects);
                foreach (QuadNode childNode in node.Nodes)
                {
                    if (childNode != null)
                        results.AddRange(GetChildObjects(childNode));
                }
                return results;
            }
        }

        public int GetQuadObjectCount()
        {
            lock (syncLock)
            {
                if (root == null)
                    return 0;
                int count = GetQuadObjectCount(root);
                return count;
            }
        }

        private int GetQuadObjectCount(QuadNode node)
        {
            lock (syncLock)
            {
                int count = node.Objects.Count;
                foreach (QuadNode childNode in node.Nodes)
                {
                    if (childNode != null)
                    {
                        count += GetQuadObjectCount(childNode);
                    }
                }
                return count;
            }
        }

        public int GetQuadNodeCount()
        {
            lock (syncLock)
            {
                if (root == null)
                    return 0;
                int count = GetQuadNodeCount(root, 1);
                return count;
            }
        }

        private int GetQuadNodeCount(QuadNode node, int count)
        {
            lock (syncLock)
            {
                if (node == null) return count;

                foreach (QuadNode childNode in node.Nodes)
                {
                    if (childNode != null)
                        count++;
                }
                return count;
            }
        }

        public List<QuadNode> GetAllNodes()
        {
            lock (syncLock)
            {
                List<QuadNode> results = new List<QuadNode>();
                if (root != null)
                {
                    results.Add(root);
                    GetChildNodes(root, results);
                }
                return results;
            }
        }

        private void GetChildNodes(QuadNode node, ICollection<QuadNode> results)
        {
            lock (syncLock)
            {
                foreach (QuadNode childNode in node.Nodes)
                {
                    if (childNode != null)
                    {
                        results.Add(childNode);
                        GetChildNodes(childNode, results);
                    }
                }
            }
        }

        public class QuadNode
        {
            private static int _id = 0;
            public readonly int ID = _id++;

            public QuadNode Parent { get; internal set; }

            private QuadNode[] _nodes = new QuadNode[4];
            public QuadNode this[QuadDirection direction]
            {
                get
                {
                    switch (direction)
                    {
                        case QuadDirection.NW:
                            return _nodes[0];
                        case QuadDirection.NE:
                            return _nodes[1];
                        case QuadDirection.SW:
                            return _nodes[2];
                        case QuadDirection.SE:
                            return _nodes[3];
                        default:
                            return null;
                    }
                }
                set
                {
                    switch (direction)
                    {
                        case QuadDirection.NW:
                            _nodes[0] = value;
                            break;
                        case QuadDirection.NE:
                            _nodes[1] = value;
                            break;
                        case QuadDirection.SW:
                            _nodes[2] = value;
                            break;
                        case QuadDirection.SE:
                            _nodes[3] = value;
                            break;
                    }
                    if (value != null)
                        value.Parent = this;
                }
            }

            public ReadOnlyCollection<QuadNode> Nodes;

            internal List<T> quadObjects = new List<T>();
            public ReadOnlyCollection<T> Objects;

            public RectangleF Bounds { get; internal set; }

            public bool HasChildNodes()
            {
                return _nodes[0] != null;
            }

            public QuadNode(RectangleF bounds)
            {
                Bounds = bounds;
                Nodes = new ReadOnlyCollection<QuadNode>(_nodes);
                Objects = new ReadOnlyCollection<T>(quadObjects);
            }

            public QuadNode(float x, float y, float width, float height)
                : this(new RectangleF(x, y, width, height))
            {

            }
        }
    }

    public enum QuadDirection : int
    {
        NW = 0,
        NE = 1,
        SW = 2,
        SE = 3
    }
}