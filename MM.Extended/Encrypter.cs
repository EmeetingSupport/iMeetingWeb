using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
 
namespace MM.Extended
{
    public class Encryptor
    {
        public static string KeyValue = "jMj@^!^n@23";

        public static string EncryptString(string Message = "")
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

        public static string DecryptString(string Message = "mI6qJoSmlR4=", string KeyValue = "")
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


        public static void EncryptFile(string strFullPath, string passPhrase)
        {
            //Set up the encryption objects
            using (AesCryptoServiceProvider acsp = GetProvider(Encoding.Default.GetBytes(passPhrase)))
            {
                // byte[] sourceBytes = Encoding.ASCII.GetBytes(plainSourceStringToEncrypt);
                ICryptoTransform ictE = acsp.CreateEncryptor();

                using (FileStream fsOutput = new FileStream(strFullPath + "1", FileMode.Create))
                {
                    //Encrypt file and save
                    using (CryptoStream cs = new CryptoStream(fsOutput, ictE, CryptoStreamMode.Write))
                    {
                        using (FileStream fsInput = new FileStream(strFullPath, FileMode.Open))
                        {
                            int data;
                            while ((data = fsInput.ReadByte()) != -1)
                            {
                                cs.WriteByte((byte)data);
                            }
                        }
                    }
                }
            }
        }
        
        public static void DecryptFile(string base64StringToDecrypt, string passphrase)
        {
            //Set up the encryption objects
            using (AesCryptoServiceProvider acsp = GetProvider(Encoding.Default.GetBytes(passphrase)))
            {
                //  byte[] RawBytes = Convert.FromBase64String(base64StringToDecrypt);
                ICryptoTransform ictD = acsp.CreateDecryptor();

                //RawBytes now contains original byte array, still in Encrypted state

                //Decrypt into stream
                //  MemoryStream msD = new MemoryStream(RawBytes, 0, RawBytes.Length);
                using (FileStream msD = new FileStream(base64StringToDecrypt, FileMode.Open))
                {
                    using (CryptoStream csD = new CryptoStream(msD, ictD, CryptoStreamMode.Read))
                    {
                        using (FileStream fsOutput = new FileStream(base64StringToDecrypt + "11", FileMode.Create))
                        {
                            int data;
                            while ((data = csD.ReadByte()) != -1)
                            {
                                fsOutput.WriteByte((byte)data);
                            }
                        }
                    }
                }
                //csD now contains original byte array, fully decrypted

                //return the content of msD as a regular string
                // return (new StreamReader(csD)).ReadToEnd();
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

    }
}
