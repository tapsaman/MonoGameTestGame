using System.Xml.Serialization;
using System.IO.IsolatedStorage;
using System.IO;

namespace TapsasEngine
{
    public class SaveFileManager<T> where T : class
    {
        public static string Directory = "SaveGames";
        private static XmlSerializer _serializer = new XmlSerializer(typeof(T));

        public void Save(T saving, string fileName)
        {
            string filePath = Directory + "/" + fileName;

            using (IsolatedStorageFile storage = GetIsolatedStorageFile())
            {
                if (!storage.DirectoryExists(Directory))
                {
                    storage.CreateDirectory(Directory);
                }
                else if (storage.FileExists(filePath))
                {
                    storage.DeleteFile(filePath);
                }

                using (IsolatedStorageFileStream stream = storage.CreateFile(filePath))
                {
                    // Set the position to the begining of the file.
                    stream.Seek(0, SeekOrigin.Begin);
                    // Serialize the new data object.
                    _serializer.Serialize(stream, saving);
                    // Set the length of the file.
                    stream.SetLength(stream.Position);

                    //isolatedFileStream.Close();
                    stream.Dispose();
                }

                storage.Close();
            }
        }

        public T Load(string fileName)
        {
            string filePath = Directory + "/" + fileName;
            T loading = null;

            using (IsolatedStorageFile storage = GetIsolatedStorageFile())
            {
                if (!storage.FileExists(filePath))
                {
                    return null;
                }

                using (IsolatedStorageFileStream isolatedFileStream = storage.OpenFile(filePath, FileMode.Open /*, FileAccess.ReadWrite*/))
                {
                    loading = (T)_serializer.Deserialize(isolatedFileStream);

                    //isolatedFileStream.Close();
                    isolatedFileStream.Dispose();
                }

                storage.Close();
            }

            return loading;
        }

        private IsolatedStorageFile GetIsolatedStorageFile()
        {
            // idk which to use

            // Windows default path -> \ProgramData\IsolatedStorage
            return IsolatedStorageFile.GetMachineStoreForDomain();
            
            // Windows default path -> \Users\<user>\AppData\Local\IsolatedStorage
            // return IsolatedStorageFile.GetUserStoreForAssembly();
        }
    }
} 