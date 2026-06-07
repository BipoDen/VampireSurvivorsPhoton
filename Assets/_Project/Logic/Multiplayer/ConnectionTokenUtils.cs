using System;

namespace _Project.Logic.Multiplayer
{
    public static class ConnectionTokenUtils
    {
        public static byte[] NewToken() => Guid.NewGuid().ToByteArray();
        
        public static int HashToken(byte[] token) => new Guid(token).GetHashCode();
        
        public static string TokenToString(byte[] token) => new Guid(token).ToString();
    }
}