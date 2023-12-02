// using System;
// using System.IO;
// using System.Text;
// using System.Text.Json;

// public class Coder
// {
//     public static T Decode<T>(byte[] bytes) where T : class
//     {
//         if (bytes is null)
//         {
//             throw new ArgumentNullException();
//         }
        
//         T deserialized = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(bytes)) ?? throw new InvalidDataException();

//         return deserialized;
//     }

//     public static byte[] Encode<T>(T obj) where T : class
//     {
//         if (obj is null)
//         {
//             throw new ArgumentNullException();
//         }

//         return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));
//     }
// }