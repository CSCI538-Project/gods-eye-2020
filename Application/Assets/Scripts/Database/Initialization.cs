using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
#if WINDOWS_UWP
using Windows.Storage;
using Windows.Data.Xml.Dom;
#endif

namespace Assets.Scripts.Database
{
    /**
     * This class will be used for obtaining the SQLite's database file when the application is started at the first time.
     * The database will store information about identified victims received from the server. (Currently)
     * More information you can review the file of History.cs.
     */
    class Initialization
    {
        public void ObtainDatabaseFile()
        {
            if (!File.Exists(Configs.Global.LocalStorage.Paths.DatabaseFile))
            {
                Debug.Log("There is no database file in " + Configs.Global.LocalStorage.Paths.DatabaseFile);
                UnityMainThreadDispatcher.Instance().Enqueue(getDatabaseFile());
            }
            else { Debug.Log("Database File already exists."); }
        }

        /**
         * You can create the file of `godseye.db` using DB.Browser.for.SQLite also.
         */
        private IEnumerator getDatabaseFile() 
        {
            // The Init database file downloaded from web (12KB)
            using (UnityWebRequest webRequest = UnityWebRequest.Get("https://www.liwenbin.com/"+Configs.Global.FILE_NAME_DATABASE))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.isHttpError || webRequest.isNetworkError)
                {
                    Debug.LogError(webRequest.error + "\n" + webRequest.downloadHandler.text);
                }
                else
                {
                    Debug.Log("Database Data Received, and it will be saved locally for future use.");
                    this.writeInitalDatabase(webRequest.downloadHandler.data);

                    yield break;
                }
            }
        }

        private async void writeInitalDatabase(byte[] data) 
        {
#if WINDOWS_UWP
                    StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                    StorageFile databaseFileForWrite = await storageFolder.CreateFileAsync(Configs.Global.FILE_NAME_DATABASE);
                    await FileIO.WriteBytesAsync(databaseFileForWrite, data);
#endif
        }
    }
}
