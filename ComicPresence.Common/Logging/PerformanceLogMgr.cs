using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using ComicPresence.Common.Config;
using ComicPresence.Common.Patterns;

namespace ComicPresence.Common.Logging
{
    /// <summary>
    /// Performance Logger
    /// </summary>
    public class PerformanceLogMgr : Singleton<PerformanceLogMgr>
    {
        private const int cFlushToFileIntervalMillis = 10000;

        /// <summary>
        /// The items
        /// </summary>
        private List<PerformanceLogItem> Items;

        /// <summary>
        /// The stream
        /// </summary>
        private StreamWriter Stream;

        /// <summary>
        /// The sync
        /// </summary>
        private object Synk = new object();

        /// <summary>
        /// The enabled
        /// </summary>
        public bool Enabled
        {
            get
            {
                return AppSettingMgr.Instance.GetValue<bool>(AppSettingId.PerformanceLoggingEnabled);
            }
        }

        public bool Configured { get; protected set; }


        /// <summary>
        /// Initializes the <see cref="PerformanceLogMgr"/> class.
        /// </summary>
        private PerformanceLogMgr()
        {
        }

        public void Configure(string logFilePath)
        {
            if (Configured)
                throw new InvalidOperationException("Already configured");

            if (String.IsNullOrEmpty(logFilePath))
            {
                throw new ArgumentNullException("logFilePath");
            }

            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
            FileStream stream = null;
            try
            {
                stream = new FileStream(logFilePath, FileMode.Create,
                                        FileAccess.Write, FileShare.Read);
            }
            catch (IOException)
            {
                // May indicate file is locked - try alternate name
                FileInfo fileInfo = new FileInfo(logFilePath);

                string altFileName = fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length) + "2" + fileInfo.Extension;

                stream = new FileStream(Path.Combine(fileInfo.Directory.FullName, altFileName), FileMode.Create, FileAccess.Write, FileShare.Read);
            }
            Stream = new StreamWriter(stream);
            var thread = new Thread(WriteToFile);
            Items = new List<PerformanceLogItem>();
            thread.Start();

            Configured = true;
        }

        /// <summary>
        /// Writes to file.
        /// </summary>
        private void WriteToFile()
        {
            try
            {
                while (true)
                {
                    var list = new List<PerformanceLogItem>();
                    lock (Synk)
                    {
                        list.AddRange(Items);
                        Items.Clear();
                    }

                    if (list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            Stream.WriteLine("{0};{1};{2};{3};{4}",
                                item.CreationDate.ToString("HH:mm:ss.fff"),
                                item.DurationMilliseconds,
                                item.TypeName,
                                item.MethodName,
                                item.Section);
                        }
                        Stream.Flush();
                    }
                    Thread.Sleep(cFlushToFileIntervalMillis);
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError("Exception in performance logger thread", ex);
            }
        }

        /// <summary>
        /// Logs an item
        /// </summary>
        /// <param name="durationMilliseconds">Duration in milliseconds</param>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="section">The section.</param>
        public void Log(long durationMilliseconds, string typeName, string methodName, string section = "")
        {
            CheckConfigured();

            if (Enabled)
            {
                var item = new PerformanceLogItem
                {
                    DurationMilliseconds = durationMilliseconds,
                    TypeName = typeName,
                    MethodName = methodName,
                    CreationDate = DateTime.Now,
                    Section = section
                };
                lock (Synk)
                {
                    Items.Add(item);
                }
            }
        }

        private void CheckConfigured()
        {
            if (!Configured)
            {
                throw new InvalidOperationException("Instance has not been configured");
            }
        }
    }
}
