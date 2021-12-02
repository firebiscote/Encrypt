using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Encrypt
{
    public class Crypto
    {
        public List<byte> FileBytes { get; }
        public List<byte> EncryptedFileBytes { get; }
        public string TargetDirectory { get; }
        private ulong key = 0b_1100_1001_0000_0000_0000_0000_0001_0001_1100_1001_0000_0000_0000_0000_0001_0001;

        public Crypto(string sourceFile, string targetFile)
        {
            FileBytes = File.ReadAllBytes(sourceFile).ToList();
            EncryptedFileBytes = new List<byte>();
            TargetDirectory = targetFile + sourceFile[(sourceFile.LastIndexOf('\\')+1)..sourceFile.LastIndexOf('.')] + ".crypt";
            Encrypt();
            Save();
        }

        public void Encrypt()
        {
            byte[] bits = new byte[64];
            while (FileBytes.Count % 64 != 0)
                FileBytes.Add(0x20);
            for (int i = 0; i < FileBytes.Count; i += 64)
            {
                Array.Copy(FileBytes.ToArray(), i, bits, 0, 64);
                EncryptedFileBytes.AddRange(BitConverter.GetBytes(BitConverter.ToUInt64(bits) ^ key));
            }
        }

        public void Save()
        {
            File.WriteAllBytes(TargetDirectory, EncryptedFileBytes.ToArray());
        }
    }
}
