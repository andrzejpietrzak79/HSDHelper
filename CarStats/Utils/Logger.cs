using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace HSDHelper.Utils {
    public class Logger
    {
        private StringBuilder _sb = new StringBuilder();

        public void AppendLine(string line)
        {
            //_sb.AppendLine(line);
        }
        public void Append(string line)
        {
            //_sb.Append(line);
        }

        public async Task Save(string fileName) {
	        return;
            var documents = KnownFolders.DocumentsLibrary;

            // Get the first child folder, which represents the SD card.

            if (documents != null) {
                // An SD card is present and the sdCard variable now contains a reference to it.
                var folder = await documents.CreateFolderAsync("HybridAssistant", CreationCollisionOption.OpenIfExists);
                var file = await folder.CreateFileAsync(fileName);
                await FileIO.WriteTextAsync(file, _sb.ToString());
                _sb.Clear();
            }

        }
    }
    public class RuntimeLogger {
        public enum LogLevel {
            None = 3,
            Info = 0,
            Warning = 1,
            Error = 2,
            Timing = 4,
            ValueCheck = 5
        }

        public struct LogEntry {
            public LogLevel Level { get; set; }
            public string Message { get; set; }
        }

        //#if DEBUG
        private const string _logFile = "HybridAssistantLog.txt";
        //#endif

        #region Public Methods

        // Appends user defined level message and prints exception's data
        private static async Task AppendAsync(String message, LogLevel level, Exception e) {
            try {
                if (e != null) {
                    message += $"#Exception caught:#Message: {e.Message}#Call stack:#{e.StackTrace.Replace("\n", "#")}#";

                    if (e.InnerException != null) {
                        message += $"#Inner exception:#Message: {e.InnerException.Message}#Call stack:#{e.InnerException.StackTrace.Replace("\n", "#")}#";
                    }
                }
                var tmp = "Info";

                LogLevel logLevel;
                switch (tmp) {
                    case "Info":
                        logLevel = LogLevel.Info;
                        break;
                    case "Warning":
                        logLevel = LogLevel.Warning;
                        break;
                    case "Error":
                        logLevel = LogLevel.Error;
                        break;
                    case "None":
                        logLevel = LogLevel.None;
                        break;
                    default:
                        logLevel = LogLevel.Warning;
                        break;
                }


                if (logLevel > level) {
                    return;
                }

                var dt = DateTime.Now;

                var line = $"{dt}.{dt.Millisecond.ToString("D3")}|{level}|{message}\n";
                var folder = await KnownFolders.DocumentsLibrary.CreateFolderAsync("HybridAssistant", CreationCollisionOption.OpenIfExists);
                await folder.AppendToStorage(line, _logFile);
            }
            catch (Exception ex) {
                if (ex.Message == "")
                    throw;
            }
        }

        public static async Task InfoAsync(String message, Exception e = null) {
            await AppendAsync(message, LogLevel.Info, e);
        }

        private static async Task WarningAsync(String message, Exception e = null) {
            await AppendAsync(message, LogLevel.Warning, e);
        }
        private static async Task ErrorAsync(String message, Exception e = null) {
            await AppendAsync(message, LogLevel.Error, e);
        }


        // Clears log file contents
        public static async void Clear() {
            var folder = await KnownFolders.DocumentsLibrary.CreateFolderAsync("HybridAssistant", CreationCollisionOption.OpenIfExists);
            try {
                var file = await folder.GetFileAsync(_logFile);
                await file.DeleteAsync();
            }
            catch (FileNotFoundException) {

            }
        }
        #endregion


        public static void Error(String message, Exception e = null) {
            var task = Task.Run(async () => { await ErrorAsync(message, e); });
            task.Wait();
        }
        public static void Warning(String message, Exception e = null) {
            var task = Task.Run(async () => { await WarningAsync(message, e); });
            task.Wait();
        }
        public static void Info(String message, Exception e = null) {
            var task = Task.Run(async () => { await InfoAsync(message, e); });
            task.Wait();
        }


    }
    public static class IOExtensions {
        public static async Task<ulong> SaveToStorage(this IStorageFolder folder, MemoryStream stream, String filename) {
            var storageFile = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

            using (Stream x = await storageFile.OpenStreamForWriteAsync()) {
                x.Seek(0, SeekOrigin.Begin);
                stream.WriteTo(x);
            }
            return (await storageFile.GetBasicPropertiesAsync()).Size;
        }
        public static async Task<ulong> SaveToStorage(this IStorageFolder folder, string line, String filename) {
            try {
                var storageFile = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                var bytes = Encoding.UTF8.GetBytes(line);
                using (var stream = new MemoryStream(bytes)) {
                    using (Stream x = await storageFile.OpenStreamForWriteAsync()) {
                        x.Seek(0, SeekOrigin.Begin);
                        stream.WriteTo(x);
                        //stream.Flush();
                        x.Flush();
                    }
                }
                return (await storageFile.GetBasicPropertiesAsync()).Size;
            }
            catch (Exception) {
                return 0;
            }
        }

        public static async Task<ulong> AppendToStorage(this IStorageFolder folder, MemoryStream stream, String filename) {
            var storageFile = await folder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);

            using (Stream x = await storageFile.OpenStreamForWriteAsync()) {
                x.Seek(0, SeekOrigin.End);
                stream.WriteTo(x);
            }
            return (await storageFile.GetBasicPropertiesAsync()).Size;
        }
        public static async Task<ulong> AppendToStorage(this IStorageFolder folder, string line, String filename) {
            try {
                var storageFile = await folder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
                var bytes = Encoding.UTF8.GetBytes(line);
                using (var stream = new MemoryStream(bytes)) {
                    using (Stream x = await storageFile.OpenStreamForWriteAsync()) {
                        x.Seek(0, SeekOrigin.End);
                        stream.WriteTo(x);
                        //stream.Flush();
                        x.Flush();
                    }
                }
                return (await storageFile.GetBasicPropertiesAsync()).Size;
            }
            catch (Exception) {
                return 0;
            }
        }
        public static async Task<MemoryStream> LoadFromStorage(this IStorageFolder folder, string filename) {
            var storageFile = await folder.GetFileAsync(filename);

            using (var fileStream = await storageFile.OpenStreamForReadAsync()) {
                var memoryStream = new MemoryStream();
                await fileStream.CopyToAsync(memoryStream);

                memoryStream.Seek(0, SeekOrigin.Begin);
                return memoryStream;
            }
        }

        public static async Task<IStorageFile> CheckFileExists(this IStorageFolder folder, string filename) {
            try {
                return await folder.GetFileAsync(filename);
            }
            catch (Exception) {
                return null;
            }
        }
    }
}