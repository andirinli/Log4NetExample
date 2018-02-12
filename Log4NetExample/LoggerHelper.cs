using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System.Windows;
using System.IO;

namespace Log4NetExample
{
    public static class LoggerHelper
    {
        #region Private Variables
        private static string logFilePath;
        private static readonly ILog Log;
        private static string appDir;
        #endregion
        #region Construct
        static LoggerHelper()
        {
            appDir = AppDomain.CurrentDomain.BaseDirectory;
            var type = Application.Current.GetType();
            Log = LogManager.GetLogger(type);


            ///For File Log
            ConfigureFileLog();




        }
        #endregion
        #region Private Methods
        #region File Log
        private static IAppender GetFileAppender(string logFile)
        {
            var layout = new PatternLayout("[%thread] [%date] [%level] [%message] %newline %exception %newline");
            layout.ActivateOptions();

            var appender = new RollingFileAppender();
            appender.AppendToFile = true;
            appender.File = logFile;
            appender.Layout = layout;
            /// ek dosya en fazla kaç tane olacak örn : 11-53-18.log.1, 11-53-18.log.2 , ....
            /// şu durumda 100 * 5 MB = 500 MB her bir log için üst limit. 
            appender.MaxSizeRollBackups = 100;
            appender.MaximumFileSize = "5MB";
            appender.RollingStyle = RollingFileAppender.RollingMode.Size;

            ///https://stackoverflow.com/a/12175475/7224210
            appender.LockingModel = new LogLockMechanism();

            appender.ActivateOptions();

            return appender;

        }
        private static void ConfigureFileLog()
        {
            var dirPath = Path.Combine(appDir, "Logs");
            Directory.CreateDirectory(dirPath);

            logFilePath = Path.Combine(dirPath, "logFile.log");
            var fileAppender = GetFileAppender(logFilePath);
            BasicConfigurator.Configure(fileAppender);

            ((Hierarchy)LogManager.GetRepository()).Root.Level = Level.All;
        }
        #endregion



        #endregion
        #region Public Methods
        public static void Info(object message, Exception ex = null)
        {
            Log.Info(message, ex);
        }
        public static void Debug(object message, Exception ex = null)
        {
            Log.Debug(message, ex);
        }
        public static void Warning(object message, Exception ex = null)
        {
            Log.Warn(message, ex);
        }
        public static void Error(object message, Exception ex = null)
        {
            Log.Error(message, ex);
        }
        public static void Fatal(object message, Exception ex = null)
        {
            Log.Fatal(message, ex);
        }
        #endregion
        #region Internal Class
        public class LogLockMechanism : FileAppender.MinimalLock
        {
            public override void ReleaseLock()
            {
                base.ReleaseLock();

                var logFile = new FileInfo(CurrentAppender.File);
                if (logFile.Exists && logFile.Length <= 0)
                {
                    logFile.Delete();
                }
            }

        }
        #endregion
    }
}

