//hash oid to 6 digits (can be duplicate)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DoAn.Module.Controllers.Global
{
    static class HashOIDtoDigits
    {
        static public  int init(string oid)
        {
            using var sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(oid));
            int hashInt = BitConverter.ToInt32(hashBytes, 0);

            // Ensure it's positive and only 6 digits
            return Math.Abs(hashInt) % 900000 + 100000;
        }
    }
}
