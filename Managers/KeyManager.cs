using System.IO;
using System.Security.Cryptography;

namespace GreenThumb.Managers
{
    internal static class KeyManager
    {
        //Returns the key as a string. If it doesnt exist it is created
        public static string getEncryptionKey()
        {
            string keyLocation = Path.Combine(Directory.GetCurrentDirectory(), "Key.txt");
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

        //Generates a random key string
        private static string generateEncryptionKey()
        {
            var rng = new RNGCryptoServiceProvider();
            var byteArray = new byte[16];
            rng.GetBytes(byteArray);
            return Convert.ToBase64String(byteArray);

        }
    }
}
