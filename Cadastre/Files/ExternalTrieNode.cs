using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Files.Templates
{
    internal class ExternalTrieNode : TrieNode
    {
        public int Address { get; set; }
        public int Count { get; set; }
        public ExternalTrieNode(TrieNode paParent, int depth) : base(paParent, depth)
        { 
            Count = 0;
            Address = -1;
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
