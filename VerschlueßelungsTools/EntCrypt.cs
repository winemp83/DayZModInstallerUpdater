using Logging;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace VerschlueßelungsTools
{
    public class EntCrypt
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

        public EntCrypt(string Value = null)
        {
            _Cyper = new Models.Crypt();
            if (Value != null || Value.Length > 0)
            {
                _Cyper.Value = Value;
                Encrypt();
            }
        }
        private void Encrypt()
        {
            try
            {
                byte[] initVectorBytes = Encoding.UTF8.GetBytes(_Cyper.Secret);
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(_Cyper.Value);
                PasswordDeriveBytes password = new PasswordDeriveBytes(_Cyper.Password, null);
                byte[] keyBytes = password.GetBytes(_Cyper.Keysize / 8);
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
                _Cyper.Result = Convert.ToBase64String(cipherTextBytes);

            }
            catch (Exception ex)
            {
                EventLog.WriteEventLog(EventTyp.Error,ex.Message);
            }
        }
    }
}
