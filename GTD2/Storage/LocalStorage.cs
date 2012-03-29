using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MyToDoList.Storage
{
    public class LocalStorage : IStorage
    {
        const string FilePath = "data.xml";

        public byte[] GetFile()
        {
            return File.ReadAllBytes(FilePath);
        }

        public void SaveFile(byte[] file)
        {
            File.WriteAllBytes(FilePath, file);
        }
    }
}
