﻿using System;
using System.ComponentModel;
using System.IO.Compression;
using IO = System.IO;

namespace Conesoft.Files
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class Zip : File, IDisposable
    {
        readonly ZipArchive zip;

        internal Zip(File zipFile) : base(zipFile)
        {
            zip = new(new IO.FileStream(Path, IO.FileMode.OpenOrCreate), ZipArchiveMode.Update, false);
        }

        public byte[] this[string name]
        {
            set
            {
                using var file = zip.CreateEntry(name).Open();
                using var writer = new IO.BinaryWriter(file);
                writer.Write(value);
            }
            get
            {
                using var file = zip.GetEntry(name).Open();
                using var memory = new IO.MemoryStream();
                file.CopyTo(memory);
                return memory.ToArray();
            }
        }

        public void Dispose()
        {
            zip.Dispose();
        }
    }
}
