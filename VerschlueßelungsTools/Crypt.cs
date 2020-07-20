namespace VerschlueßelungsTools
{
    public class Crypt
    {
        private readonly DeCrypt _DeCrypt;
        private readonly EntCrypt _EntCrypt;

        /** @description Konstruktor mit übergabe des PublicKeys.
          * @param {string} PublicKey.
          * @return {void}
          */
        public Crypt(string Value = null)
        {
            if (Value == null)
            {
                _DeCrypt = new DeCrypt();
                _EntCrypt = new EntCrypt();
            }
            else
            {
                _DeCrypt = new DeCrypt(Value);
                _EntCrypt = new EntCrypt(Value);
            }
        }

        /** @description Verschlüßelt einen Text.
          * @param {string} Klar Text.
          * @return {string}
          */
        public string Entcrypt(string text)
        {
            _EntCrypt.Set = text;
            return _EntCrypt.Result;
        }
        public string Entcrypt() {
            return _EntCrypt.Result;
        }

        /** @description Entschlüßelt einen Text.
          * @param {string} Verschlüßelter Text.
          * @return {string}
          */
        public string Decrypt(string text)
        {
            _DeCrypt.Set = text;
            return _DeCrypt.Result;
        }
        public string Decrypt()
        {
            return _DeCrypt.Result;
        }
    }
}
