using Logging;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace VerschlueßelungsTools
{
    public class DeCrypt
    {
        private readonly string _SecretKey = "!HashP2020";
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
                byte[] privatekeyByte = { };
                privatekeyByte = System.Text.Encoding.UTF8.GetBytes(_SecretKey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(PublicKey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = new byte[Text.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(Text.Replace(" ", "+"));
                using DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                ms = new MemoryStream();
                cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                cs.FlushFinalBlock();
                Encoding encoding = Encoding.UTF8;
                Result = encoding.GetString(ms.ToArray());

            }
            catch (Exception ex)
            {
                EventLog.WriteEventLog(EventTyp.Error, ex.Message);
            }
        }
    }
}
