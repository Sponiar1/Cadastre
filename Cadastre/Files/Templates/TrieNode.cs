using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Files.Templates
{
    internal class TrieNode<T>
    {
        private TrieNode<T> parent;
        public TrieNode(TrieNode<T> paParent)
        {
            parent = paParent;
        }
    }
}
