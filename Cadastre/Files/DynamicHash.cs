﻿using Cadastre.Files.Templates;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Files
{
    internal class DynamicHash<T> where T: IData<T>
    {
        private TrieNode<T> root;
        private int blockFactor;
        private string fileName;
        private int blockFactorOverflow;
        private string fileNameOverflow;
        private int usedBlocks;
        private int usedBlocksOverflow;

        public DynamicHash(int blockFactor, string fileName, int blocckFactorOverload, string fileNameOverflow)
        {
            this.root = new ExternalTrieNode<T>(null,0);
            this.blockFactor = blockFactor;
            this.fileName = fileName;
            this.blockFactorOverflow = blocckFactorOverload;
            this.fileNameOverflow = fileNameOverflow;

            //subor
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                using (BinaryWriter writer = new BinaryWriter(fs))
                {
                    writer.Write(blockFactor);  //block faktor
                    writer.Write(0);            //použité bloky
                    writer.Write(-1);            //prazdny blok
                }
            }
            //preplnovaci subor
            using (FileStream fs = new FileStream(fileNameOverflow, FileMode.Create, FileAccess.Write))
            {
                using (BinaryWriter writer = new BinaryWriter(fs))
                {
                    writer.Write(blockFactorOverflow);  //block faktor
                    writer.Write(0);            //použité bloky
                    writer.Write(-1);            //prazdny blok
                }
            }
        }

        public DynamicHash(string fileName, string fileNameOverflow)
        {
            this.fileName= fileName;
            this.fileNameOverflow = fileNameOverflow;
            //subor
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    fs.Seek(0, SeekOrigin.Begin);

                    blockFactor = reader.ReadInt32();
                    usedBlocks = reader.ReadInt32();
                }
            }
            //preplnovaci subor
            using (FileStream fs = new FileStream(fileNameOverflow, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    fs.Seek(0, SeekOrigin.Begin);

                    blockFactorOverflow = reader.ReadInt32();
                    usedBlocksOverflow = reader.ReadInt32();
                }
            }
        }

        private int  GetPrefixSize()
        {
            return 3*sizeof(int);
        }

        public int GetEmptyBlock()
        {
            int address = -1;
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    fs.Seek(2*sizeof(int), SeekOrigin.Begin);

                    address = reader.ReadInt32();
                }
            }
            if(address == -1)
            {
                address = usedBlocks + 1;
                usedBlocks++;
            }
            else
            {
                int successorAddress = -1;
                Block<T> block = new Block<T>(blockFactor);
                using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        fs.Seek(GetPrefixSize() + address * block.GetSize() + block.GetSuccessorPosition(), SeekOrigin.Begin);

                        successorAddress = reader.ReadInt32();
                    }
                    fs.Seek(2 * sizeof(int), SeekOrigin.Begin);
                    using (BinaryWriter writer = new BinaryWriter(fs))
                    {
                        writer.Write(successorAddress);
                    }
                }
            }
            return address;
        }

        public ExternalTrieNode<T> Find(T item)
        {
            BitArray hash = item.GetHash();
            TrieNode<T> currentNode = this.root;
            while(currentNode.GetType() != typeof(ExternalTrieNode<T>))
            {
                InternalTrieNode<T> currentInternal = (InternalTrieNode<T>)currentNode;
                if (hash[currentNode.Depth] == false)
                {
                    currentNode = currentInternal.LeftSon;
                }
                else
                {
                    currentNode = currentInternal.RightSon;
                }
            }

            ExternalTrieNode<T> currentExternal = (ExternalTrieNode<T>)currentNode;
            return currentExternal;
        }
        public void Divide(ExternalTrieNode<T> node)
        {
            InternalTrieNode<T> newParent = new InternalTrieNode<T>(node.Parent,node.Depth);
            int parent = node.whichSon();
            switch(parent)
            {
                case 0:
                    ((InternalTrieNode<T>)newParent.Parent).LeftSon = newParent;
                    break;
                case 1:
                    ((InternalTrieNode<T>)newParent.Parent).RightSon = newParent;
                    break;
                case -1:
                    break;
            }

            Block<T> oldContent = new Block<T>(blockFactor);
            Block<T> leftBlock = new Block<T>(blockFactor);
            Block<T> rightBlock = new Block<T>(blockFactor);

            byte[] bytes;
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    fs.Seek(GetPrefixSize() + node.Address * oldContent.GetSize(), SeekOrigin.Begin);
                    bytes = reader.ReadBytes(oldContent.GetSize());
                    oldContent.FromByteArray(bytes);
                }
                
                for(int i = 0; i < oldContent.ValidCount; i++)
                {
                    BitArray hash = oldContent.Records[i].GetHash();
                    if (hash[node.Depth] == false)
                    {
                        leftBlock.AddRecord(oldContent.Records[i]);
                    }
                    else
                    {
                        rightBlock.AddRecord(oldContent.Records[i]);
                    }
                }

                node.Count = leftBlock.ValidCount;
                node.Depth++;
                newParent.LeftSon = node;
                bytes = leftBlock.ToByteArray();
                fs.Seek(GetPrefixSize() + node.Address * leftBlock.GetSize(), SeekOrigin.Begin);
                fs.Write(bytes, 0, bytes.Length);

                ExternalTrieNode<T> rightSon = new ExternalTrieNode<T>(newParent, node.Depth);
                newParent.RightSon = rightSon;
                rightSon.Address = GetEmptyBlock();
                bytes = rightBlock.ToByteArray();
                fs.Seek(GetPrefixSize() + rightSon.Address * rightBlock.GetSize(), SeekOrigin.Begin);
                fs.Write(bytes, 0, bytes.Length);

            }

        }
        public bool Insert(T item)
        {
            int address;
            ExternalTrieNode<T> destination = Find(item);
            if(destination.Count == blockFactor)
            {
                while(destination.Count == blockFactor)
                {
                    Divide(destination);
                    destination = Find(item);
                }
            }
                destination.Count++;
                address = destination.Address;
                if (address == -1)
                {
                    address = GetEmptyBlock();
                    destination.Address = address;
                }
                Block<T> block = new Block<T>(blockFactor);
                byte[] bytes;
                using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        fs.Seek(GetPrefixSize() + address * block.GetSize(), SeekOrigin.Begin);
                        bytes = reader.ReadBytes(block.GetSize());
                        block.FromByteArray(bytes);
                    }
                    if(!block.AddRecord(item))
                    {
                        return false;
                    }
                    bytes = block.ToByteArray();

                    fs.Seek(GetPrefixSize() + address * block.GetSize(), SeekOrigin.Begin);
                    fs.Write(bytes, 0, bytes.Length);
                }
            return true;
        }
        public void Save(string fileName)
        {

        }
        public void Load(string fileName)
        {

        }
        public void TestCreateFile()
        {
                string filePath = "test.bin";
                byte[] byteArray = { 1, 33, 45, 87, 50 };

                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    using (BinaryWriter writer = new BinaryWriter(fs))
                    {
                        writer.Write(byteArray);
                    }
                }
        }

        public void TestReadFile()
        {
            /*
            string filePath = "test.bin";

            int startOffset = 2; // 1-based index
            int count = 2;

            // Create a FileStream with FileMode.Open to open an existing file
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                // Create a BinaryReader to read binary data from the file stream
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    // Set the position to the start offset
                    fs.Seek(startOffset - 1, SeekOrigin.Begin);

                    // Read the specified number of bytes
                    byte[] resultBytes = reader.ReadBytes(count);

                    int i = 5;
                }
            }*/

            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    fs.Seek(0, SeekOrigin.Begin);

                    blockFactor = reader.ReadInt32();
                }
            }
            int i = 5;
        }
    }
}
