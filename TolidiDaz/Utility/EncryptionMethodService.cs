using System.Security.Cryptography;
using System.Text;


namespace Utility
{
    public class EncryptionMethodService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        // کلید ۳۲ بایتی (برای AES-256) و IV ۱۶ بایتی
        // مهم: این کلید رو در production از appsettings یا Azure Key Vault بگیرید
        private const string SecretKey ="!ASD#@!$$%558%#$%#^%#$!@#gfdgd$#";// دقیقاً ۳۲ کاراکتر
        private const string Salt = "YanpiSalt!@#123"; // برای مشتق‌گیری بهتر

        public EncryptionMethodService()
        {
            using var sha256 = SHA256.Create();
            _key = sha256.ComputeHash(Encoding.UTF8.GetBytes(SecretKey + Salt));
            _iv = sha256.ComputeHash(Encoding.UTF8.GetBytes("FixedIVString123")); // ۱۶ بایت اول
            Array.Resize(ref _iv, 16);
        }

        public string EncryptString(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var writer = new StreamWriter(cs))
            {
                writer.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        public string? DecryptString(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;

            try
            {
                var cipherBytes = Convert.FromBase64String(cipherText);

                using var aes = Aes.Create();
                aes.Key = _key;
                aes.IV = _iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using var ms = new MemoryStream(cipherBytes);
                using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                using var reader = new StreamReader(cs);

                return reader.ReadToEnd();
            }
            catch
            {
                // اگر کلید اشتباه باشد یا داده خراب، null برگردان
                return null;
            }
        }
    }
}