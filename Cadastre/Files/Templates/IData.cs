using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Files.Templates
{
    public interface IData<T>: IRecord<T>
    {
        public bool Equals(T obj);
        public BitArray GetHash();
    }
}
