using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Files.Templates
{
    internal class TrieNode
    {
        public TrieNode Parent {get; set;}
        public int Depth { get; set; }
        public TrieNode(TrieNode paParent, int depth)
        {
            Parent = paParent;
            Depth = depth;
        }

    }
}
