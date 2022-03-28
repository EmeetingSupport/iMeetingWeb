using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MM.Core
{
    public class AesEncryption
    {

        /// <summary>
        /// Encrpyts the sourceString, returns this result as an Aes encrpyted, BASE64 encoded string
        /// </summary>
        /// <param name="plainSourceStringToEncrypt">a plain, Framework string (ASCII, null terminated)</param>
        /// <param name="passPhrase">The pass phrase.</param>
        /// <returns>
        /// returns an Aes encrypted, BASE64 encoded string
        /// </returns>
        public static string EncryptString(string plainSourceStringToEncrypt, string passPhrase = "J&!S!%}^<a@^*)R-m")//"s@^jMj%r!a*)~n&p")
        {
            //Set up the encryption objects
            using (AesCryptoServiceProvider acsp = GetProvider(Encoding.Default.GetBytes(passPhrase)))
            {
                byte[] sourceBytes = Encoding.ASCII.GetBytes(plainSourceStringToEncrypt);
                ICryptoTransform ictE = acsp.CreateEncryptor();

                //Set up stream to contain the encryption
                MemoryStream msS = new MemoryStream();

                //Perform the encrpytion, storing output into the stream
                CryptoStream csS = new CryptoStream(msS, ictE, CryptoStreamMode.Write);
                csS.Write(sourceBytes, 0, sourceBytes.Length);
                csS.FlushFinalBlock();

                //sourceBytes are now encrypted as an array of secure bytes
                byte[] encryptedBytes = msS.ToArray(); //.ToArray() is important, don't mess with the buffer

                //return the encrypted bytes as a BASE64 encoded string
                return Convert.ToBase64String(encryptedBytes);
            }
        }


        /// <summary>
        /// Decrypts a BASE64 encoded string of encrypted data, returns a plain string
        /// </summary>
        /// <param name="base64StringToDecrypt">an Aes encrypted AND base64 encoded string</param>
        /// <param name="passphrase">The passphrase.</param>
        /// <returns>returns a plain string</returns>
        public static string DecryptString(string base64StringToDecrypt, string passphrase = "J&!S!%}^<a@^*)R-m")//"s@^jMj%r!a*)~n&p")
        {
            //Set up the encryption objects
            using (AesCryptoServiceProvider acsp = GetProvider(Encoding.Default.GetBytes(passphrase)))
            {
                byte[] RawBytes = Convert.FromBase64String(base64StringToDecrypt);
                ICryptoTransform ictD = acsp.CreateDecryptor();

                //RawBytes now contains original byte array, still in Encrypted state

                //Decrypt into stream
                MemoryStream msD = new MemoryStream(RawBytes, 0, RawBytes.Length);
                CryptoStream csD = new CryptoStream(msD, ictD, CryptoStreamMode.Read);
                //csD now contains original byte array, fully decrypted

                //return the content of msD as a regular string
                return (new StreamReader(csD)).ReadToEnd();
            }
        }

        private static AesCryptoServiceProvider GetProvider(byte[] key)
        {
            AesCryptoServiceProvider result = new AesCryptoServiceProvider();
            result.BlockSize = 128;
            result.KeySize = 128;
            result.Mode = CipherMode.CBC;
            result.Padding = PaddingMode.PKCS7;

            result.GenerateIV();
            result.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            byte[] RealKey = GetKey(key, result);
            result.Key = RealKey;
            // result.IV = RealKey;
            return result;
        }

        private static byte[] GetKey(byte[] suggestedKey, SymmetricAlgorithm p)
        {
            byte[] kRaw = suggestedKey;
            List<byte> kList = new List<byte>();

            for (int i = 0; i < p.LegalKeySizes[0].MinSize; i += 8)
            {
                kList.Add(kRaw[(i / 8) % kRaw.Length]);
            }
            byte[] k = kList.ToArray();
            return k;
        }

        public static string EncryptString256(string strEncrypt, string passPhrase = "J&!S!%}^<a@^*)R-m>vlpoxzbcvTNaUM") //Aes 256 encryption
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            //  byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                byte[] bytesToBeEncrypted = Encoding.ASCII.GetBytes(strEncrypt);
                using (RijndaelManaged rijndael = new RijndaelManaged())
                {
                    byte[] ba = Encoding.Default.GetBytes(passPhrase);
                    var hexString = BitConverter.ToString(ba);
                    hexString = hexString.Replace("-", "");

                    byte[] bsa = Encoding.Default.GetBytes("0000000000000000");
                    var hexStrings = BitConverter.ToString(bsa);
                    hexStrings = hexStrings.Replace("-", "");

                    rijndael.Mode = CipherMode.CBC;
                    rijndael.Padding = PaddingMode.PKCS7;
                    rijndael.KeySize = 256;
                    rijndael.BlockSize = 128;
                    rijndael.Key = StringToByteArray(hexString);//"3030303030303030303030303030303030303030303030303030303030303030"); //prekey2; //key;

                    rijndael.IV = StringToByteArray(hexStrings);//"30303030303030303030303030303030");//iv;

                    using (var cs = new CryptoStream(ms, rijndael.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                
                    encryptedBytes = ms.ToArray();
                }
            }
            return Convert.ToBase64String(encryptedBytes);
            //File.WriteAllBytes(strEncrypt + "_", encryptedBytes);
            //System.IO.File.Delete(strEncrypt);
            //System.IO.File.Move(strEncrypt + "_", strEncrypt);
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        public static string AES_Decrypt256(string base64StringToDecrypt, string passPhrase = "J&!S!%}^<a@^*)R-m>vlpoxzbcvTNaUM")
        {                     
            byte[] bytesToBeEncrypted = System.Convert.FromBase64String(base64StringToDecrypt);
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged rijndael = new RijndaelManaged())
                {
                    byte[] ba = Encoding.Default.GetBytes(passPhrase);
                    var hexString = BitConverter.ToString(ba);
                    hexString = hexString.Replace("-", "");

                    byte[] bsa = Encoding.Default.GetBytes("0000000000000000");
                    var hexStrings = BitConverter.ToString(bsa);
                    hexStrings = hexStrings.Replace("-", "");

                    rijndael.Mode = CipherMode.CBC;
                    rijndael.Padding = PaddingMode.PKCS7;
                    rijndael.KeySize = 256;
                    rijndael.BlockSize = 128;
                    rijndael.Key = StringToByteArray(hexString);//"3030303030303030303030303030303030303030303030303030303030303030"); //prekey2; //key;

                    rijndael.IV = StringToByteArray(hexStrings);//"30303030303030303030303030303030");//iv;


                    //using (var cs = new CryptoStream(ms, rijndael.CreateDecryptor(), CryptoStreamMode.Write))
                    //{
                    //    cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                    //    cs.Close();
                    //}
                    using (ICryptoTransform decrypt = rijndael.CreateDecryptor())
                    {
                        byte[] dest = decrypt.TransformFinalBlock(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        return Encoding.ASCII.GetString(dest);
                    }
                  //  decryptedBytes = ms.ToArray();
                }
            }
           // return Encoding.ASCII.GetString(decryptedBytes); ;
        }

    }

}
