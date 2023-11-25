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
    }
}
