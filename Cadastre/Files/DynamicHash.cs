﻿using Cadastre.Files.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Files
{
    internal class DynamicHash<T>
    {
        private TrieNode<T> root;
        private int blockFactor;
        private string fileName;
        private int blockFactorOverflow;
        private string fileNameOverflow;

        public DynamicHash(int blockFactor, string fileName)
        {
            this.root = new InternalTrieNode<T>(null);
            this.blockFactor = blockFactor;
            this.fileName = fileName;
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
            }
        }
    }
}
