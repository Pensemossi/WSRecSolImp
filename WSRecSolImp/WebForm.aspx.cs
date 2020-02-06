using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WSRecSolImp
{
    public partial class WebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            WSInicial webService = new WSInicial();
            List<int> lstIntegers = new List<int> { 5, 6, 7 };
            //Label1.Text = "Output of WebService: " + webService.Add(lstIntegers).ToString();

        }
    }
}