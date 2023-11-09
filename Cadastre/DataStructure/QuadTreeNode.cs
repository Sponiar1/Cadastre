using Cadastre.DataStructure.Templates;
using System.Collections;

namespace Cadastre.DataStructure
{
    public class QuadTreeNode<T>
    {
        public List<T> Items { get; }
        public QuadTreeNode<T>[] sons { get; private set; }
        public QuadTreeNode<T> parent { get; private set; }
        public QuadTreeRectangle Zone { get; }
        public Boolean IsLeaf { get; set; }
        public int Height { get; set; }

        public QuadTreeNode(QuadTreeRectangle zone)
        {
            Items = new List<T>();
            sons = new QuadTreeNode<T>[4];
            Zone = zone;
            IsLeaf = true;
        }
        public void Insert(T item)
        {
            Items.Add(item);
        }
        public T Remove(T item)
        {
            Items.Remove(item);
            return item;
        }
        public void CreateSons()
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
        public void DeleteSons()
        {
            IsLeaf = true;
            sons[0] = null;
            sons[1] = null;
            sons[2] = null;
            sons[3] = null;
        }
    }
}
