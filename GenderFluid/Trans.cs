using System.Text;

namespace GenderFluid;

public static class Trans
{
    public static Entry[] ReadTransFile(Stream stream)
    {
        BEBinaryReader reader = new(stream);

        //Read the entry count
        uint entryCount = reader.ReadUInt32();

        //Allocate the final entries size
        Entry[] entries = new Entry[entryCount];
        for (uint i = 0; i < entryCount; i++)
        {
            //Read each entry info in
            entries[i] = new Entry
            {
                Key = reader.ReadUInt32(),
                StringOffset = reader.ReadUInt32(),
            };
        }

        //Get the position right after the header
        long afterHeaderPos = entries.Length * 8 + 4;

        //Temp buffer to read into
        byte[] read = new byte[2];
        for (uint i = 0; i < entryCount; i++)
        {
            //Seek to the start of the string (after the BOM)
            reader.BaseStream.Position = entries[i].StringOffset + afterHeaderPos + 2;
            long length = 0;
            //While we haven't reached a BOM, increment the string length
            while (stream.ReadAtLeast(read, 2, false) == 2 && read[0] != 0xFE && read[1] != 0xFF)
            {
                length += 1;
            }

            //Seek back to the start of the string (after the BOM)
            reader.BaseStream.Position = entries[i].StringOffset + afterHeaderPos + 2;
            
            //Allocate the data that will store the string
            byte[] str = new byte[length * 2];
            //Read the string into the buffer
            stream.ReadExactly(str);

            //Read the contents as a UTF-16 BE string
            entries[i].Contents = Encoding.BigEndianUnicode.GetString(str);
        }

        return entries;
    }

    public static void WriteTransFile(Entry[] entries, Stream stream)
    {
        BEBinaryWriter writer = new(stream);
        
        //Write the entry count
        writer.Write((uint)entries.Length);
        
        using MemoryStream contentsStream = new();
        BEBinaryWriter contentsWriter = new(contentsStream);

        for (int i = 0; i < entries.Length; i++)
        {
            entries[i].StringOffset = (uint)contentsStream.Length;

            //Write the BOM
            contentsWriter.Write((byte)0xFE);
            contentsWriter.Write((byte)0xFF);
            
            //Write the string
            contentsWriter.Write(Encoding.BigEndianUnicode.GetBytes(entries[i].Contents));
        }

        foreach (Entry entry in entries)
        {
            writer.Write(entry.Key);
            writer.Write(entry.StringOffset);
        }
        
        stream.Write(contentsStream.ToArray());
        stream.Flush();
    }
}   