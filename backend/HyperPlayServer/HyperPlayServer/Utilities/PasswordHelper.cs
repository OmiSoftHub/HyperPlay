﻿using System.Security.Cryptography;
using System.Text;

namespace HyperPlayServer.Utilities;

public class PasswordHelper
{
    public static string Hash(string password)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(password);
        byte[] inputHash = SHA256.HashData(inputBytes);
        return Encoding.UTF8.GetString(inputHash);
    }
}
