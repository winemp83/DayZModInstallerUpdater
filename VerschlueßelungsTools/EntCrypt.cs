using Logging;
using System;
using System.IO;
using System.Security.Cryptography;

namespace VerschlueßelungsTools
{
    public class EntCrypt
    {
        private readonly string _SecretKey = "!HashP2020";
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
                byte[] secretkeyByte = { };
                secretkeyByte = System.Text.Encoding.UTF8.GetBytes(_SecretKey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(_PublicKey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(Text);
                using DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                ms = new MemoryStream();
                cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                cs.FlushFinalBlock();
                Result = Convert.ToBase64String(ms.ToArray());

            }
            catch (Exception ex)
            {
                EventLog.WriteEventLog(EventTyp.Error,ex.Message);
            }
        }
    }
}
