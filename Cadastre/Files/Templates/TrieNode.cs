using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Files.Templates
{
    internal class TrieNode<T>
    {
        public TrieNode<T> Parent {get; private set;}
        public int Depth { get; set; }
        public TrieNode(TrieNode<T> paParent, int depth)
        {
            Parent = paParent;
            Depth = depth;
        }

    }
}
