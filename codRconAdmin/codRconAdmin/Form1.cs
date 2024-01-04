using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using WindowsInput.Native;
using WindowsInput;

namespace codRconAdmin
{
    public partial class Form1 : Form
    {
        // The function that brings the window to foreground
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        //[DllImport("kernel32.dll")]
        //private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);

        //[DllImport("kernel32.dll")]
        //private static extern IntPtr GetConsoleWindow();

        private const int ATTACH_PARENT_PROCESS = -1;

        Process consoleAppProcess = new Process();
        InputSimulator InpSim = new InputSimulator();

        private bool loggedIn = false;
        private char exit = 'N';

        public Form1()
        {
            InitializeComponent();
            openConsoleApp();
            IPbox.Text = "IP field";
            PORTbox.Text = "PORT field";
            rconPWbox.Text = "Rcon password field";

            serverInfoList.AllowDrop = false;
        }

        private void openConsoleApp()
        {
            // Retrieve the Windows Form application directory
            string currentDirectory = Path.GetDirectoryName(Application.ExecutablePath);

            // Let's start the console application
            consoleAppProcess.StartInfo.FileName = Path.Combine(currentDirectory, "cod4RconProgramC.exe");
            //consoleAppProcess.StartInfo.UseShellExecute = false;
            //consoleAppProcess.StartInfo.RedirectStandardInput = true;
            consoleAppProcess.Start();

            // Wait a little bit while for the console application to initialize
            System.Threading.Thread.Sleep(1000);

            // Attach the console window to the Windows Form window
            AttachConsole(consoleAppProcess.Id);

            // Do this step only if you don't have a console window yet
            /*if (GetConsoleWindow() == IntPtr.Zero)
            {
                AllocConsole();
            }*/
        }

        private void bringForegroundCMD()
        {
            if (consoleAppProcess != null && !consoleAppProcess.HasExited)
            {
                // Put foreground the window
                //if( consoleAppProcess.MainWindowTitle == "cod4RconProgramC" )
                SetForegroundWindow(consoleAppProcess.MainWindowHandle);
            }
        }

