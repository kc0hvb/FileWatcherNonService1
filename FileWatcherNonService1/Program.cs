using System;
using System.IO;
using System.Configuration;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherNonService1
{
    class Program
    {
        private void MinimizeToTray()
        {

            NotifyIcon trayIcon = new NotifyIcon();
            trayIcon.Text = "My application";
            trayIcon.Icon = TheIcon

                // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;

            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.
        }
        private void Form1_Resize(object sender, System.EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
                Hide();
        }

        private void notifyIcon1_DoubleClick(object sender,
                                     System.EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        static void Main()
        {

            try
            {
                while (true)
                {
                    processFileWatcher();
                }
            }
            catch(Exception ex)
            {
                errorLogCreation(ex);
            }
        }
        static void processFileWatcher()
        {
                
                string sSource = ConfigurationManager.AppSettings["PAK_Location"];
                string sTarget = ConfigurationManager.AppSettings["PAK_Target_Location"];
                //string[] files = System.IO.Directory.GetFiles(sSource.ToString, "*.pak", SearchOption.AllDirectories);

                string[] fileEntries = System.IO.Directory.GetFiles(sSource, "*.*", System.IO.SearchOption.AllDirectories);

                if (!Directory.Exists(sTarget))
                {
                    Directory.CreateDirectory(sTarget);
                }

                foreach (string fileName in fileEntries)
                {
                    if (fileName.Contains(".pak"))
                    {
                        string sFileName = Path.GetFileName(fileName);
                        string sFileNameDest = sTarget + '\\' + sFileName;
                        bool sFileNameDestExist = File.Exists(sFileNameDest);
                        if (sFileNameDestExist)
                        {
                            FileInfo fFileInfoSource = new FileInfo(fileName);
                            FileInfo fFileInfoDest = new FileInfo(sFileNameDest);
                            if (fFileInfoSource.LastWriteTimeUtc > fFileInfoDest.LastWriteTimeUtc)
                            {
                                File.Copy(fileName, sFileNameDest, true);
                                Console.WriteLine($"File: {sFileName} was updated.");
                            }
                            else
                            {
                                
                            }
                        }
                        else
                        {
                            File.Copy(fileName, sFileNameDest, true);
                            Console.WriteLine($"File: {sFileName} did not exist but exists now.");
                        }   
                    }
                }

                Thread.Sleep(Int32.Parse(ConfigurationManager.AppSettings["Sleep_Time"]));
                return;
            }
            

            static void errorLogCreation(Exception ex)
            {
                string sErrorFilePath = AppDomain.CurrentDomain.BaseDirectory + $"Error Log {DateTime.Today.Millisecond}.txt";
                using (StreamWriter file =
                new StreamWriter(sErrorFilePath))
                {
                    file.WriteLine(ex);
                }
            }
        }
    
}
