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
        public Crypt(string key = null)
        {
            if (key == null)
            {
                _DeCrypt = new DeCrypt();
                _EntCrypt = new EntCrypt();
            }
            else
            {
                _DeCrypt = new DeCrypt(key);
                _EntCrypt = new EntCrypt(key);
            }
        }

        /** @description Verschlüßelt einen Text.
          * @param {string} Klar Text.
          * @return {string}
          */
        public string Entcrypt(string text)
        {
            _EntCrypt.Text = text;
            return _EntCrypt.Result;
        }

        /** @description Entschlüßelt einen Text.
          * @param {string} Verschlüßelter Text.
          * @return {string}
          */
        public string Decrypt(string text)
        {
            _DeCrypt.Text = text;
            return _DeCrypt.Result;
        }
    }
}
