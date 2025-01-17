﻿using Cadastre.Files.Templates;
using System.Collections;
using System.IO;
using System.Windows.Forms.Design;

namespace Cadastre.Files
{
    internal class DynamicHash<T> where T : IData<T>
    {
        private TrieNode root;
        private int blockFactor;
        private string fileName;
        private int blockFactorOverflow;
        private string fileNameOverflow;
        private int usedBlocks = 0;
        private int usedBlocksOverflow = 0;
        private int emptyBlock = -1;
        private int emptyOverflowBlock = -1;
        private int maxDepth = 10;
        FileStream mainFile;
        FileStream overflowFile;
        BinaryWriter mainWriter;
        BinaryReader mainReader;
        BinaryWriter overflowWriter;
        BinaryReader overflowReader;

        public DynamicHash(int blockFactor, string fileName, int blocckFactorOverload, string fileNameOverflow)
        {
            this.root = new ExternalTrieNode(null, 0);
            this.blockFactor = blockFactor;
            this.fileName = fileName;
            this.blockFactorOverflow = blocckFactorOverload;
            this.fileNameOverflow = fileNameOverflow;
            mainFile = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
            overflowFile = new FileStream(fileNameOverflow, FileMode.Create, FileAccess.ReadWrite);
            mainReader = new BinaryReader(mainFile);
            overflowReader = new BinaryReader(overflowFile);
            mainWriter = new BinaryWriter(mainFile);
            overflowWriter = new BinaryWriter(overflowFile);
        }

        public DynamicHash(string fileName, string fileNameOverflow, string trieFile, string indexPropertiesFile)
        {
            this.fileName = fileName;
            this.fileNameOverflow = fileNameOverflow;
            //subor
            mainFile = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
            overflowFile = new FileStream(fileNameOverflow, FileMode.Open, FileAccess.ReadWrite);
            mainReader = new BinaryReader(mainFile);
            overflowReader = new BinaryReader(overflowFile);
            overflowWriter = new BinaryWriter(overflowFile);
            mainWriter = new BinaryWriter(mainFile);
            LoadIndex(trieFile, indexPropertiesFile);
        }

