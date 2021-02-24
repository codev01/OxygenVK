using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;

namespace OxygenVK.AppSource.LocalFolder
{
	public class LocalFile
	{
		private readonly string FileName;
		private readonly StorageFolder LocalFolder;

		public LocalFile(string fileName)
		{
			FileName = fileName;
			LocalFolder = ApplicationData.Current.LocalFolder;
		}

		/// <summary>
		/// Запись в файл
		/// </summary>
		/// <param name="text"> Текст для записи в файл </param>
		public async void Write(string text)
		{
			StorageFile storageFile = await LocalFolder.CreateFileAsync(FileName, CreationCollisionOption.OpenIfExists);

			using(FileStream fstream = new FileStream(storageFile.Path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
			{
				fstream.SetLength(0);
				byte[] array = Encoding.Default.GetBytes(text);
				await fstream.WriteAsync(array, 0, array.Length);
			}
		}

		/// <summary>
		/// Чтение из файла
		/// </summary>
		/// <returns>
		/// Содержимое файла в виде текста
		/// </returns>
		public async Task<string> Read()
		{
			StorageFile storageFile = await LocalFolder.CreateFileAsync(FileName, CreationCollisionOption.OpenIfExists);

			using(FileStream fstream = File.OpenRead(storageFile.Path))
			{
				byte[] array = new byte[fstream.Length];
				await fstream.ReadAsync(array, 0, array.Length);

				return Encoding.Default.GetString(array);
			}
		}
	}
}
