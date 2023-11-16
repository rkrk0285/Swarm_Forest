using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Coder{
    public static T Decode<T>(byte[] bytes)where T: class{
        if(bytes is null){
            throw new ArgumentNullException();
        }
           
        var bf = new BinaryFormatter();
        using var ms = new MemoryStream(bytes);
        object obj = bf.Deserialize(ms);

        T deserialized = obj as T ?? throw new InvalidDataException();

        return deserialized;
    }

    public static byte[] Encode<T>(T obj)where T: class{
        if(obj is null){
            throw new ArgumentNullException();
        }

        var bf = new BinaryFormatter();
        using var ms = new MemoryStream();

        bf.Serialize(ms, obj);
        return ms.ToArray();
    }
}