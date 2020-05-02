using System.Runtime.CompilerServices;
using UnityEngine;
#if WINDOWS_UWP
using Windows.Storage;
#endif

namespace Assets.Scripts.Configs
{
    public static class Global
    {
        public static bool HTTP_PROTOCOL_IMAGES = true; // For obtaining images
        public static string WEBSOCKETS_PROTOCOL = "ws";
        public static string SERVER_IP_ADDR = "13.52.100.31";
        public static int WEBSOCKETS_PORT = 5000;
        public static string PHOTO_PATH_ON_SERVER = "/DB/pictures/";
        public static string SCREENSHOT_PATH_ON_SERVER = "/DB/pictures/";

        public static string FILE_NAME_DATABASE = "godseye.db";

        public struct LocalStorage
        {
            public struct Paths 
            {
                //public static string Profiles = "profile_dir/";
#if WINDOWS_UWP
                public static string DataFolder = ApplicationData.Current.LocalFolder.Path;
                public static string DatabaseFile = DataFolder + "\\" + FILE_NAME_DATABASE;
#else
                public static string DataFolder = Application.dataPath;
                public static string DatabaseFile = DataFolder + "\\" + FILE_NAME_DATABASE;
#endif

            }
            
        }
    }
}
