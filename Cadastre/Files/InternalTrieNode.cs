using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Files.Templates
{
    internal class InternalTrieNode : TrieNode
    {
        public TrieNode LeftSon { get; set; }
        public TrieNode RightSon { get; set; }
        public InternalTrieNode(TrieNode paParent, int depth) : base(paParent, depth)
        {

        }

        public bool CanMerge(int blockFactor)
        {
            if(LeftSon == null || LeftSon.GetType() == typeof(InternalTrieNode) /*|| ((ExternalTrieNode<T>)LeftSon).Address == -1*/) 
                return false;
            if(RightSon == null || RightSon.GetType() == typeof(InternalTrieNode) /*|| ((ExternalTrieNode<T>)RightSon).Address == -1*/) 
                return false;
            if(((ExternalTrieNode)LeftSon).Count + ((ExternalTrieNode)RightSon).Count <= blockFactor)
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
            else if (((InternalTrieNode)Parent).LeftSon == this)
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
