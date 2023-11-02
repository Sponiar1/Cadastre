using Cadastre.DataStructure.Templates;
using System.Collections;

namespace Cadastre.DataStructure
{
    public class QuadTreeNode<T>
    {
        public List<T> Items { get; }
        public QuadTreeNode<T>[] sons { get; set; }
        public QuadTreeNode<T> parent { get; set; }
        public QuadTreeRectangle Zone { get; }
        public Boolean isLeaf { get; set; }
        public int Height { get; set; }
        public QuadTreeNode(QuadTreeRectangle zone)
        {
            Items = new List<T>();
            sons = new QuadTreeNode<T>[4];
            Zone = zone;
            isLeaf = true;
        }

        public void insert(T item)
        {
            Items.Add(item);
        }

        public T remove(T item)
        {
            Items.Remove(item);
            return item;
        }

        public int getNumberOfItems()
        {
            return Items.Count;
        }

        public void createSons()
        {
            if (sons[0] == null)
            {
                sons[0] = new QuadTreeNode<T>(new QuadTreeRectangle(Zone.BottomLeftX, (Zone.UpperRightY - Zone.BottomLeftY) / 2 + Zone.BottomLeftY,
                                                    (Zone.UpperRightX - Zone.BottomLeftX) / 2 + Zone.BottomLeftX, Zone.UpperRightY));
                sons[0].Height = Height + 1;
                sons[0].parent = this;
                sons[1] = new QuadTreeNode<T>(new QuadTreeRectangle((Zone.UpperRightX - Zone.BottomLeftX) / 2 + Zone.BottomLeftX, (Zone.UpperRightY - Zone.BottomLeftY) / 2 + Zone.BottomLeftY,
                                                    Zone.UpperRightX, Zone.UpperRightY));
                sons[1].Height = Height + 1;
                sons[1].parent = this;
                sons[2] = new QuadTreeNode<T>(new QuadTreeRectangle((Zone.UpperRightX - Zone.BottomLeftX) / 2 + Zone.BottomLeftX, Zone.BottomLeftY,
                                                        Zone.UpperRightX, (Zone.UpperRightY - Zone.BottomLeftY) / 2 + Zone.BottomLeftY));
                sons[2].Height = Height + 1;
                sons[2].parent = this;
                sons[3] = new QuadTreeNode<T>(new QuadTreeRectangle(Zone.BottomLeftX, Zone.BottomLeftY,
                                                    (Zone.UpperRightX - Zone.BottomLeftX) / 2 + Zone.BottomLeftX, (Zone.UpperRightY - Zone.BottomLeftY) / 2 + Zone.BottomLeftY));
                sons[3].Height = Height + 1;
                sons[3].parent = this;
            }
        }
        public void deleteSons()
        {
            isLeaf = true;
            sons[0] = null;
            sons[1] = null;
            sons[2] = null;
            sons[3] = null;
        }
    }
}
