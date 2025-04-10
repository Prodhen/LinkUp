using System;
using System.Security.Cryptography;
using System.Text;

namespace API.Helper;

public static class AuthHelper
{
    public static bool VerifyPasswordHash(byte[] passwordHash, byte[] passwordSalt, string password)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        if (computedHash.Length != passwordHash.Length)
            return false;

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != passwordHash[i])
                return false;
        }

        return true;
    }
}
