using Cadastre.Files.Templates;
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
        private bool newBlock;
        private int maxDepth = 32;

        public DynamicHash(int blockFactor, string fileName, int blocckFactorOverload, string fileNameOverflow)
        {
            this.root = new ExternalTrieNode<T>(null,0);
            this.blockFactor = blockFactor;
            this.fileName = fileName;
            this.blockFactorOverflow = blocckFactorOverload;
            this.fileNameOverflow = fileNameOverflow;
            this.newBlock = false;
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

        public DynamicHash(string fileName, string fileNameOverflow, string trieFile)
        {
            this.fileName= fileName;
            this.fileNameOverflow = fileNameOverflow;
            this.newBlock = false;
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

        private int GetPrefixSize()
        {
            return 3*sizeof(int);
        }
        public int GetEmptyBlock()
        {
            int address = -1;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    fs.Seek(2*sizeof(int), SeekOrigin.Begin);

                    address = reader.ReadInt32();
                }
            }
            if(address == -1)
            {
                address = usedBlocks;
                usedBlocks++;
                newBlock = true;
            }
            else
            {
                int successorAddress = -1;
                Block<T> block = new Block<T>(blockFactor);
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        fs.Seek(GetPrefixSize() + address * block.GetSize() + block.GetSuccessorPosition(), SeekOrigin.Begin);

                        successorAddress = reader.ReadInt32();
                    }
                }
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Write))
                {
                    fs.Seek(2 * sizeof(int), SeekOrigin.Begin);
                    using (BinaryWriter writer = new BinaryWriter(fs))
                    {
                        writer.Write(successorAddress);
                    }
                }
            }
            return address;
        }
        public int GetEmptyOverflowBlock()
        {
                int address = -1;
                using (FileStream fs = new FileStream(fileNameOverflow, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        fs.Seek(2 * sizeof(int), SeekOrigin.Begin);

                        address = reader.ReadInt32();
                    }
                }
                if (address == -1)
                {
                    address = usedBlocksOverflow;
                    usedBlocksOverflow++;
                    newBlock = true;
                }
                else
                {
                    int successorAddress = -1;
                    Block<T> block = new Block<T>(blockFactorOverflow);
                    using (FileStream fs = new FileStream(fileNameOverflow, FileMode.Open, FileAccess.Read))
                    {
                        using (BinaryReader reader = new BinaryReader(fs))
                        {
                            fs.Seek(GetPrefixSize() + address * block.GetSize() + block.GetSuccessorPosition(), SeekOrigin.Begin);

                            successorAddress = reader.ReadInt32();
                        }
                    }
                    using (FileStream fs = new FileStream(fileNameOverflow, FileMode.Open, FileAccess.Write))
                    {
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
                    root = newParent;
                    break;
            }

            Block<T> oldContent = new Block<T>(blockFactor);
            Block<T> leftBlock = new Block<T>(blockFactor);
            Block<T> rightBlock = new Block<T>(blockFactor);

            byte[] bytes;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
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
                
            }

            node.Count = leftBlock.ValidCount;
            node.Depth++;
            newParent.LeftSon = node;
            node.Parent = newParent;
            if (node.Count > 0)
            {
                bytes = leftBlock.ToByteArray();
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Write))
                {
                    using (BinaryWriter writer = new BinaryWriter(fs))
                    {
                        fs.Seek(GetPrefixSize() + node.Address * leftBlock.GetSize(), SeekOrigin.Begin);
                        writer.Write(bytes);
                    }
                }
            }
            else
            {
                int emptyBlock;
                //pridať ho do zratazenia prazdnych adries
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader reader = new BinaryReader(fs))
                    {

                        fs.Seek(0, SeekOrigin.Begin);
                        reader.ReadInt32();
                        reader.ReadInt32();
                        emptyBlock = reader.ReadInt32();
                        
                    }
                }
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Write))
                {
                    using (BinaryWriter writer = new BinaryWriter(fs))
                    {
                        if (emptyBlock == -1)
                        {
                            leftBlock.Successor = emptyBlock;
                        }
                        fs.Seek(2 * sizeof(int), SeekOrigin.Begin);
                        writer.Write(node.Address); //možno to dať do array
                    }
                }
            }

            ExternalTrieNode<T> rightSon = new ExternalTrieNode<T>(newParent, node.Depth);
            newParent.RightSon = rightSon;
            if(rightBlock.ValidCount > 0)
            {
                rightSon.Count = rightBlock.ValidCount;
                rightSon.Address = GetEmptyBlock();
                if(newBlock)
                {
                    newBlock = false;
                }
                bytes = rightBlock.ToByteArray();
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Write))
                {
                    using (BinaryWriter writer = new BinaryWriter(fs))
                    {
                        fs.Seek(GetPrefixSize() + rightSon.Address * rightBlock.GetSize(), SeekOrigin.Begin);
                        writer.Write(bytes);
                    }
                }
            }
            else
            {
                rightSon.Address = -1;
            }
        }
        public bool Insert(T item)
        {
            int address = -1;
            ExternalTrieNode<T> destination = Find(item);
            if (destination.Count == blockFactor && destination.Depth != maxDepth)
            {
                while (destination.Count == blockFactor && destination.Depth != maxDepth)
                {
                    Divide(destination);
                    destination = Find(item);
                }
            }
            destination.Count++;
            if (destination.Count <= blockFactor)
            {
                address = destination.Address;
                if (address == -1)
                {
                    address = GetEmptyBlock();
                    destination.Address = address;
                }
                Block<T> block = new Block<T>(blockFactor);
                byte[] bytes;
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        if (!newBlock)
                        {
                            fs.Seek(GetPrefixSize() + address * block.GetSize(), SeekOrigin.Begin);
                            bytes = reader.ReadBytes(block.GetSize());
                            block.FromByteArray(bytes);
                        }
                        else
                        {
                            newBlock = false;
                        }
                        if (!block.AddRecord(item))
                        {
                            return false;
                        }
                    }
                }
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Write))
                {
                    bytes = block.ToByteArray();
                    using (BinaryWriter writer = new BinaryWriter(fs))
                    {
                        writer.Seek(GetPrefixSize() + address * block.GetSize(), SeekOrigin.Begin);
                        writer.Write(bytes, 0, bytes.Length);
                    }
                }
                return true;
            }
            else
            {
                Block<T> fileBlock = new Block<T>(blockFactor);
                Block<T> overflowblock = new Block<T>(blockFactorOverflow);
                byte[] bytes;
                int successorAddress = -1;
                int totalBlocks = 0;
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        fs.Seek(GetPrefixSize() + destination.Address * fileBlock.GetSize(), SeekOrigin.Begin);
                        bytes = reader.ReadBytes(fileBlock.GetSize());
                    }
                }
                fileBlock.FromByteArray(bytes);
                if(!fileBlock.CheckOriginality(item))
                {
                    return false;
                }
                totalBlocks = fileBlock.UsedOverflowBlocks;
                successorAddress = fileBlock.Successor;
                if (successorAddress == -1)
                {
                    address = GetEmptyOverflowBlock();
                    fileBlock.UsedOverflowBlocks++;
                    fileBlock.Successor = address;
                    overflowblock.AddRecord(item);
                    overflowblock.UsedOverflowBlocks++;
                    using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Write))
                    {
                        using (BinaryWriter writer = new BinaryWriter(fs))
                        {
                            fs.Seek(GetPrefixSize() + destination.Address * fileBlock.GetSize(), SeekOrigin.Begin);
                            writer.Write(fileBlock.ToByteArray());
                        }
                    }
                    using (FileStream fs = new FileStream(fileNameOverflow, FileMode.Open, FileAccess.Write))
                    {
                        using (BinaryWriter writer = new BinaryWriter(fs))
                        {
                            fs.Seek(GetPrefixSize() + address * overflowblock.GetSize(), SeekOrigin.Begin);
                            writer.Write(overflowblock.ToByteArray());
                        }
                    }
                    destination.Count++;
                    return true;
                }
                else
                {
                    bool foundSpot = false;
                    bool original = true;
                    int bestSpot = -1;
                    int count=0;
                    int dummy=0;
                    int predecessor = -1;
                    int successor = successorAddress;
                    using (FileStream fs = new FileStream(fileNameOverflow, FileMode.Open, FileAccess.Read))
                    {
                        using (BinaryReader reader = new BinaryReader(fs))
                        {
                            while(successorAddress != -1)
                            {
                                fs.Seek(GetPrefixSize() + successorAddress * overflowblock.GetSize(), SeekOrigin.Begin);
                                bytes = reader.ReadBytes(overflowblock.GetSize());
                                overflowblock.FromByteArray(bytes);
                                if(!overflowblock.CheckOriginality(item))
                                {
                                    return false;
                                }
                                if(overflowblock.ValidCount < blockFactorOverflow && bestSpot == -1)
                                {
                                    bestSpot = successorAddress;
                                }
                                predecessor = successorAddress;
                                successorAddress = overflowblock.Successor;
                            }
                        }
                    }
                    if(bestSpot == -1)
                    {
                        int overflowAddress = GetEmptyOverflowBlock();
                        overflowblock = new Block<T>(blockFactorOverflow);
                        overflowblock.AddRecord(item);
                        overflowblock.Predecessor = predecessor;
                        overflowblock.UsedOverflowBlocks = totalBlocks + 1;
                        using (FileStream fs = new FileStream(fileNameOverflow, FileMode.Open, FileAccess.Write))
                        {
                            using (BinaryWriter writer = new BinaryWriter(fs))
                            {
                                fs.Seek(GetPrefixSize() + overflowAddress * overflowblock.GetSize(), SeekOrigin.Begin);
                                writer.Write(overflowblock.ToByteArray());
                            }
                        }
                        using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Write))
                        {
                            using (BinaryWriter writer = new BinaryWriter(fs))
                            {
                                fs.Seek(GetPrefixSize() + overflowAddress * fileBlock.GetSize() + fileBlock.GetUsedBlocksPosition(), SeekOrigin.Begin);
                                writer.Write(totalBlocks + 1);
                            }
                        }
                        if (predecessor != -1)
                        {
                            using (FileStream fs = new FileStream(fileNameOverflow, FileMode.Open, FileAccess.Write))
                            {
                                using (BinaryWriter writer = new BinaryWriter(fs))
                                {
                                    fs.Seek(GetPrefixSize() + predecessor * overflowblock.GetSize() + overflowblock.GetSuccessorPosition(), SeekOrigin.Begin);
                                    writer.Write(overflowAddress);
                                }
                            }
                        }
                        destination.Count++;
                        return true;
                    }
                    else
                    {
                        using (FileStream fs = new FileStream(fileNameOverflow, FileMode.Open, FileAccess.Read))
                        {
                            using (BinaryReader reader = new BinaryReader(fs))
                            {
                                fs.Seek(GetPrefixSize() + bestSpot * overflowblock.GetSize(), SeekOrigin.Begin);
                                bytes = reader.ReadBytes(overflowblock.GetSize());
                            }
                        }
                        overflowblock.FromByteArray(bytes);
                        overflowblock.AddRecord(item);
                        using (FileStream fs = new FileStream(fileNameOverflow, FileMode.Open, FileAccess.Write))
                        {
                            using (BinaryWriter writer = new BinaryWriter(fs))
                            {
                                fs.Seek(GetPrefixSize() + bestSpot * overflowblock.GetSize(), SeekOrigin.Begin);
                                writer.Write(overflowblock.ToByteArray());
                            }
                        }
                    }
                    destination.Count++;
                    return true;
                }
            }
        }
        public T FindItem(T item)
        {
            int address;
            ExternalTrieNode<T> destination = Find(item);
            Block<T> block = new Block<T>(blockFactor);
            byte[] bytes;
            address = destination.Address;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                        fs.Seek(GetPrefixSize() + address * block.GetSize(), SeekOrigin.Begin);
                        bytes = reader.ReadBytes(block.GetSize());
                        block.FromByteArray(bytes);
                }
            }
            T foundItem = block.FindRecord(item);
            if(foundItem != null)
            {
                return foundItem;
            }
            else
            {
                if(block.ValidCount < destination.Count)
                {
                    Block<T> overflowBlock = new Block<T>(blockFactorOverflow);
                    using (FileStream fs = new FileStream(fileNameOverflow, FileMode.Open, FileAccess.Read))
                    {
                        using (BinaryReader reader = new BinaryReader(fs))
                        {
                            fs.Seek(GetPrefixSize() + block.GetSuccessorPosition() * overflowBlock.GetSize(), SeekOrigin.Begin);
                            while(block.Successor != -1)
                            {
                                bytes = reader.ReadBytes(overflowBlock.GetSize());
                                block.FromByteArray(bytes);
                                foundItem = block.FindRecord(item);
                                if(foundItem != null)
                                {
                                    return foundItem;
                                }
                                fs.Seek(GetPrefixSize() + overflowBlock.GetSuccessorPosition() * overflowBlock.GetSize(), SeekOrigin.Begin);
                            }
                        }
                    }
                }
                return foundItem;
            }
        }
        public bool DeleteItem(T item)
        {
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
        public string[] FileExtract()
        {
            string[] content = new string[usedBlocks*(blockFactor+5) + usedBlocksOverflow*(blockFactorOverflow+5) + 6];
            int stringPointer = 0;
            byte[] bytes = null;
            Block<T> block = new Block<T>(blockFactor);
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    content[stringPointer] = reader.ReadInt32().ToString(); //blockFaktor
                    content[stringPointer+1] = reader.ReadInt32().ToString(); //usedBlocks
                    content[stringPointer+2] = reader.ReadInt32().ToString(); //emptyBlok
                    stringPointer = 3;
                    for(int i = 0; i < usedBlocks; i++)
                    {
                        content[stringPointer] = "Block number: " + i;
                        stringPointer++;
                        bytes = reader.ReadBytes(block.GetSize());
                        block.FromByteArray(bytes);
                        content[stringPointer] = block.ExtractPrefix();
                        stringPointer++;
                        for (int j = 0; j < blockFactor; j++)
                        {
                            content[stringPointer] = block.ExtractItem(j);
                            stringPointer++;
                        }
                    }

                }
            }
            content[stringPointer] = "---------------------------------";
            stringPointer++; 
            content[stringPointer] = "Overflow Block";
            stringPointer++; 
            content[stringPointer] = "---------------------------------";
            stringPointer++;
            block = new Block<T>(blockFactorOverflow);
            using (FileStream fs = new FileStream(fileNameOverflow, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    content[stringPointer] = reader.ReadInt32().ToString(); //blockFaktor
                    content[stringPointer + 1] = reader.ReadInt32().ToString(); //usedBlocks
                    content[stringPointer + 2] = reader.ReadInt32().ToString(); //emptyBlok
                    stringPointer += 3;
                    for (int i = 0; i < usedBlocksOverflow; i++)
                    {
                        content[stringPointer] = "Block number: " + i;
                        stringPointer++;
                        bytes = reader.ReadBytes(block.GetSize());
                        block.FromByteArray(bytes);
                        content[stringPointer] = block.ExtractPrefix();
                        stringPointer++;
                        for (int j = 0; j < blockFactorOverflow; j++)
                        {
                            content[stringPointer] = block.ExtractItem(j);
                            stringPointer++;
                        }
                    }

                }
            }
            return content;
        }
    }
}
