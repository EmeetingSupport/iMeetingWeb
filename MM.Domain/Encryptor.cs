using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace MM.Core
{
 public  class Encryptor
    {

        public static string EncryptString(string Message= "")
        {
            string Passphrase = "J&!S!%}^<a@^*)R-m";
            using (AesCryptoServiceProvider acsp = GetProvider(Encoding.Default.GetBytes(Passphrase)))
            {
                byte[] sourceBytes = Encoding.ASCII.GetBytes(Message);
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

        public static string DecryptString(string Message = "mI6qJoSmlR4=")
        {
            if (Message.Equals(""))
            {
                Message = "mI6qJoSmlR4=";
            }

            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            string Passphrase = "J&!S!%}^<a@^*)R-m";
            byte[] Results;
          

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            try
            {
                using (AesCryptoServiceProvider acsp = GetProvider(Encoding.Default.GetBytes(Passphrase)))
                {
                    byte[] RawBytes = Convert.FromBase64String(Message);
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
            catch (Exception Ex)
            {
                return "";
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format
            return UTF8.GetString(Results);
        }

        //public void test(string strTest)
        //{
        //    System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
        //    byte[] Results = UTF8.GetBytes(strTest);
         
        //    MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
        //    TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
        //    string Passphrase = "nEo@^!n@23";         

        //    // Step 1. We hash the passphrase using MD5
        //    // We use the MD5 hash generator as the result is a 128 bit byte array
        //    // which is a valid length for the TripleDES encoder we use below

        //    try
        //    {
        //        byte[] TDESKey = HashProvider.ComputeHash(Results);

        //        // Step 2. Create a new TripleDESCryptoServiceProvider object


        //        // Step 3. Setup the decoder
        //        TDESAlgorithm.Key = TDESKey;
        //        TDESAlgorithm.Mode = CipherMode.ECB;
        //        TDESAlgorithm.Padding = PaddingMode.PKCS7;

        //        // Step 4. Convert the input string to a byte[]
        //        byte[] DataToDecrypt = Convert.FromBase64String(Message);

        //        // Step 5. Attempt to decrypt the string

        //        ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
        //        Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
        //    }
        //    catch (Exception Ex)
        //    {
        //        return;
        //    }
        //    finally
        //    {
        //        // Clear the TripleDes and Hashprovider services of any sensitive information
        //        TDESAlgorithm.Clear();
        //        HashProvider.Clear();
        //    }

        //    // Step 6. Return the decrypted string in UTF8 format
          
        //}
    }
}
