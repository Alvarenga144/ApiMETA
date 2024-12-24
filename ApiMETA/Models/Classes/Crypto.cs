using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ApiMETA.Models.Classes
{
    public class Crypto : IDisposable
    {
        private static byte[] rawSecretKey = { };  // Almacena la clave secreta en formato de bytes.
        private readonly string _Phrase = System.Configuration.ConfigurationManager.AppSettings["phraseKey"];  // Obtiene la clave secreta desde la configuración de la aplicación.
        private readonly byte[] passwordKey = { };  // Almacena la clave derivada de la frase de contraseña.

        public Crypto(string passPhrase)
        {
            rawSecretKey = Encoding.UTF8.GetBytes(_Phrase); // Convierte la clave secreta a bytes.
            passwordKey = EncodeDigest(passPhrase);// Deriva la clave de la frase de contraseña utilizando la función EncodeDigest.
        }

        public byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (var ms = new MemoryStream())
            {
                using (var aes = Rijndael.Create())
                {
                    aes.Key = key; // Asigna la clave para la encriptación.
                    aes.IV = iv; // Asigna el vector de inicialización para la encriptación.
                    var stream = new CryptoStream(ms, aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write);
                    stream.Write(data, 0, data.Length); // Escribe los datos encriptados en el flujo de datos.
                    stream.FlushFinalBlock(); // Limpia el flujo de datos final.
                    return ms.ToArray(); // Devuelve los datos encriptados como un array de bytes.
                }
            }
        }

        public string EncryptFromBase64(string clearData)
        {
            string pass = Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(clearData), passwordKey, rawSecretKey));
            // Convierte la cadena de datos a bytes, luego encripta y convierte los datos encriptados a una cadena Base64.
            return pass;  // Devuelve la cadena encriptada en formato Base64.
        }

        private byte[] EncodeDigest(string text)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = Encoding.UTF8.GetBytes(text); // Convierte la cadena de texto a bytes.
            return x.ComputeHash(data); // Devuelve el resumen de mensaje (hash) de la cadena de texto utilizando MD5.
        }

        #region Disposable
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: eliminar el estado administrado (objetos administrados)
                }

                // TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
                // TODO: establecer los campos grandes como NULL
                disposedValue = true;
            }
        }

        // // TODO: reemplazar el finalizador solo si "Dispose(bool disposing)" tiene código para liberar los recursos no administrados
        // ~TransaccionModels()
        // {
        //     // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
        //     Dispose(disposing: false);
        // }

        void IDisposable.Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}