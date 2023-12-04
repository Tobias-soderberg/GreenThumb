using System.IO;
using System.Security.Cryptography;

namespace GreenThumb.Managers
{
    internal static class KeyManager
    {
        public static string getEncryptionKey()
        {
            string keyLocation = "C:\\Users\\tobbe\\OneDrive\\Skrivbord\\Key.txt";
            string key;
            if (File.Exists(keyLocation))
            {
                key = File.ReadAllText(keyLocation);
                if (string.IsNullOrEmpty(key))
                {
                    key = generateEncryptionKey();
                    File.WriteAllText(keyLocation, key);
                }
            }
            else
            {
                key = generateEncryptionKey();
                File.WriteAllText(keyLocation, key);
            }
            return key;
        }
        private static string generateEncryptionKey()
        {
            var rng = new RNGCryptoServiceProvider();
            var byteArray = new byte[16];
            rng.GetBytes(byteArray);
            return Convert.ToBase64String(byteArray);

        }
    }
}
