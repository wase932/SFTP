using System;
using System.Linq;
using Renci.SshNet;
using System.IO;

namespace SFTP
{
    class Program
    {
        private static string[] _arguments;
        private static string _host;
        private static string _user;
        private static string _password;
        private static string _remoteFile;
        private static string _localFile;
        private static string _loggerFile;
        private static string _action;

        static void Main(string[] args)
        {
            _arguments = Environment.GetCommandLineArgs();
            
            ReadParameters(_arguments);

            using (var client = GetClient())
            {
                LogEvent("Connecting to Remote Host", "...");
                try
                {
                    client.Connect();
                }
                catch (Exception ex)
                {
                    LogEvent("ERROR Connecting to Remote Host", ex.Message);
                    Environment.Exit(1);
                }
                LogEvent("SUCCESS", "Connection Established");
                
                if (_action.ToLower() == Action.download.ToString())
                {
                    LogEvent("Attempting to download file", "...");
                    try
                    {
                        client.DownloadFile(_remoteFile, new StreamWriter(_localFile).BaseStream);
                    }
                    catch (Exception ex)
                    {
                        LogEvent("ERROR Downloading " + _remoteFile + " from " + _host, ex.Message);
                        Environment.Exit(1);
                    }
                    finally
                    {
                        client.Disconnect();
                    }

                    LogEvent("SUCCESS", "Successfully downloaded " + _remoteFile + " to " + _localFile);
                }
                
                if (_action.ToLower() == Action.upload.ToString())
                {
                    LogEvent("Attempting to Upload file", "...");
                    try
                    {
                        client.UploadFile(new StreamReader(_localFile).BaseStream, _remoteFile);
                    }
                    catch (Exception ex)
                    {
                        LogEvent("ERROR Uploading " + _localFile + " to " + _host, ex.Message);
                        Environment.Exit(1);
                    }
                    finally
                    {
                        client.Disconnect();
                    }

                    LogEvent("SUCCESS", "Successfully uploaded " + _localFile + " to " + _remoteFile);
                }
            }
        }

        private static SftpClient GetClient()
        {
            var connectionInfo = new ConnectionInfo(_host, _user, new PasswordAuthenticationMethod(_user, _password));
            return new SftpClient(connectionInfo);
        }

        static void SftpConnect(string url, string username, string password, string remotefilepath, string localFilePath, string loggerFilePath)
        {
            var connectionInfo = new ConnectionInfo(url, username, new PasswordAuthenticationMethod(username, password));
            using (var client = new SftpClient(connectionInfo))
            {
                client.Connect();
                var localStream = new StreamWriter(localFilePath);
                Stream stream = localStream.BaseStream;
                client.DownloadFile(remotefilepath, stream);
            }
        }
        public static void ReadParameters(string[] args)
        {
            var arguments = args.ToList();
            try
            {
                _loggerFile = arguments[arguments.IndexOf("-logger") + 1];
                _host = arguments[arguments.IndexOf("-host") + 1];
                _user = arguments[arguments.IndexOf("-user") + 1];
                _password = arguments[arguments.IndexOf("-pwd") + 1];
                _remoteFile = arguments[arguments.IndexOf("-remote") + 1];
                _localFile = arguments[arguments.IndexOf("-local") + 1];
                _action = arguments[arguments.IndexOf("-action") + 1];
            }
            catch (Exception ex)
            {
                LogEvent("ERROR while trying to read Arguments", ex.Message);
                LogEvent("Arguments Supplied", string.Join(",", arguments));
                Environment.Exit(1);
            }
            LogEvent("Arguments Supplied", string.Join(",", arguments));
        }

        private static void LogEvent(string eventName, string message)
        {
            using (StreamWriter sw = File.AppendText(_loggerFile))
            {
                sw.WriteLine(DateTime.Now + " " + eventName + ": " + message);
            }
        }
        private enum Action
        {
            upload, download
        }
    }
}
