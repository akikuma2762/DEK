using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Support
{
    public class misc
    {
        //--------------------------------------------------------------------
        public static void Graphics_DrawGrid(Control ctrl, int grid_w, int grid_h, Pen pen)
        {
            Graphics gr = ctrl.CreateGraphics();

            gr.Clear(ctrl.BackColor);

            // Horizental Lines
            for (int y = 0; y < ctrl.ClientSize.Height; y += grid_h)
            {
                gr.DrawLine(pen, 0, y, ctrl.ClientSize.Width, y);
            }
            // Vertical Lines
            for (int x = 0; x < ctrl.ClientSize.Width; x += grid_w)
            {
                gr.DrawLine(pen, x, 0, x, ctrl.ClientSize.Height);
            }

            gr.Dispose();
        }
        public static int Math_Interpolate(int val, int min, int max, int new_min, int new_max)
        {
            //float ans = (float)new_max / max * val;
            //return Convert.ToInt32(ans);
            return (new_min + (val - min) * (new_max - new_min) / (max - min));
        }

        public static string Str_char(int Length, bool Sleep)
        {
            if (Sleep) System.Threading.Thread.Sleep(3);
            string result = "";
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < Length; i++)
            {
                int rnd = random.Next(0, 26);
                result += 'A' + rnd;
            }
            return result;
        }
    }

    public class Encrypt
    {
        // This size of the IV (in bytes) must = (keysize / 8).  
        // Default keysize is 256, so the IV must be 32 bytes long.
        // Using a 16 character string here gives us 32 bytes when converted to a byte array.
        // This constant is used to determine the keysize of the encryption algorithm
        private const int keysize = 256;
        private const string initVector = "d!e@k#%$**^(^!@#"; //16 chars
        private static byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
        //Encrypt
        public static string EncryptString(string plainText, string passPhrase)
        {
            try
            {
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] keyBytes = new PasswordDeriveBytes(passPhrase, null).GetBytes(keysize / 8);
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                byte[] cipherTextBytes = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();
                return Convert.ToBase64String(cipherTextBytes);
            }
            catch { return ""; }
        }
        //Decrypt
        public static string DecryptString(string cipherText, string passPhrase)
        {
            try
            {
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
                byte[] keyBytes = new PasswordDeriveBytes(passPhrase, null).GetBytes(keysize / 8);
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
            catch { return ""; }
        }
    }

}
