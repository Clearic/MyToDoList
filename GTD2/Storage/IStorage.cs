using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyToDoList.Storage
{
    public interface IStorage
	{
        byte[] GetFile();
        void SaveFile(byte[] file);
	}

    public class FileModel
    {
        public byte[] FileContent { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
