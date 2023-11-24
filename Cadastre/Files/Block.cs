using Cadastre.Files.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Files
{
    internal class Block<T> : IRecord<T> where T : IData<T>
    {
        public List<T> Records {get; set;}
        private int blockFactor;
        private int ValidCount;
        public Block(int paBlockFactor)
        {
            Records = new List<T>(paBlockFactor);
            blockFactor = paBlockFactor;
        }

        public int GetSize()
        {
            return blockFactor * Records[0].GetSize() ;
        }

        public byte[] ToByteArray()
        {
            throw new NotImplementedException();
        }

        public void FromByteArray(byte[] byteArray)
        {
            throw new NotImplementedException();
        }
    }
}
