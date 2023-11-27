using Cadastre.Files.Templates;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Files
{
    internal class Block<T> : IRecord<T> where T : IData<T>
    {
        public List<T> Records {get; set;}
        private int blockFactor;
        public int ValidCount { get; set;}
        private int usedOverflowBlocks;
        private int successor;
        private int predecessor;
        public Block(int paBlockFactor)
        {
            Records = new List<T>(paBlockFactor);
            blockFactor = paBlockFactor;
            for (int i = 0; i < blockFactor; i++)
            {
                T dummyInstance = Activator.CreateInstance<T>();
                Records.Add(dummyInstance.CreateInstance());
            }
        }
        public int GetSize()
        {
            return 4*sizeof(int) + blockFactor * Records[0].GetSize();
        }

        public byte[] ToByteArray()
        {
            byte[] bytes = new byte[GetSize()];
            int totalLength = 0;

            byte[] validArray = BitConverter.GetBytes(ValidCount);
            Array.Copy(validArray, 0, bytes, 0, validArray.Length);
            totalLength += validArray.Length;

            byte[] overFlowArray = BitConverter.GetBytes(usedOverflowBlocks);
            Array.Copy(overFlowArray, 0, bytes, totalLength, overFlowArray.Length);
            totalLength += overFlowArray.Length;

            byte[] successorArray = BitConverter.GetBytes(successor);
            Array.Copy(successorArray, 0, bytes, totalLength, successorArray.Length);
            totalLength += successorArray.Length;

            byte[] predecessorArray = BitConverter.GetBytes(predecessor);
            Array.Copy(predecessorArray, 0, bytes, totalLength, predecessorArray.Length);
            totalLength += predecessorArray.Length;

            for(int i = 0; i < blockFactor; i++)
            {
                byte[] recordBytes= Records[i].ToByteArray();
                Array.Copy(recordBytes, 0, bytes, totalLength, recordBytes.Length);
                totalLength += Records[i].GetSize();
            }

            return bytes;
        }

        public void FromByteArray(byte[] byteArray)
        {
            int offset = 0;
            ValidCount = BitConverter.ToInt32(byteArray, offset);
            offset += sizeof(int);

            usedOverflowBlocks = BitConverter.ToInt32(byteArray, offset);
            offset += sizeof(int);

            successor = BitConverter.ToInt32(byteArray, offset);
            offset += sizeof(int);

            predecessor = BitConverter.ToInt32(byteArray, offset);
            offset += sizeof(int);


            for(int i = 0; i < blockFactor; i++)
            {
                byte[] recordsArray = new byte[byteArray.Length - offset];
                Array.Copy(byteArray, offset, recordsArray, 0, byteArray.Length - offset);

                Records[i].FromByteArray(byteArray);
                offset += Records[i].GetSize();
            }

        }

        public int GetSuccessorPosition()
        {
            return 2 * sizeof(int);
        }
        public int GetPredeccessorPosition()
        {
            return 3 * sizeof(int);
        }

        public bool AddRecord(T item)
        {
            for(int i = 0; i < ValidCount; i++)
            {
                if (Records[i].Equals(item))
                {
                    return false;
                }
            }
            Records[ValidCount] = item;
            ValidCount++;
            if (Records[1] == null)
            {
                for(int i = 1; i < blockFactor; i++)
                {
                    Records[i] = Records[0].CreateInstance();
                }
            }
            return true;
        }
        public bool RemoveRecord(T item)
        {
            for (int i = 0; i < ValidCount; i++)
            {
                if (Records[i].Equals(item))
                {
                    T removedItem = Records[i];
                    Records[i] = Records[ValidCount - 1];
                    Records[ValidCount - 1] = removedItem;
                    break;
                }
            }
            ValidCount--;
            return true;
        }
        public T FindRecord(T item)
        {
            for (int i = 0; i < ValidCount; i++)
            {
                if (Records[i].Equals(item))
                {
                    return Records[i];
                }
            }
            T foundRecord = default;
            return foundRecord;
        }

        public string ExtractPrefix()
        {
            return "Valid Count:" + ValidCount + ", Used Overflow Blocks: " + usedOverflowBlocks + ", Successor Address: " + successor + ", Predecessor Address: " + predecessor;
        }
        public string ExtractItem(int index)
        {
            return Records[index].ExtractInfo();
        }
    }
}
