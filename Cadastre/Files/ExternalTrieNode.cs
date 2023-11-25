using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Files.Templates
{
    internal class ExternalTrieNode<T> : TrieNode<T>
    {
        public int Address { get; set; }
        public int Count { get; set; }
        public ExternalTrieNode(TrieNode<T> paParent, int depth) : base(paParent, depth)
        { 
            Count = 0;
        }
    }
}