        private int GetEmptyBlock()
        {
            int address = emptyBlock;

            if (address == -1)
            {
                address = usedBlocks;
                usedBlocks++;
            }
            else
            {
                Block<T> block = ReadBlock(fileName, address);
                int successor = block.Successor;
                if (successor != -1)
                {
                    block = ReadBlock(fileName, successor);
                    block.Predecessor = -1;
                    WriteBlock(fileName, successor, block);
                }
                emptyBlock = successor;
            }
            return address;
        }
        private int GetEmptyOverflowBlock()
        {
            int address = emptyOverflowBlock;
            if (address == -1)
            {
                address = usedBlocksOverflow;
                usedBlocksOverflow++;
            }
            else
            {
                Block<T> block = ReadBlock(fileNameOverflow, address);
                int successor = block.Successor;
                if (successor != -1)
                {
                    block = ReadBlock(fileNameOverflow, successor);
                    block.Predecessor = -1;
                    WriteBlock(fileNameOverflow, successor, block);
                }
                emptyOverflowBlock = successor;
            }
            return address;
        }
        private ExternalTrieNode FindNode(T item)
        {
            BitArray hash = item.GetHash();
            TrieNode currentNode = this.root;
            while (currentNode.GetType() != typeof(ExternalTrieNode))
            {
                if (hash[currentNode.Depth] == false)
                {
                    currentNode = ((InternalTrieNode)currentNode).LeftSon;
                }
                else
                {
                    currentNode = ((InternalTrieNode)currentNode).RightSon;
                }
            }
            return (ExternalTrieNode)currentNode;
        }
        private void Divide(ExternalTrieNode node)
        {
            InternalTrieNode newParent = new InternalTrieNode(node.Parent, node.Depth);
            int parent = node.whichSon();
            switch (parent)
            {
                case 0:
                    ((InternalTrieNode)newParent.Parent).LeftSon = newParent;
                    break;
                case 1:
                    ((InternalTrieNode)newParent.Parent).RightSon = newParent;
                    break;
                case -1:
                    root = newParent;
                    break;
            }

            Block<T> leftBlock = new Block<T>(blockFactor);
            Block<T> rightBlock = new Block<T>(blockFactor);

            Block<T> oldContent = ReadBlock(fileName, node.Address);
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
            ExternalTrieNode rightSon = new ExternalTrieNode(newParent, node.Depth);
            newParent.RightSon = rightSon;
            rightSon.Parent = newParent;
            if (node.Count > 0)
            {
                WriteBlock(fileName, node.Address, leftBlock);
            }
            else
            {
                rightSon.Address = node.Address;
                node.Address = -1;

            }

            if (rightBlock.ValidCount > 0)
            {
                rightSon.Count = rightBlock.ValidCount;
                if (rightSon.Address == -1)
                {
                    rightSon.Address = GetEmptyBlock();
                }
                WriteBlock(fileName, rightSon.Address, rightBlock);
            }
        }
        private int Merge(ExternalTrieNode node)
        {
            if (node.Parent == null || !((InternalTrieNode)node.Parent).CanMerge(blockFactor))
            {
                return 0;
            }
            int brotherAddress;
            if (node.whichSon() == 1)
            {
                brotherAddress = ((ExternalTrieNode)((InternalTrieNode)node.Parent).LeftSon).Address;
            }
            else
            {
                brotherAddress = ((ExternalTrieNode)((InternalTrieNode)node.Parent).RightSon).Address;
            }
            Block<T> myBlock;
            Block<T> brotherBlock;
            Block<T> mergeBlock = new Block<T>(blockFactor);
            int address = -1;
            if (node.Address != -1)
            {
                myBlock = ReadBlock(fileName, node.Address);
                for (int i = 0; i < myBlock.ValidCount; i++)
                {
                    mergeBlock.AddRecord(myBlock.Records[i]);
                }
                myBlock.ValidCount = 0;
                address = node.Address;
                if (brotherAddress != -1)
                {
                    FreeBlock(myBlock, node.Address, fileName);
                }
            }
            if (brotherAddress != -1)
            {
                brotherBlock = ReadBlock(fileName, brotherAddress);
                for (int i = 0; i < brotherBlock.ValidCount; i++)
                {
                    mergeBlock.AddRecord(brotherBlock.Records[i]);
                }
                brotherBlock.ValidCount = 0;
                address = brotherAddress;
            }
            node.Count = mergeBlock.ValidCount;
            node.Address = address;
            WriteBlock(fileName, address, mergeBlock);


            if (((InternalTrieNode)node.Parent).whichSon() == 0)
            {
                ((InternalTrieNode)((InternalTrieNode)node.Parent).Parent).LeftSon = node;
                node.Parent = (InternalTrieNode)((InternalTrieNode)node.Parent).Parent;

            }
            else if (((InternalTrieNode)node.Parent).whichSon() == 1)
            {
                ((InternalTrieNode)((InternalTrieNode)node.Parent).Parent).RightSon = node;
                node.Parent = (InternalTrieNode)((InternalTrieNode)node.Parent).Parent;
            }
            else
            {
                root = node;
                node.Parent = null;
            }
            node.Depth--;
            return 1;
        }
        public bool Insert(T item)
        {
            ExternalTrieNode destination = FindNode(item);
            while (destination.Count == blockFactor && destination.Depth != maxDepth)
            {
                Divide(destination);
                destination = FindNode(item);
            }

            Block<T> block;
            int address = destination.Address;
            if (address == -1) //ak block neexistuje
            {
                address = GetEmptyBlock();
                destination.Address = address;
                block = new Block<T>(blockFactor);
                block.AddRecord(item);
                destination.Count++;
                WriteBlock(fileName, address, block);
                return true;
            }
            block = ReadBlock(fileName, address);
            if (block.UsedOverflowBlocks == 0 && block.ValidCount < blockFactor)//ak sa zmesti do main blocku
            {
                if (block.AddRecord(item))
                {
                    destination.Count++;
                    WriteBlock(fileName, address, block);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (block.UsedOverflowBlocks == 0) //ak je 0 prepl a main je plny
            {
                if (!block.CheckOriginality(item))
                {
                    return false;
                }
                address = GetEmptyOverflowBlock();
                block.Successor = address;
                block.UsedOverflowBlocks = 1;
                WriteBlock(fileName, destination.Address, block);
                block = new Block<T>(blockFactorOverflow);
                block.AddRecord(item);
                WriteBlock(fileNameOverflow, address, block);
                destination.Count++;
                return true;
            }
            else
            {
                int bestSpot = -1;
                int predecessor = -1;
                int totalOverflowBlocks = block.UsedOverflowBlocks;
                address = block.Successor;
                Block<T> overflowBlock;
                if (!block.CheckOriginality(item))
                {
                    return false;
                }
                while (address != -1)
                {
                    overflowBlock = ReadBlock(fileNameOverflow, address);
                    if (!overflowBlock.CheckOriginality(item))
                    {
                        return false;
                    }
                    if (overflowBlock.ValidCount < blockFactorOverflow && bestSpot == -1)
                    {
                        bestSpot = address;
                    }
                    predecessor = address;
                    address = overflowBlock.Successor;
                }
                if (block.ValidCount < blockFactor) //je miesto v maine
                {
                    if (block.AddRecord(item))
                    {
                        destination.Count++;
                        WriteBlock(fileName, destination.Address, block);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (bestSpot == -1) // nenašiel sa prazdny blok
                {
                    address = GetEmptyOverflowBlock();
                    overflowBlock = new Block<T>(blockFactorOverflow);
                    overflowBlock.Predecessor = predecessor;
                    if (!overflowBlock.AddRecord(item))
                    {
                        return false;
                    }
                    WriteBlock(fileNameOverflow, address, overflowBlock);
                    overflowBlock = ReadBlock(fileNameOverflow, predecessor);
                    overflowBlock.Successor = address;
                    WriteBlock(fileNameOverflow, predecessor, overflowBlock);
                    block.UsedOverflowBlocks++;
                    WriteBlock(fileName, destination.Address, block);
                    destination.Count++;
                    return true;
                }
                else
                {
                    overflowBlock = ReadBlock(fileNameOverflow, bestSpot);
                    if (!overflowBlock.AddRecord(item))
                    {
                        return false;
                    }
                    WriteBlock(fileNameOverflow, bestSpot, overflowBlock);
                    destination.Count++;
                    return true;
                }
            }
        }
        public bool TryInsert(T item)
        {
            ExternalTrieNode destination = FindNode(item);
            Block<T> block;
            int address = destination.Address;
            if (address == -1) //ak block neexistuje
            {
                return true;
            }
            block = ReadBlock(fileName, address);
            if (!block.CheckOriginality(item))//ak sa zmesti do main blocku
            {
                return false;
            }
            if (block.UsedOverflowBlocks == 0) //ak je 0 prepl a main je ok
            {
                return true;
            }
            else
            {
                while (block.Successor != -1)
                {
                    block = ReadBlock(fileNameOverflow, block.Successor);
                    if (!block.CheckOriginality(item))
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        public T FindItem(T item)
        {
            ExternalTrieNode destination = FindNode(item);
            if (destination.Address == -1)
            {
                return default;
            }
            Block<T> block = ReadBlock(fileName, destination.Address);
            T foundItem = block.FindRecord(item);
            if (foundItem != null)
            {
                return foundItem;
            }
            else
            {
                if (block.ValidCount < destination.Count)
                {
                    while (block.Successor != -1)
                    {
                        block = ReadBlock(fileNameOverflow, block.Successor);
                        foundItem = block.FindRecord(item);
                        if (foundItem != null)
                        {
                            return foundItem;
                        }
                    }
                }
                return foundItem;
            }
        }
        public T DeleteItem(T item)
        {
            int address = -1;
            ExternalTrieNode destination = FindNode(item);
            if(destination.Address == -1)
            {
                return default;
            }
            Block<T> block = ReadBlock(fileName, destination.Address);
            int totalBlocks = block.UsedOverflowBlocks + 1;
            T removedItem = block.RemoveRecord(item);
            if (removedItem != null)
            {
                destination.Count--;
                WriteBlock(fileName, destination.Address, block);
                if (destination.Count == 0 && block.Successor == -1)
                {
                    FreeBlock(block, destination.Address, fileName);
                    destination.Address = -1;
                }
                else if (CheckShake(destination.Count, totalBlocks, block.ValidCount))
                {
                    Shake(destination);
                }
                int merge = 1;
                while (merge == 1)
                {
                    merge = Merge(destination);
                }
                return removedItem;
            }
            else
            {
                int mainCount = block.ValidCount;
                if (block.ValidCount < destination.Count)
                {
                    while (block.Successor != -1)
                    {
                        address = block.Successor;
                        block = ReadBlock(fileNameOverflow, block.Successor);
                        removedItem = block.RemoveRecord(item);
                        if (removedItem != null)
                        {
                            break;
                        }
                    }
                    if (removedItem != null)
                    {
                        destination.Count--;
                        WriteBlock(fileNameOverflow, address, block);
                        if (block.ValidCount == 0)
                        {
                            Block<T> helpBlock;
                            if (block.Successor != -1)
                            {
                                helpBlock = ReadBlock(fileNameOverflow, block.Successor);
                                helpBlock.Predecessor = block.Predecessor;
                                WriteBlock(fileNameOverflow, block.Successor, helpBlock);
                            }
                            if (block.Predecessor != -1)
                            {
                                helpBlock = ReadBlock(fileNameOverflow, block.Predecessor);
                                helpBlock.Successor = block.Successor;
                                WriteBlock(fileNameOverflow, block.Predecessor, helpBlock);
                            }
                            helpBlock = ReadBlock(fileName, destination.Address);
                            if (block.Predecessor == -1)
                            {
                                helpBlock.Successor = block.Successor;
                            }
                            helpBlock.UsedOverflowBlocks--;
                            WriteBlock(fileName, destination.Address, helpBlock);
                            FreeBlock(block, address, fileNameOverflow);
                        }
                        else if (CheckShake(destination.Count, totalBlocks, mainCount))
                        {
                            Shake(destination);
                        }
                        int merge = 1;
                        while (merge == 1)
                        {
                            merge = Merge(destination);
                        }
                        return removedItem;
                    }
                }
            }
            return default;
        }
        public bool UpdateItem(T item)
        {
            int address;
            ExternalTrieNode destination = FindNode(item);
            Block<T> block = ReadBlock(fileName, destination.Address);
            T foundItem = block.FindRecord(item);
            if (foundItem != null)
            {
                for (int i = 0; i < block.ValidCount; i++)
                {
                    if (item.Equals(block.Records[i]))
                    {
                        block.Records[i] = item;
                        break;
                    }
                }
                WriteBlock(fileName, destination.Address, block);
                return true;
            }
            else
            {
                if (block.ValidCount < destination.Count)
                {
                    while (block.Successor != -1)
                    {
                        address = block.Successor;
                        block = ReadBlock(fileNameOverflow, block.Successor);
                        foundItem = block.FindRecord(item);
                        if (foundItem != null)
                        {
                            for (int i = 0; i < block.ValidCount; i++)
                            {
                                if (item.Equals(block.Records[i]))
                                {
                                    block.Records[i] = item;
                                    break;
                                }
                            }
                            WriteBlock(fileNameOverflow, address, block);
                            return true;
                        }
                    }
                }
                return false;
            }
        }
        private Block<T> ReadBlock(string nameOfFile, int address)
        {
            if (nameOfFile == fileName)
            {
                Block<T> block = new Block<T>(blockFactor);
                address = address * block.GetSize();
                mainFile.Seek(address, SeekOrigin.Begin);
                block.FromByteArray(mainReader.ReadBytes(block.GetSize()));
                return block;
            }
            else
            {
                Block<T> block = new Block<T>(blockFactorOverflow);
                address = address * block.GetSize();
                overflowFile.Seek(address, SeekOrigin.Begin);
                block.FromByteArray(overflowReader.ReadBytes(block.GetSize()));
                return block;
            }
        }
        private void WriteBlock(string nameOfFile, int address, Block<T> block)
        {
            address = address * block.GetSize();
            if (nameOfFile == fileName)
            {
                mainFile.Seek(address, SeekOrigin.Begin);
                mainWriter.Write(block.ToByteArray());
            }
            else
            {
                overflowFile.Seek(address, SeekOrigin.Begin);
                overflowWriter.Write(block.ToByteArray());
            }
        }
        private bool CheckShake(int totalItems, int totalBlocks, int mainItems)
        {
            if (totalBlocks == 1)
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
                    int overflowItems = totalItems - mainItems;
                    if (Math.Ceiling((double)overflowItems / (double)blockFactorOverflow) < totalBlocks - 1)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        private void FreeBlock(Block<T> block, int address, string file)
        {
            if (file == fileName)
            {
                if (address == usedBlocks - 1)
                {
                    usedBlocks--;
                    bool empty = true;
                    Block<T> helpBlock;
                    while(empty && usedBlocks != 0)
                    {
                        block = ReadBlock(fileName, usedBlocks - 1);
                        if(block.ValidCount == 0)
                        {
                            usedBlocks--;
                            address -= 1;
                            if(block.Predecessor != -1)
                            {
                                helpBlock = ReadBlock(fileName, block.Predecessor);
                                helpBlock.Successor = block.Successor;
                                WriteBlock(fileName, block.Predecessor, helpBlock);
                            }
                            else
                            {
                                emptyBlock = block.Successor;
                            }
                            if(block.Successor != -1)
                            {
                                helpBlock = ReadBlock(fileName, block.Successor);
                                helpBlock.Predecessor = block.Predecessor;
                                WriteBlock(fileName, block.Successor, helpBlock);
                            }
                        }
                        else
                        {
                            empty = false;
                        }
                    }
                    mainFile.SetLength(address * block.GetSize());
                }
                else
                {
                    block.Successor = emptyBlock;
                    block.Predecessor = -1;
                    emptyBlock = address;
                    if (block.Successor != -1)
                    {
                        Block<T> helpBlock = ReadBlock(fileName, block.Successor);
                        helpBlock.Predecessor = address;
                        WriteBlock(fileName, block.Successor, helpBlock);
                    }
                    WriteBlock(fileName, address, block);
                }
            }
            else
            {
                if (address == usedBlocksOverflow - 1)
                {
                    usedBlocksOverflow--;
                    bool empty = true;
                    Block<T> helpBlock;
                    while (empty && usedBlocksOverflow != 0)
                    {
                        block = ReadBlock(fileNameOverflow, usedBlocksOverflow - 1);
                        if (block.ValidCount == 0)
                        {
                            usedBlocksOverflow--;
                            address -= 1;
                            if (block.Predecessor != -1)
                            {
                                helpBlock = ReadBlock(fileNameOverflow, block.Predecessor);
                                helpBlock.Successor = block.Successor;
                                WriteBlock(fileNameOverflow, block.Predecessor, helpBlock);
                            }
                            else
                            {
                                emptyOverflowBlock = block.Successor;
                            }
                            if (block.Successor != -1)
                            {
                                helpBlock = ReadBlock(fileNameOverflow, block.Successor);
                                helpBlock.Predecessor = block.Predecessor;
                                WriteBlock(fileNameOverflow, block.Successor, helpBlock);
                            }
                        }
                        else
                        {
                            empty = false;
                        }
                    }
                    overflowFile.SetLength(address * block.GetSize());
                }
                else
                {
                    block.Successor = emptyOverflowBlock;
                    block.Predecessor = -1;
                    emptyOverflowBlock = address;
                    if (block.Successor != -1)
                    {
                        Block<T> helpBlock = ReadBlock(fileNameOverflow, block.Successor);
                        helpBlock.Predecessor = address;
                        WriteBlock(fileNameOverflow, block.Successor, helpBlock);
                    }
                    WriteBlock(fileNameOverflow, address, block);
                }
            }
        }
        private void Shake(ExternalTrieNode node)
        {
            Block<T> mainBlock = ReadBlock(fileName, node.Address);
            int deleteAddress = mainBlock.Successor;
            Block<T> blockToDelete = ReadBlock(fileNameOverflow, mainBlock.Successor);
            int helpBlockAddress = blockToDelete.Successor;
            if (mainBlock.ValidCount < blockFactor)
            {
                while (mainBlock.ValidCount < blockFactor && blockToDelete.ValidCount != 0)
                {
                    mainBlock.AddRecord(blockToDelete.Records[blockToDelete.ValidCount - 1]);
                    blockToDelete.RemoveRecord(blockToDelete.Records[blockToDelete.ValidCount - 1]);
                }
            }
            if (blockToDelete.ValidCount != 0)
            {
                while (blockToDelete.ValidCount != 0)
                {
                    Block<T> helpBlock = ReadBlock(fileNameOverflow, helpBlockAddress);
                    while (helpBlock.ValidCount < blockFactorOverflow && blockToDelete.ValidCount != 0)
                    {
                        helpBlock.AddRecord(blockToDelete.Records[blockToDelete.ValidCount - 1]);
                        blockToDelete.RemoveRecord(blockToDelete.Records[blockToDelete.ValidCount - 1]);
                    }
                    WriteBlock(fileNameOverflow, helpBlockAddress, helpBlock);
                    helpBlockAddress = helpBlock.Successor;
                }
            }
            if (blockToDelete.Successor != -1)
            {
                Block<T> helpBlock = ReadBlock(fileNameOverflow, blockToDelete.Successor);
                helpBlock.Predecessor = -1;
                WriteBlock(fileNameOverflow, blockToDelete.Successor, helpBlock);
            }
            mainBlock.Successor = blockToDelete.Successor;
            mainBlock.UsedOverflowBlocks--;
            WriteBlock(fileName, node.Address, mainBlock);
            FreeBlock(blockToDelete, deleteAddress, fileNameOverflow);
        }
        public void SaveIndex(string fileName, string indexProperties)
        {
            List<string> list = new List<string>();
            string filePath = Path.Combine(Application.StartupPath, fileName);
            Stack<TrieNode> stack = new Stack<TrieNode>();
            TrieNode currentNode = root;
            ExternalTrieNode externalTrieNode;
            while (currentNode != null || stack.Count != 0)
            {
                while (currentNode != null)
                {
                    if (currentNode.GetType() == typeof(ExternalTrieNode))
                    {
                        externalTrieNode = (ExternalTrieNode)currentNode;
                        list.Add("1;" + externalTrieNode.Depth + ";" + externalTrieNode.Address + ";" + externalTrieNode.Count + ";");
                        currentNode = null;
                    }
                    else
                    {
                        list.Add("0;" + currentNode.Depth + ";");
                        stack.Push(currentNode);
                        currentNode = ((InternalTrieNode)currentNode).LeftSon;
                    }
                }
                if (stack.Count != 0)
                {
                    currentNode = stack.Pop();
                    currentNode = ((InternalTrieNode)currentNode).RightSon;
                }
            }
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    writer.WriteLine(list[i]);
                }
            }

            filePath = Path.Combine(Application.StartupPath, indexProperties);
            using (StreamWriter textWriter = new StreamWriter(filePath))
            {
                textWriter.WriteLine(blockFactor);
                textWriter.WriteLine(usedBlocks);
                textWriter.WriteLine(emptyBlock);
                textWriter.WriteLine(blockFactorOverflow);
                textWriter.WriteLine(usedBlocksOverflow);
                textWriter.WriteLine(emptyOverflowBlock);
            }
        }
        public void LoadIndex(string fileName, string indexPropertiesName)
        {
            string filePath = Path.Combine(Application.StartupPath, fileName);
            int type;
            int depth;
            int address;
            int count;
            int lineNumber = 0;
            Stack<TrieNode> stack = new Stack<TrieNode>();
            TrieNode currentNode = null;
            TrieNode newNode = null;
            using (StreamReader reader = new StreamReader(filePath))
            {
                lineNumber++;
                var line = reader.ReadLine();
                var values = line.Split(';');
                type = int.Parse(values[0]);
                depth = int.Parse(values[1]);
                if (type == 0)
                {
                    newNode = new InternalTrieNode(null, depth);
                    root = newNode;
                }
                else
                {
                    address = int.Parse(values[2]);
                    count = int.Parse(values[3]);
                    newNode = new ExternalTrieNode(null, depth);
                    ((ExternalTrieNode)newNode).Count = count;
                    ((ExternalTrieNode)newNode).Address = address;
                    root = newNode;
                    return;
                }
                stack.Push(newNode);
                currentNode = newNode;

                while (!reader.EndOfStream)
                {
                    lineNumber++;
                    line = reader.ReadLine();
                    values = line.Split(';');
                    type = int.Parse(values[0]);
                    if (type == 0)
                    {
                        depth = int.Parse(values[1]);
                        newNode = new InternalTrieNode(currentNode, depth);
                        if (((InternalTrieNode)currentNode).LeftSon == null)
                        {
                            ((InternalTrieNode)currentNode).LeftSon = newNode;
                            stack.Push(newNode);
                            currentNode = newNode;
                        }
                        else if (((InternalTrieNode)currentNode).RightSon == null)
                        {
                            ((InternalTrieNode)currentNode).RightSon = newNode;
                            stack.Push(newNode);
                            currentNode = newNode;
                        }
                    }
                    else
                    {
                        depth = int.Parse(values[1]);
                        address = int.Parse(values[2]);
                        count = int.Parse(values[3]);
                        newNode = new ExternalTrieNode(currentNode, depth);
                        ((ExternalTrieNode)newNode).Count = count;
                        ((ExternalTrieNode)newNode).Address = address;
                        if (((InternalTrieNode)currentNode).LeftSon == null)
                        {
                            ((InternalTrieNode)currentNode).LeftSon = newNode;
                        }
                        else
                        {
                            if (currentNode.Depth == 31)
                            {
                                int i = 5;
                            }
                            ((InternalTrieNode)currentNode).RightSon = newNode;
                            while (((InternalTrieNode)currentNode).RightSon != null)
                            {
                                if(stack.Count == 0)
                                {
                                    break;
                                }
                                currentNode = stack.Pop();
                                
                            }
                        }
                    }
                }
            }
            filePath = Path.Combine(Application.StartupPath, indexPropertiesName);
            using (StreamReader reader = new StreamReader(filePath))
            {
                blockFactor = int.Parse(reader.ReadLine());
                usedBlocks = int.Parse(reader.ReadLine());
                emptyBlock = int.Parse(reader.ReadLine());
                blockFactorOverflow = int.Parse(reader.ReadLine());
                usedBlocksOverflow = int.Parse(reader.ReadLine());
                emptyOverflowBlock = int.Parse(reader.ReadLine());
            }
        }
        public string[] FileExtract()
        {
            string[] content = new string[usedBlocks * (blockFactor + 5) + usedBlocksOverflow * (blockFactorOverflow + 5) + 9];
            int stringPointer = 0;
            byte[] bytes = null;
            Block<T> block = new Block<T>(blockFactor);
            content[stringPointer] = "Empty Block: " + emptyBlock;
            content[stringPointer + 1] = "Used blocks: " + usedBlocks;
            stringPointer += 2;
            for (int i = 0; i < usedBlocks; i++)
            {
                content[stringPointer] = "Block number: " + i;
                stringPointer++;
                block = ReadBlock(fileName, i);
                content[stringPointer] = block.ExtractPrefix();
                stringPointer++;
                for (int j = 0; j < blockFactor; j++)
                {
                    content[stringPointer] = block.ExtractItem(j);
                    stringPointer++;
                }
            }

            content[stringPointer] = "---------------------------------";
            stringPointer++;
            content[stringPointer] = "Overflow Block";
            stringPointer++;
            content[stringPointer] = "---------------------------------";
            stringPointer++;
            content[stringPointer] = "Empty Block: " + emptyOverflowBlock;
            content[stringPointer + 1] = "Used blocks: " + usedBlocksOverflow;
            stringPointer += 2;
            for (int i = 0; i < usedBlocksOverflow; i++)
            {
                content[stringPointer] = "Block number: " + i;
                stringPointer++;
                block = ReadBlock(fileNameOverflow, i);
                content[stringPointer] = block.ExtractPrefix();
                stringPointer++;
                for (int j = 0; j < blockFactorOverflow; j++)
                {
                    content[stringPointer] = block.ExtractItem(j);
                    stringPointer++;
                }
            }

            return content;
        }

        
    }
}
