using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Files.Templates
{
    internal class InternalTrieNode<T> : TrieNode<T>
    {
        public TrieNode<T> LeftSon { get; set; }
        public TrieNode<T> RightSon { get; set; }
        public InternalTrieNode(TrieNode<T> paParent, int depth) : base(paParent, depth)
        {

        }

        public bool CanMerge(int blockFactor)
        {
            if(LeftSon == null || LeftSon.GetType() == typeof(InternalTrieNode<T>) /*|| ((ExternalTrieNode<T>)LeftSon).Address == -1*/) 
                return false;
            if(RightSon == null || RightSon.GetType() == typeof(InternalTrieNode<T>) /*|| ((ExternalTrieNode<T>)RightSon).Address == -1*/) 
                return false;
            if(((ExternalTrieNode<T>)LeftSon).Count + ((ExternalTrieNode<T>)RightSon).Count <= blockFactor)
            {
                return true;
            }
            return false;
        }
        public int whichSon()
        {
            if (Parent == null)
            {
                return -1;
            }
            else if (((InternalTrieNode<T>)Parent).LeftSon == this)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}
