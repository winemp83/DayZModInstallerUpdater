using Logging;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace VerschlueßelungsTools
{
    public class EntCrypt
    {
        private const int keysize = 256;
        private readonly string _SecretKey = "!HashP2020MKFidb";
        private string _PublicKey = "";
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
                    Encrypt();
                    return _Result;
                }
                else
                    throw new Exception("Ohne Text kein Verschlüßeln");
            }
            private set
            {
                _Result = value;
            }
        }

        public EntCrypt (string publickey = "Helena2014!")
        {
            PublicKey = publickey;
        }
        private void Encrypt()
        {
            try
            {
                byte[] initVectorBytes = Encoding.UTF8.GetBytes(_SecretKey);
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(Text);
                PasswordDeriveBytes password = new PasswordDeriveBytes(PublicKey, null);
                byte[] keyBytes = password.GetBytes(keysize / 8);
                RijndaelManaged symmetricKey = new RijndaelManaged
                {
                    Mode = CipherMode.CBC
                };
                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                byte[] cipherTextBytes = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();
                Result = Convert.ToBase64String(cipherTextBytes);

            }
            catch (Exception ex)
            {
                EventLog.WriteEventLog(EventTyp.Error,ex.Message);
            }
        }
    }
}
