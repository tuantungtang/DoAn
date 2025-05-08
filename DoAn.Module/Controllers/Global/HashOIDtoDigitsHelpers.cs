using System.Security.Cryptography;
using System.Text;

namespace DoAn.Module.Controllers.Global
{
    internal static class HashOIDtoDigitsHelpers
    {
        static public int HashOIDtoDigits(string oid)
        {
            using var sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(oid));
            int hashInt = BitConverter.ToInt32(hashBytes, 0);

            // Ensure it's positive and only 6 digits
            return Math.Abs(hashInt) % 900000 + 100000;
        }
    }
}