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
        int count;
        public ExternalTrieNode(TrieNode<T> paParent) : base(paParent)
        { 

        }
    }
}
