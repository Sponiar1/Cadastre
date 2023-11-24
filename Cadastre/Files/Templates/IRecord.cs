using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Files.Templates
{
    public interface IRecord<T>
    {
        public int GetSize();
        public byte[] ToByteArray();
        public void FromByteArray(byte[] byteArray);
    }
}
