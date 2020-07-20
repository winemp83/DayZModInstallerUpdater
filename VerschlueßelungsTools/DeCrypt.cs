using Logging;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Models;

namespace VerschlueßelungsTools
{
    public class DeCrypt
    {
        private readonly Models.Crypt _Cyper;

        public string Set
        {
            set { _Cyper.Value = value; }
        }
        public string Result
        {
            get { return _Cyper.Result; }
        }
        
        public DeCrypt(string Value = null)
        {
            _Cyper = new Models.Crypt();
            if (Value != null || Value.Length > 0)
            {
                _Cyper.Value = Value;
                Decrypt();
            }
        }

        private void Decrypt()
        {
            try
            {
                byte[] initVectorBytes = Encoding.UTF8.GetBytes(_Cyper.Secret);
                byte[] cipherTextBytes = Convert.FromBase64String(_Cyper.Value);
                PasswordDeriveBytes password = new PasswordDeriveBytes(_Cyper.Password, null);
                byte[] keyBytes = password.GetBytes(_Cyper.Keysize / 8);
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
                _Cyper.Result = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

            }
            catch (Exception ex)
            {
                EventLog.WriteEventLog(EventTyp.Error, ex.Message);
            }
        }
    }
}
