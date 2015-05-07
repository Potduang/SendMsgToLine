using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace WebApplication1
{

    
    public partial class WebForm1 : System.Web.UI.Page
    {
        //連接資料庫字串
        String conString = "SERVER = localhost; DATABASE = linemsg; User ID = user_name; PASSWORD = password;";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(conString))
            {
                try
                {
                    String insertCmd = "INSERT INTO usr(usr_name,card_no) VALUES(@user,@card)";
                    MySqlCommand cmd = new MySqlCommand(insertCmd, connection);
                    cmd.Parameters.AddWithValue("@user", TextBox1.Text);
                    cmd.Parameters.AddWithValue("@card", TextBox2.Text);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    Label1.Text = "新增成功!!";
                }
                catch(Exception ex)
                {
                    Label1.Text = ex.ToString();
                }
            }

        }
    }
}