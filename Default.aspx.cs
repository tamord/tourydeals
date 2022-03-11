using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyTouristBook
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["aid"] != null)
            {
                HttpCookie myCookie2 = new HttpCookie("tourydealsrefer");
                myCookie2.Value = Request.QueryString["aid"].ToString();
                myCookie2.Expires = DateTime.Now.AddDays(90);
                Response.Cookies.Add(myCookie2);
            }

            if (Request.Cookies["tourydeals"] != null)
            {
                Response.Redirect("~/center");

            }


            HttpCookie myCookie3 = new HttpCookie("tourydeals");

            string username = "guesttamord6455";
            myCookie3.Value = username;
            myCookie3.Expires = DateTime.Now.AddDays(120);
            Response.Cookies.Add(myCookie3);
            Response.Redirect("~/center");
        }
    
    }
}