using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MySql.Data.MySqlClient;
using System.Threading;

namespace SendMsgToLine
{
    public partial class Form1 : Form
    {
        //Import Windows API
        
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpwindowName);
        [DllImport ( "user32.dll", EntryPoint = "FindWindowEx", SetLastError = true )]
        private static extern IntPtr FindWindowEx( IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow );
        [DllImport ( "user32.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto )]
	    private static extern int SendMessage( IntPtr hwnd, uint wMsg, int wParam, int lParam );
        [DllImport ( "user32.dll", EntryPoint = "SetForegroundWindow", SetLastError = true )]
        private static extern void SetForegroundWindow( IntPtr hwnd );
        [DllImport ( "user32.dll", EntryPoint = "GetWindowText", SetLastError = true)]
        private static extern Int32 GetWindowText(Int32 hWnd, StringBuilder lpsb, Int32 count);
   //     [DllImport("user32.dll", EntryPoint = "GetWindowLong", SetLastError = true)]
   //     private static extern IntPtr GetWindowLong(IntPtr hwnd,Int32 nIndex);
        //宣告
        String conString = "SERVER = localhost; DATABASE = linemsg; User ID = root; PASSWORD = mysql@vghtc;";
        string user_name = "";
        string SendMsg = "" ;
        int dlcount = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Tick += new EventHandler(Timer1_Tick);
            timer1.Interval = 3000;
        }

        private void Timer1_Tick(object Sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToString();
            GetMsgData();
            if (dlcount >= 3) 
            {
                textBox3.Text = "";
                dlcount = 0;
               
            }
            else
            dlcount = dlcount + 1;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            insertMsg(textBox2.Text,textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled == true)
            {
                timer1.Enabled = false;
            }
            else
            {
                timer1.Enabled = true;
            }
        }

        public Boolean insertMsg(String user , String content)
        {
            

            using (MySqlConnection connection = new MySqlConnection(conString))
            {
                try
                {
                String insertCmd = "INSERT INTO msg(to_user_name,msg_content) VALUES(@to_user,@content_body)";
                MySqlCommand cmd = new MySqlCommand(insertCmd, connection);
                cmd.Parameters.AddWithValue("@to_user", user);
                cmd.Parameters.AddWithValue("@content_body", content);
               // cmd.Parameters["@to_user"].Value = user;
               // cmd.Parameters["@contend_body"].Value = contend;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("OK!!");
                    return true;

                }
                catch(Exception ex)
                {
                    MessageBox.Show("Insert fail" + ex.ToString());
                    return false;
                }
            }
        }

        public void GetMsgData()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(conString);
                MySqlConnection connUp = new MySqlConnection(conString);
                connection.Open();
                connUp.Open();
                String command = "Select * from msg where is_send = 0";
                MySqlCommand cmd = new MySqlCommand(command, connection);

                using (MySqlDataReader data_reader = cmd.ExecuteReader())
                {
                    if (data_reader.HasRows)
                    {
                        while (data_reader.Read())
                        {
                            String msg_id = data_reader.GetValue(0).ToString();
                            String name = data_reader.GetValue(1).ToString();
                            String content = data_reader.GetValue(2).ToString();


                            if (SendtoLine(name, content) == true)
                            {
                                textBox3.Text += "TO:" + name + "訊息:" + content + "發送成功!\r\n\r\n";
                                String update_is_send = "UPDATE msg SET is_send = 1 WHERE msg_id =" + msg_id;
                                MySqlCommand cmd_update = new MySqlCommand(update_is_send, connUp);
                                cmd_update.ExecuteNonQuery();
                            }
                            else
                            {
                                textBox3.Text += "Error:未開啟視窗:" + user_name + "\r\n\r\n";
                            }
                           
                        }
                        data_reader.NextResult();
                        
                    }
                    else
                    {
                        textBox3.Text += DateTime.Now.ToString() + "無待發送資料\r\n\r\n";
                    }
                    
                    cmd.Cancel();
                    data_reader.Close();
                }
            }
            catch (Exception ex)
            {
                textBox3.Text = ex.ToString();
            }
        }


        public Boolean SendtoLine(String user ,String msgcontent)
        {
            user_name = user;
            if (user_name == "")
            {
                user_name = "VghtcGroup";
            }
            IntPtr hwndCalc = FindWindow(null, user_name);
            SetForegroundWindow(hwndCalc);
            Thread.Sleep(100);
            if (hwndCalc != IntPtr.Zero)
            {
                SendMsg = msgcontent.Replace("\r\n", "+{Enter 1}");
                SendKeys.SendWait(SendMsg + "{ENTER}");
                return true;

                /*  開發測試用程式碼--暫存
                StringBuilder Window_name = new StringBuilder(256);
                GetWindowText(hwndCalc.ToInt32(), Window_name, Window_name.Capacity);

                if (String.Compare(Window_name.ToString(), user_name,true) == 0)
                {
                    //取代 換行字元
                   // textBox3.Text += "activeWindow:" + Window_name.ToString() + "\r\n";
                    
                    SendMsg = msgcontent.Replace("\r\n", "+{Enter 1}");
                    SendKeys.SendWait(SendMsg + "{ENTER}");
                    return true;
                }
                else
                {
                    return false;
                }
                 */
            }

            else
            {
                return false;
            }
        }
    }
}
