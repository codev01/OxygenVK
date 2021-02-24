using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OxygenVK.AppSource.Authorization
{
	public class Crypto
	{
		public static string EncryptStringAES(string plainText, string sharedSecret)
		{
			if(string.IsNullOrEmpty(plainText))
			{
				throw new ArgumentNullException("plainText");
			}

			if(string.IsNullOrEmpty(sharedSecret))
			{
				throw new ArgumentNullException("sharedSecret");
			}

			RijndaelManaged aesAlg = null;

			try
			{
				aesAlg = new RijndaelManaged();
				aesAlg.Key = new Rfc2898DeriveBytes(sharedSecret, Encoding.UTF32.GetBytes(sharedSecret)).GetBytes(aesAlg.KeySize / 8);

				ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

				using(MemoryStream msEncrypt = new MemoryStream())
				{
					msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
					msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
					using(CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						using(StreamWriter swEncrypt = new StreamWriter(csEncrypt))
						{
							swEncrypt.Write(plainText);
						}
					}
					return Convert.ToBase64String(msEncrypt.ToArray());
				}
			}
			catch
			{
				return null;
			}
			finally
			{
				if(aesAlg != null)
				{
					aesAlg.Clear();
				}
			}
		}

		public static string DecryptStringAES(string cipherText, string sharedSecret)
		{
			if(string.IsNullOrEmpty(cipherText))
			{
				throw new ArgumentNullException("cipherText");
			}

			if(string.IsNullOrEmpty(sharedSecret))
			{
				throw new ArgumentNullException("sharedSecret");
			}

			RijndaelManaged aesAlg = null;

			try
			{
				using(MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
				{
					aesAlg = new RijndaelManaged();
					aesAlg.Key = new Rfc2898DeriveBytes(sharedSecret, Encoding.UTF32.GetBytes(sharedSecret)).GetBytes(aesAlg.KeySize / 8);
					aesAlg.IV = ReadByteArray(msDecrypt);
					ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
					using(CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
					{
						using(StreamReader srDecrypt = new StreamReader(csDecrypt))
						{
							return srDecrypt.ReadToEnd();
						}
					}
				}
			}
			catch
			{
				return null;
			}
			finally
			{
				if(aesAlg != null)
				{
					aesAlg.Clear();
				}
			}
		}

		private static byte[] ReadByteArray(Stream s)
		{
			byte[] rawLength = new byte[sizeof(int)];
			if(s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
			{
				throw new SystemException("Поток не содержал правильно отформатированный массив байтов");
			}

			byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
			if(s.Read(buffer, 0, buffer.Length) != buffer.Length)
			{
				throw new SystemException("Не удалось правильно прочитать массив байтов");
			}

			return buffer;
		}
	}
}
