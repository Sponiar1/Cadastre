using Cadastre.DataStructure.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.DataStructure
{
    public class QuadTree<T> where T : IComparator<T>
    {
        QuadTreeNode<T> root;
        int maxHeight;
        public int Size { get; set; }

        public QuadTree(double paSizeX1, double paSizeX2, double paSizeY1, double paSizeY2, int height)
        {
            root = new QuadTreeNode<T>(new QuadTreeRectangle(paSizeX1, paSizeX2, paSizeY1, paSizeY2));
            root.Height = 0;
            maxHeight = height;
            Size = 0;
        }
        public QuadTree(double paSizeX1, double paSizeX2, double paSizeY1, double paSizeY2, int height, Queue<T> items)
        {
            root = new QuadTreeNode<T>(new QuadTreeRectangle(paSizeX1, paSizeX2, paSizeY1, paSizeY2));
            root.Height = 0;
            maxHeight = height;
            Size = 0;
            int queueSize = items.Count;
            for (int i = 0; i < queueSize; i++)
            {
                insert(items.Dequeue());
            }
        }
        public Boolean insert(T item)
        {
            return insertAt(item, root);
        }
        public Boolean insertAt(T item, QuadTreeNode<T> node)
        {
            QuadTreeNode<T> currentNode = node;
            QuadTreeNode<T> parent = node.parent;
            Queue<T> items = new Queue<T>();
            Queue<QuadTreeNode<T>> startingNode = new Queue<QuadTreeNode<T>>();
            Boolean createdSons = false;
            items.Enqueue(item);
            startingNode.Enqueue(currentNode);
            while (items.Count() != 0)
            {
                if (currentNode == null)
                {
                    currentNode = startingNode.Peek();
                }
                if (currentNode.Height == maxHeight || (currentNode.getNumberOfItems() == 0 && currentNode.isLeaf))
                {
                    currentNode.insert(items.Dequeue());
                    startingNode.Dequeue();
                    currentNode = null;
                }
                else
                {
                    if (currentNode.Items.Count == 1 && currentNode.Height != maxHeight && ((T)currentNode.Items[0]).ChangePossible() != 0)
                    {
                        items.Enqueue((T)currentNode.Items[0]);
                        currentNode.Items.RemoveAt(0);
                        startingNode.Enqueue(currentNode);
                    }

                    parent = currentNode;
                    if (currentNode.sons[0] == null)
                    {
                        currentNode.createSons();
                        createdSons = true;
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        if (items.Peek().CompareTo(parent.sons[i].Zone) == 0)
                        {
                            parent.isLeaf = false;
                            currentNode = currentNode.sons[i];
                            createdSons = false;
                            break;
                        }

                        if (i == 3 && createdSons)
                        {
                            createdSons = false;
                            currentNode.deleteSons();
                        }
                    }
                    if (currentNode == parent)
                    {
                        items.Peek().FoundSmallestZone();
                        currentNode.insert(items.Dequeue());
                        startingNode.Dequeue();
                        currentNode = null;
                    }
                }
            }
            Size++;
            return true;
        }
        public List<T> find(QuadTreeRectangle searchArea)
        {
            List<T> results = new List<T>();
            Queue<QuadTreeNode<T>> nodes = new Queue<QuadTreeNode<T>>();

            if (root == null || !root.Zone.blending(searchArea))
            {
                return results;
            }
            nodes.Enqueue(root);

            while (nodes.Count != 0)
            {
                QuadTreeNode<T> currentNode = nodes.Dequeue();

                foreach (T item in currentNode.Items)
                {
                    if (item.CompareTo(searchArea) == 0)
                    {
                        results.Add(item);
                    }
                }
                if (currentNode.sons[0] != null)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (currentNode.sons[i].Zone.blending(searchArea))
                        {
                            nodes.Enqueue(currentNode.sons[i]);
                        }
                    }
                }
            }
            return results;
        }
        public Boolean remove(T item)
        {
            QuadTreeNode<T> currentNode = root;
            QuadTreeNode<T> previousNode = null;
            while (currentNode != previousNode)
            {
                previousNode = currentNode;
                if (currentNode.sons[0] != null)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (item.CompareTo(currentNode.sons[i].Zone) == 0)
                        {
                            currentNode = currentNode.sons[i];
                            break;
                        }
                    }
                }
            }
            foreach (T searchedItem in currentNode.Items)
            {
                if (item.CompareById(searchedItem) == 0)
                {
                    return remove(item, currentNode);
                }
            };
            return false;
        }
        public Boolean remove(T item, QuadTreeNode<T> node)
        {
            if (node.Items.Count > 1) //ak su tam 2 itemy, je dovod prečo su 2 prave tam
            {
                node.remove(item);
            }
            else if (node.isLeaf) //súrodenci sú tiež listy
            {
                node.remove(item);
                Boolean hasUsedChildren = false;
                Boolean moreChildsInOneNode = false;
                int numberOfChildItems = 0;
                QuadTreeNode<T> childNodeWithItems = null;
                //zistiť či súrodenci majú max 1 item a sú listy, ak hej tak vymazať
                for (int i = 0; i < 4; i++)
                {
                    if (!node.parent.sons[i].isLeaf)
                    {
                        hasUsedChildren = true;
                    }
                    if (node.parent.sons[i].Items.Count > 0)
                    {
                        numberOfChildItems += node.parent.sons[i].Items.Count;
                        childNodeWithItems = node.parent.sons[i];
                    }
                }
                if (numberOfChildItems == 0 && !hasUsedChildren)
                {
                    node.parent.deleteSons();
                }
                else if (numberOfChildItems == 1 && !hasUsedChildren)
                {
                    if (node.parent.Items.Count == 0)
                    {
                        ((T)childNodeWithItems.Items[0]).RelocatedToBiggerZone();
                        node.parent.Items.Add(childNodeWithItems.Items[0]);
                        childNodeWithItems.Items.RemoveAt(0);
                        node.parent.deleteSons();
                    }
                }
            }
            else //môže mať synov čo nie/sú listy a majú 1/viac itemov
            {
                node.remove(item);
                Boolean hasUsedChildren = false;
                int numberOfChildItems = 0;
                QuadTreeNode<T> childNodeWithItems = null;
                for (int i = 0; i < 4; i++)
                {
                    if (!node.sons[i].isLeaf)
                    {
                        hasUsedChildren = true;
                    }
                    if (node.sons[i].Items.Count > 0)
                    {
                        numberOfChildItems += node.sons[i].Items.Count;
                        childNodeWithItems = node.sons[i];
                    }
                }
                if (numberOfChildItems == 1 && !hasUsedChildren)
                {
                    ((T)childNodeWithItems.Items[0]).RelocatedToBiggerZone();
                    node.Items.Add(childNodeWithItems.Items[0]);
                    childNodeWithItems.Items.RemoveAt(0);
                    node.deleteSons();
                }
            }
            Size--;
            return true;
        }
        public void changeMaxHeight(int newHeight)
        {
            int oldMaxHeight = maxHeight;
            maxHeight = newHeight;

            if (root == null)
            {
                return;
            }


            if (oldMaxHeight < maxHeight)
            {
                Queue<QuadTreeNode<T>> queue = new Queue<QuadTreeNode<T>>();
                Queue<T> newTreeOrder = new Queue<T>();
                queue.Enqueue(root);

                while (queue.Count > 0)
                {
                    QuadTreeNode<T> currentNode = queue.Dequeue();

                    for (int i = 0; i < 4; i++)
                    {
                        if (currentNode.sons[i] != null)
                        {
                            queue.Enqueue(currentNode.sons[i]);
                        }
                    }

                    if (currentNode.Height == oldMaxHeight)
                    {
                        recheckItems(currentNode);
                    }
                }
            }
            else
            {
                Queue<QuadTreeNode<T>> queue = new Queue<QuadTreeNode<T>>();
                Queue<T> newTreeOrder = new Queue<T>();
                Stack<QuadTreeNode<T>> stack = new Stack<QuadTreeNode<T>>();
                Stack<QuadTreeNode<T>> stackNewLeafs = new Stack<QuadTreeNode<T>>();

                queue.Enqueue(root);

                while (queue.Count > 0)
                {
                    QuadTreeNode<T> currentNode = queue.Dequeue();

                    for (int i = 0; i < 4; i++)
                    {
                        if (currentNode.sons[i] != null)
                        {
                            queue.Enqueue(currentNode.sons[i]);
                        }
                    }

                    if (currentNode.Height > maxHeight)
                    {
                        stack.Push(currentNode);
                    }
                    else if (currentNode.Height == maxHeight)
                    {
                        stackNewLeafs.Push(currentNode);
                    }
                }

                int nodesToDestroy = stack.Count;
                for (int i = 0; i < nodesToDestroy; i++)
                {
                    recheckItems(stack.Pop());
                }
                int newLeafs = stackNewLeafs.Count;
                for (int i = 0; i < newLeafs; i++)
                {
                    stackNewLeafs.Pop().deleteSons();
                }
            }
        }
        private Boolean recheckItems(QuadTreeNode<T> node)
        {
            if (node == null)
            {
                return true;
            }

            QuadTreeNode<T> designatedNode = node;
            if (node.Height > maxHeight)
            {
                while (designatedNode.Height != maxHeight)
                {
                    designatedNode = designatedNode.parent;
                }

                for (int i = 0; i < node.Items.Count; i++)
                {
                    T item = (T)node.Items[i];
                    item.RelocatedToBiggerZone();
                    designatedNode.insert(item);
                    node.Items.RemoveAt(i);
                    i--;
                }
                node.deleteSons();
            }
            else if (node.Height < maxHeight && node.isLeaf)
            {
                if (node.Items.Count > 1)
                {
                    for (int i = 0; i < node.Items.Count; i++)
                    {
                        if (((T)node.Items[i]).ChangePossible() == 1) //pokial mrvok nemoze ist nizsie tak sa vrati loop
                        {
                            T item = (T)node.Items[i];
                            node.Items.RemoveAt(i);
                            insertAt(item, node);
                            i--;
                        }
                    }
                }
            }

            return true;
        }
        public double[] calculateHealth()
        {
            double[] items = new double[5];
            Stack<QuadTreeNode<T>> postOrderStack = new Stack<QuadTreeNode<T>>();
            QuadTreeNode<T> currentNode;
            QuadTreeNode<T> lastVisited = null;

            for (int i = 0; i < 4; i++)
            {
                currentNode = root.sons[i];

                Queue<QuadTreeNode<T>> queue = new Queue<QuadTreeNode<T>>();
                queue.Enqueue(currentNode);

                while (queue.Count > 0)
                {
                    currentNode = queue.Dequeue();

                    for (int j = 0; j < 4; j++)
                    {
                        if (currentNode.sons[j] != null)
                        {
                            queue.Enqueue(currentNode.sons[j]);
                        }
                        else
                        {
                            break;
                        }
                    }
                    items[i] += currentNode.Items.Count;
                }
            }
            double maxItems = items.Max();
            double minItems = items.Take(4).Min();
            items[4] = minItems / maxItems;

            return items;
        }
        public QuadTree<T> rebuildTree()
        {
            double[] treeInfo = this.calculateHealth();
            Queue<QuadTreeNode<T>> queue = new Queue<QuadTreeNode<T>>();
            Queue<T> items = new Queue<T>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                QuadTreeNode<T> currentNode = queue.Dequeue();

                foreach (T item in currentNode.Items)
                {
                    items.Enqueue(item);
                }

                for (int i = 0; i < 4; i++)
                {
                    if (currentNode.sons[i] != null)
                    {
                        queue.Enqueue(currentNode.sons[i]);
                    }
                }
            }

            if (treeInfo[4] > 0.7)
            {
                return this;
            }
            else
            {
                QuadTree<T> newTree;
                if (treeInfo.Max() == treeInfo[0])
                {
                    newTree = new QuadTree<T>(root.Zone.BottomLeftX - ((root.Zone.UpperRightX - root.Zone.BottomLeftX) / 3),
                                                    root.Zone.BottomLeftY,
                                                    root.Zone.UpperRightX,
                                                    root.Zone.UpperRightY + ((root.Zone.UpperRightY - root.Zone.BottomLeftY) / 3),
                                                    maxHeight,
                                                    items);
                }
                else if (treeInfo.Max() == treeInfo[1])
                {
                    newTree = new QuadTree<T>(root.Zone.BottomLeftX,
                                                    root.Zone.BottomLeftY,
                                                    root.Zone.UpperRightX + ((root.Zone.UpperRightX - root.Zone.BottomLeftX) / 3),
                                                    root.Zone.UpperRightY + ((root.Zone.UpperRightY - root.Zone.BottomLeftY) / 3),
                                                    maxHeight,
                                                    items);
                }
                else if (treeInfo.Max() == treeInfo[2])
                {
                    newTree = new QuadTree<T>(root.Zone.BottomLeftX,
                                                    root.Zone.BottomLeftY - ((root.Zone.UpperRightY - root.Zone.BottomLeftY) / 3),
                                                    root.Zone.UpperRightX + ((root.Zone.UpperRightX - root.Zone.BottomLeftX) / 3),
                                                    root.Zone.UpperRightY,
                                                    maxHeight,
                                                    items);
                }
                else
                {
                    newTree = new QuadTree<T>(root.Zone.BottomLeftX - ((root.Zone.UpperRightX - root.Zone.BottomLeftX) / 3),
                                                    root.Zone.BottomLeftY - ((root.Zone.UpperRightY - root.Zone.BottomLeftY) / 3),
                                                    root.Zone.UpperRightX,
                                                    root.Zone.UpperRightY,
                                                    maxHeight,
                                                    items);
                }
                return newTree;
            }
        }
    }
}
