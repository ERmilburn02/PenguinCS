using System;
using System.Security.Cryptography;
using System.Text;

namespace PenguinCS.Common;

public static class Crypto
{
    public static string GenerateRandomKey(uint length = 0)
    {
        var randomBytes = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToHexStringLower(randomBytes);
    }

    public static string Hash(string value)
    {
        var hash = MD5.HashData(Encoding.UTF8.GetBytes(value));
        return Convert.ToHexStringLower(hash);
    }

    public static string Hash(int value) => Hash(value.ToString());

    public static string EncryptPassword(string password, bool alreadyHashed = false)
    {
        if (!alreadyHashed)
        {
            password = Hash(password);
        }

        return password[16..32] + password[0..16];
    }
}