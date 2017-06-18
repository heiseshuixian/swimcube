using System.IO;
using System.Text;

namespace Giu.Basic{

    public static class Files {

        public static char PathSeparator {
            get { 
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_XBOXONE || UNITY_WINRT_8_1
	            return '\\';
#else
                return '/';
#endif
                //        #elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
                //string pluginFileName = pluginName + ".bundle";
                //        #elif UNITY_PS4
                //            string pluginFileName = pluginName + ".prx"
                //        #elif UNITY_ANDROID || UNITY_STANDALONE_LINUX
                //            string pluginFileName = "lib" + pluginName + ".so";
                //        #endif

                //        #if UNITY_EDITOR_WIN && UNITY_EDITOR_64
                //            string pluginFolder = Application.dataPath + "/Plugins/X86_64/";
                //        #elif UNITY_EDITOR_WIN
                //            string pluginFolder = Application.dataPath + "/Plugins/X86/";
                //        #elif UNITY_STANDALONE_WIN || UNITY_PS4 || UNITY_XBOXONE || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX || UNITY_STANDALONE_LINUX
                //            string pluginFolder = Application.dataPath + "/Plugins/";
                //        #elif UNITY_WINRT_8_1
                //            string pluginFolder = "";
                //        #elif UNITY_ANDROID            
                //var dirInfo = new global::System.IO.DirectoryInfo(Application.persistentDataPath);
                //string packageName = dirInfo.Parent.Name;
                //            string pluginFolder = "/data/data/" + packageName + "/lib/";
                //        #else
                //            string pluginFolder = "";
                //        #endif 
            }
        }

        public static string GetExtention(string path) { return Path.GetExtension(path); }

        public static string GetFileNameWithoutExtension(string path) { return Path.GetFileNameWithoutExtension(path); }

        public static DirectoryInfo EnsureFolder(string path) {
            string dirPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
            return new DirectoryInfo(dirPath);
        }

        public static Seq<string> GetFilenamesAtDir(string path) {
            Seq<FileInfo> files = EnsureFolder(path).GetFiles();
            return files.Map(f => f.Name);
        }

        public static void ForEachFile(string dirPath, _D_InnerT<FileInfo> func) {
            if (func == null) return;
            DirectoryInfo di = EnsureFolder(dirPath);
            Seq<FileInfo> files = di.GetFiles();
            Seq<DirectoryInfo> directories = di.GetDirectories();
            files.DoSeq(f => func(f)); 
            directories.DoSeq(d => ForEachFile(d.FullName + PathSeparator, func));
        }

        public static Seq<string> GetFilenamesAtDirOfExtention(string path, string extention) {
            Seq<FileInfo> files = EnsureFolder(path).GetFiles();
            return files.FilterX(f => f.Extension == extention).Map<string>(f => f.Name);
        }

        public static Seq<string> GetFilePathesAtDir(string path) {
            Seq<FileInfo> files = EnsureFolder(path).GetFiles();
            return files.Map(f => f.FullName);
        }


        public static void WriteStringIntoFile(this string code, string path) {
            EnsureFolder(path);
            File.WriteAllText(path, code, Encoding.UTF8); 
        }

        public static void WriteBytesIntoFile(this byte[] data, string path) {
            EnsureFolder(path);
            File.WriteAllBytes(path, data);
        }

        public static string ReadStringIfFileExist(string path, Encoding encoding = null) {
            if (!File.Exists(path)) return "";
            string text = ""; 
            if(encoding == null) using (StreamReader reader = File.OpenText(path)) text = reader.ReadToEnd();
            else using (StreamReader reader = new StreamReader(path, encoding)) text = reader.ReadToEnd(); 
            return text;
        }

        public static byte[] ReadAllBytesIfFileExist(string path) {
            if (!File.Exists(path)) return null;
            return File.ReadAllBytes(path);
        }

        public static bool Exist(string filePath) { return File.Exists(filePath); }
        
    }

}
