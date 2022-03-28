using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.Text;
using System.IO;


namespace MeetingMinder.Web
{
    public class EncryptionHelper
    {
        public static string GetPdfName(out string pdfPassword)
        {
            string rnd1 = Guid.NewGuid().ToString().Substring(8).Replace("-", "");//GetChar(8);
            string rnd2 = GetChar(8);
            pdfPassword = GetSHA512(rnd2);//GetMd5String(rnd2);
            string name = rnd1.Substring(0, 4) + rnd2 + rnd1.Substring(4, 4);

            string encyptedName = MM.Extended.Encryptor.EncryptString(name);
//..  
//./   
//\   
//:   
//%   
//&  
            // encyptedName = encyptedName.Replace("/", ".").Replace("+", ";");
            encyptedName = encyptedName.Replace("&", "-").Replace("/", "_").Replace("\\", "'").Replace("+", ";").Replace("..", ".").Replace("%","`").Replace(":",",");
            return encyptedName;
        }

        public static string GetMd5String(string rawString)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(rawString);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        public static string GetChar(int Length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var random = new Random((int)DateTime.Now.Ticks);// new Random();
            var result = new string(
                Enumerable.Repeat(chars, Length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result.ToString();
        }

        public static string GetPassword(string pdfName, string AgendaKey)
        {
            string decrypted = "";
            if (AgendaKey == "")
            {
                 AgendaKey = MM.Extended.Encryptor.KeyValue;
                //decrypted = MM.Extended.Encryptor.DecryptString((pdfName.Remove(pdfName.Length - 4, 4)).Replace(".", "/").Replace(";", "+"), AgendaKey.Trim());
                
                  decrypted = MM.Extended.Encryptor.DecryptString((pdfName.Remove(pdfName.Length - 4, 4)).Replace("-", "&").Replace("_", "/").Replace("'", "\\").Replace(";", "+").Replace(".", "..").Replace("\"", "%").Replace(",", ":"), AgendaKey.Trim());
            }
            else
            {
                AgendaKey = MM.Core.Encryptor.DecryptString(AgendaKey.Trim());
               // decrypted = MM.Extended.Encryptor.DecryptString((pdfName.Remove(pdfName.Length - 4, 4)).Replace(".", "/").Replace(";", "+"), AgendaKey.Trim().Substring(0, AgendaKey.Length - 4));
                decrypted = MM.Extended.Encryptor.DecryptString((pdfName.Remove(pdfName.Length - 4, 4)).Replace("-", "&").Replace("_", "/").Replace("'", "\\").Replace(";", "+").Replace(".", "..").Replace("`", "%").Replace(",", ":"), AgendaKey.Trim().Substring(0, AgendaKey.Length - 4));
            }

            string pass = "";
            if (decrypted != "")
            {
                pass = decrypted.Substring(4, 8);
                pass = GetSHA512(pass);
            }

            //GetMd5String(pass);
            return pass;
        }

        public static string GetSHA512(string text)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            byte[] message = UE.GetBytes(text);

            SHA512Managed hashString = new SHA512Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        public static void DecryptString(string base64StringToDecrypt, string passphrase)
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

        //public static AesCryptoServiceProvider GetProvider(byte[] key)
        //{
        //    AesCryptoServiceProvider result = new AesCryptoServiceProvider();
        //    result.BlockSize = 128;
        //    result.KeySize = 128;
        //    result.Mode = CipherMode.CBC;
        //    result.Padding = PaddingMode.PKCS7;

        //    result.GenerateIV();
        //    result.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        //    byte[] RealKey = GetKey(key, result);
        //    result.Key = RealKey;
        //    // result.IV = RealKey;
        //    return result;
        //}

        //public static byte[] GetKey(byte[] suggestedKey, SymmetricAlgorithm p)
        //{
        //    byte[] kRaw = suggestedKey;
        //    List<byte> kList = new List<byte>();

        //    for (int i = 0; i < p.LegalKeySizes[0].MinSize; i += 8)
        //    {
        //        kList.Add(kRaw[(i / 8) % kRaw.Length]);
        //    }
        //    byte[] k = kList.ToArray();
        //    return k;
        //}
        public static void EncryptString_old(string strFullPath, string passPhrase) //Aes 128 encryption
        {
            if (passPhrase.Length == 0)
            {
                passPhrase = System.Web.HttpContext.Current.Session["EncryptionKey"].ToString();
            }
            //Set up the encryption objects
            using (AesCryptoServiceProvider acsp = GetProvider(Encoding.Default.GetBytes(passPhrase)))
            {
                // byte[] sourceBytes = Encoding.ASCII.GetBytes(plainSourceStringToEncrypt);
                ICryptoTransform ictE = acsp.CreateEncryptor();

                using (FileStream fsOutput = new FileStream(strFullPath + "_", FileMode.Create))
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
            System.IO.File.Delete(strFullPath);
            System.IO.File.Move(strFullPath + "_", strFullPath);
        }


        public static AesCryptoServiceProvider GetProvider(byte[] key)
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

        public static byte[] GetKey(byte[] suggestedKey, SymmetricAlgorithm p)
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

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static void EncryptString(string strFullPath, string passPhrase) //Aes 256 encryption
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            //  byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                byte[] bytesToBeEncrypted = File.ReadAllBytes(strFullPath);
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
            File.WriteAllBytes(strFullPath + "_", encryptedBytes);
            System.IO.File.Delete(strFullPath);
            System.IO.File.Move(strFullPath + "_", strFullPath);
        }


        public byte[] AES_Decrypt(string strFullPath, string passPhrase)
        {
            byte[] decryptedBytes = null;

            byte[] bytesToBeEncrypted = File.ReadAllBytes(strFullPath);
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


                    using (var cs = new CryptoStream(ms, rijndael.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }
        

    }
}