        private void AddServerInfoItem(string key, StreamReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith(key))
                {
                    ListViewItem item = new ListViewItem(new string[] { key, line.Substring(21) });
                    serverInfoList.Items.Add(item);
                    if (key == "sv_hostname")
                        this.Text = "IW rcon tool | " + line.Substring(21);
                    break;
                }
            }
        }

        private void getInformations()
        {
            string executablePath = AppDomain.CurrentDomain.BaseDirectory;
            string rconInfo = "rconAdm.txt";
            string infoPath = Path.Combine(executablePath, rconInfo);

            if (System.IO.File.Exists(infoPath))
            {
                using (StreamReader Read = new StreamReader(infoPath))
                {
                    AddServerInfoItem("sv_maxclients", Read);
                    AddServerInfoItem("fs_game", Read);
                    AddServerInfoItem("version", Read);
                    AddServerInfoItem("shortversion", Read);
                    AddServerInfoItem("build", Read);
                    AddServerInfoItem("branch", Read);
                    AddServerInfoItem("sv_privateClients", Read);
                    AddServerInfoItem("sv_hostname", Read);
                    AddServerInfoItem("sv_minPing", Read);
                    AddServerInfoItem("sv_maxPing", Read);
                    AddServerInfoItem("sv_maxRate", Read);
                    AddServerInfoItem("sv_floodprotect", Read);
                    AddServerInfoItem("sv_pure", Read);

                    Read.Close();
                    System.IO.File.WriteAllText(infoPath, string.Empty);
                }
            }
        }

        private void updSerInfoBtn_Click(object sender, EventArgs e)
        {
            serverInfoList.Items.Clear(); // Empty the serverInfoList

            if (consoleAppProcess == null && consoleAppProcess.HasExited)
            {
                MessageBox.Show("The command prompt must be open!");
                return;
            }

            if (loggedIn)
            {
                bringForegroundCMD();
                InpSim.Keyboard.TextEntry("..");
                System.Threading.Thread.Sleep(500);
                InpSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

                System.Threading.Thread.Sleep(500);

                InpSim.Keyboard.TextEntry(exit);
                System.Threading.Thread.Sleep(500);
                InpSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

                // Bring foreground the Windows Form application
                SetForegroundWindow(this.Handle);
                MessageBox.Show("Server informations updated.");
                getInformations();
            }
            else
            {
                MessageBox.Show("Please fill IP, PORT and password fields!");
            }
        }

        private void quitBtn_Click(object sender, EventArgs e)
        {
            if (consoleAppProcess != null && !consoleAppProcess.HasExited)
            {
                /*consoleAppProcess.CloseMainWindow();  // Let's try the CloseMainWindow method
                if (!consoleAppProcess.WaitForExit(1000))
                {
                    // If CloseMainWindow does not close it, we use Kill
                    consoleAppProcess.Kill();
                }*/

                Form popupForm = new Form();
                popupForm.Text = "Do you want to exit?";
                popupForm.Size = new Size(450, 200);
                popupForm.StartPosition = FormStartPosition.CenterParent;

                Button buttonYes = new Button();
                buttonYes.Text = "Yes";
                buttonYes.Location = new Point(60, 60);
                buttonYes.Click += (sender, e) =>
                {
                    exit = 'Y';
                    popupForm.Close(); // Close popup
                };
                popupForm.Controls.Add(buttonYes);

                Button buttonNo = new Button();
                buttonNo.Text = "No";
                buttonNo.Location = new Point(295, 60);
                buttonNo.Click += (sender, e) =>
                {
                    exit = 'N';
                    popupForm.Close(); // Close popup
                };
                popupForm.Controls.Add(buttonNo);
                popupForm.ShowDialog();

                if (exit == 'N')
                    return;

                bringForegroundCMD();
                InpSim.Keyboard.TextEntry(".");
                System.Threading.Thread.Sleep(1000);
                InpSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

                System.Threading.Thread.Sleep(500);

                InpSim.Keyboard.TextEntry(exit);
                System.Threading.Thread.Sleep(500);
                InpSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

                System.Threading.Thread.Sleep(500);

                Environment.Exit(0);
            }
        }

        private void rconLoginBtn_Click(object sender, EventArgs e)
        {
            string ip = IPbox.Text;
            string port = PORTbox.Text;
            string pw = rconPWbox.Text;

            if (consoleAppProcess == null && consoleAppProcess.HasExited)
            {
                MessageBox.Show("The command prompt must be open!");
                return;
            }

            //if ((!string.IsNullOrEmpty(ip) || ip != "IP field") && (!string.IsNullOrEmpty(port) || port != "PORT field") && (!string.IsNullOrEmpty(pw) || pw != "Rcon password field"))
            if (ip != "IP field" && port != "PORT field" && pw != "Rcon password field")
            {
                bringForegroundCMD();
                InpSim.Keyboard.TextEntry(ip);
                System.Threading.Thread.Sleep(500);
                InpSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

                System.Threading.Thread.Sleep(500);

                InpSim.Keyboard.TextEntry(port);
                System.Threading.Thread.Sleep(500);
                InpSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

                System.Threading.Thread.Sleep(500);

                InpSim.Keyboard.TextEntry(pw);
                System.Threading.Thread.Sleep(500);
                InpSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

                loggedIn = true;
                // Bring foreground the Windows Form application
                SetForegroundWindow(this.Handle);
                MessageBox.Show("Login successfully!");
                getInformations();
            }
            else
            {
                MessageBox.Show("Please fill IP, PORT and password fields!");
            }
        }

        private void sendCMDbtn_Click(object sender, EventArgs e)
        {
            //    InputSimulator InpSim = new InputSimulator();
            // Take the command from the textbox
            string command = CMDbox.Text;

            if (consoleAppProcess == null && consoleAppProcess.HasExited)
            {
                MessageBox.Show("The command prompt must be open!");
                return;
            }

            // If have a command, send it to the console application
            if (!string.IsNullOrEmpty(command) && loggedIn)
            {
                bringForegroundCMD();
                // Simulate keystrokes in the console
                //InputSimulator.SimulateTextEntry(command + "{ENTER}");
                InpSim.Keyboard.TextEntry(command);
                System.Threading.Thread.Sleep(1000);
                InpSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

                System.Threading.Thread.Sleep(500);

                InpSim.Keyboard.TextEntry(exit);
                System.Threading.Thread.Sleep(500);
                InpSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

                SetForegroundWindow(this.Handle);
                MessageBox.Show("Command sent successfully!");
            }
            else if (!loggedIn)
            {
                MessageBox.Show("Please fill IP, PORT and password fields and click Rcon Login!");
            }
            else if (string.IsNullOrEmpty(command))
            {
                MessageBox.Show("Please fill command field!");
            }
        }
    }
}