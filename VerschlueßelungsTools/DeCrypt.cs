using Logging;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace VerschlueßelungsTools
{
    public class DeCrypt
    {
        private const int keysize = 256;
        private readonly string _SecretKey = "!HashP2020MKFidb";
        private string _PublicKey;
        private string _Text;
        private string _Result;

        protected string PublicKey
        {
            get { return _PublicKey; }
            set { _PublicKey = value; }
        }
        public string Text
        {
            get { return _Text; }
            set
            {
                _Text = value;
            }
        }
        public string Result
        {
            get
            {
                if (Text != null)
                {
                    Decrypt();
                    return _Result;
                }
                else
                    throw new Exception("Ohne Text kein Entschlüßeln");
            }
            private set
            {
                _Result = value;
            }
        }

        public DeCrypt(string publickey = "Helena2014!")
        {
            PublicKey = publickey;
        }

        private void Decrypt()
        {
            try
            {
                byte[] initVectorBytes = Encoding.UTF8.GetBytes(_SecretKey);
                byte[] cipherTextBytes = Convert.FromBase64String(Text);
                PasswordDeriveBytes password = new PasswordDeriveBytes(PublicKey, null);
                byte[] keyBytes = password.GetBytes(keysize / 8);
                RijndaelManaged symmetricKey = new RijndaelManaged
                {
                    Mode = CipherMode.CBC
                };
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                Result = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

            }
            catch (Exception ex)
            {
                EventLog.WriteEventLog(EventTyp.Error, ex.Message);
            }
        }
    }
}
