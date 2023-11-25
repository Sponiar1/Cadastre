using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Files.Templates
{
    internal class TrieNode<T>
    {
        protected TrieNode<T> parent;
        public int Depth { get; set; }
        public TrieNode(TrieNode<T> paParent, int depth)
        {
            parent = paParent;
            Depth = depth;
        }
    }
}
