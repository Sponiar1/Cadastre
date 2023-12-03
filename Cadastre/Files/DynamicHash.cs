﻿using Cadastre.Files.Templates;
using System.Collections;

namespace Cadastre.Files
{
    internal class DynamicHash<T> where T : IData<T>
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
            this.root = new ExternalTrieNode<T>(null, 0);
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
            this.fileName = fileName;
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
            return 3 * sizeof(int);
        }
        public int GetEmptyBlock()
        {
            int address = -1;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    fs.Seek(2 * sizeof(int), SeekOrigin.Begin);
                    address = reader.ReadInt32();
                }
            }
            if (address == -1)
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
            while (currentNode.GetType() != typeof(ExternalTrieNode<T>))
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
            InternalTrieNode<T> newParent = new InternalTrieNode<T>(node.Parent, node.Depth);
            int parent = node.whichSon();
            switch (parent)
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
            int address = GetPrefixSize() + node.Address * oldContent.GetSize();
            byte[] bytes = ReadBlock(fileName, address, oldContent.GetSize());
            oldContent.FromByteArray(bytes);

            for (int i = 0; i < oldContent.ValidCount; i++)
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
            node.Parent = newParent;
            if (node.Count > 0)
            {
                bytes = leftBlock.ToByteArray();
                address = GetPrefixSize() + node.Address * leftBlock.GetSize();
                WriteBlock(fileName, address, bytes);
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
                        if (emptyBlock != -1)
                        {
                            leftBlock.Successor = emptyBlock;
                            fs.Seek(GetPrefixSize() + emptyBlock * oldContent.GetSize() + oldContent.GetPredeccessorPosition(), SeekOrigin.Begin);
                            writer.Write(node.Address);
                        }
                        fs.Seek(2 * sizeof(int), SeekOrigin.Begin);
                        writer.Write(node.Address); //možno to dať do array
                    }
                }
                bytes = leftBlock.ToByteArray();
                address = GetPrefixSize() + node.Address * leftBlock.GetSize();
                WriteBlock(fileName, address, bytes);
                node.Address = -1;
            }

            ExternalTrieNode<T> rightSon = new ExternalTrieNode<T>(newParent, node.Depth);
            newParent.RightSon = rightSon;
            if (rightBlock.ValidCount > 0)
            {
                rightSon.Count = rightBlock.ValidCount;
                rightSon.Address = GetEmptyBlock();
                if (newBlock)
                {
                    newBlock = false;
                }
                bytes = rightBlock.ToByteArray();
                address = GetPrefixSize() + rightSon.Address * rightBlock.GetSize();
                WriteBlock(fileName, address, bytes);
            }
            else
            {
                rightSon.Address = -1;
            }
        }
        public bool Insert(T item)
        {
            int address;
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
                if (!newBlock)
                {
                    bytes = ReadBlock(fileName, GetPrefixSize() + address * block.GetSize(), block.GetSize());
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
                bytes = block.ToByteArray();
                WriteBlock(fileName, GetPrefixSize() + address * block.GetSize(), bytes);
                return true;
            }
            else
            {
                Block<T> fileBlock = new Block<T>(blockFactor);
                Block<T> overflowblock = new Block<T>(blockFactorOverflow);
                byte[] bytes;

                bytes = ReadBlock(fileName, GetPrefixSize() + destination.Address * fileBlock.GetSize(), fileBlock.GetSize());
                fileBlock.FromByteArray(bytes);
                if (!fileBlock.CheckOriginality(item))
                {
                    return false;
                }

                int totalBlocks = fileBlock.UsedOverflowBlocks;
                int successorAddress = fileBlock.Successor;

                if (successorAddress == -1) //ak nemá overflow block
                {
                    address = GetEmptyOverflowBlock();
                    fileBlock.UsedOverflowBlocks++;
                    fileBlock.Successor = address;
                    overflowblock.AddRecord(item);
                    overflowblock.UsedOverflowBlocks++;
                    WriteBlock(fileName, GetPrefixSize() + destination.Address * fileBlock.GetSize(), fileBlock.ToByteArray());
                    WriteBlock(fileNameOverflow, GetPrefixSize() + address * overflowblock.GetSize(), overflowblock.ToByteArray());

                    destination.Count++;
                    return true;
                }
                else
                {
                    int bestSpot = -1;
                    int predecessor = -1;
                    while (successorAddress != -1)
                    {
                        bytes = ReadBlock(fileNameOverflow, GetPrefixSize() + successorAddress * overflowblock.GetSize(), overflowblock.GetSize());
                        overflowblock.FromByteArray(bytes);
                        if (!overflowblock.CheckOriginality(item))
                        {
                            return false;
                        }
                        if (overflowblock.ValidCount < blockFactorOverflow && bestSpot == -1)
                        {
                            bestSpot = successorAddress;
                        }
                        predecessor = successorAddress;
                        successorAddress = overflowblock.Successor;
                    }
                    if (bestSpot == -1) //ak sa nezmestí do existujúcich
                    {
                        int overflowAddress = GetEmptyOverflowBlock();
                        overflowblock = new Block<T>(blockFactorOverflow);
                        overflowblock.AddRecord(item);
                        overflowblock.Predecessor = predecessor;
                        overflowblock.UsedOverflowBlocks = totalBlocks + 1;
                        WriteBlock(fileNameOverflow, GetPrefixSize() + overflowAddress * overflowblock.GetSize(), overflowblock.ToByteArray());

                        //update počet preplnujucich blokov
                        using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Write))
                        {
                            using (BinaryWriter writer = new BinaryWriter(fs))
                            {
                                fs.Seek(GetPrefixSize() + destination.Address * fileBlock.GetSize() + fileBlock.GetUsedBlocksPosition(), SeekOrigin.Begin);
                                writer.Write(totalBlocks + 1);
                            }
                        }

                        if (predecessor != -1) // predchodzovi odkaz na noveho successora
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
                        bytes = ReadBlock(fileNameOverflow, GetPrefixSize() + bestSpot * overflowblock.GetSize(), overflowblock.GetSize());
                        overflowblock.FromByteArray(bytes);
                        overflowblock.AddRecord(item);
                        WriteBlock(fileNameOverflow, GetPrefixSize() + bestSpot * overflowblock.GetSize(), overflowblock.ToByteArray());
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
            address = GetPrefixSize() + destination.Address * block.GetSize();
            bytes = ReadBlock(fileName, address, block.GetSize());
            block.FromByteArray(bytes);
            T foundItem = block.FindRecord(item);
            if (foundItem != null)
            {
                return foundItem;
            }
            else
            {
                if (block.ValidCount < destination.Count)
                {
                    Block<T> overflowBlock = new Block<T>(blockFactorOverflow);
                    int successor = block.Successor;
                    using (FileStream fs = new FileStream(fileNameOverflow, FileMode.Open, FileAccess.Read))
                    {
                        using (BinaryReader reader = new BinaryReader(fs))
                        {
                            while (successor != -1)
                            {
                                fs.Seek(GetPrefixSize() + successor * overflowBlock.GetSize(), SeekOrigin.Begin);
                                bytes = reader.ReadBytes(overflowBlock.GetSize());
                                overflowBlock.FromByteArray(bytes);
                                foundItem = overflowBlock.FindRecord(item);
                                if (foundItem != null)
                                {
                                    return foundItem;
                                }
                                successor = overflowBlock.Successor;
                            }
                        }
                    }
                }
                return foundItem;
            }
        }
        public bool DeleteItem(T item)
        {
            int address;
            int totalBlocks;
            int totalItems;
            ExternalTrieNode<T> destination = Find(item);
            Block<T> block = new Block<T>(blockFactor);
            byte[] bytes;
            address = GetPrefixSize() + destination.Address * block.GetSize();
            bytes = ReadBlock(fileName, address, block.GetSize());
            block.FromByteArray(bytes);

            totalBlocks = block.UsedOverflowBlocks;
            totalItems = destination.Count - 1;

            bool found = block.RemoveRecord(item);
            if (found == true)
            {
                destination.Count--;
                WriteBlock(fileName, address, block.ToByteArray());
                if (totalItems == 0)
                {
                    FreeBlock(destination.Address, block, fileName);
                    destination.Address = -1;
                }
                else if (CheckSpace(totalItems, totalBlocks, block.ValidCount))
                {
                    ShakeOff(destination);
                }
                return true;
            }
            else
            {
                if (block.ValidCount < destination.Count)
                {
                    Block<T> overflowBlock = new Block<T>(blockFactorOverflow);
                    int successor = block.Successor;
                    using (FileStream fs = new FileStream(fileNameOverflow, FileMode.Open, FileAccess.Read))
                    {
                        using (BinaryReader reader = new BinaryReader(fs))
                        {
                            while (successor != -1)
                            {
                                fs.Seek(GetPrefixSize() + successor * overflowBlock.GetSize(), SeekOrigin.Begin);
                                bytes = reader.ReadBytes(overflowBlock.GetSize());
                                overflowBlock.FromByteArray(bytes);
                                found = overflowBlock.RemoveRecord(item);
                                if (found == true)
                                {
                                    break;
                                }
                                successor = overflowBlock.Successor;
                            }
                        }
                    }
                    if (found == true) // fixnut ked predchodza je main block 105-120
                    {
                        destination.Count--;
                        WriteBlock(fileNameOverflow, GetPrefixSize() + successor * overflowBlock.GetSize(), overflowBlock.ToByteArray());
                        if (overflowBlock.ValidCount == 0)
                        {
                            if (overflowBlock.Successor != -1)
                            {
                                if (overflowBlock.Predecessor != -1)
                                {
                                    using (FileStream fs = new FileStream(fileNameOverflow, FileMode.Open, FileAccess.Write))
                                    {
                                        using (BinaryWriter writer = new BinaryWriter(fs))
                                        {
                                            fs.Seek(GetPrefixSize() + overflowBlock.Successor * overflowBlock.GetSize() + overflowBlock.GetPredeccessorPosition(), SeekOrigin.Begin);
                                            writer.Write(BitConverter.GetBytes(overflowBlock.Predecessor));
                                        }
                                    }
                                }
                                else
                                {
                                    using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Write))
                                    {
                                        using (BinaryWriter writer = new BinaryWriter(fs))
                                        {
                                            fs.Seek(GetPrefixSize() + destination.Address * block.GetSize() + block.GetSuccessorPosition(), SeekOrigin.Begin);
                                            writer.Write(BitConverter.GetBytes(overflowBlock.Successor));
                                        }
                                    }
                                }
                            }
                            if(overflowBlock.Predecessor != -1)
                            {
                                using (FileStream fs = new FileStream(fileNameOverflow, FileMode.Open, FileAccess.Write))
                                {
                                    using (BinaryWriter writer = new BinaryWriter(fs))
                                    {
                                        fs.Seek(GetPrefixSize() + overflowBlock.Predecessor * overflowBlock.GetSize() + overflowBlock.GetSuccessorPosition(), SeekOrigin.Begin);
                                        writer.Write(BitConverter.GetBytes(overflowBlock.Successor));
                                    }
                                }
                            }
                            else
                            {
                                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Write))
                                {
                                    using (BinaryWriter writer = new BinaryWriter(fs))
                                    {
                                        fs.Seek(GetPrefixSize() + destination.Address * block.GetSize() + block.GetSuccessorPosition(), SeekOrigin.Begin);
                                        writer.Write(BitConverter.GetBytes(overflowBlock.Successor));
                                    }
                                }
                            }
                            FreeBlock(successor, overflowBlock, fileNameOverflow);
                        }
                        else if (CheckSpace(totalItems, totalBlocks, block.ValidCount))
                        {
                            ShakeOff(destination);
                        }
                        return true;
                    }
                }
            }
            return false;
        }
        private void ShakeOff(ExternalTrieNode<T> node)
        {
            bool shaken = false;
            Block<T> mainBlock = new Block<T>(blockFactor);
            byte[] bytes = ReadBlock(fileName, GetPrefixSize() + node.Address * mainBlock.GetSize(), mainBlock.GetSize());
            mainBlock.FromByteArray(bytes);
            int successor = mainBlock.Successor;
            List<Block<T>> overflowList = new List<Block<T>>();
            List<int> addressList = new List<int>();
            Block<T> blockToDestroy = new Block<T>(blockFactorOverflow);
            int minimal = int.MaxValue;
            int minimalAddress = -1;
            while (successor != -1)
            {
                Block<T> overflowBlock = new Block<T>(blockFactorOverflow);
                overflowBlock.FromByteArray(ReadBlock(fileNameOverflow, GetPrefixSize() + successor * overflowBlock.GetSize(), overflowBlock.GetSize()));
                overflowList.Add(overflowBlock);
                addressList.Add(successor);
                if (minimal > overflowBlock.ValidCount)
                {
                    minimal = overflowBlock.ValidCount;
                    blockToDestroy = overflowBlock;
                    minimalAddress = successor;
                }
                successor = overflowBlock.Successor;
            }
            if (mainBlock.ValidCount < blockFactor)
            {
                while (mainBlock.ValidCount != blockFactor && blockToDestroy.ValidCount != 0)
                {
                    mainBlock.AddRecord(blockToDestroy.Records[blockToDestroy.ValidCount - 1]);
                    blockToDestroy.RemoveRecord(blockToDestroy.Records[blockToDestroy.ValidCount - 1]);
                }
                WriteBlock(fileName, node.Address, mainBlock.ToByteArray());
            }
            int block = 0;
            while (blockToDestroy.ValidCount != 0)
            {
                if (addressList[block] != minimalAddress && overflowList[block].ValidCount < blockFactorOverflow)
                {
                    while (overflowList[block].ValidCount < blockFactorOverflow && blockToDestroy.ValidCount != 0)
                    {
                        overflowList[block].AddRecord(blockToDestroy.Records[blockToDestroy.ValidCount - 1]);
                        blockToDestroy.RemoveRecord(blockToDestroy.Records[blockToDestroy.ValidCount - 1]);
                    }
                    WriteBlock(fileNameOverflow, addressList[block], overflowList[block].ToByteArray());
                }
                block++;
            }
            if (blockToDestroy.Successor != -1)
            {
                using (FileStream fs = new FileStream(fileNameOverflow, FileMode.Open, FileAccess.Write))
                {
                    using (BinaryWriter writer = new BinaryWriter(fs))
                    {
                        fs.Seek(GetPrefixSize() + blockToDestroy.Successor * blockToDestroy.GetSize() + blockToDestroy.GetPredeccessorPosition(), SeekOrigin.Begin);
                        writer.Write(blockToDestroy.Predecessor);
                    }
                }
            }
            if (blockToDestroy.Predecessor != -1)
            {
                using (FileStream fs = new FileStream(fileNameOverflow, FileMode.Open, FileAccess.Write))
                {
                    using (BinaryWriter writer = new BinaryWriter(fs))
                    {
                        fs.Seek(GetPrefixSize() + blockToDestroy.Predecessor * blockToDestroy.GetSize() + blockToDestroy.GetSuccessorPosition(), SeekOrigin.Begin);
                        writer.Write(blockToDestroy.Successor);
                    }
                }
            }
            FreeBlock(minimalAddress, blockToDestroy, fileNameOverflow);

        }
        private void FreeBlock(int address, Block<T> block, string file)
        {
            int successor;
            if (file == fileName)
            {
                if (address == usedBlocks - 1)
                {
                    usedBlocks--;
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Write))
                    {
                        fs.SetLength(GetPrefixSize() + address * block.GetSize());
                    }
                }
                else
                {
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        using (BinaryReader reader = new BinaryReader(fs))
                        {
                            fs.Seek(2 * sizeof(int), SeekOrigin.Begin);
                            successor = reader.ReadInt32();
                        }
                    }
                    block.Successor = successor;
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Write))
                    {
                        using (BinaryWriter writer = new BinaryWriter(fs))
                        {
                            fs.Seek(2 * sizeof(int), SeekOrigin.Begin);
                            writer.Write(BitConverter.GetBytes(address));
                            if (successor != -1)
                            {
                                fs.Seek(GetPrefixSize() + successor * block.GetSize() + block.GetPredeccessorPosition(), SeekOrigin.Begin);
                                writer.Write(BitConverter.GetBytes(address));
                            }
                        }
                    }
                    WriteBlock(fileName, GetPrefixSize() + address * block.GetSize(), block.ToByteArray());
                }
            }
            else
            {
                if (address == usedBlocksOverflow - 1) //dorobiť ak je uprostred zreťazenia?
                {
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Write))
                    {
                        fs.SetLength(GetPrefixSize() + address * block.GetSize());
                    }
                    usedBlocksOverflow--;
                }
                else
                {
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        using (BinaryReader reader = new BinaryReader(fs))
                        {
                            fs.Seek(2 * sizeof(int), SeekOrigin.Begin);
                            successor = reader.ReadInt32();
                        }
                    }
                    block.Successor = successor;
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Write))
                    {
                        using (BinaryWriter writer = new BinaryWriter(fs))
                        {
                            fs.Seek(2 * sizeof(int), SeekOrigin.Begin);
                            writer.Write(BitConverter.GetBytes(address));
                            if (successor != -1)
                            {
                                fs.Seek(GetPrefixSize() + successor * block.GetSize() + block.GetPredeccessorPosition(), SeekOrigin.Begin);
                                writer.Write(BitConverter.GetBytes(address));
                            }
                        }
                    }
                    block.Predecessor = -1;
                    WriteBlock(file, GetPrefixSize() + address * block.GetSize(), block.ToByteArray());
                }
            }
        }
        private bool CheckSpace(int totalItems, int overFlowBlocks, int mainFileItems)
        {
            if (overFlowBlocks == 0)
            {
                return false;
            }
            else
            {
                if (totalItems == blockFactor)
                {
                    return true;
                }
                else
                {
                    int overflowItems = totalItems - mainFileItems;
                    if (Math.Ceiling((double)overflowItems / (double)blockFactorOverflow) < overFlowBlocks)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public byte[] ReadBlock(string nameOfFile, int address, int size)
        {
            byte[] bytes;
            using (FileStream fs = new FileStream(nameOfFile, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    fs.Seek(address, SeekOrigin.Begin);
                    bytes = reader.ReadBytes(size);
                }
            }
            return bytes;
        }
        public void WriteBlock(string nameOfFile, int address, byte[] bytes)
        {
            using (FileStream fs = new FileStream(nameOfFile, FileMode.Open, FileAccess.Write))
            {
                using (BinaryWriter writer = new BinaryWriter(fs))
                {
                    fs.Seek(address, SeekOrigin.Begin);
                    writer.Write(bytes);
                }
            }
        }
        public void Save(string fileName)
        {

        }
        public void Load(string fileName)
        {

        }
        public string[] FileExtract()
        {
            string[] content = new string[usedBlocks * (blockFactor + 5) + usedBlocksOverflow * (blockFactorOverflow + 5) + 6];
            int stringPointer = 0;
            byte[] bytes = null;
            Block<T> block = new Block<T>(blockFactor);
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    content[stringPointer] = reader.ReadInt32().ToString(); //blockFaktor
                    content[stringPointer + 1] = reader.ReadInt32().ToString(); //usedBlocks
                    content[stringPointer + 2] = reader.ReadInt32().ToString(); //emptyBlok
                    stringPointer = 3;
                    for (int i = 0; i < usedBlocks; i++)
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
