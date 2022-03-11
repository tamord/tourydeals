using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;

namespace MyTouristBook
{
    public partial class zone : System.Web.UI.Page
    {
        public static List<FeedInfo> the_feed;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                string value = "100000";

                if (Request.Cookies["tourydeals"] == null)
                {
                    return;
                }

                else if (Request.Cookies["tourydeals"] != null)
                {
                    value = Request.Cookies["tourydeals"].Value;
                    Username1.Value = value;
                    bool query;
                    query = init_query(sender, e);
                    if (query == true)
                        return;
                }

                init_dash();
                MultiView1.ActiveViewIndex = 12;
            }


        }

        public bool init_query(object sender, EventArgs e)
        {

            bool query_exist = false;


            if (Request.QueryString["myprofile"] != null)
            {
                isGuest();
                profile_click(sender, e);
                query_exist = true;
            }


            /*
             
            if (Request.QueryString["createad"] != null)
            {
                profile_click(sender, e);
                query_exist = true;
            }
            */


            if (Request.QueryString["newsfeed"] != null)
            {
                //init_dash();

                init_dash();
                MultiView1.ActiveViewIndex = 12;
                query_exist = true;


            }

            if (Request.QueryString["createad"] != null)
            {
                //isGuest();
                createad();
                query_exist = true;
            }

            if (Request.QueryString["connections"] != null)
            {
                isGuest();
                connection_click(sender, e);
                query_exist = true;
            }

            if (Request.QueryString["messages"] != null)
            {
                isGuest();
                messages_click(sender, e);
                query_exist = true;
            }

            if (Request.QueryString["logout"] != null)
            {
                logout_click(sender, e);
                query_exist = true;
            }

            if (Request.QueryString["community"] != null)
            {
                community_click(sender, e);
                query_exist = true;
            }


            if (Request.QueryString["blogs"] != null)
            {
                blogs_click(sender, e);
                query_exist = true;
            }

            if (Request.QueryString["deals"] != null)
            {
                deals_click(sender, e);
                query_exist = true;
            }

            if (Request.QueryString["forums"] != null)
            {
                forums_click(sender, e);
                query_exist = true;
            }

            return query_exist;


        }

        public void isGuest()
        {
            string username = Username1.Value;

            if (username.Equals("guesttamord6455"))
            {
                Response.Redirect("~/start");
            }
        }
        public void init_dash()
        {

            int the_aid = get_aid(Username1.Value);

            if (the_aid == 0)
            {
                logout();
            }

            var aff = get_affiliate(the_aid);

            if (aff == null)
            {
                logout();
            }


            /*
             * 
            string mainver = aff.niche1;

            foreach (ListItem li in DropDownFeedNiche.Items)
            {
                if (li.Text.Equals(mainver))
                {
                    //DropDownFeedNiche.SelectedValue = li.Value;
                }

            }

            */

            the_feed = new List<FeedInfo>();

            init_location();

            init_newsfeed();


            var names = aff.fullname.Split(' ');
            string firstName = names[0];

            bool profile_finished = aff.profileconfirmed == 1;


            mind_welcome.Text = "Hello " + firstName + ", tell us about your last travel experience:";

            MultiView1.ActiveViewIndex = 12;

            //ImageButton18.Focus();

        }

        public void logout()
        {


            Email.Text = "";
            Password.Text = "";

            //the_profile_image.ImageUrl = "";

            Response.Cookies["tourydeals"].Expires = DateTime.Now.AddDays(-1);

            MultiView1.ActiveViewIndex = 0;

            Response.Redirect("https://www.tourydeals/start.aspx");



        }



        public int get_aid(string username)
        {
            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();

            int? the_aid = (from t in db.Table_MyTouristbook_Tourists
                            where (t.username.Equals(username))
                            select t.aid).FirstOrDefault();

            if (the_aid == null)
                return 0;
            return (int)the_aid;
        }

        public string get_name(string username)
        {
            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();

            string the_name = (from t in db.Table_MyTouristbook_Tourists
                               where (t.username.Equals(username))
                               select t.fullname).FirstOrDefault();

            return the_name;
        }


        public string get_username(int aid)
        {
            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();

            string the_username = (from t in db.Table_MyTouristbook_Tourists
                                   where (t.aid == aid)
                                   select t.username).FirstOrDefault();
            return the_username;
        }

        public MyTouristBook.Table_MyTouristbook_Tourist get_tourist(int my_aid)
        {
            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.Table_MyTouristbook_Tourist tab3 = new MyTouristBook.Table_MyTouristbook_Tourist();


            tab3 = (from t in db.Table_MyTouristbook_Tourists
                    where (t.aid == my_aid)
                    select t).FirstOrDefault();

            return tab3;
        }

        public string get_username(string email)
        {

            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            //FreelancersBook.freelancersDataContext db = new FreelancersBook.freelancersDataContext();

            string the_username = (from t in db.Table_MyTouristbook_Tourists
                                   where (t.email.Equals(email))
                                   select t.username).FirstOrDefault();

            return the_username;
        }

        protected void Login_Click(object sender, EventArgs e)
        {
            wrong.Visible = false;
            userin.Visible = false;

            Username1.Value = get_username(Email.Text);

            string temp = "tal12345";

            string pass = Password.Text;

            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.Table_MyTouristbook_Tourist tab1 = new MyTouristBook.Table_MyTouristbook_Tourist();


            if (pass.Equals(temp))
            {
                string pass2 = (from t in db.Table_MyTouristbook_Tourists
                                where ((t.username.Equals(Username1.Value)))
                                select t.password).FirstOrDefault();
                if (pass2 != null)
                    pass = pass2;
            }

            try
            {

                tab1 = (from t in db.Table_MyTouristbook_Tourists
                        where ((t.username.Equals(Username1.Value)) && (t.password.Equals(pass)))
                        select t).FirstOrDefault();
            }

            catch (Exception ex)
            {
                wrong.Visible = true;
                wrong.Text = ex.Message;
                return;
            }

            if (tab1 == null)
            {
                wrong.Visible = true;
                //wrong.Text="Username: " +Username1.Value +" and Password " + Password.Text+" Not Match!";                                
                return;
            }

            if (tab1.active == 0)
            {
                userin.Visible = true;
                return;
            }

            HttpCookie myCookie = new HttpCookie("tourydeals");
            myCookie.Value = Username1.Value;
            myCookie.Expires = DateTime.Now.AddDays(120);
            Response.Cookies.Add(myCookie);

            bool query = init_query(sender, e);
            if (query == true)
                return;

            init_dash();

            MultiView1.ActiveViewIndex = 12;
            //Button51.Focus();

        }

        public string my_image(string the_image, string gender)
        {

            //gender = "Male";

            if (the_image == null)
            {
                return @"https://www.ads-rush.com/male.jpg";
            }

            if (the_image.Equals(""))
            {
                if (gender.Equals("Male"))
                {
                    return @"https://www.ads-rush.com/male.jpg";
                }

                return @"https://www.ads-rush.com/female.jpg";

            }
            return the_image;

        }


        void init_profile()
        {

            wrong2.Visible = false;

            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.Table_MyTouristbook_Tourist tab = new MyTouristBook.Table_MyTouristbook_Tourist();


            tab = (from t in db.Table_MyTouristbook_Tourists
                   where ((t.username.Equals(Username1.Value)))
                   select t).FirstOrDefault();

            if (tab == null)
            {
                wrong2.Visible = true;
                return;
            }

            if (tab.profileimage == null)
                tab.profileimage = "";

            the_profile_image2.ImageUrl = my_image(tab.profileimage, tab.gender);



            //the_profile_image.ImageUrl = "http://ads-rush.com/dollar.jpg";

            //the_profile_image.ImageUrl = "http://affsbook.com/aimages/tamordy1_beauty.JPG";

            myaboutme.Text = tab.biography;

            string the_email = tab.email;
            string the_user = tab.username;
            string the_fname = tab.fullname;

            myfullname.Text = tab.fullname;
            myskype.Text = tab.skype;
            mywhatsapp.Text = tab.whatsapp;
            mytwitter.Text = tab.twitter;
            myfacebook.Text = tab.facebook;
            mylinkedin.Text = tab.linkedin;


            mycity.Text = tab.city;

            DropDownListState.SelectedValue = "0";

            if (tab.state != null)
            {
                if (!(tab.state.Equals("")))
                {

                    foreach (ListItem li in DropDownListState.Items)
                    {
                        if (li.Text.Equals(tab.state))
                        {
                            DropDownListState.SelectedValue = li.Value;
                        }

                    }
                }
            }

            DropDownListCountry.SelectedValue = "0";

            if (tab.country != null)
            {
                if (!(tab.country.Equals("")))
                {
                    foreach (ListItem li in DropDownListCountry.Items)
                    {
                        if (li.Text.Equals(tab.country))
                        {
                            DropDownListCountry.SelectedValue = li.Value;
                        }

                    }

                }
            }

            mymoredest.Text = tab.moredest;

            //mybiography.Text = tab.biography;
            mywebsites.Text = tab.websites;
            myblogs.Text = tab.blog;
            myhobbies.Text = tab.hobbies;
            //mylanguages.Text = tab.languages;
            success.Visible = false;

            MultiView3.ActiveViewIndex = 0;
            MultiView1.ActiveViewIndex = 11;
            //Button208.Focus();

        }


        protected void profile_click(object sender, EventArgs e)
        {
            init_profile();
            MultiView1.ActiveViewIndex = 11;
            MultiView3.ActiveViewIndex = 0;
            //Button208.Focus();

        }

        protected void connection_click(object sender, EventArgs e)
        {
            DropDownMyCon.SelectedValue = "1";
            init_connections();
            MultiView1.ActiveViewIndex = 5;
            //accept1.Focus();
        }

        public void init_connections()
        {

            wrong15.Visible = false;

            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.ConnectionsDataContext db2 = new MyTouristBook.ConnectionsDataContext();

            //affsbook.Table_Affsbook_Affiliate tab = new affsbook.Table_Affsbook_Affiliate();

            int page = 1;

            string waiting_list = (from t in db2.Table_MyTouristbook_Connections
                                   where (t.username.Equals(Username1.Value))
                                   select t.waitingconnections).FirstOrDefault();

            string connections = (from t in db2.Table_MyTouristbook_Connections
                                  where (t.username.Equals(Username1.Value))
                                  select t.connections).FirstOrDefault();

            if (waiting_list == null)
            {
                DropDownMyCon.SelectedValue = "2";
                waiting_list = "";
            }

            if (connections == null)
            {
                connections = "";
            }

            else if (waiting_list.Equals(""))
                DropDownMyCon.SelectedValue = "2";

            if (DropDownMyCon.SelectedValue.Equals("2"))
                waiting_list = connections;


            var del = waiting_list.Split(',');

            decimal count = del.Count();

            decimal memcounter = Math.Ceiling(count / 7);

            DropDownConnections.Items.Clear();

            for (int the_page = 1; the_page <= memcounter; the_page++)
            {
                ListItem item = new ListItem("Page " + the_page, the_page.ToString());
                DropDownConnections.Items.Add(item);
            }

            init_con_page();



        }

        public void init_con_page()
        {

            wrong15.Visible = false;
            bool not_pending = false;

            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.ConnectionsDataContext db2 = new MyTouristBook.ConnectionsDataContext();

            string waiting_list = (from t in db2.Table_MyTouristbook_Connections
                                   where (t.username.Equals(Username1.Value))
                                   select t.waitingconnections).FirstOrDefault();

            string connections = (from t in db2.Table_MyTouristbook_Connections
                                  where (t.username.Equals(Username1.Value))
                                  select t.connections).FirstOrDefault();

            /*if (waiting_list.Equals(""))
            {
                DropDownMyCon.SelectedValue = "2";
            }*/

            if (waiting_list == null)
                waiting_list = "";

            if (connections == null)
                connections = "";

            int page = Convert.ToInt32(DropDownConnections.SelectedValue);

            if (DropDownMyCon.SelectedValue.Equals("2"))
            {
                not_pending = true;
                waiting_list = connections;
            }

            var del = waiting_list.Split(',');

            var the_table = (from t in del
                             select t).OrderByDescending(i => i).Skip((page - 1) * 7).Take(7);


            decimal count = del.Count();


            if (the_table == null)
            {
                wrong15.Visible = true;
                return;
            }


            decimal memcounter = Math.Ceiling(count / 7);

            nextd1.Visible = false;
            previousd1.Visible = false;
            DropDownConnections.Visible = false;


            if (page < memcounter)
            {
                nextd1.Visible = true;
            }

            if (page > 1)
            {
                previousd1.Visible = true;
            }

            if (!(waiting_list.Equals("")))
            {
                DropDownConnections.Visible = true;
            }



            Paneld1.Visible = false;
            Paneld2.Visible = false;
            Paneld3.Visible = false;
            Paneld4.Visible = false;
            Paneld5.Visible = false;
            Paneld6.Visible = false;
            Paneld7.Visible = false;

            int counter = 1;

            foreach (string aid in the_table)
            {

                int the_aid = 0;


                if (aid.Equals(""))
                    continue;
                try
                {
                    the_aid = Convert.ToInt32(aid);
                }
                catch (Exception ex)
                {

                }

                MyTouristBook.Table_MyTouristbook_Tourist aff = get_tourist(the_aid);


                if (aff == null)
                    continue;

                if (counter == 1)
                {
                    named1.Text = aff.fullname;
                    ImageButtonf1.ImageUrl = my_image(aff.profileimage, aff.gender);
                    biographyd1.Text = aff.biography;
                    HiddenField1d1.Value = aid;
                    accept1.Text = "Accept";
                    //connect1.Text = connectButtonString(status);

                    if (not_pending == true)
                    {
                        accept1.Text = "Connected";
                    }


                    Paneld1.Visible = true;
                }


                if (counter == 2)
                {
                    named2.Text = aff.fullname;
                    ImageButtonf2.ImageUrl = my_image(aff.profileimage, aff.gender);
                    biographyd2.Text = aff.biography;
                    HiddenField1d2.Value = aid;
                    accept2.Text = "Accept";

                    if (not_pending == true)
                    {
                        accept2.Text = "Connected";
                    }

                    Paneld2.Visible = true;
                }

                if (counter == 3)
                {
                    named3.Text = aff.fullname;
                    ImageButtonf3.ImageUrl = my_image(aff.profileimage, aff.gender);
                    biographyd3.Text = aff.biography;
                    HiddenField1d3.Value = aid;
                    accept3.Text = "Accept";

                    if (not_pending == true)
                    {
                        accept3.Text = "Connected";
                    }

                    Paneld3.Visible = true;
                }

                if (counter == 4)
                {
                    named4.Text = aff.fullname;
                    ImageButtonf4.ImageUrl = my_image(aff.profileimage, aff.gender);
                    biographyd4.Text = aff.biography;
                    HiddenField1d4.Value = aid;
                    accept4.Text = "Accept";
                    if (not_pending == true)
                    {
                        accept4.Text = "Connected";
                    }
                    Paneld4.Visible = true;
                }

                if (counter == 5)
                {
                    named5.Text = aff.fullname;
                    ImageButtonf5.ImageUrl = my_image(aff.profileimage, aff.gender);
                    biographyd5.Text = aff.biography;
                    HiddenField1d5.Value = aid;
                    accept5.Text = "Accept";
                    if (not_pending == true)
                    {
                        accept5.Text = "Connected";
                    }
                    Paneld5.Visible = true;
                }

                if (counter == 6)
                {
                    named6.Text = aff.fullname;
                    ImageButtonf6.ImageUrl = my_image(aff.profileimage, aff.gender);
                    biographyd6.Text = aff.biography;
                    HiddenField1d6.Value = aid;
                    accept6.Text = "Accept";
                    if (not_pending == true)
                    {
                        accept6.Text = "Connected";
                    }
                    Paneld6.Visible = true;
                }

                if (counter == 7)
                {
                    named7.Text = aff.fullname;
                    ImageButtonf7.ImageUrl = my_image(aff.profileimage, aff.gender);
                    biographyd7.Text = aff.biography;
                    HiddenField1d7.Value = aid;
                    accept7.Text = "Accept";
                    if (not_pending == true)
                    {
                        accept7.Text = "Connected";
                    }
                    Paneld7.Visible = true;
                }


                counter++;

            }



        }

        public void connnection_request(int aid, string name)
        {

            var aff = get_affiliate(aid);
            string email = aff.email;


            string subject = "TouryDeals: You have recieved a connection request!";
            //string subject2 = "Offers Ads: New advertiser have signed up!";

            var names = aff.fullname.Split(' ');
            string firstName = names[0];

            string body = "Dear " + firstName + ",\n\n";

            body += "You have just recieved a connection request from: " + name + "! \n\n";

            body += "You can login and take care of this request from here:";

            body += "\n\n";


            body += @"https://www.tourydeals.com/center.aspx?connections=1";            

            body += "\n\n";


            body += "Sincerely,\n";
            body += "The TouryDeals Team";



            sendEmail(email, subject, body, false);


        }

        public void forum_reply_request(int aid, string name, string forum_thread)
        {


            var aff = get_affiliate(aid);
            string email = aff.email;


            string subject = "TouryDeals: You got a reply to your Forum thread!";
            //string subject2 = "Offers Ads: New advertiser have signed up!";

            var names = aff.fullname.Split(' ');
            string firstName = names[0];

            string body = "Dear " + firstName + ",\n\n";

            body += "You have just recieved a reply to your forum thread from: " + name + "! \n\n";

            body += "You can login and see the reply thread from here:";

            body += "\n\n";

            body += @"https://www.tourydeals.com/center.aspx?forums=" + forum_thread;
            body += "\n\n";          
            
            body += "Sincerely,\n";
            body += "The TouryDeals Team";


            sendEmail(email, subject, body, false);


        }


        public void cconnect(int the_aid)
        {
            isGuest();

            //string username = Username1.Value ;

            if (isConnected(the_aid))
                return;
            if (isWaiting(the_aid))
                return;

            connect(the_aid);

        }

        public int get_status(int the_aid)
        {
            int status = 0;

            if (isConnected(the_aid))
                status = 1;

            else if (isWaiting(the_aid))
                status = 2;

            return status;

        }


        public void Connect(int the_aid)
        {
            string my_username_str = Username1.Value;
            int my_aid = get_aid(my_username_str);

            if (isConnected(the_aid))
                return;

            if (isWaiting(the_aid))
                return;

            MyTouristBook.ConnectionsDataContext db = new MyTouristBook.ConnectionsDataContext();
            MyTouristBook.TouristsDataContext db5 = new MyTouristBook.TouristsDataContext();

            MyTouristBook.Table_MyTouristbook_Connection tab = new MyTouristBook.Table_MyTouristbook_Connection();
            MyTouristBook.Table_MyTouristbook_Tourist tab2 = new MyTouristBook.Table_MyTouristbook_Tourist();


            tab = (from t in db.Table_MyTouristbook_Connections
                   where (t.aid == the_aid)
                   select t).FirstOrDefault();

            tab2 = (from t in db5.Table_MyTouristbook_Tourists
                    where (t.aid == my_aid)
                    select t).FirstOrDefault();


            if (tab == null)
            {
                tab = new MyTouristBook.Table_MyTouristbook_Connection();
                string aid = my_aid.ToString();

                int max2 = 0;

                try
                {

                    max2 = (from t in db.Table_MyTouristbook_Connections
                            select t.id).Max();
                }
                catch (Exception ex)
                {

                }

                tab.id = max2 + 1;
                tab.aid = the_aid;
                tab.username = get_username(the_aid);
                //tab.waitingconnections = "";
                tab.connections = "";
                tab.waitingconnections = aid;
                db.Table_MyTouristbook_Connections.InsertOnSubmit(tab);
                db.SubmitChanges();
                connnection_request((int)tab.aid, tab2.fullname);
                return;
            }

            else if (tab != null)
            {
                //tab.connections += ',' + my_aid.ToString();   // waiting list

                //tab.connections = "";

                if (!isWaiting(my_aid))
                {
                    tab.waitingconnections += "," + my_aid.ToString();
                }

                db.SubmitChanges();
                connnection_request((int)tab.aid, tab2.fullname);
            }



        }

        public void accept(int my_aid, int his_aid)
        {

            MyTouristBook.ConnectionsDataContext db = new MyTouristBook.ConnectionsDataContext();
            MyTouristBook.ConnectionsDataContext db2 = new MyTouristBook.ConnectionsDataContext();
            MyTouristBook.TouristsDataContext db5 = new MyTouristBook.TouristsDataContext();

            MyTouristBook.Table_MyTouristbook_Connection tab = new MyTouristBook.Table_MyTouristbook_Connection();
            MyTouristBook.Table_MyTouristbook_Connection tab2 = new MyTouristBook.Table_MyTouristbook_Connection();
            MyTouristBook.Table_MyTouristbook_Tourist tab5 = new MyTouristBook.Table_MyTouristbook_Tourist();


            tab = (from t in db.Table_MyTouristbook_Connections
                   where (t.aid == my_aid)
                   select t).FirstOrDefault();

            tab2 = (from t in db2.Table_MyTouristbook_Connections
                    where (t.aid == his_aid)
                    select t).FirstOrDefault();

            tab5 = (from t in db5.Table_MyTouristbook_Tourists
                    where (t.aid == my_aid)
                    select t).FirstOrDefault();

            bool tab2_null = false;

            if (tab2 == null)
            {
                tab2 = new MyTouristBook.Table_MyTouristbook_Connection();
                int max2 = 0;

                try
                {

                    max2 = (from t in db2.Table_MyTouristbook_Connections
                            select t.id).Max();
                }
                catch (Exception ex)
                {

                }


                tab2.id = max2 + 1;
                tab2.aid = his_aid;
                tab2.username = get_username(his_aid);
                tab2.connections = my_aid.ToString();
                tab2.waitingconnections = "";
                db2.Table_MyTouristbook_Connections.InsertOnSubmit(tab2);
                db2.SubmitChanges();
                connnection_request2(his_aid, tab5.fullname);
                tab2_null = true;


            }


            string waiting1 = remove_aid_from_list(tab.waitingconnections, his_aid.ToString());
            string waiting2 = remove_aid_from_list(tab2.waitingconnections, my_aid.ToString());

            string approvedlist = add_aid_to_list(tab.connections, his_aid.ToString());
            string approvedlist2 = add_aid_to_list(tab2.connections, my_aid.ToString());

            tab.waitingconnections = waiting1;
            tab2.waitingconnections = waiting2;
            tab.connections = approvedlist;
            tab2.connections = approvedlist2;

            db.SubmitChanges();

            connnection_request2(his_aid, tab5.fullname);

            if (tab2_null == false)
                db2.SubmitChanges();

            init_connections();
            //init_con_page();


        }

        public bool exist_in_list(string del, string aid)
        {
            var str = del.Split(',');
            foreach (string st in str)
            {
                if (st.Equals(aid))
                    return true;
            }

            return false;

        }

        public string remove_aid_from_list(string del, string aid)
        {
            var str = del.Split(',');
            string result = "";
            foreach (string st in str)
            {
                if (st.Equals(aid))
                    continue;
                result += st;

                if (!st.Equals(""))
                    result += ',';
            }
            return result;
        }

        public string add_aid_to_list(string del, string aid)
        {
            if (exist_in_list(del, aid))
            {
                return del;
            }
            string del2 = del;
            if (!(del.Equals("")))
                del2 += ',';
            del2 += aid;
            return del2;
        }

        public void connnection_request2(int aid, string name)
        {

            var aff = get_affiliate(aid);
            string email = aff.email;


            string subject = "TouryDeals: Your connection request has just been accepted!";
            //string subject2 = "Offers Ads: New advertiser have signed up!";

            var names = aff.fullname.Split(' ');
            string firstName = names[0];

            string body = "Dear " + firstName + ",\n\n";

            body += "Your connection request has just been accepted by " + name + "! \n\n";


            body += "You can login and see all your connections from here:";

            body += "\n\n";


            body += @"https://www.tourydeals.com/center.aspx?connections=1";            

            body += "\n\n";


            body += "Sincerely,\n";
            body += "The TouryDeals Team";



            sendEmail(email, subject, body, false);


        }


        public bool isWaiting(int the_aid)
        {
            string my_aid_str = Username1.Value;
            int my_aid = get_aid(my_aid_str);


            MyTouristBook.ConnectionsDataContext db = new MyTouristBook.ConnectionsDataContext();
            MyTouristBook.Table_MyTouristbook_Connection tab = new MyTouristBook.Table_MyTouristbook_Connection();


            string waitingconnections = (from t in db.Table_MyTouristbook_Connections
                                         where (t.aid == the_aid)
                                         select t.waitingconnections).FirstOrDefault();

            if (waitingconnections == null)
                return false;

            if (waitingconnections.Equals(""))
                return false;

            var aids = waitingconnections.Split(',');

            foreach (var aid in aids)
            {

                if (aid.Equals(my_aid.ToString()))
                    return true;
            }

            return false;

        }

        public bool isConnected(int the_aid)
        {
            string my_aid_str = Username1.Value;
            int my_aid = get_aid(my_aid_str);


            MyTouristBook.ConnectionsDataContext db = new MyTouristBook.ConnectionsDataContext();
            MyTouristBook.Table_MyTouristbook_Connection tab = new MyTouristBook.Table_MyTouristbook_Connection();

            string connections = (from t in db.Table_MyTouristbook_Connections
                                  where (t.aid == the_aid)
                                  select t.connections).FirstOrDefault();

            if (connections == null)
                return false;

            if (connections.Equals(""))
                return false;

            var aids = connections.Split(',');

            foreach (var aid in aids)
            {

                if (aid.Equals(my_aid.ToString()))
                    return true;
            }

            return false;
        }


        public void connect(int the_aid)
        {

            Connect(the_aid);
        }



        protected void messages_click(object sender, EventArgs e)
        {
            init_messages();

            MultiView5.ActiveViewIndex = 2;

            MultiView1.ActiveViewIndex = 9;

            /*
            if (readmessage2.Visible)
            {
                readmessage1.Focus();
            }
            else
            {
                readmessage1.Focus();
            }*/
        }

        protected void community_click(object sender, EventArgs e)
        {
            init_community();
            MultiView1.ActiveViewIndex = 10;
            MultiView4.ActiveViewIndex = 1;
        }

        public string connectButtonString(int status)
        {
            if (status == 0)
                return "Connect";
            if (status == 1)   // connected
                return "Connected";
            return "Request Sent";


        }



        public void init_community()
        {

            wrong3.Visible = false;

            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();

            //affsbook.Table_Affsbook_Affiliate tab = new affsbook.Table_Affsbook_Affiliate();

            int page = 1;

            var total_table = (from t in db.Table_MyTouristbook_Tourists
                               where (!(t.username.Equals(Username1.Value)))
                               orderby t.signupdate
                               select t);

            decimal count = total_table.Count();

            decimal memcounter = Math.Ceiling(count / 7);

            DropDownPagesa.Items.Clear();

            for (int the_page = 1; the_page <= memcounter; the_page++)
            {
                ListItem item = new ListItem("Page " + the_page, the_page.ToString());
                DropDownPagesa.Items.Add(item);
            }

            init_page();



        }

        public void init_page()
        {
            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();

            //affsbook.Table_Affsbook_Affiliate tab = new affsbook.Table_Affsbook_Affiliate();            

            int page = Convert.ToInt32(DropDownPagesa.SelectedValue);

            var total_table = (from t in db.Table_MyTouristbook_Tourists
                               where (!((t.username.Equals(Username1.Value))))
                               orderby t.signupdate descending
                               select t);

            var the_table = (from t in db.Table_MyTouristbook_Tourists
                             where (!((t.username.Equals(Username1.Value))))
                             orderby t.signupdate descending
                             select t).Skip((page - 1) * 7).Take(7);

            if (the_table == null)
            {
                wrong3.Visible = true;
                return;
            }


            decimal count = total_table.Count();

            decimal memcounter = Math.Ceiling(count / 7);

            nexta.Visible = false;
            previousa.Visible = false;


            if (page < memcounter)
            {
                nexta.Visible = true;
            }

            if (page > 1)
            {
                previousa.Visible = true;
            }



            int counter = 1;

            PanelCom1.Visible = false;
            PanelCom2.Visible = false;
            PanelCom3.Visible = false;
            PanelCom4.Visible = false;
            PanelCom5.Visible = false;
            PanelCom6.Visible = false;
            PanelCom7.Visible = false;


            foreach (var tab2 in the_table)
            {

                int status = 0;

                if (tab2.aid != null)
                {

                    if (isConnected((int)tab2.aid))
                        status = 1;

                    else if (isWaiting((int)tab2.aid))
                        status = 2;
                }


                if (counter == 1)
                {
                    namea1.Text = tab2.fullname;
                    ImageButtona1.ImageUrl = my_image(tab2.profileimage, tab2.gender);
                    biographya1.Text = tab2.biography;
                    HiddenFielda1.Value = tab2.aid.ToString();
                    connecta1.Text = connectButtonString(status);
                    PanelCom1.Visible = true;
                }


                if (counter == 2)
                {
                    namea2.Text = tab2.fullname;
                    ImageButtona2.ImageUrl = my_image(tab2.profileimage, tab2.gender);
                    biographya2.Text = tab2.biography;
                    HiddenFielda2.Value = tab2.aid.ToString();
                    connecta2.Text = connectButtonString(status);
                    PanelCom2.Visible = true;
                }

                if (counter == 3)
                {
                    ImageButtona3.ImageUrl = my_image(tab2.profileimage, tab2.gender);
                    biographya3.Text = tab2.biography;
                    namea3.Text = tab2.fullname;
                    HiddenFielda3.Value = tab2.aid.ToString();
                    connecta3.Text = connectButtonString(status);
                    PanelCom3.Visible = true;
                }

                if (counter == 4)
                {
                    ImageButtona4.ImageUrl = my_image(tab2.profileimage, tab2.gender);
                    biographya4.Text = tab2.biography;
                    namea4.Text = tab2.fullname;
                    HiddenFielda4.Value = tab2.aid.ToString();
                    connecta4.Text = connectButtonString(status);
                    PanelCom4.Visible = true;
                }

                if (counter == 5)
                {
                    ImageButtona5.ImageUrl = my_image(tab2.profileimage, tab2.gender);
                    biographya5.Text = tab2.biography;
                    namea5.Text = tab2.fullname;
                    HiddenFielda5.Value = tab2.aid.ToString();
                    connecta5.Text = connectButtonString(status);
                    PanelCom5.Visible = true;
                }

                if (counter == 6)
                {
                    ImageButtona6.ImageUrl = my_image(tab2.profileimage, tab2.gender);
                    biographya6.Text = tab2.biography;
                    namea6.Text = tab2.fullname;
                    HiddenFielda6.Value = tab2.aid.ToString();
                    connecta6.Text = connectButtonString(status);
                    PanelCom6.Visible = true;
                }

                if (counter == 7)
                {
                    ImageButtona7.ImageUrl = my_image(tab2.profileimage, tab2.gender);
                    biographya7.Text = tab2.biography;
                    namea7.Text = tab2.fullname;
                    HiddenFielda7.Value = tab2.aid.ToString();
                    connecta7.Text = connectButtonString(status);
                    PanelCom7.Visible = true;
                }

                counter++;

            }



        }


        protected void home_click(object sender, EventArgs e)
        {
            my_news.Text = "";
            init_newsfeed();
            MultiView1.ActiveViewIndex = 12;
            //ImageButton27.Focus();            
        }

        protected void forums_click(object sender, EventArgs e)
        {
            init_forum_location();
            init_forums();
            init_forums_related();
            MultiView7.ActiveViewIndex = 2;
            MultiView1.ActiveViewIndex = 8;
            //readthread1.Focus();
        }



        protected void blogs_click(object sender, EventArgs e)
        {
            init_blog_location();
            init_blogs();
            init_blogs_related();


            MultiView8.ActiveViewIndex = 2;
            MultiView1.ActiveViewIndex = 7;
            //readblog1.Focus();
        }

        protected void logout_click(object sender, EventArgs e)
        {


            Email.Text = "";
            Password.Text = "";

            the_profile_image2.ImageUrl = "";

            Response.Cookies["tourydeals"].Expires = DateTime.Now.AddDays(-1);


            MultiView1.ActiveViewIndex = 0;

            Response.Redirect("~/center");


        }

        protected void panel_button_click(object sender, EventArgs e)
        {
            init_profile();
            return;
        }

        protected void create_offer_click(object sender, ImageClickEventArgs e)
        {
            Create_Deal(my_news.Text);
        }

        protected void post_blog_click(object sender, ImageClickEventArgs e)
        {
            create_blog(my_news.Text);
        }

        protected void post_thread_click(object sender, ImageClickEventArgs e)
        {
            create_forum(my_news.Text);
        }

        public string init_picture(string img)
        {
            if ((img == null) || (img.Equals("")))
            {
                return "~/images/mytouristbooklogosilver.jpg";
            }
            return img;

        }

        protected void deals_click(object sender, EventArgs e)
        {
            //init_connections();            
            MultiView1.ActiveViewIndex = 6;

            init_deal_location();
            init_deals();
            reset_deal();

            init_deal_related();

            MultiView9.ActiveViewIndex = 3;
            //info1.Focus();

            //info1.Focus();
        }

        public void reset_deal()
        {
            dealnameval.Visible = false;
            dealcountryval.Visible = false;
            dealcityval.Visible = false;
            dealshortdescval.Visible = false;
            deallongdescval.Visible = false;
            dealurlval.Visible = false;
        }

        public void reset_deals_controls()
        {


            mydealname.Text = "";
            //the_deal_image2.ImageUrl = "";

            mydealshortdesc.Text = "";
            mydealdesc.Text = "";
            mydealpricing.Text = "";

            mydealurl.Text = "";
            //mydealorderurl.Text = "";

            DropDownDeal2Country.SelectedValue = "0";
            DropDownDeal2City.SelectedValue = "0";


        }

        public DropDownList fIllCountries()
        {
            DropDownList drop = new DropDownList();

            drop.Items.Clear();
            ListItem item3 = new ListItem("Select Country");
            drop.Items.Add(item3);

            MyTouristBook.CountriesDataContext db4 = new MyTouristBook.CountriesDataContext();

            var countries = (from t in db4.Table_Countries
                             orderby t.country
                             select t.country);

            foreach (var country in countries)
            {
                ListItem item2 = new ListItem(country);
                drop.Items.Add(item2);
            }

            return drop;
        }

        public void init_deals()
        {
            wrong16.Visible = false;
            DropDownDeals.Visible = true;


            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.dealsDataContext db2 = new MyTouristBook.dealsDataContext();

            if (DropDownCountriesDeals.Items.Count == 1)
            {

                DropDownCountriesDeals.Items.Clear();
                ListItem item3 = new ListItem("Select Country");
                DropDownCountriesDeals.Items.Add(item3);


                MyTouristBook.CountriesDataContext db4 = new MyTouristBook.CountriesDataContext();

                var countries = (from t in db4.Table_Countries
                                 orderby t.country
                                 select t.country);

                foreach (var country in countries)
                {
                    ListItem item2 = new ListItem(country);
                    DropDownCountriesDeals.Items.Add(item2);
                }
            }


            //MyTouristBook.Table_City tab4 = new MyTouristBook.Table_City();


            /*tab4 = (from t in db4.Table_Cities
                    where (t.city.Equals(city))
                    select t).FirstOrDefault();*/

            string dest = DropDownCitiesDeals.SelectedItem.Text;



            //affsbook.Table_Affsbook_Affiliate tab = new affsbook.Table_Affsbook_Affiliate();

            int page = 1;

            var total_table = (from t in db2.Table_MyTouristbook_Deals
                               where ((t.dest1.Equals(dest)) || (dest.Equals("Select City")))
                               select t);

            decimal count = total_table.Count();

            decimal memcounter = Math.Ceiling(count / 7);

            DropDownDeals.Items.Clear();

            if (count == 0)
            {
                ListItem item = new ListItem("Page 1", "1");
                DropDownDeals.Items.Add(item);
                init_deal_page();
                DropDownDeals.Visible = false;
                return;
            }


            for (int the_page = 1; the_page <= memcounter; the_page++)
            {
                ListItem item = new ListItem("Page " + the_page, the_page.ToString());
                DropDownDeals.Items.Add(item);
            }

            init_deal_page();

        }

        public void init_deal_page()
        {
            wrong16.Visible = false;
            manageoffers.Visible = true;
            nodeals.Visible = false;

            string dest = DropDownCitiesDeals.SelectedItem.Text;

            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.dealsDataContext db2 = new MyTouristBook.dealsDataContext();

            //affsbook.Table_Affsbook_Affiliate tab = new affsbook.Table_Affsbook_Affiliate();            

            int page = Convert.ToInt32(DropDownDeals.SelectedValue);

            var total_table = (from t in db2.Table_MyTouristbook_Deals
                               where ((t.dest1.Equals(dest)) || (dest.Equals("Select City")))
                               select t);

            var the_table = (from t in db2.Table_MyTouristbook_Deals
                             where ((t.dest1.Equals(dest)) || (dest.Equals("Select City")))
                             orderby t.date descending
                             select t).Skip((page - 1) * 7).Take(7);

            int my_aid = get_aid(Username1.Value);

            int count_manage_table = (from t in db2.Table_MyTouristbook_Deals
                                      where t.owneraid == my_aid
                                      select t).Count();

            bool manage_exist = (count_manage_table > 0);

            if (manage_exist == false)
            {
                manageoffers.Visible = false;
            }


            if (the_table == null)
            {
                wrong16.Visible = true;
                return;
            }


            decimal count = total_table.Count();

            decimal memcounter = Math.Ceiling(count / 7);

            nextdeal.Visible = false;
            previousdeal.Visible = false;

            if (count == 0)
            {
                nodeals.Visible = true;
            }


            if (page < memcounter)
            {

                nextdeal.Visible = true;
            }

            if (page > 1)
            {
                previousdeal.Visible = true;

            }



            int counter = 1;

            PanelOffers1.Visible = false;
            PanelOffers2.Visible = false;
            PanelOffers3.Visible = false;
            PanelOffers4.Visible = false;
            PanelOffers5.Visible = false;
            PanelOffers6.Visible = false;
            PanelOffers7.Visible = false;


            foreach (MyTouristBook.Table_MyTouristbook_Deal tab2 in the_table)
            {
                //affsbook.Table_Affsbook_Affiliate aff = get_affiliate((int)tab2.authoraid);

                tab2.dest1 = destination(tab2.dest1);

                if (counter == 1)
                {

                    dealtitle1.Text = tab2.title;
                    dealshort1.Text = tab2.shortdescription;
                    DealImageButton1.ImageUrl = init_picture(tab2.imageurl);

                    if ((tab2.imageurl == null) || (tab2.imageurl.Equals("")))
                        DealImageButton1.BorderWidth = 0;

                    HiddenFieldDealid1.Value = tab2.id.ToString();

                    dest1.Text = tab2.dest1;
                    PanelOffers1.Visible = true;
                }


                if (counter == 2)
                {
                    dealtitle2.Text = tab2.title;
                    dealshort2.Text = tab2.shortdescription;
                    DealImageButton2.ImageUrl = init_picture(tab2.imageurl);

                    if ((tab2.imageurl == null) || (tab2.imageurl.Equals("")))
                        DealImageButton2.BorderWidth = 0;

                    HiddenFieldDealid2.Value = tab2.id.ToString();

                    dest2.Text = tab2.dest1;
                    PanelOffers2.Visible = true;
                }



                if (counter == 3)
                {
                    dealtitle3.Text = tab2.title;
                    dealshort3.Text = tab2.shortdescription;
                    DealImageButton3.ImageUrl = init_picture(tab2.imageurl);

                    if ((tab2.imageurl == null) || (tab2.imageurl.Equals("")))
                        DealImageButton3.BorderWidth = 0;

                    HiddenFieldDealid3.Value = tab2.id.ToString();

                    dest3.Text = tab2.dest1;
                    PanelOffers3.Visible = true;

                }

                if (counter == 4)
                {
                    dealtitle4.Text = tab2.title;
                    dealshort4.Text = tab2.shortdescription;
                    DealImageButton4.ImageUrl = init_picture(tab2.imageurl);

                    if ((tab2.imageurl == null) || (tab2.imageurl.Equals("")))
                        DealImageButton4.BorderWidth = 0;

                    HiddenFieldDealid4.Value = tab2.id.ToString();

                    dest4.Text = tab2.dest1;
                    PanelOffers4.Visible = true;

                }

                if (counter == 5)
                {
                    dealtitle5.Text = tab2.title;
                    dealshort5.Text = tab2.shortdescription;
                    DealImageButton5.ImageUrl = init_picture(tab2.imageurl);

                    if ((tab2.imageurl == null) || (tab2.imageurl.Equals("")))
                        DealImageButton5.BorderWidth = 0;

                    HiddenFieldDealid5.Value = tab2.id.ToString();

                    dest5.Text = tab2.dest1;
                    PanelOffers5.Visible = true;

                }

                if (counter == 6)
                {
                    dealtitle6.Text = tab2.title;
                    dealshort6.Text = tab2.shortdescription;
                    DealImageButton6.ImageUrl = init_picture(tab2.imageurl);

                    if ((tab2.imageurl == null) || (tab2.imageurl.Equals("")))
                        DealImageButton6.BorderWidth = 0;

                    HiddenFieldDealid6.Value = tab2.id.ToString();

                    dest6.Text = tab2.dest1;
                    PanelOffers6.Visible = true;

                }

                if (counter == 7)
                {
                    dealtitle7.Text = tab2.title;
                    dealshort7.Text = tab2.shortdescription;
                    DealImageButton7.ImageUrl = init_picture(tab2.imageurl);

                    if ((tab2.imageurl == null) || (tab2.imageurl.Equals("")))
                        DealImageButton7.BorderWidth = 0;

                    HiddenFieldDealid7.Value = tab2.id.ToString();

                    dest7.Text = tab2.dest1;
                    PanelOffers7.Visible = true;

                }

                counter++;

            }




        }


        protected void profile_image_uploaded(object sender, EventArgs e)
        {
            if (FileUpload2.HasFile)
            {

                string fname = Username1.Value;

                //fname = "tamordy";
                fname += "1";
                fname += "_";
                fname += FileUpload2.FileName;


                string localpath = @"E:\HostingSpaces\shadad\mytouristbook.com\wwwroot\aimages\";
                FileUpload2.SaveAs(localpath + fname);

                the_profile_image2.ImageUrl = @"https://www.mytouristbook.com/aimages/" + fname;


                wrong2.Visible = false;

                MyTouristBook.TouristsDataContext db2 = new MyTouristBook.TouristsDataContext();
                MyTouristBook.Table_MyTouristbook_Tourist tab = new MyTouristBook.Table_MyTouristbook_Tourist();



                //myUsername1.Value  = dropdownemail.SelectedItem.Text;      



                tab = (from t in db2.Table_MyTouristbook_Tourists
                       where (t.username.Equals(Username1.Value))
                       select t).FirstOrDefault();


                if (tab == null)
                {
                    wrong2.Visible = true;
                    //error.Text = "Error Updating User";                
                    return;
                }

                tab.profileimage = the_profile_image2.ImageUrl;

                db2.SubmitChanges();

                // update database

            }

        }

        public void init_forums()
        {

            wrong10.Visible = false;
            noforums.Visible = false;
            DropDownForums.Visible = true;

            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.ForumsDataContext db2 = new MyTouristBook.ForumsDataContext();

            if (DropDownCountriesForum.Items.Count == 1)
            {

                DropDownCountriesForum.Items.Clear();
                ListItem item3 = new ListItem("Select Country");
                DropDownCountriesForum.Items.Add(item3);


                MyTouristBook.CountriesDataContext db4 = new MyTouristBook.CountriesDataContext();

                var countries = (from t in db4.Table_Countries
                                 orderby t.country
                                 select t.country);

                foreach (var country in countries)
                {
                    ListItem item2 = new ListItem(country);
                    DropDownCountriesForum.Items.Add(item2);
                }
            }

            string dest = DropDownCitiesForum.SelectedItem.Text;

            //affsbook.Table_Affsbook_Affiliate tab = new affsbook.Table_Affsbook_Affiliate();

            int page = 1;

            var total_table = (from t in db2.Table_MyTouristbook_Forum_Threads
                               where ((t.replynumber == 0) && ((t.dest1.Equals(dest)) || (dest.Equals("Select City"))))
                               orderby t.startdate descending
                               select t);

            decimal count = total_table.Count();

            decimal memcounter = Math.Ceiling(count / 7);

            DropDownForums.Items.Clear();

            if (count == 0)
            {
                ListItem item = new ListItem("Page 1", "1");
                DropDownForums.Items.Add(item);
                init_forum_page();
                noforums.Visible = true;
                DropDownForums.Visible = false;

                return;
            }


            for (int the_page = 1; the_page <= memcounter; the_page++)
            {
                ListItem item = new ListItem("Page " + the_page, the_page.ToString());
                DropDownForums.Items.Add(item);
            }

            init_forum_page();            

        }

        public void init_forum_page()
        {
            wrong10.Visible = false;
            noforums.Visible = false;

            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.ForumsDataContext db2 = new MyTouristBook.ForumsDataContext();


            string dest = DropDownCitiesForum.SelectedItem.Text;

            //affsbook.Table_Affsbook_Affiliate tab = new affsbook.Table_Affsbook_Affiliate();            

            int page = Convert.ToInt32(DropDownForums.SelectedValue);


            var total_table = (from t in db2.Table_MyTouristbook_Forum_Threads
                               where ((t.replynumber == 0) && ((t.dest1.Equals(dest)) || (dest.Equals("Select City"))))
                               orderby t.startdate descending
                               select t);

            var the_table = (from t in db2.Table_MyTouristbook_Forum_Threads
                             where ((t.replynumber == 0) && ((t.dest1.Equals(dest)) || (dest.Equals("Select City"))))
                             orderby t.startdate descending
                             select t).Skip((page - 1) * 7).Take(7);



            if (the_table == null)
            {
                wrong10.Visible = true;
                return;
            }


            decimal count = total_table.Count();

            if (count == 0)
            {
                noforums.Visible = true;
            }

            decimal memcounter = Math.Ceiling(count / 7);

            nextc2.Visible = false;
            previousc2.Visible = false;
            //nextc9.Visible = false;


            if (count > 0)
            {
                //nextc9.Visible = true;
            }


            if (page < memcounter)
            {

                nextc2.Visible = true;
            }

            if (page > 1)
            {
                previousc2.Visible = true;

            }



            int counter = 1;

            PanelForum1.Visible = false;
            PanelForum2.Visible = false;
            PanelForum3.Visible = false;
            PanelForum4.Visible = false;
            PanelForum5.Visible = false;
            PanelForum6.Visible = false;
            PanelForum7.Visible = false;


            foreach (MyTouristBook.Table_MyTouristbook_Forum_Thread tab2 in the_table)
            {
                var aff = get_tourist((int)tab2.authoraid);                

                tab2.dest1 = destination(tab2.dest1);

                if (aff == null)
                {
                    continue;
                }
                if (counter == 1)
                {
                    threadauthor1.Text = aff.fullname;
                    ImageButtonc1.ImageUrl = my_image(aff.profileimage, aff.gender);
                    threadsubject1.Text = tab2.subject;
                    HiddenFieldForumId1.Value = tab2.thread_id.ToString();
                    HiddenFieldForumAuthorId1.Value = tab2.authoraid.ToString();
                    PanelForum1.Visible = true;
                    replies1.Text = tab2.replies.ToString();

                    if ((tab2.dest1 == null) || (tab2.dest1.Equals("")))
                        forumdest1.Text = "General";
                    else
                        forumdest1.Text = tab2.dest1;
                    //views1.Text = tab2.views.ToString();

                }

                if (counter == 2)
                {
                    threadauthor2.Text = aff.fullname;
                    ImageButtonc2.ImageUrl = my_image(aff.profileimage, aff.gender);
                    threadsubject2.Text = tab2.subject;
                    HiddenFieldForumId2.Value = tab2.thread_id.ToString();
                    HiddenFieldForumAuthorId2.Value = tab2.authoraid.ToString();
                    PanelForum2.Visible = true;
                    replies2.Text = tab2.replies.ToString();

                    if ((tab2.dest1 == null) || (tab2.dest1.Equals("")))
                        forumdest2.Text = "General";
                    else
                        forumdest2.Text = tab2.dest1;
                    //views1.Text = tab2.views.ToString();

                }

                if (counter == 3)
                {

                    threadauthor3.Text = aff.fullname;
                    ImageButtonc3.ImageUrl = my_image(aff.profileimage, aff.gender);
                    threadsubject3.Text = tab2.subject;
                    HiddenFieldForumId3.Value = tab2.thread_id.ToString();
                    HiddenFieldForumAuthorId3.Value = tab2.authoraid.ToString();
                    PanelForum3.Visible = true;
                    replies3.Text = tab2.replies.ToString();

                    if ((tab2.dest1 == null) || (tab2.dest1.Equals("")))
                        forumdest3.Text = "General";
                    else
                        forumdest3.Text = tab2.dest1;
                    //views1.Text = tab2.views.ToString();
                }

                if (counter == 4)
                {
                    threadauthor4.Text = aff.fullname;
                    ImageButtonc4.ImageUrl = my_image(aff.profileimage, aff.gender);
                    threadsubject4.Text = tab2.subject;
                    HiddenFieldForumId4.Value = tab2.thread_id.ToString();
                    HiddenFieldForumAuthorId4.Value = tab2.authoraid.ToString();
                    PanelForum4.Visible = true;
                    replies4.Text = tab2.replies.ToString();

                    if ((tab2.dest1 == null) || (tab2.dest1.Equals("")))
                        forumdest4.Text = "General";
                    else
                        forumdest4.Text = tab2.dest1;
                    //views1.Text = tab2.views.ToString();

                }

                if (counter == 5)
                {

                    threadauthor5.Text = aff.fullname;
                    ImageButtonc5.ImageUrl = my_image(aff.profileimage, aff.gender);
                    threadsubject5.Text = tab2.subject;
                    HiddenFieldForumId5.Value = tab2.thread_id.ToString();
                    HiddenFieldForumAuthorId5.Value = tab2.authoraid.ToString();
                    PanelForum5.Visible = true;
                    replies5.Text = tab2.replies.ToString();

                    if ((tab2.dest1 == null) || (tab2.dest1.Equals("")))
                        forumdest5.Text = "General";
                    else
                        forumdest5.Text = tab2.dest1;
                    //views1.Text = tab2.views.ToString();
                }

                if (counter == 6)
                {
                    threadauthor6.Text = aff.fullname;
                    ImageButtonc6.ImageUrl = my_image(aff.profileimage, aff.gender);
                    threadsubject6.Text = tab2.subject;
                    HiddenFieldForumId6.Value = tab2.thread_id.ToString();
                    HiddenFieldForumAuthorId6.Value = tab2.authoraid.ToString();
                    PanelForum6.Visible = true;
                    replies6.Text = tab2.replies.ToString();

                    if ((tab2.dest1 == null) || (tab2.dest1.Equals("")))
                        forumdest6.Text = "General";
                    else
                        forumdest6.Text = tab2.dest1;
                    //views1.Text = tab2.views.ToString();

                }

                if (counter == 7)
                {
                    threadauthor7.Text = aff.fullname;
                    ImageButtonc7.ImageUrl = my_image(aff.profileimage, aff.gender);
                    threadsubject7.Text = tab2.subject;
                    HiddenFieldForumId7.Value = tab2.thread_id.ToString();
                    HiddenFieldForumAuthorId7.Value = tab2.authoraid.ToString();
                    PanelForum7.Visible = true;
                    replies7.Text = tab2.replies.ToString();

                    if ((tab2.dest1 == null) || (tab2.dest1.Equals("")))
                        forumdest7.Text = "General";
                    else
                        forumdest7.Text = tab2.dest1;
                    //views1.Text = tab2.views.ToString();

                }

                counter++;

            }

            //readthread1.Focus();


        }


        public bool sendEmail(string myto, string mysubject, string mybody, bool copytamord)
        {

            //string pass = "Ta815730!";
            //string pass = "tal12345";
            string pass = "Ta81573049&";
            MailMessage message = new MailMessage();
            string from = "tal@adsrushx.com";
            //string to = "tamord@gmail.com";
            string to = myto;
            //string mysubject = "You have referred a sale!";


            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("tal@adsrushx.com", pass),
                    //Credentials = new NetworkCredential("asherhadad5@gmail.com", "Ta81573049"),                    
                    EnableSsl = true

                };

                client.Send(from, to, mysubject, mybody);

                if (copytamord)
                {

                    client.Send(from, "tamord@gmail.com", mysubject, mybody);
                }

                //emaillabel.Text = "New Advertiser E-Mail Suceeded!";
                //emaillabel.Visible = true;
            }
            catch (Exception ex)
            {
                return false;
                //emaillabel.Visible = true;
                //Label1.Text = "Send Email Failed.<br>" + ex.Message;
            }

            return true;

        }

        public void message_recieved_email(int aid, string name)
        {

            var aff = get_affiliate(aid);
            string email = aff.email;


            string subject = "TouryDeals: You have recieved a message!";
            //string subject2 = "Offers Ads: New advertiser have signed up!";

            var names = aff.fullname.Split(' ');
            string firstName = names[0];

            string body = "Dear " + firstName + ",\n\n";

            body += "You have just recieved a message from: " + name + "! \n\n";

            body += "You can login and read the message from here:";

            body += "\n\n";


            body += @"https://www.tourydeals.com/center.aspx?messages=1";
            body += "\n\n";


            body += "Sincerely,\n";
            body += "The TouryDeals Team";



            sendEmail(email, subject, body, false);


        }


        protected void next1_click(object sender, EventArgs e)
        {
            wrong2.Visible = false;

            MyTouristBook.TouristsDataContext db2 = new MyTouristBook.TouristsDataContext();
            MyTouristBook.Table_MyTouristbook_Tourist tab = new MyTouristBook.Table_MyTouristbook_Tourist();

            if (myaboutme.Equals(""))
            {
                return;
            }

            //myUsername1.Value  = dropdownemail.SelectedItem.Text;      



            tab = (from t in db2.Table_MyTouristbook_Tourists
                   where (t.username.Equals(Username1.Value))
                   select t).FirstOrDefault();


            if (tab == null)
            {
                wrong2.Visible = true;
                //error.Text = "Error Updating User";                
                return;
            }

            tab.fullname = myfullname.Text;
            tab.skype = myskype.Text;
            tab.facebook = myfacebook.Text;
            tab.whatsapp = mywhatsapp.Text;
            tab.linkedin = mylinkedin.Text;
            tab.twitter = mytwitter.Text;
            tab.biography = myaboutme.Text;

            db2.SubmitChanges();

            MultiView3.ActiveViewIndex = 1;
            Button211.Focus();
        }


        protected void next2_click(object sender, EventArgs e)
        {

            wrong2.Visible = false;

            MyTouristBook.TouristsDataContext db2 = new MyTouristBook.TouristsDataContext();
            MyTouristBook.Table_MyTouristbook_Tourist tab = new MyTouristBook.Table_MyTouristbook_Tourist();


            //myUsername1.Value  = dropdownemail.SelectedItem.Text;      



            tab = (from t in db2.Table_MyTouristbook_Tourists
                   where (t.username.Equals(Username1.Value))
                   select t).FirstOrDefault();


            if (tab == null)
            {
                wrong2.Visible = true;
                //error.Text = "Error Updating User";                
                return;
            }

            tab.city = mycity.Text;
            tab.state = DropDownListState.SelectedItem.Text;
            tab.country = DropDownListCountry.SelectedItem.Text;




            db2.SubmitChanges();



            MultiView3.ActiveViewIndex = 3;
            Button213.Focus();
        }

        protected void next3_click(object sender, EventArgs e)
        {

            wrong2.Visible = false;

            MyTouristBook.TouristsDataContext db2 = new MyTouristBook.TouristsDataContext();
            MyTouristBook.Table_MyTouristbook_Tourist tab = new MyTouristBook.Table_MyTouristbook_Tourist();


            //myUsername1.Value  = dropdownemail.SelectedItem.Text;      



            tab = (from t in db2.Table_MyTouristbook_Tourists
                   where (t.username.Equals(Username1.Value))
                   select t).FirstOrDefault();


            if (tab == null)
            {
                wrong2.Visible = true;
                //error.Text = "Error Updating User";                
                return;
            }

            //tab.dest1 = DropDownDest.SelectedItem.Text;
            tab.moredest = mymoredest.Text;


            db2.SubmitChanges();




            MultiView3.ActiveViewIndex = 3;
            mywebsites.Focus();

        }

        protected void back2_click(object sender, EventArgs e)
        {
            MultiView3.ActiveViewIndex = 0;
            Button208.Focus();
        }

        protected void back3_click(object sender, EventArgs e)
        {
            MultiView3.ActiveViewIndex = 1;
            Button211.Focus();
        }

        protected void back4_click(object sender, EventArgs e)
        {
            MultiView3.ActiveViewIndex = 1;
            Button213.Focus();
        }

        protected void finish_click(object sender, EventArgs e)
        {
            wrong2.Visible = false;

            MyTouristBook.TouristsDataContext db2 = new MyTouristBook.TouristsDataContext();
            MyTouristBook.Table_MyTouristbook_Tourist tab = new MyTouristBook.Table_MyTouristbook_Tourist();


            //myUsername1.Value  = dropdownemail.SelectedItem.Text;      



            tab = (from t in db2.Table_MyTouristbook_Tourists
                   where (t.username.Equals(Username1.Value))
                   select t).FirstOrDefault();


            if (tab == null)
            {
                wrong2.Visible = true;
                //error.Text = "Error Updating User";                
                return;
            }

            tab.websites = mywebsites.Text;
            tab.blog = myblogs.Text;
            tab.hobbies = myhobbies.Text;

            db2.SubmitChanges();

            string body2 = tab.fullname + " have completed filling profile!";
            body2 += "\n\n";


            string subject2 = "Tourist: " + tab.fullname + " have completed filling his profile!";
            sendEmail("tamord@gmail.com", subject2, body2, false);

            success.Visible = true;

            my_news.Text = "";

        }

        public void init_profile(string aid)
        {
            wrong4.Visible = false;

            int my_aid = get_aid(Username1.Value);
            /*if (aid.Equals(my_aid.ToString()))
            {

                profile_click(this, null);
                return;
            }*/

            string the_aid = aid;
            int the_int_aid;
            try
            {
                the_int_aid = Convert.ToInt32(aid);
            }
            catch (Exception ex)
            {
                wrong4.Visible = false;
                return;
            }

            //string the_username = get_affiliate(the_int_aid).username;

            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.Table_MyTouristbook_Tourist tab = new MyTouristBook.Table_MyTouristbook_Tourist();


            tab = (from t in db.Table_MyTouristbook_Tourists
                   where (t.aid.ToString().Equals(the_aid.ToString()))
                   select t).FirstOrDefault();

            if (tab == null)
            {
                wrong4.Visible = true;
                return;
            }

            //bool connected = isConnected((int)tab.aid);


            the_display_name.Text = tab.fullname;
            the_image.ImageUrl = my_image(tab.profileimage, tab.gender);
            the_biography.Text = tab.biography;

            the_websites.Text = "Not Available";
            the_hobbies.Text = "Not Available";

            if (tab.websites != null)
            {
                the_websites.Text = (tab.websites.Equals("")) ? "Not Available" : tab.websites;
            }

            if (tab.hobbies != null)
            {
                the_hobbies.Text = (tab.hobbies.Equals("")) ? "Not Available" : tab.hobbies;
            }

            the_city.Text = tab.city;
            the_country.Text = tab.country;
            the_dest.Text = tab.dest1;



            if (the_country.Text.Equals("Select Country"))
            {
                the_country.Text = "";
            }


            bool connected = isConnected((int)tab.aid);




            if ((Username1.Value.Equals("tamordy")) || (Username1.Value.Equals("asher5")) || (Username1.Value.Equals("steve56")))
            {
                connected = true;
            }


            the_skype.Text = (connected == true) ? tab.skype : "Not Connected";
            the_email.Text = (connected == true) ? tab.email : "Not Connected";
            the_whatsapp.Text = (connected == true) ? tab.whatsapp : "Not Connected";
            the_linkedin.Text = (connected == true) ? tab.linkedin : "Not Connected";




            HiddenFieldAid.Value = aid.ToString();

            int status = 0;



            if (isConnected(the_int_aid))
                status = 1;

            else if (isWaiting(the_int_aid))
                status = 2;


            connect9.Text = connectButtonString(status);

            MultiView4.ActiveViewIndex = 0;
            MultiView1.ActiveViewIndex = 10;
            connect9.Focus();

        }




        protected void cconnect1(object sender, EventArgs e)
        {
            string aid = HiddenFielda1.Value;
            int the_aid = Convert.ToInt32(aid);
            cconnect(the_aid);
            int status = get_status(the_aid);
            connecta1.Text = connectButtonString(status);
            init_page();
            connecta2.Focus();
        }

        protected void connect_click(object sender, EventArgs e)
        {
            string aid = HiddenFieldAid.Value;
            int the_aid = Convert.ToInt32(aid);
            cconnect(the_aid);
            int status = get_status(the_aid);
            connect9.Text = connectButtonString(status);
            init_profile(aid);
            connect9.Focus();
        }

        protected void profile_send_message(object sender, EventArgs e)
        {
            isGuest();
            string aid = HiddenFieldAid.Value;
            SendMessage(aid);
        }

        protected void cclick1(object sender, EventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenFielda1.Value;
            init_profile(the_aid);
        }

        protected void iclick1(object sender, ImageClickEventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenFielda1.Value;
            init_profile(the_aid);
        }

        protected void smessage1(object sender, EventArgs e)
        {
            string to = HiddenFielda1.Value;
            SendMessage(to);
        }

        protected void previous_click(object sender, EventArgs e)
        {

        }

        protected void next_Click(object sender, EventArgs e)
        {

        }

        protected void page_changed_click(object sender, EventArgs e)
        {

        }

        protected void cclick2(object sender, EventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenFielda2.Value;
            init_profile(the_aid);
        }

        protected void iclick2(object sender, ImageClickEventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenFielda2.Value;
            init_profile(the_aid);
        }

        protected void cclick3(object sender, EventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenFielda3.Value;
            init_profile(the_aid);
        }

        protected void iclick3(object sender, ImageClickEventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenFielda3.Value;
            init_profile(the_aid);
        }

        protected void cclick4(object sender, EventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenFielda4.Value;
            init_profile(the_aid);
        }

        protected void iclick4(object sender, ImageClickEventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenFielda4.Value;
            init_profile(the_aid);
        }

        protected void cclick5(object sender, EventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenFielda5.Value;
            init_profile(the_aid);
        }

        protected void iclick5(object sender, ImageClickEventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenFielda5.Value;
            init_profile(the_aid);
        }


        protected void cclick6(object sender, EventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenFielda6.Value;
            init_profile(the_aid);
        }

        protected void iclick6(object sender, ImageClickEventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenFielda6.Value;
            init_profile(the_aid);
        }

        protected void cclick7(object sender, EventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenFielda7.Value;
            init_profile(the_aid);
        }

        protected void iclick7(object sender, ImageClickEventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenFielda7.Value;
            init_profile(the_aid);
        }

        public void init_messages(string message_id)
        {
            wrong7.Visible = false;
            //int mid = Convert.ToInt32(message_id);

            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.Table_MyTouristbook_Message tab = new MyTouristBook.Table_MyTouristbook_Message();


            tab = (from t in db.Table_MyTouristbook_Messages
                   where (t.id.Equals(message_id))
                   select t).FirstOrDefault();

            if (tab == null)
            {
                wrong7.Visible = true;
                return;
            }

            MyTouristBook.Table_MyTouristbook_Tourist aff = get_affiliate((int)tab.senderaid);


            the_body.Text = tab.body;
            read_msg_from.Text = aff.fullname;
            Read_Message_ImageButton.ImageUrl = my_image(aff.profileimage, aff.gender);
            the_titleb.Text = tab.subject;
            HiddenFieldFrom2.Value = tab.senderaid.ToString();

            string aid = tab.senderaid.ToString();
            int the_aid = Convert.ToInt32(aid);
            int status = get_status(the_aid);
            connectmessage.Text = connectButtonString(status);

            //HiddenFieldSubject.Value = tab.subject;

            MultiView5.ActiveViewIndex = 1;
            connectmessage.Focus();

        }

        public void init_messages()
        {
            nomesages.Visible = false;
            wrong6.Visible = false;
            DropDownPagesMessages.Visible = true;

            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.Table_MyTouristbook_Tourist tab = new MyTouristBook.Table_MyTouristbook_Tourist();

            int? the_aid = (from t in db.Table_MyTouristbook_Tourists
                            where (t.username.Equals(Username1.Value))
                            select t.aid).FirstOrDefault();

            int page = 1;

            var total_table = (from t in db.Table_MyTouristbook_Messages
                               where (t.recieveraid == the_aid)
                               orderby t.date descending
                               select t);

            decimal count = total_table.Count();

            decimal memcounter = Math.Ceiling(count / 7);

            DropDownPagesMessages.Items.Clear();

            if (count == 0)
            {
                ListItem item = new ListItem("Page 1", "1");
                DropDownPagesMessages.Items.Add(item);
                init_messages_page();
                nomesages.Visible = true;
                DropDownPagesMessages.Visible = false;
                return;
            }


            for (int the_page = 1; the_page <= memcounter; the_page++)
            {
                ListItem item = new ListItem("Page " + the_page, the_page.ToString());
                DropDownPagesMessages.Items.Add(item);
            }


            init_messages_page();



        }


        public void init_messages_page()
        {
            wrong6.Visible = false;

            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();

            //affsbook.Table_Affsbook_Affiliate tab = new affsbook.Table_Affsbook_Affiliate();            

            int page = Convert.ToInt32(DropDownPagesMessages.SelectedValue);

            int the_aid = get_aid(Username1.Value);

            var total_table = (from t in db.Table_MyTouristbook_Messages
                               where ((t.recieveraid == the_aid))
                               orderby t.date descending
                               select t);

            var the_table = (from t in db.Table_MyTouristbook_Messages
                             where (t.recieveraid == the_aid)
                             orderby t.date descending
                             select t).Skip((page - 1) * 7).Take(7);

            if (the_table == null)
            {
                wrong6.Visible = true;
                return;
            }


            decimal count = total_table.Count();

            decimal memcounter = Math.Ceiling(count / 7);

            nextb1.Visible = false;
            previousb1.Visible = false;


            if (page < memcounter)
            {
                nextb1.Visible = true;
            }

            if (page > 1)
            {
                previousb1.Visible = true;
            }



            int counter = 1;

            PanelMsg1.Visible = false;
            PanelMsg2.Visible = false;
            PanelMsg3.Visible = false;
            PanelMsg4.Visible = false;
            PanelMsg5.Visible = false;
            PanelMsg6.Visible = false;
            PanelMsg7.Visible = false;

            foreach (var tab2 in the_table)
            {
                var aff = get_affiliate((int)tab2.senderaid);

                if (aff == null)
                {
                    continue;
                }

                if (counter == 1)
                {
                    fromb1.Text = aff.fullname;
                    ImageButtonb1.ImageUrl = my_image(aff.profileimage, aff.gender);
                    subjectb1.Text = tab2.subject;
                    HiddenFieldb1.Value = tab2.id.ToString();
                    HiddenFieldDest1.Value = aff.username;
                    PanelMsg1.Visible = true;

                }

                if (counter == 2)
                {
                    fromb2.Text = aff.fullname;
                    ImageButtonb2.ImageUrl = my_image(aff.profileimage, aff.gender);
                    subjectb2.Text = tab2.subject;
                    HiddenFieldb2.Value = tab2.id.ToString();
                    HiddenFieldDest2.Value = aff.username;
                    PanelMsg2.Visible = true;
                }

                if (counter == 3)
                {
                    fromb3.Text = aff.fullname;
                    ImageButtonb3.ImageUrl = my_image(aff.profileimage, aff.gender);
                    subjectb3.Text = tab2.subject;
                    HiddenFieldb3.Value = tab2.id.ToString();
                    HiddenFieldDest3.Value = aff.username;
                    PanelMsg3.Visible = true;
                }



                if (counter == 4)
                {
                    fromb4.Text = aff.fullname;
                    ImageButtonb4.ImageUrl = my_image(aff.profileimage, aff.gender);
                    subjectb4.Text = tab2.subject;
                    HiddenFieldb4.Value = tab2.id.ToString();
                    HiddenFieldDest4.Value = aff.username;
                    PanelMsg4.Visible = true;
                }

                if (counter == 5)
                {
                    fromb5.Text = aff.fullname;
                    ImageButtonb5.ImageUrl = my_image(aff.profileimage, aff.gender);
                    subjectb5.Text = tab2.subject;
                    HiddenFieldb5.Value = tab2.id.ToString();
                    HiddenFieldDest5.Value = aff.username;
                    PanelMsg5.Visible = true;
                }




                if (counter == 6)
                {
                    fromb6.Text = aff.fullname;
                    ImageButtonb6.ImageUrl = my_image(aff.profileimage, aff.gender);
                    subjectb6.Text = tab2.subject;
                    HiddenFieldb6.Value = tab2.id.ToString();
                    HiddenFieldDest6.Value = aff.username;
                    PanelMsg6.Visible = true;
                }



                if (counter == 7)
                {
                    fromb7.Text = aff.fullname;
                    ImageButtonb7.ImageUrl = my_image(aff.profileimage, aff.gender);
                    subjectb7.Text = tab2.subject;
                    HiddenFieldb7.Value = tab2.id.ToString();
                    HiddenFieldDest7.Value = aff.username;
                    PanelMsg7.Visible = true;
                }


                counter++;

            }


        }

        public void SendMessage(string the_aid_to)
        {
            int aid_to = 0;
            try
            {
                aid_to = Convert.ToInt32(the_aid_to);
            }
            catch (Exception ex)
            {
                return;
            }
            SendMessage(aid_to);
        }

        public void SendMessage(int the_aid_to)
        {
            isGuest();

            if (the_aid_to == 0)
                return;
            MyTouristBook.Table_MyTouristbook_Tourist aff = get_affiliate(the_aid_to);
            Send_Message_ImageButton.ImageUrl = my_image(aff.profileimage, aff.gender);
            send_msg_dest.Text = aff.fullname;
            the_subject3.Text = "";
            the_body3.Text = "";
            wrong8.Visible = false;

            //the_subject3.Text = HiddenFieldSubject.Value;
            //HiddenField_Destination.Value = HiddenFieldFrom2.Value;
            //HiddenField_Title.Value = HiddenFieldSubject.Value;
            HiddenField_Destination.Value = the_aid_to.ToString();
            the_subject3.Enabled = true;

            //message_back.Visible = true;

            string aid = HiddenField_Destination.Value;
            //HiddenFieldFrom2.Value = tab.senderaid.ToString();

            //string aid = tab.senderaid.ToString();
            int the_aid2 = Convert.ToInt32(aid);
            int status = get_status(the_aid2);
            connectmessage0.Text = connectButtonString(status);

            MultiView1.ActiveViewIndex = 9;
            MultiView5.ActiveViewIndex = 0;
            connectmessage0.Focus();
        }



        public MyTouristBook.Table_MyTouristbook_Tourist get_affiliate(int my_aid)

        {
            MyTouristBook.Table_MyTouristbook_Tourist tab3 = new MyTouristBook.Table_MyTouristbook_Tourist();
            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();


            tab3 = (from t in db.Table_MyTouristbook_Tourists
                    where (t.aid == my_aid)
                    select t).FirstOrDefault();

            return tab3;
        }




        protected void read_message1_click(object sender, EventArgs e)
        {
            wrong6.Visible = false;
            string the_message_id = HiddenFieldb1.Value;
            init_messages(the_message_id);
        }

        protected void read_message2_click(object sender, EventArgs e)
        {
            wrong6.Visible = false;
            string the_message_id = HiddenFieldb2.Value;
            init_messages(the_message_id);
        }

        protected void read_message3_click(object sender, EventArgs e)
        {
            wrong6.Visible = false;
            string the_message_id = HiddenFieldb3.Value;
            init_messages(the_message_id);
        }

        protected void read_message4_click(object sender, EventArgs e)
        {
            wrong6.Visible = false;
            string the_message_id = HiddenFieldb4.Value;
            init_messages(the_message_id);
        }

        protected void read_message5_click(object sender, EventArgs e)
        {
            wrong6.Visible = false;
            string the_message_id = HiddenFieldb5.Value;
            init_messages(the_message_id);
        }

        protected void read_message6_click(object sender, EventArgs e)
        {
            wrong6.Visible = false;
            string the_message_id = HiddenFieldb6.Value;
            init_messages(the_message_id);
        }

        protected void read_message7_click(object sender, EventArgs e)
        {
            wrong6.Visible = false;
            string the_message_id = HiddenFieldb7.Value;
            init_messages(the_message_id);
        }

        protected void connectmessage_Click2(object sender, EventArgs e)
        {
            string aid = HiddenField_Destination.Value;
            int the_aid = Convert.ToInt32(aid);
            cconnect(the_aid);
            int status = get_status(the_aid);
            connectmessage0.Text = connectButtonString(status);
            init_messages_page();
            connectmessage0.Focus();
        }

        protected void send_message_click(object sender, EventArgs e)
        {
            isGuest();

            string reciever_aid_str = HiddenField_Destination.Value;
            int reciever_aid = Convert.ToInt32(reciever_aid_str);
            string reciever_username = get_username(reciever_aid);
            string sender_username = Username1.Value;
            int sender_aid = get_aid(sender_username);
            string name = get_name(sender_username);

            wrong8.Visible = false;

            if (the_subject3.Text.Equals(""))
            {
                wrong8.Visible = true;
                wrong8.Text = "Your Message Subject Cannot be Empty!";
                connectmessage0.Focus();
                return;
            }

            if (the_body3.Text.Equals(""))
            {
                wrong8.Visible = true;
                wrong8.Text = "Your Message Body Cannot be Empty!";
                connectmessage0.Focus();
                return;
            }


            if (the_body3.Text.Length > 300)
            {
                wrong8.Visible = true;
                wrong8.Text = "Your Message Body is too longe!";
                connectmessage0.Focus();
                return;
            }

            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.Table_MyTouristbook_Message tab = new Table_MyTouristbook_Message();


            int max2 = 0;

            try
            {

                max2 = (from t in db.Table_MyTouristbook_Messages
                        select t.id).Max();
            }
            catch (Exception ex)
            {

            }


            // get referral aid and username

            tab.id = max2 + 1;

            tab.senderaid = sender_aid;
            tab.senderusername = sender_username;
            tab.recieveraid = reciever_aid;
            tab.recieverusername = reciever_username;
            tab.date = DateTime.Now;
            tab.subject = the_subject3.Text;
            tab.body = the_body3.Text;

            int the_aid = (int)tab.recieveraid;

            db.Table_MyTouristbook_Messages.InsertOnSubmit(tab);



            try
            {

                db.SubmitChanges();
                message_recieved_email(the_aid, name);

            }
            catch (Exception ex)
            {
                wrong8.Visible = true;
                return;
            }

            init_messages();
            MultiView5.ActiveViewIndex = 2;

        }

        protected void connectmessage_Click(object sender, EventArgs e)
        {
            string aid = HiddenFieldFrom2.Value;
            int the_aid = Convert.ToInt32(aid);
            cconnect(the_aid);
            int status = get_status(the_aid);
            connectmessage.Text = connectButtonString(status);
            init_messages_page();
            connectmessage.Focus();
        }

        protected void send_reply_click(object sender, EventArgs e)
        {
            string the_aid = HiddenFieldFrom2.Value;


            wrong7.Visible = false;
            int the_aid_to = 0;
            try
            {
                the_aid_to = Convert.ToInt32(the_aid);
            }
            catch (Exception ex)
            {
                wrong7.Visible = true;
                return;
            }

            SendMessage(the_aid_to);


        }

        protected void msgautclick3(object sender, ImageClickEventArgs e)
        {
            string the_author_aid = HiddenField_Destination.Value;
            Show_Profile(the_author_aid);
        }

        protected void msgautclick7(object sender, ImageClickEventArgs e)
        {
            string the_author_aid = HiddenFieldFrom2.Value;
            Show_Profile(the_author_aid);
        }

        protected void msgautclick2(object sender, EventArgs e)
        {
            string the_author_aid = HiddenField_Destination.Value;
            Show_Profile(the_author_aid);
        }

        protected void msgautclick7(object sender, EventArgs e)
        {
            string the_author_aid = HiddenFieldFrom2.Value;
            Show_Profile(the_author_aid);
        }

        protected void fromb1click(object sender, EventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = get_aid(HiddenFieldDest1.Value).ToString();
            init_profile(the_aid);
        }

        protected void fromb2click(object sender, EventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = get_aid(HiddenFieldDest2.Value).ToString();
            init_profile(the_aid);
        }

        protected void fromb3click(object sender, EventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = get_aid(HiddenFieldDest3.Value).ToString();
            init_profile(the_aid);
        }

        protected void fromb4click(object sender, EventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = get_aid(HiddenFieldDest4.Value).ToString();
            init_profile(the_aid);
        }

        protected void fromb5click(object sender, EventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = get_aid(HiddenFieldDest5.Value).ToString();
            init_profile(the_aid);
        }

        protected void fromb6click(object sender, EventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = get_aid(HiddenFieldDest6.Value).ToString();
            init_profile(the_aid);
        }

        protected void fromb7click(object sender, EventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = get_aid(HiddenFieldDest7.Value).ToString();
            init_profile(the_aid);
        }

        protected void sendreplyd1(object sender, EventArgs e)
        {

            wrong7.Visible = false;
            int the_aid_to = 0;
            try
            {
                the_aid_to = get_aid(HiddenFieldDest1.Value);
            }
            catch (Exception ex)
            {
                wrong7.Visible = true;
                return;
            }

            SendMessage(the_aid_to);
        }

        protected void sendreplyd2(object sender, EventArgs e)
        {
            wrong7.Visible = false;
            int the_aid_to = 0;
            try
            {
                the_aid_to = get_aid(HiddenFieldDest1.Value);
            }
            catch (Exception ex)
            {
                wrong7.Visible = true;
                return;
            }

            SendMessage(the_aid_to);

        }

        protected void sendreplyd3(object sender, EventArgs e)
        {
            wrong7.Visible = false;
            int the_aid_to = 0;
            try
            {
                the_aid_to = get_aid(HiddenFieldDest2.Value);
            }
            catch (Exception ex)
            {
                wrong7.Visible = true;
                return;
            }

            SendMessage(the_aid_to);

        }

        protected void sendreplyd4(object sender, EventArgs e)
        {
            wrong7.Visible = false;
            int the_aid_to = 0;
            try
            {
                the_aid_to = get_aid(HiddenFieldDest3.Value);
            }
            catch (Exception ex)
            {
                wrong7.Visible = true;
                return;
            }

            SendMessage(the_aid_to);

        }

        protected void sendreplyd5(object sender, EventArgs e)
        {
            wrong7.Visible = false;
            int the_aid_to = 0;
            try
            {
                the_aid_to = get_aid(HiddenFieldDest4.Value);
            }
            catch (Exception ex)
            {
                wrong7.Visible = true;
                return;
            }

            SendMessage(the_aid_to);

        }

        protected void sendreplyd6(object sender, EventArgs e)
        {
            wrong7.Visible = false;
            int the_aid_to = 0;
            try
            {
                the_aid_to = get_aid(HiddenFieldDest5.Value);
            }
            catch (Exception ex)
            {
                wrong7.Visible = true;
                return;
            }

            SendMessage(the_aid_to);

        }

        protected void sendreplyd7(object sender, EventArgs e)
        {
            wrong7.Visible = false;
            int the_aid_to = 0;
            try
            {
                the_aid_to = get_aid(HiddenFieldDest6.Value);
            }
            catch (Exception ex)
            {
                wrong7.Visible = true;
                return;
            }

            SendMessage(the_aid_to);

        }

        protected void previous_message_click(object sender, EventArgs e)
        {
            wrong7.Visible = false;
            int the_aid_to = 0;
            try
            {
                the_aid_to = get_aid(HiddenFieldDest7.Value);
            }
            catch (Exception ex)
            {
                wrong7.Visible = true;
                return;
            }

            SendMessage(the_aid_to);

        }

        protected void next_message_click(object sender, EventArgs e)
        {

        }

        protected void message_page_changed_click(object sender, EventArgs e)
        {

        }

        protected void demo(object sender, EventArgs e)
        {
            SendMessage(603108);
        }

        protected void myselfclick(object sender, ImageClickEventArgs e)
        {
            Show_MySelf_Profile();
            return;
        }

        protected void myselfclick1(object sender, EventArgs e)
        {
            Show_MySelf_Profile();
            return;
        }

        protected void insdefrmautclick1(object sender, ImageClickEventArgs e)
        {
            string the_author_aid = HiddenFieldTAuthor1.Value;
            Show_Profile(the_author_aid);
        }

        protected void insdefrmautclick2(object sender, ImageClickEventArgs e)
        {
            string the_author_aid = HiddenFieldTAuthor2.Value;
            Show_Profile(the_author_aid);
        }

        protected void insdefrmautclick3(object sender, ImageClickEventArgs e)
        {
            string the_author_aid = HiddenFieldTAuthor3.Value;
            Show_Profile(the_author_aid);
        }

        protected void connectforum_Click3(object sender, EventArgs e)
        {
            string aid = HiddenFieldTAuthor3.Value;
            int the_aid = Convert.ToInt32(aid);
            cconnect(the_aid);
            int status = get_status(the_aid);
            connectmessage3.Text = connectButtonString(status);
            init_thread_page();
            connectmessage3.Focus();
        }

        protected void connectforum_Click1(object sender, EventArgs e)
        {
            string aid = HiddenFieldTAuthor1.Value;
            int the_aid = Convert.ToInt32(aid);
            cconnect(the_aid);
            int status = get_status(the_aid);
            connectmessage1.Text = connectButtonString(status);
            init_thread_page();
            connectmessage1.Focus();
        }

        protected void connectforum_Click2(object sender, EventArgs e)
        {
            string aid = HiddenFieldTAuthor2.Value;
            int the_aid = Convert.ToInt32(aid);
            cconnect(the_aid);
            int status = get_status(the_aid);
            connectmessage2.Text = connectButtonString(status);
            init_thread_page();
            connectmessage2.Focus();
        }

        protected void insdefrmautclick3(object sender, EventArgs e)
        {
            string the_author_aid = HiddenFieldTAuthor3.Value;
            Show_Profile(the_author_aid);
        }

        protected void previous_thread_click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(DropDownThread.SelectedValue);
            if (page == 1)
            {
                return;
            }
            page--;
            DropDownThread.SelectedValue = page.ToString();
            init_thread_page();
        }

        protected void next_thread_click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(DropDownThread.SelectedValue);
            page++;
            DropDownThread.SelectedValue = page.ToString();
            init_thread_page();
        }

        protected void thread_page_changed_click(object sender, EventArgs e)
        {
            init_thread_page();
        }

        protected void frmautclick1(object sender, EventArgs e)
        {
            string the_aid = HiddenFieldForumAuthorId1.Value;
            Show_Profile(the_aid);
        }

        protected void frmautclick2(object sender, EventArgs e)
        {
            string the_aid = HiddenFieldForumAuthorId2.Value;
            Show_Profile(the_aid);
        }

        protected void frmautclick3(object sender, EventArgs e)
        {
            string the_aid = HiddenFieldForumAuthorId3.Value;
            Show_Profile(the_aid);
        }

        protected void frmautclick4(object sender, EventArgs e)
        {
            string the_aid = HiddenFieldForumAuthorId4.Value;
            Show_Profile(the_aid);
        }

        protected void frmautclick5(object sender, EventArgs e)
        {
            string the_aid = HiddenFieldForumAuthorId5.Value;
            Show_Profile(the_aid);
        }

        protected void frmautclick6(object sender, EventArgs e)
        {
            string the_aid = HiddenFieldForumAuthorId6.Value;
            Show_Profile(the_aid);
        }

        protected void frmautclick7(object sender, EventArgs e)
        {
            string the_aid = HiddenFieldForumAuthorId7.Value;
            Show_Profile(the_aid);
        }

        protected void read_thread1_click(object sender, EventArgs e)
        {
            int the_thread_id = Convert.ToInt32(HiddenFieldForumId1.Value);
            init_thread(the_thread_id);
        }

        protected void previous_forum_click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(DropDownForums.SelectedValue);
            if (page == 1)
            {
                return;
            }
            page--;
            DropDownForums.SelectedValue = page.ToString();
            init_forum_page();
            readthread2.Focus();
        }

        protected void next_forum_click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(DropDownForums.SelectedValue);
            page++;
            DropDownForums.SelectedValue = page.ToString();
            init_forum_page();
            readthread2.Focus();
        }

        protected void forum_page_changed_click(object sender, EventArgs e)
        {
            init_forum_page();
            readthread2.Focus();
        }

        public bool has_profile_image_and_bio()
        {

            return true;

            MyTouristBook.Table_MyTouristbook_Tourist tab = new MyTouristBook.Table_MyTouristbook_Tourist();

            int my_aid = get_aid(Username1.Value);

            tab = get_tourist(my_aid);

            if ((tab.profileimage != null) && (!(tab.profileimage.Equals(""))))
            {
                if ((tab.biography != null) && (!(tab.biography.Equals(""))))
                {
                    return true;
                }
            }


            return false;


        }


        public void create_forum(string body)
        {
            the_subject4.Enabled = true;
            DropDownForum2City.Enabled = true;

            the_thread_body.Text = "";
            the_subject4.Text = "";
            wrong99.Visible = false;
            verval.Visible = false;

            string username = Username1.Value;
            int the_aid = get_aid(username);

            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.dealsDataContext db2 = new MyTouristBook.dealsDataContext();
            MyTouristBook.CountriesDataContext db4 = new MyTouristBook.CountriesDataContext();

            if (DropDownForum2Country.Items.Count == 1)
            {

                DropDownForum2Country.Items.Clear();
                ListItem item3 = new ListItem("Select Country", "0");
                DropDownForum2Country.Items.Add(item3);


                var countries = (from t in db4.Table_Countries
                                 orderby t.country
                                 select t.country);

                foreach (var country3 in countries)
                {
                    ListItem item2 = new ListItem(country3);
                    DropDownForum2Country.Items.Add(item2);
                }
            }



            MyTouristBook.Table_MyTouristbook_Tourist aff = get_tourist(the_aid);
            the_thread_body.Text = body;

            The_Image_Myself.ImageUrl = my_image(aff.profileimage, aff.gender);
            the_myself.Text = aff.fullname;

            MultiView7.ActiveViewIndex = 0;
            MultiView1.ActiveViewIndex = 8;
            connect19.Focus();
        }
        protected void forum_post_click(object sender, EventArgs e)
        {
            the_subject4.Enabled = true;
            DropDownForum2City.Enabled = true;

            the_thread_body.Text = "";
            the_subject4.Text = "";
            wrong99.Visible = false;
            verval.Visible = false;

            string username = Username1.Value;
            int the_aid = get_aid(username);

            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.dealsDataContext db2 = new MyTouristBook.dealsDataContext();
            MyTouristBook.CountriesDataContext db4 = new MyTouristBook.CountriesDataContext();

            if (DropDownForum2Country.Items.Count == 1)
            {

                DropDownForum2Country.Items.Clear();
                ListItem item3 = new ListItem("Select Country", "0");
                DropDownForum2Country.Items.Add(item3);


                var countries = (from t in db4.Table_Countries
                                 orderby t.country
                                 select t.country);

                foreach (var country3 in countries)
                {
                    ListItem item2 = new ListItem(country3);
                    DropDownForum2Country.Items.Add(item2);
                }
            }



            MyTouristBook.Table_MyTouristbook_Tourist aff = get_tourist(the_aid);


            The_Image_Myself.ImageUrl = my_image(aff.profileimage, aff.gender);
            the_myself.Text = aff.fullname;

            MultiView7.ActiveViewIndex = 0;
            connect19.Focus();

        }

        public void init_thread(int thread_id)
        {

            HiddenField_The_Thread_ID.Value = thread_id.ToString();

            wrong14.Visible = false;
            //nothreads.Visible = false;

            DropDownThread.Visible = true;

            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.ForumsDataContext db2 = new MyTouristBook.ForumsDataContext();


            //affsbook.Table_Affsbook_Affiliate tab = new affsbook.Table_Affsbook_Affiliate();

            int page = 1;

            var total_table = (from t in db2.Table_MyTouristbook_Forum_Threads
                               where (t.thread_id == thread_id)
                               select t);

            decimal count = total_table.Count();

            decimal memcounter = Math.Ceiling(count / 3);

            DropDownThread.Items.Clear();

            if (count == 0)
            {
                ListItem item = new ListItem("Page 1", "1");
                DropDownThread.Items.Add(item);
                init_thread_page();
                wrong14.Visible = true;
                DropDownThread.Visible = false;
                return;
            }


            for (int the_page = 1; the_page <= memcounter; the_page++)
            {
                ListItem item = new ListItem("Page " + the_page, the_page.ToString());
                DropDownThread.Items.Add(item);
            }

            init_thread_page();


        }

        public void init_thread_page()
        {
            wrong14.Visible = false;


            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.ForumsDataContext db2 = new MyTouristBook.ForumsDataContext();

            //affsbook.Table_Affsbook_Affiliate tab = new affsbook.Table_Affsbook_Affiliate();            

            int page = Convert.ToInt32(DropDownThread.SelectedValue);
            int threadId = Convert.ToInt32(HiddenField_The_Thread_ID.Value);
            //threadId = 1;

            var total_table = (from t in db2.Table_MyTouristbook_Forum_Threads
                               where (t.thread_id == threadId)
                               select t);

            var the_table = (from t in db2.Table_MyTouristbook_Forum_Threads
                             where (t.thread_id == threadId)
                             select t).Skip((page - 1) * 3).Take(3);

            if (the_table == null)
            {
                wrong14.Visible = true;
                return;
            }


            decimal count = total_table.Count();

            decimal memcounter = Math.Ceiling(count / 3);

            nextthread1.Visible = false;
            previousthread1.Visible = false;



            if (page < memcounter)
            {

                nextthread1.Visible = true;
            }

            if (page > 1)
            {
                previousthread1.Visible = true;

            }



            int counter = 1;

            PanelThread1.Visible = false;
            PanelThread2.Visible = false;
            PanelThread3.Visible = false;

            foreach (MyTouristBook.Table_MyTouristbook_Forum_Thread tab2 in the_table)
            {

                int the_aid = 0;

                if ((tab2.replyaid == 0) || (tab2.replyaid == null))
                {
                    the_aid = (int)tab2.authoraid;
                }

                else if (tab2.replyaid != 0)
                {


                    the_aid = (int)tab2.replyaid;

                }

                MyTouristBook.Table_MyTouristbook_Tourist aff = get_tourist(the_aid);

                if (aff == null)
                {
                    continue;
                }


                if (counter == 1)
                {

                    connectmessage1.Visible = true;
                    the_thread_subject.Text = tab2.subject;
                    HiddenField_The_Thread_ID.Value = tab2.thread_id.ToString();
                    the_body_thread1.Text = tab2.body;
                    The_Image_Thread1.ImageUrl = my_image(aff.profileimage, aff.gender);
                    thread_from1.Text = aff.fullname;
                    //int p1_aid = (int)tab2.replyaid;
                    HiddenFieldTAuthor1.Value = the_aid.ToString();

                    int status = 0;



                    if (isConnected(the_aid))
                        status = 1;

                    else if (isWaiting(the_aid))
                        status = 2;



                    connectmessage1.Text = connectButtonString(status);

                    if (the_aid == get_aid(Username1.Value))
                    {
                        connectmessage1.Visible = false;
                    }


                    PanelThread1.Visible = true;


                }

                if (counter == 2)
                {

                    //the_thread_subject.Text = tab2.subject;
                    //HiddenField_The_Thread_ID.Value = tab2.thread_id.ToString();

                    connectmessage2.Visible = true;
                    the_body_thread2.Text = tab2.body;
                    The_Image_Thread2.ImageUrl = my_image(aff.profileimage, aff.gender);
                    thread_from2.Text = aff.fullname;
                    //int p1_aid = (int)tab2.replyaid;
                    HiddenFieldTAuthor2.Value = the_aid.ToString();

                    int status = 0;


                    if (isConnected(the_aid))
                        status = 1;

                    else if (isWaiting(the_aid))
                        status = 2;



                    connectmessage2.Text = connectButtonString(status);

                    if (the_aid == get_aid(Username1.Value))
                    {
                        connectmessage2.Visible = false;
                    }

                    PanelThread2.Visible = true;

                }

                if (counter == 3)
                {
                    connectmessage3.Visible = true;
                    the_body_thread3.Text = tab2.body;
                    The_Image_Thread3.ImageUrl = my_image(aff.profileimage, aff.gender);
                    thread_from3.Text = aff.fullname;
                    //int p1_aid = (int)tab2.replyaid;
                    HiddenFieldTAuthor3.Value = the_aid.ToString();

                    int status = 0;


                    if (isConnected(the_aid))
                        status = 1;

                    else if (isWaiting(the_aid))
                        status = 2;

                    connectmessage3.Text = connectButtonString(status);

                    if (the_aid == get_aid(Username1.Value))
                    {
                        connectmessage3.Visible = false;
                    }

                    PanelThread3.Visible = true;

                }


                counter++;

            }

            MultiView7.ActiveViewIndex = 1;
            reply1.Focus();

        }

        public void init_blogs()
        {
            wrong21.Visible = false;
            DropDownBlogs.Visible = true;

            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.blogsDataContext db3 = new MyTouristBook.blogsDataContext();

            if (DropDownCountriesBlog.Items.Count == 1)
            {

                DropDownCountriesBlog.Items.Clear();
                ListItem item3 = new ListItem("Select Country");
                DropDownCountriesBlog.Items.Add(item3);


                MyTouristBook.CountriesDataContext db4 = new MyTouristBook.CountriesDataContext();

                var countries = (from t in db4.Table_Countries
                                 orderby t.country
                                 select t.country);

                foreach (var country in countries)
                {
                    ListItem item2 = new ListItem(country);
                    DropDownCountriesBlog.Items.Add(item2);
                }
            }

            //affsbook.OffersDataContext db2 = new affsbook.OffersDataContext();


            //affsbook.Table_Affsbook_Affiliate tab = new affsbook.Table_Affsbook_Affiliate();

            int page = 1;
            string dest = DropDownCitiesBlog.SelectedItem.Text;

            var total_table = (from t in db3.Table_MyTouristbook_Blogs
                               where ((t.dest1.Equals(dest)) || (dest.Equals("Select City")))
                               select t);

            decimal count = total_table.Count();

            decimal memcounter = Math.Ceiling(count / 7);

            DropDownBlogs.Items.Clear();

            if (count == 0)
            {
                ListItem item = new ListItem("Page 1", "1");
                DropDownBlogs.Items.Add(item);
                init_blog_page();
                DropDownBlogs.Visible = false;
                return;
            }


            for (int the_page = 1; the_page <= memcounter; the_page++)
            {
                ListItem item = new ListItem("Page " + the_page, the_page.ToString());
                DropDownBlogs.Items.Add(item);
            }

            init_blog_page();

        }

        public void init_blog_page()
        {
            wrong21.Visible = false;
            noblogs.Visible = false;

            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.blogsDataContext db2 = new MyTouristBook.blogsDataContext();

            string dest = DropDownCitiesBlog.SelectedItem.Text;

            int page = Convert.ToInt32(DropDownBlogs.SelectedValue);

            var total_table = (from t in db2.Table_MyTouristbook_Blogs
                               where ((t.dest1.Equals(dest)) || (dest.Equals("Select City")))
                               orderby t.startdate descending
                               select t);

            var the_table = (from t in db2.Table_MyTouristbook_Blogs
                             where ((t.dest1.Equals(dest)) || (dest.Equals("Select City")))
                             orderby t.startdate descending
                             select t).Skip((page - 1) * 7).Take(7);

            int my_aid = get_aid(Username1.Value);


            if (the_table == null)
            {
                wrong21.Visible = true;
                return;
            }


            decimal count = total_table.Count();

            decimal memcounter = Math.Ceiling(count / 7);

            nextblog.Visible = false;
            previousblog.Visible = false;

            if (count == 0)
            {
                noblogs.Visible = true;
            }
            if (page < memcounter)
            {

                nextblog.Visible = true;
            }

            if (page > 1)
            {
                previousblog.Visible = true;

            }



            int counter = 1;

            PanelBlog1.Visible = false;
            PanelBlog2.Visible = false;
            PanelBlog3.Visible = false;
            PanelBlog4.Visible = false;
            PanelBlog5.Visible = false;
            PanelBlog6.Visible = false;
            PanelBlog7.Visible = false;

            foreach (MyTouristBook.Table_MyTouristbook_Blog tab2 in the_table)
            {

                MyTouristBook.Table_MyTouristbook_Tourist aff = get_tourist((int)tab2.authoraid);
                tab2.dest1 = destination(tab2.dest1);

                if (aff == null)
                {
                    continue;
                }

                if (counter == 1)
                {
                    theblogtitle1.Text = tab2.title;
                    ImageButtond1.ImageUrl = my_image(aff.profileimage, aff.gender);
                    blogauthor1.Text = aff.fullname;

                    HiddenFieldBlogId1.Value = tab2.blog_id.ToString();
                    HiddenFieldBlogAuthorId1.Value = tab2.authoraid.ToString();

                    if ((tab2.dest1 == null) || (tab2.dest1.Equals("")))
                        blogdest1.Text = "General";
                    else
                        blogdest1.Text = tab2.dest1;

                    PanelBlog1.Visible = true;

                }


                if (counter == 2)
                {
                    theblogtitle2.Text = tab2.title;
                    ImageButtond2.ImageUrl = my_image(aff.profileimage, aff.gender);
                    blogauthor2.Text = aff.fullname;

                    HiddenFieldBlogId2.Value = tab2.blog_id.ToString();
                    HiddenFieldBlogAuthorId2.Value = tab2.authoraid.ToString();

                    if ((tab2.dest1 == null) || (tab2.dest1.Equals("")))
                        blogdest2.Text = "General";
                    else
                        blogdest2.Text = tab2.dest1;

                    PanelBlog2.Visible = true;
                }



                if (counter == 3)
                {
                    theblogtitle3.Text = tab2.title;
                    ImageButtond3.ImageUrl = my_image(aff.profileimage, aff.gender);
                    blogauthor3.Text = aff.fullname;

                    HiddenFieldBlogId3.Value = tab2.blog_id.ToString();
                    HiddenFieldBlogAuthorId3.Value = tab2.authoraid.ToString();

                    if ((tab2.dest1 == null) || (tab2.dest1.Equals("")))
                        blogdest3.Text = "General";
                    else
                        blogdest3.Text = tab2.dest1;

                    PanelBlog3.Visible = true;

                }

                if (counter == 4)
                {
                    theblogtitle4.Text = tab2.title;
                    ImageButtond4.ImageUrl = my_image(aff.profileimage, aff.gender);
                    blogauthor4.Text = aff.fullname;

                    HiddenFieldBlogId4.Value = tab2.blog_id.ToString();
                    HiddenFieldBlogAuthorId4.Value = tab2.authoraid.ToString();

                    if ((tab2.dest1 == null) || (tab2.dest1.Equals("")))
                        blogdest4.Text = "General";
                    else
                        blogdest4.Text = tab2.dest1;

                    PanelBlog4.Visible = true;

                }

                if (counter == 5)
                {
                    theblogtitle5.Text = tab2.title;
                    ImageButtond5.ImageUrl = my_image(aff.profileimage, aff.gender);
                    blogauthor5.Text = aff.fullname;

                    HiddenFieldBlogId5.Value = tab2.blog_id.ToString();
                    HiddenFieldBlogAuthorId5.Value = tab2.authoraid.ToString();

                    if ((tab2.dest1 == null) || (tab2.dest1.Equals("")))
                        blogdest5.Text = "General";
                    else
                        blogdest5.Text = tab2.dest1;

                    PanelBlog5.Visible = true;

                }

                if (counter == 6)
                {
                    theblogtitle6.Text = tab2.title;
                    ImageButtond6.ImageUrl = my_image(aff.profileimage, aff.gender);
                    blogauthor6.Text = aff.fullname;

                    HiddenFieldBlogId6.Value = tab2.blog_id.ToString();
                    HiddenFieldBlogAuthorId6.Value = tab2.authoraid.ToString();

                    if ((tab2.dest1 == null) || (tab2.dest1.Equals("")))
                        blogdest6.Text = "General";
                    else
                        blogdest6.Text = tab2.dest1;

                    PanelBlog6.Visible = true;

                }

                if (counter == 7)
                {
                    theblogtitle7.Text = tab2.title;
                    ImageButtond7.ImageUrl = my_image(aff.profileimage, aff.gender);
                    blogauthor7.Text = aff.fullname;

                    HiddenFieldBlogId7.Value = tab2.blog_id.ToString();
                    HiddenFieldBlogAuthorId7.Value = tab2.authoraid.ToString();

                    if ((tab2.dest1 == null) || (tab2.dest1.Equals("")))
                        blogdest7.Text = "General";
                    else
                        blogdest7.Text = tab2.dest1;

                    PanelBlog7.Visible = true;

                }


                counter++;

            }




        }


        protected void post_thread_click(object sender, EventArgs e)
        {

            

            wrong99.Visible = false;
            verval.Visible = false;
            wrong102.Visible = false;
            wrong103.Visible = false;
            



            if (DropDownForum2City.SelectedIndex == 0)
            {
                verval.Visible = true;
                connect19.Focus();
                return;
            }

            if (the_subject4.Text.Equals(""))
            {
                wrong102.Visible = true;
                wrong102.Text = "Your Post Title Cannot be Empty!";
                connect19.Focus();
                return;
            }

            if (the_thread_body.Text.Equals(""))
            {
                wrong103.Visible = true;
                wrong103.Text = "Your Post Body Cannot be Empty!";
                connect19.Focus();
                return;
            }



            isGuest();


            /*
            if (the_thread_body.Text.Length < 4000)
            {
                wrong99.Visible = true;
                wrong99.Text = "Your Post Body is too small!";
                the_subject4.Focus();
                return;
            }*/

            if (the_subject4.Enabled)
            {

                if (DropDownForum2City.SelectedIndex == 0)
                {
                    verval.Visible = true;
                    the_subject4.Focus();
                    return;
                }

                MyTouristBook.Table_MyTouristbook_Forum_Thread tab5 = new MyTouristBook.Table_MyTouristbook_Forum_Thread();
                MyTouristBook.ForumsDataContext db4 = new MyTouristBook.ForumsDataContext();


                int max3 = 0;

                try
                {

                    max3 = (from t in db4.Table_MyTouristbook_Forum_Threads
                            select t.id).Max();
                }
                catch (Exception ex)
                {

                }



                int? max_thread = 0;

                try
                {

                    max_thread = (from t in db4.Table_MyTouristbook_Forum_Threads
                                  select t.thread_id).Max();
                }
                catch (Exception ex)
                {
                    max_thread = 0;
                }

                if (max_thread == null)
                    max_thread = 0;


                int thread_id2 = (int)max_thread + 1;
                string thread_id_str2 = thread_id2.ToString();

                tab5.id = max3 + 1;
                tab5.thread_id = thread_id2;
                tab5.startdate = DateTime.Now;

                tab5.authoraid = get_aid(Username1.Value);
                tab5.autherusername = Username1.Value;

                tab5.subject = the_subject4.Text;
                tab5.body = the_thread_body.Text;

                tab5.replynumber = 0;
                tab5.replyaid = get_aid(Username1.Value);
                tab5.replyusername = Username1.Value;
                tab5.the_reply_date = tab5.startdate;

                tab5.replies = 0;
                tab5.views = 0;
                tab5.dest1 = DropDownForum2City.SelectedItem.Text;


                //tab5.niche1 = tab.niche1;                                    // set niche
                //tab2.niche2 = tab.niche2;
                //tab2.niche3 = tab.niche3;

                if (!(has_profile_image_and_bio()))
                {
                    profile_click(this, e);
                    return;
                }


                db4.Table_MyTouristbook_Forum_Threads.InsertOnSubmit(tab5);

                try
                {

                    db4.SubmitChanges();

                }
                catch (Exception ex)
                {
                    wrong99.Visible = true;
                    return;
                }

                forums_click(sender, e);
                MultiView7.ActiveViewIndex = 2;
                return;
            }


            string thread_id_str = HiddenField_The_Thread_ID.Value;
            int thread_id = Convert.ToInt32(thread_id_str);

            MyTouristBook.Table_MyTouristbook_Forum_Thread tab = new MyTouristBook.Table_MyTouristbook_Forum_Thread();
            MyTouristBook.ForumsDataContext db = new MyTouristBook.ForumsDataContext();

            tab = (from t in db.Table_MyTouristbook_Forum_Threads
                   where (t.thread_id == thread_id)
                   select t).FirstOrDefault();

            if (tab == null)
            {
                wrong99.Visible = true;
                return;
            }

            MyTouristBook.Table_MyTouristbook_Forum_Thread tab2 = new MyTouristBook.Table_MyTouristbook_Forum_Thread();

            int max2 = 0;

            try
            {

                max2 = (from t in db.Table_MyTouristbook_Forum_Threads
                        select t.id).Max();
            }
            catch (Exception ex)
            {
                max2 = 0;
            }

            tab2.id = max2 + 1;
            tab2.thread_id = tab.thread_id;
            tab2.autherusername = tab.autherusername;
            tab2.authoraid = tab.authoraid;
            tab2.startdate = tab.startdate;
            tab2.subject = tab.subject;
            tab2.body = the_thread_body.Text;

            int? max_reply = 0;

            try
            {

                max_reply = (from t in db.Table_MyTouristbook_Forum_Threads
                             where (t.thread_id == thread_id)
                             select t.replynumber).Max();
            }
            catch (Exception ex)
            {
                max_reply = 0;
            }

            if (max_reply == null)
                max_reply = 0;

            tab2.replynumber = max_reply + 1;

            tab2.replyusername = Username1.Value;
            tab2.replyaid = get_aid(Username1.Value);
            tab2.the_reply_date = DateTime.Now;
            tab2.dest1 = tab.dest1;



            if (!(has_profile_image_and_bio()))
            {
                profile_click(this, e);
                return;
            }

            string the_thread_id = tab2.thread_id.ToString();

            db.Table_MyTouristbook_Forum_Threads.InsertOnSubmit(tab2);


            try
            {

                db.SubmitChanges();

            }
            catch (Exception ex)
            {
                wrong99.Visible = true;
                return;
            }

            /*
            if (the_subject4.Enabled)
            {
                
            }
            */

            MyTouristBook.Table_MyTouristbook_Forum_Thread tab4 = new MyTouristBook.Table_MyTouristbook_Forum_Thread();


            tab4 = (from t in db.Table_MyTouristbook_Forum_Threads
                    where ((t.thread_id == thread_id) && (t.replynumber == 0))
                    select t).FirstOrDefault();


            tab4.replies++;

            if (!the_subject4.Enabled)
            {
                string name = get_name(Username1.Value);

                int my_aid = get_aid(Username1.Value);

                if (my_aid != tab4.authoraid)
                {
                    forum_reply_request((int)tab4.authoraid, name, the_thread_id);
                }




            }


            db.SubmitChanges();

            forums_click(sender, e);
            
            MultiView7.ActiveViewIndex = 2;
            readthread1.Focus();
        }

        protected void thread_reply_click(object sender, EventArgs e)
        {
            wrong99.Visible = false;
            verval.Visible = false;

            string thread_id_str = HiddenField_The_Thread_ID.Value;
            int thread_id = Convert.ToInt32(thread_id_str);

            MyTouristBook.Table_MyTouristbook_Forum_Thread tab = new MyTouristBook.Table_MyTouristbook_Forum_Thread();
            MyTouristBook.ForumsDataContext db = new MyTouristBook.ForumsDataContext();

            the_subject4.Enabled = false;
            DropDownForum2City.Enabled = false;


            the_thread_body.Text = "";

            tab = (from t in db.Table_MyTouristbook_Forum_Threads
                   where (t.thread_id == thread_id)
                   select t).FirstOrDefault();

            if (tab == null)
            {
                wrong99.Visible = true;
                return;
            }


            the_subject4.Text = tab.subject;


            foreach (ListItem li in DropDownForum2City.Items)
            {
                if (li.Text.Equals(tab.dest1))
                {
                    DropDownForum2City.SelectedValue = li.Value;
                }

            }

            string username = Username1.Value;
            int the_aid = get_aid(username);

            MyTouristBook.Table_MyTouristbook_Tourist aff = get_tourist(the_aid);


            The_Image_Myself.ImageUrl = my_image(aff.profileimage, aff.gender);
            the_myself.Text = aff.fullname;

            MultiView7.ActiveViewIndex = 0;
            connect19.Focus();
        }

        protected void insdefrmautclick1(object sender, EventArgs e)
        {
            string the_author_aid = HiddenFieldTAuthor1.Value;
            Show_Profile(the_author_aid);
        }

        protected void insdefrmautclick2(object sender, EventArgs e)
        {
            string the_author_aid = HiddenFieldTAuthor2.Value;
            Show_Profile(the_author_aid);
        }

        protected void read_thread2_click(object sender, EventArgs e)
        {
            int the_thread_id = Convert.ToInt32(HiddenFieldForumId2.Value);
            init_thread(the_thread_id);
        }

        protected void read_thread3_click(object sender, EventArgs e)
        {
            int the_thread_id = Convert.ToInt32(HiddenFieldForumId3.Value);
            init_thread(the_thread_id);
        }

        protected void read_thread4_click(object sender, EventArgs e)
        {
            int the_thread_id = Convert.ToInt32(HiddenFieldForumId4.Value);
            init_thread(the_thread_id);
        }

        protected void read_thread5_click(object sender, EventArgs e)
        {
            int the_thread_id = Convert.ToInt32(HiddenFieldForumId5.Value);
            init_thread(the_thread_id);
        }

        protected void read_thread6_click(object sender, EventArgs e)
        {
            int the_thread_id = Convert.ToInt32(HiddenFieldForumId6.Value);
            init_thread(the_thread_id);
        }

        protected void read_thread7_click(object sender, EventArgs e)
        {
            int the_thread_id = Convert.ToInt32(HiddenFieldForumId7.Value);
            init_thread(the_thread_id);
        }

        public void create_blog(string the_body)
        {
            reset_blog();
            //reset_offers_controls();
            //reset_defaults();    

            MyTouristBook.CountriesDataContext db4 = new MyTouristBook.CountriesDataContext();

            if (DropDownBlog2Country.Items.Count == 1)
            {

                DropDownBlog2Country.Items.Clear();
                ListItem item3 = new ListItem("Select Country", "0");
                DropDownBlog2Country.Items.Add(item3);


                var countries = (from t in db4.Table_Countries
                                 orderby t.country
                                 select t.country);

                foreach (var country3 in countries)
                {
                    ListItem item2 = new ListItem(country3);
                    DropDownBlog2Country.Items.Add(item2);
                }
            }

            the_blog_body2.Text = the_body;
            verval2.Visible = false;
            MultiView8.ActiveViewIndex = 0;
            MultiView1.ActiveViewIndex = 7;
            the_blog_body2.Focus();
        }

        protected void add_blog_click(object sender, EventArgs e)
        {
            reset_blog();
            //reset_offers_controls();
            //reset_defaults();    

            MyTouristBook.CountriesDataContext db4 = new MyTouristBook.CountriesDataContext();

            if (DropDownBlog2Country.Items.Count == 1)
            {

                DropDownBlog2Country.Items.Clear();
                ListItem item3 = new ListItem("Select Country", "0");
                DropDownBlog2Country.Items.Add(item3);


                var countries = (from t in db4.Table_Countries
                                 orderby t.country
                                 select t.country);

                foreach (var country3 in countries)
                {
                    ListItem item2 = new ListItem(country3);
                    DropDownBlog2Country.Items.Add(item2);
                }
            }

            verval2.Visible = false;
            MultiView8.ActiveViewIndex = 0;
            the_blog_body2.Focus();
        }

        public void reset_blog()
        {
            //theblogimage2.ImageUrl = "";
            the_blog_subject.Text = "";
            the_blog_body2.Text = "";
            DropDownBlog2City.SelectedValue = "0";
            int my_aid = get_aid(Username1.Value);

            MyTouristBook.Table_MyTouristbook_Tourist aff = new MyTouristBook.Table_MyTouristbook_Tourist();

            aff = get_tourist(my_aid);
            the_myself2.Text = aff.fullname;
            The_Image_Myself2.ImageUrl = my_image(aff.profileimage, aff.gender);
            HiddenFieldMyBlogId.Value = "0";
            verval2.Visible = false;
        }

        protected void post_blog_click(object sender, EventArgs e)
        {
            

            MyTouristBook.blogsDataContext db = new MyTouristBook.blogsDataContext();
            MyTouristBook.Table_MyTouristbook_Blog tab = new MyTouristBook.Table_MyTouristbook_Blog();

            wrong23.Visible = false;
            wrong100.Visible = false;
            wrong101.Visible = false;

            verval2.Visible = false;

            if (DropDownBlog2City.SelectedIndex == 0)
            {
                verval2.Visible = true;
                the_blog_subject.Focus();
                return;

            }
            if (the_blog_subject.Text.Equals(""))
            {
                wrong100.Visible = true;
                wrong100.Text = "Your Blog Title Cannot be Empty!";
                connect26.Focus();
                return;
            }

            if (the_blog_body2.Text.Equals(""))
            {
                wrong101.Visible = true;
                wrong101.Text = "Your Blog Body Cannot be Empty!";
                connect26.Focus();
                return;
            }

            /*

            if (the_blog_body2.Text.Length < 4500)
            {
                wrong101.Visible = true;
                wrong101.Text = "Your Blog Body is too small!";
                connect26.Focus();
                return;
            }

            */




            isGuest();

            if (HiddenFieldMyBlogId.Value.Equals("0"))
            {


                tab = new MyTouristBook.Table_MyTouristbook_Blog();
                int max2 = 0;
                int? max3 = 0;
                try
                {

                    max2 = (from t in db.Table_MyTouristbook_Blogs
                            select t.id).Max();
                }
                catch (Exception ex)
                {

                }

                try
                {
                    max3 = (from t in db.Table_MyTouristbook_Blogs
                            select t.blog_id).Max();
                }
                catch (Exception ex)
                {

                }

                if (max3 == null)
                    max3 = 0;

                tab.id = max2 + 1;
                tab.blog_id = max3 + 1;
                tab.active = 1;
                tab.startdate = DateTime.Now;
                tab.dest1 = DropDownBlog2City.SelectedItem.Text;

                string ownerusername = Username1.Value;
                int owneraid = get_aid(ownerusername);

                tab.authorusername = ownerusername;
                tab.authoraid = owneraid;

                //tab.imageurl = theblogimage2.ImageUrl;
                tab.title = the_blog_subject.Text;
                tab.body = the_blog_body2.Text;

                tab.imageurl = "";


                tab.sponsored = 0;
                tab.featured = 0;
                tab.priority = 0;
                tab.popular = 0;


            }

            /*

            else if (!(HiddenFieldMyBlogId.Value.Equals("0")))
            {
                int the_id = Convert.ToInt32(HiddenFieldMyBlogId.Value);

                tab = (from t in db.Table_MyTouristbook_Blogs
                       where t.blog_id == the_id
                       select t).FirstOrDefault();

                if (tab == null)
                {
                    wrong23.Visible = true;
                    return;


                }


            } */

            //tab.imageurl = theblogimage2.ImageUrl;
            tab.title = the_blog_subject.Text;
            tab.body = the_blog_body2.Text;
            tab.dest1 = DropDownBlog2City.SelectedItem.Text;

            // tab.niche1 = DropDownOfferVertical.SelectedItem.Text;

            if (!(has_profile_image_and_bio()))
            {
                profile_click(this, e);
                return;
            }


            if (HiddenFieldMyBlogId.Value.Equals("0"))
            {
                db.Table_MyTouristbook_Blogs.InsertOnSubmit(tab);
            }

            db.SubmitChanges();

            blogs_click(sender, e);

            //MultiView9.ActiveViewIndex = 2;
            MultiView8.ActiveViewIndex = 2;
            readblog2.Focus();
        }

        public void init_blog(int blog_id)
        {

            HiddenField_The_Blog_Id.Value = blog_id.ToString();

            wrong22.Visible = false;
            //nothreads.Visible = false;            

            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.blogsDataContext db3 = new MyTouristBook.blogsDataContext();


            //affsbook.Table_Affsbook_Affiliate tab = new affsbook.Table_Affsbook_Affiliate();

            int page = 1;

            init_blog_page2();


        }

        public void init_blog_page2()
        {
            wrong22.Visible = false;
            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.blogsDataContext db3 = new MyTouristBook.blogsDataContext();

            MyTouristBook.Table_MyTouristbook_Blog tab = new MyTouristBook.Table_MyTouristbook_Blog();



            //affsbook.Table_Affsbook_Affiliate tab = new affsbook.Table_Affsbook_Affiliate();            

            int page = 1;

            int blogId = Convert.ToInt32(HiddenField_The_Blog_Id.Value);

            tab = (from t in db3.Table_MyTouristbook_Blogs
                   where (t.blog_id == blogId)
                   select t).FirstOrDefault();


            if (tab == null)
            {
                wrong22.Visible = true;
                return;
            }

            int the_aid = (int)tab.authoraid;
            MyTouristBook.Table_MyTouristbook_Tourist aff = get_tourist(the_aid);

            the_blog_title.Text = tab.title;
            the_blog_body.Text = tab.body;
            HiddenField_The_Blog_Id.Value = tab.blog_id.ToString();
            HiddenFieldBlogAuthorId.Value = tab.authoraid.ToString();
            The_Image_Blog1.ImageUrl = my_image(aff.profileimage, aff.gender);
            blog_from.Text = aff.fullname;
            theblogimage.ImageUrl = blog_image(tab.imageurl);



            int status = 0;


            if (isConnected(the_aid))
                status = 1;

            else if (isWaiting(the_aid))
                status = 2;



            bool btnvisible = true;

            connectblog.Visible = true;
            contactblogger.Visible = true;
            connectblog.Text = connectButtonString(status);

            if (the_aid == get_aid(Username1.Value))
            {
                connectblog.Visible = false;
                contactblogger.Visible = false;
                btnvisible = false;
            }

            MultiView8.ActiveViewIndex = 1;
            MultiView1.ActiveViewIndex = 7;

            if (btnvisible)
            {
                readblog1.Focus();
            }

            else
            {
                readblog1.Focus();
                //contactblogger.Focus();
            }


        }

        public string blog_image(string imageurl)
        {
            string def = @"https://www.tourist-ads.com/blogimage.png";

            if ((imageurl == null) || (imageurl.Equals("")))
            {
                return def;
            }
            else
                return imageurl;

        }

        protected void blog_theme_upload(object sender, EventArgs e)
        {
            return;
        }

        protected void blog_author_click(object sender, ImageClickEventArgs e)
        {
            string the_aid = HiddenFieldBlogAuthorId.Value;
            Show_Profile(the_aid);
        }

        protected void blog_author_click(object sender, EventArgs e)
        {
            string the_aid = HiddenFieldBlogAuthorId.Value;
            Show_Profile(the_aid);
        }

        protected void blog_author_connect(object sender, EventArgs e)
        {
            string aid = HiddenFieldBlogAuthorId.Value;
            int the_aid = Convert.ToInt32(aid);
            cconnect(the_aid);
            int status = get_status(the_aid);
            connectblog.Text = connectButtonString(status);
            init_blog_page2();
            connectblog.Focus();
        }

        protected void contact_blogger(object sender, EventArgs e)
        {
            string aid = HiddenFieldBlogAuthorId.Value;
            SendMessage(aid);
        }

        protected void blgautclick1(object sender, ImageClickEventArgs e)
        {
            string the_aid = HiddenFieldBlogAuthorId1.Value;
            Show_Profile(the_aid);
        }

        protected void blgautclick2(object sender, ImageClickEventArgs e)
        {
            string the_aid = HiddenFieldBlogAuthorId2.Value;
            Show_Profile(the_aid);
        }

        protected void blgautclick3(object sender, ImageClickEventArgs e)
        {
            string the_aid = HiddenFieldBlogAuthorId3.Value;
            Show_Profile(the_aid);
        }

        protected void blgautclick4(object sender, ImageClickEventArgs e)
        {
            string the_aid = HiddenFieldBlogAuthorId4.Value;
            Show_Profile(the_aid);
        }

        protected void blgautclick5(object sender, ImageClickEventArgs e)
        {
            string the_aid = HiddenFieldBlogAuthorId5.Value;
            Show_Profile(the_aid);
        }

        protected void blgautclick6(object sender, ImageClickEventArgs e)
        {
            string the_aid = HiddenFieldBlogAuthorId6.Value;
            Show_Profile(the_aid);
        }

        protected void blgautclick7(object sender, ImageClickEventArgs e)
        {
            string the_aid = HiddenFieldBlogAuthorId7.Value;
            Show_Profile(the_aid);
        }

        protected void read_blog_click1(object sender, EventArgs e)
        {
            int the_blog_id = Convert.ToInt32(HiddenFieldBlogId1.Value);
            init_blog(the_blog_id);

            if (connectblog.Visible)
                connectblog.Focus();
            else
                The_Image_Blog1.Focus();
        }

        protected void read_blog_click2(object sender, EventArgs e)
        {
            int the_blog_id = Convert.ToInt32(HiddenFieldBlogId2.Value);
            init_blog(the_blog_id);
            if (connectblog.Visible)
                connectblog.Focus();
            else
                The_Image_Blog1.Focus();
        }

        protected void read_blog_click3(object sender, EventArgs e)
        {
            int the_blog_id = Convert.ToInt32(HiddenFieldBlogId3.Value);
            init_blog(the_blog_id);
            if (connectblog.Visible)
                connectblog.Focus();
            else
                The_Image_Blog1.Focus();
        }

        protected void read_blog_click4(object sender, EventArgs e)
        {
            int the_blog_id = Convert.ToInt32(HiddenFieldBlogId4.Value);
            init_blog(the_blog_id);
            if (connectblog.Visible)
                connectblog.Focus();
            else
                The_Image_Blog1.Focus();
        }

        protected void read_blog_click5(object sender, EventArgs e)
        {
            int the_blog_id = Convert.ToInt32(HiddenFieldBlogId5.Value);
            init_blog(the_blog_id);
            if (connectblog.Visible)
                connectblog.Focus();
            else
                The_Image_Blog1.Focus();
        }

        protected void read_blog_click6(object sender, EventArgs e)
        {
            int the_blog_id = Convert.ToInt32(HiddenFieldBlogId6.Value);

            init_blog(the_blog_id);
            if (connectblog.Visible)
                connectblog.Focus();
            else
                The_Image_Blog1.Focus();
        }

        protected void read_blog_click7(object sender, EventArgs e)
        {
            int the_blog_id = Convert.ToInt32(HiddenFieldBlogId7.Value);
            init_blog(the_blog_id);
            if (connectblog.Visible)
                connectblog.Focus();
            else
                The_Image_Blog1.Focus();
        }

        protected void blog_previous_click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(DropDownBlogs.SelectedValue);
            if (page == 1)
            {
                return;
            }
            page--;
            DropDownBlogs.SelectedValue = page.ToString();
            init_blog_page();
            readblog2.Focus();
        }

        protected void blog_next_click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(DropDownBlogs.SelectedValue);
            page++;
            DropDownBlogs.SelectedValue = page.ToString();
            init_blog_page();
            readblog2.Focus();
        }

        protected void blog_page_changed(object sender, EventArgs e)
        {
            init_blog_page();
            readblog2.Focus();
        }

        protected void blgautclick1(object sender, EventArgs e)
        {
            string the_aid = HiddenFieldBlogAuthorId1.Value;
            Show_Profile(the_aid);
        }

        protected void blgautclick2(object sender, EventArgs e)
        {
            string the_aid = HiddenFieldBlogAuthorId2.Value;
            Show_Profile(the_aid);
        }

        protected void blgautclick3(object sender, EventArgs e)
        {
            string the_aid = HiddenFieldBlogAuthorId3.Value;
            Show_Profile(the_aid);
        }

        protected void blgautclick4(object sender, EventArgs e)
        {
            string the_aid = HiddenFieldBlogAuthorId4.Value;
            Show_Profile(the_aid);
        }

        protected void blgautclick5(object sender, EventArgs e)
        {
            string the_aid = HiddenFieldBlogAuthorId5.Value;
            Show_Profile(the_aid);
        }

        protected void blgautclick6(object sender, EventArgs e)
        {
            string the_aid = HiddenFieldBlogAuthorId6.Value;
            Show_Profile(the_aid);
        }

        protected void blgautclick7(object sender, EventArgs e)
        {
            string the_aid = HiddenFieldBlogAuthorId7.Value;
            Show_Profile(the_aid);
        }

        public void Create_Deal(string desc)
        {
            reset_deal();
            reset_deals_controls();

            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.dealsDataContext db2 = new MyTouristBook.dealsDataContext();
            MyTouristBook.CountriesDataContext db4 = new MyTouristBook.CountriesDataContext();

            if (DropDownDeal2Country.Items.Count == 1)
            {

                DropDownDeal2Country.Items.Clear();
                ListItem item3 = new ListItem("Select Country", "0");
                DropDownDeal2Country.Items.Add(item3);


                var countries = (from t in db4.Table_Countries
                                 orderby t.country
                                 select t.country);

                foreach (var country3 in countries)
                {
                    ListItem item2 = new ListItem(country3);
                    DropDownDeal2Country.Items.Add(item2);
                }
            }

            mydealdesc.Text = desc;

            MultiView1.ActiveViewIndex = 6;


            //reset_defaults();

            HiddenFieldMyDealId.Value = "0";

            MultiView9.ActiveViewIndex = 1;

            mydealshortdesc.Focus();

        }
        protected void add_deal_click(object sender, EventArgs e)
        {
            Create_Deal("");
        }


        public void init_deal(int offer_id)
        {

            HiddenFieldOfferId.Value = offer_id.ToString();


            wrong17.Visible = false;
            offer_from.Enabled = true;
            //nothreads.Visible = false;

            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.dealsDataContext db2 = new MyTouristBook.dealsDataContext();
            MyTouristBook.Table_MyTouristbook_Deal tab = new MyTouristBook.Table_MyTouristbook_Deal();



            //affsbook.Table_Affsbook_Affiliate tab = new affsbook.Table_Affsbook_Affiliate();


            tab = (from t in db2.Table_MyTouristbook_Deals
                   where (t.id == offer_id)
                   select t).FirstOrDefault();


            if (tab == null)
            {
                wrong17.Visible = true;
                return;
            }


            HiddenFieldOwnerAid.Value = tab.owneraid.ToString();

            tab.dest1 = destination(tab.dest1);

            deal_title.Text = tab.title;
            dealshort.Text = tab.shortdescription;
            the_deal_desc.Text = tab.description;
            the_pricing.Text = not_available(tab.pricing);
            the_dest2.Text = not_available(tab.dest1);
            the_rating.Text = tab.rating.ToString();
            dealjoinwebsite2.NavigateUrl = tab.dealurl;
            //dealordernwebsite1.NavigateUrl = tab.orderurl;


            if ((tab.imageurl == null) || (tab.imageurl.Equals("")))
            {
                The_Deal_Image.ImageUrl = "~/images/mytouristbooklogosilver.jpg";
                The_Deal_Image.BorderWidth = 0;
            }

            else
            {
                The_Deal_Image.ImageUrl = tab.imageurl;
                The_Deal_Image.BorderWidth = 2;
            }

            int the_int_aid = Convert.ToInt32(tab.owneraid);
            int status = 0;



            if (isConnected(the_int_aid))
                status = 1;

            else if (isWaiting(the_int_aid))
                status = 2;



            connect20.Text = connectButtonString(status);

            int my_aid = get_aid(Username1.Value);
            connect20.Visible = true;
            contactdealowner.Visible = true;
            //connect27.Visible = true;

            if (the_int_aid == my_aid)
            {
                connect20.Visible = false;
                contactdealowner.Visible = false;
                //connect27.Visible = false;
            }


            //the_city2.Text = not_available(tab.city);
            //the_country2.Text = not_available(tab.country);

            MyTouristBook.Table_MyTouristbook_Tourist aff = new MyTouristBook.Table_MyTouristbook_Tourist();

            aff = get_tourist((int)tab.owneraid);

            if (aff != null)
            {
                DealOwnerImage.ImageUrl = my_image(aff.profileimage, aff.gender);
                offer_from.Text = aff.fullname;
            }

            else
            {
                DealOwnerImage.ImageUrl = my_image("", "Male");
                offer_from.Text = "Inactive User";
                connect20.Visible = false;
                contactdealowner.Visible = false;
                offer_from.Enabled = false;

            }


            //init_deal_related();


            MultiView9.ActiveViewIndex = 2;

            if (connect20.Visible == true)
            {
                connect20.Focus();
            }
            else
            {
                DealOwnerImage.Focus();
            }



        }

        public string not_available(string st)
        {
            if (st == null)
            {
                return "-";
            }

            if (st.Equals(""))
                return "-";
            return st;
        }


        protected void add_deal_submit_click(object sender, EventArgs e)
        {

            

            reset_deal();

            if (mydealname.Text.Equals(""))
            {
                dealnameval.Visible = true;
                mydealshortdesc.Focus();
                return;
            }

            if (DropDownDeal2Country.SelectedIndex == 0)
            {
                dealcountryval.Visible = true;
                mydealshortdesc.Focus();
                //Button146.Focus();
                return;
            }

            if (DropDownDeal2City.SelectedIndex == 0)
            {
                dealcityval.Visible = true;
                mydealshortdesc.Focus();
                //Button146.Focus();
                return;
            }

            if (mydealurl.Text.Equals(""))
            {
                dealurlval.Visible = true;
                mydealurl.Focus();
                return;
            }
            if (mydealshortdesc.Text.Equals(""))
            {
                dealshortdescval.Visible = true;
                mydealshortdesc.Focus();
                //Button146.Focus();
                return;
            }

            if (mydealdesc.Text.Equals(""))
            {
                deallongdescval.Visible = true;
                mydealshortdesc.Focus();
                //Button146.Focus();
                return;
            }

            isGuest();


            MyTouristBook.dealsDataContext db = new MyTouristBook.dealsDataContext();
            MyTouristBook.Table_MyTouristbook_Deal tab = new MyTouristBook.Table_MyTouristbook_Deal();


            if (HiddenFieldMyDealId.Value.Equals("0"))
            {
                tab = new MyTouristBook.Table_MyTouristbook_Deal();
                int max2 = 0;

                try
                {

                    max2 = (from t in db.Table_MyTouristbook_Deals
                            select t.id).Max();
                }
                catch (Exception ex)
                {

                }
                tab.id = max2 + 1;
                tab.active = 1;
                tab.date = DateTime.Now;

                string ownerusername = Username1.Value;
                int owneraid = get_aid(ownerusername);
                tab.ownerusername = ownerusername;
                tab.owneraid = owneraid;

                tab.dest1 = DropDownDeal2City.SelectedItem.Text;
                

                tab.rating = 5;
                tab.relatedofferid = 0;
                tab.sponsored = 0;
                tab.featured = 0;
                tab.priority = 0;
                tab.popular = 0;


            }

            else if (!(HiddenFieldMyDealId.Value.Equals("0")))
            {
                int the_id = Convert.ToInt32(HiddenFieldMyDealId.Value);

                tab = (from t in db.Table_MyTouristbook_Deals
                       where t.id == the_id
                       select t).FirstOrDefault();

                if (tab == null)
                    tab = new MyTouristBook.Table_MyTouristbook_Deal();

            }

            tab.title = mydealname.Text;

            //tab.company = mycompany.Text;
            tab.dealurl = mydealurl.Text;

            tab.imageurl = get_category_icon(DropDownDeal2City.SelectedItem.Text);

            tab.dest1 = DropDownDeal2City.SelectedItem.Text;


            /*           
            
            tab.country = DropDownMyOfferCountry.SelectedItem.Text;
            tab.city = myoffercity.Text;
            // city and country

    */

            tab.shortdescription = mydealshortdesc.Text;
            tab.description = mydealdesc.Text;
            tab.pricing = mydealpricing.Text;

            int aid = get_aid(Username1.Value);
            var aff = get_tourist(aid);

            /*
            if (aff != null)
            {
                tab.city = aff.city;
                tab.country = aff.country;
            }*/
            

            if (HiddenFieldMyDealId.Value.Equals("0"))
            {
                db.Table_MyTouristbook_Deals.InsertOnSubmit(tab);
            }

            db.SubmitChanges();

            deals_click(sender, e);            
            MultiView9.ActiveViewIndex = 3;

        }

        public string get_category_icon(string ver)
        {
            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();

            string country = (from t in db.Table_MyTouristbook_Cities
                             where (t.city.Equals(ver))
                             select t.country).FirstOrDefault();

            if (country==null)
            {
                return "";
            }

            var the_niche = (from t in db.Table_MyTouristbook_Countries
                             where (t.country.Equals(country))
                             select t).FirstOrDefault();

            if (country == null)
            {
                wrong19.Visible = true;
                return "https://www.adsrushx.com/mainlogogray.jpg";
            }

            /*
            

            var the_niche = (from t in db.Table_MyTouristbook_Cities
                             where (t.city.Equals(ver))
                             select t).FirstOrDefault();

            */

            if (the_niche == null)
            {
                wrong19.Visible = true;
                return "https://www.adsrushx.com/mainlogogray.jpg";
            }


            string icon = the_niche.icon;
            return icon;

        }

        public string get_country(string ver)
        {
            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();


            var the_niche = (from t in db.Table_MyTouristbook_Cities
                             where (t.city.Equals(ver))
                             select t).FirstOrDefault();

            if (the_niche == null)
            {
                return "";
            }


            string icon = the_niche.country;
            return icon;

        }

        protected void deal_click1(object sender, EventArgs e)
        {
            int the_deal_id = Convert.ToInt32(HiddenFieldDealid1.Value);
            init_deal(the_deal_id);
        }

        protected void manage_deal_click(object sender, EventArgs e)
        {
            DropDownManageDeals.Items.Clear();

            MyTouristBook.dealsDataContext db = new MyTouristBook.dealsDataContext();

            string username = Username1.Value;
            int my_aid = get_aid(username);

            var table = (from t in db.Table_MyTouristbook_Deals
                         where t.owneraid == my_aid
                         select t);

            foreach (var row in table)
            {
                ListItem item = new ListItem(row.title, row.id.ToString());
                DropDownManageDeals.Items.Add(item);
            }

            MultiView9.ActiveViewIndex = 0;
            addoffer1.Focus();
        }

        protected void deal_manage_click(object sender, EventArgs e)
        {
            wrong18.Visible = false;
            string deal_id = DropDownManageDeals.SelectedValue;

            HiddenFieldMyDealId.Value = deal_id;


            int coupon_id_int = Convert.ToInt32(deal_id);

            MyTouristBook.dealsDataContext db = new MyTouristBook.dealsDataContext();
            MyTouristBook.Table_MyTouristbook_Deal tab = new MyTouristBook.Table_MyTouristbook_Deal();



            tab = (from t in db.Table_MyTouristbook_Deals
                   where t.id == coupon_id_int
                   select t).FirstOrDefault();

            if (tab == null)
            {
                wrong18.Visible = true;
                return;

            }

            reset_deal();


            mydealname.Text = tab.title;

            mydealurl.Text = tab.dealurl;

            MyTouristBook.CountriesDataContext db4 = new MyTouristBook.CountriesDataContext();

            if (DropDownDeal2Country.Items.Count == 1)
            {

                DropDownDeal2Country.Items.Clear();
                ListItem item5 = new ListItem("Select Country", "0");
                DropDownDeal2Country.Items.Add(item5);


                var countries = (from t in db4.Table_Countries
                                 orderby t.country
                                 select t.country);

                foreach (var country3 in countries)
                {
                    ListItem item2 = new ListItem(country3);
                    DropDownDeal2Country.Items.Add(item2);
                }
            }

            string country = DropDownDeal2Country.SelectedItem.Text;

            DropDownDeal2City.Items.Clear();
            ListItem item3 = new ListItem("Select City", "0");
            DropDownDeal2City.Items.Add(item3);

            if (country.Equals("Select Country"))
            {
                var cities = (from t in db4.Table_Cities
                              where t.country.Equals(country)
                              orderby t.city
                              select t.city);

                foreach (var city in cities)
                {
                    ListItem item2 = new ListItem(city);
                    DropDownDeal2City.Items.Add(item2);
                }
            }



            /*
             * 
            foreach (ListItem li in DropDownOfferVertical.Items)
            {
                if (li.Text.Equals(tab.niche1))
                {
                    DropDownOfferVertical.SelectedValue = li.Value;
                }

            }

            foreach (ListItem li in DropDownOfferType.Items)
            {
                if (li.Text.Equals(tab.commissiontype))
                {
                    DropDownOfferType.SelectedValue = li.Value;
                }

            }

    


            DropDownMyOfferCountry.SelectedValue = "0";

            foreach (ListItem li in DropDownMyOfferCountry.Items)
            {
                if (li.Text.Equals(tab.country))
                {
                    DropDownMyOfferCountry.SelectedValue = li.Value;
                }

            }

    */


            //myoffercountry.Text=tab.country;

            mydealshortdesc.Text = tab.shortdescription;
            mydealdesc.Text = tab.description;
            mydealpricing.Text = tab.pricing;

            HiddenFieldMyDealId.Value = tab.id.ToString();


            MultiView9.ActiveViewIndex = 1;
            mydealshortdesc.Focus();
            //Button146.Focus();
        }

        protected void deal_promote_click(object sender, EventArgs e)
        {

        }

        protected void offer_image_upload_click(object sender, EventArgs e)
        {
            return;
        }

        protected void deal_connect_click(object sender, EventArgs e)
        {
            string aid = HiddenFieldOwnerAid.Value;
            int the_aid = Convert.ToInt32(aid);
            cconnect(the_aid);
            int status = get_status(the_aid);
            connect20.Text = connectButtonString(status);
            init_deal_page();
            connect20.Focus();
        }

        protected void contact_deal_owner(object sender, EventArgs e)
        {
            string aid = HiddenFieldOwnerAid.Value;
            SendMessage(aid);
        }


        protected void deal_click1(object sender, ImageClickEventArgs e)
        {
            int the_deal_id = Convert.ToInt32(HiddenFieldDealid1.Value);
            init_deal(the_deal_id);
        }

        protected void deal_click2(object sender, ImageClickEventArgs e)
        {
            int the_deal_id = Convert.ToInt32(HiddenFieldDealid2.Value);
            init_deal(the_deal_id);
        }

        protected void deal_click2(object sender, EventArgs e)
        {
            int the_deal_id = Convert.ToInt32(HiddenFieldDealid2.Value);
            init_deal(the_deal_id);
        }

        protected void deal_click3(object sender, ImageClickEventArgs e)
        {
            int the_deal_id = Convert.ToInt32(HiddenFieldDealid3.Value);
            init_deal(the_deal_id);
        }

        protected void deal_click3(object sender, EventArgs e)
        {
            int the_deal_id = Convert.ToInt32(HiddenFieldDealid3.Value);
            init_deal(the_deal_id);
        }

        protected void deal_click4(object sender, ImageClickEventArgs e)
        {
            int the_deal_id = Convert.ToInt32(HiddenFieldDealid4.Value);
            init_deal(the_deal_id);
        }

        protected void deal_click4(object sender, EventArgs e)
        {
            int the_deal_id = Convert.ToInt32(HiddenFieldDealid4.Value);
            init_deal(the_deal_id);
        }

        protected void deal_click5(object sender, ImageClickEventArgs e)
        {
            int the_deal_id = Convert.ToInt32(HiddenFieldDealid5.Value);
            init_deal(the_deal_id);
        }

        protected void deal_click5(object sender, EventArgs e)
        {
            int the_deal_id = Convert.ToInt32(HiddenFieldDealid5.Value);
            init_deal(the_deal_id);
        }

        protected void deal_click6(object sender, ImageClickEventArgs e)
        {
            int the_deal_id = Convert.ToInt32(HiddenFieldDealid6.Value);
            init_deal(the_deal_id);
        }

        protected void deal_click6(object sender, EventArgs e)
        {
            int the_deal_id = Convert.ToInt32(HiddenFieldDealid6.Value);
            init_deal(the_deal_id);
        }

        protected void deal_click7(object sender, ImageClickEventArgs e)
        {
            int the_deal_id = Convert.ToInt32(HiddenFieldDealid7.Value);
            init_deal(the_deal_id);
        }

        protected void deal_click7(object sender, EventArgs e)
        {
            int the_deal_id = Convert.ToInt32(HiddenFieldDealid7.Value);
            init_deal(the_deal_id);
        }



        protected void previous_deal_click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(DropDownDeals.SelectedValue);
            if (page == 1)
            {
                return;
            }
            page--;
            DropDownDeals.SelectedValue = page.ToString();
            init_deal_page();
            info2.Focus();
        }

        protected void next_deal_click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(DropDownDeals.SelectedValue);
            page++;
            DropDownDeals.SelectedValue = page.ToString();
            init_deal_page();
            info2.Focus();
        }

        protected void deal_page_changed(object sender, EventArgs e)
        {
            init_deal_page();
            info2.Focus();
        }

        protected void dealownerclick(object sender, ImageClickEventArgs e)
        {
            string the_aid = HiddenFieldOwnerAid.Value;
            Show_Profile(the_aid);
        }

        protected void dealownerclick(object sender, EventArgs e)
        {
            string the_aid = HiddenFieldOwnerAid.Value;
            Show_Profile(the_aid);
        }

        public bool city_included(string country, string city)
        {

            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();

            if (country.Equals("Select Country"))
            {
                return true;
            }

            if (country.Equals("Select City"))
            {
                var the_cities3 = (from t in db.Table_MyTouristbook_Countries
                                   where t.country.Equals(country)
                                   select t);

                if (the_cities3 != null)
                    return true;

                return false;
            }

            var the_cities = (from t in db.Table_MyTouristbook_Cities
                              where t.country.Equals(country)
                              select t);

            foreach (var mycity in the_cities)
            {
                if (mycity.Equals(city))
                    return true;

            }


            return false;


        }

        public bool find_dest(string dropcountry, string dropcity, string dest)
        {


            if (dropcity.Equals("Select City"))
            {
                string find = find_country(dest);

                if (find.Equals(""))
                    return false;

                return (find.Equals(dropcountry));
            }

            return dropcity.Equals(dest);
        }



        public void init_newsfeed()
        {
            MyTouristBook.ForumsDataContext db1 = new MyTouristBook.ForumsDataContext();
            MyTouristBook.dealsDataContext db2 = new MyTouristBook.dealsDataContext();
            MyTouristBook.TouristsDataContext db3 = new MyTouristBook.TouristsDataContext();
            MyTouristBook.blogsDataContext db4 = new MyTouristBook.blogsDataContext();

            /*


            if (DropDownFeedCountry.Items.Count == 1)
            {

                DropDownFeedCountry.Items.Clear();
                ListItem item3 = new ListItem("Select Country");
                DropDownFeedCountry.Items.Add(item3);


                MyTouristBook.CountriesDataContext db5 = new MyTouristBook.CountriesDataContext();

                var countries = (from t in db5.Table_Countries
                                 orderby t.country
                                 select t.country);

                foreach (var country in countries)
                {
                    ListItem item2 = new ListItem(country);
                    DropDownFeedCountry.Items.Add(item2);
                }
            }

            */

            string country = DropDownCountries.SelectedItem.Text;
            string dest = DropDownCities.SelectedItem.Text;

            DropDownMain.Visible = true;

            var the_forums = (from t in db1.Table_MyTouristbook_Forum_Threads
                              where ((t.replynumber == 0) && ((t.dest1.Equals(dest)) || (dest.Equals("Select City"))))
                              orderby t.startdate descending
                              select t).Take(10);

            /*var the_forums = (from t in db1.Table_MyTouristbook_Forum_Threads
                  where (find_dest(country,dest,t.dest1))
                  orderby t.startdate descending
                  select t).Take(10); */



            /*var the_deals = (from t in db2.Table_MyTouristbook_Deals
                              where (find_dest(country,dest,t.dest1))
                              orderby t.date descending
                              select t).Take(15);

            */

            var the_deals = (from t in db2.Table_MyTouristbook_Deals
                             where ((t.dest1.Equals(dest)) || (dest.Equals("Select City")))
                             orderby t.date descending
                             select t).Take(15);

            var the_blogs = (from t in db4.Table_MyTouristbook_Blogs
                             where ((t.dest1.Equals(dest)) || (dest.Equals("Select City")))
                             orderby t.startdate descending
                             select t).Take(10);




            int count_frms = the_forums.Count();
            int count_deals = the_deals.Count();
            int count_blgs = the_blogs.Count();

            //the_feed = new List<FeedInfo>();

            List<FeedInfo> the_feed_temp = new List<FeedInfo>();

            foreach (var feed in the_forums)
            {
                DateTime? feeddate = feed.startdate;
                FeedInfo myfeed = new FeedInfo("forum", (int)feed.authoraid, feeddate, feed.subject, feed.dest1);
                myfeed.threadid = feed.thread_id;
                the_feed_temp.Add(myfeed);
            }


            foreach (var feed in the_blogs)
            {
                DateTime? feeddate = feed.startdate;
                FeedInfo myfeed = new FeedInfo("blog", (int)feed.authoraid, feeddate, feed.title, feed.dest1);
                myfeed.blogid = feed.blog_id;
                the_feed_temp.Add(myfeed);
            }

            the_feed = the_feed_temp.OrderByDescending(date => date.date).ToList();

            the_deals = the_deals.OrderBy(date => date.date);

            foreach (var feed in the_deals)
            {
                DateTime? feeddate = feed.date;
                FeedInfo myfeed = new FeedInfo("deal", (int)feed.owneraid, feeddate, feed.shortdescription, feed.dest1);

                myfeed.title = feed.title;
                myfeed.shortdesc = feed.shortdescription;
                myfeed.dealid = feed.id;
                myfeed.imageurl = feed.imageurl;
                the_feed.Insert(0, myfeed);
                //the_feed_temp.Add(myfeed);
            }

            int page = 1;

            decimal count = the_feed.Count();

            decimal memcounter = Math.Ceiling(count / 7);

            DropDownMain.Items.Clear();

            if (count == 0)
            {
                DropDownMain.Visible = false;
            }


            for (int the_page = 1; the_page <= memcounter; the_page++)
            {
                ListItem item = new ListItem("Page " + the_page, the_page.ToString());
                DropDownMain.Items.Add(item);
            }

            init_newsfeed_page();

            init_related();

        }


        public string get_icon(int kind)
        {
            if (kind == 1)    // forum
            {
                return "~/images/forumthread.jpg";
            }

            if (kind == 2)    // deal
            {
                return "~/images/deal3.jpg";
            }

            if (kind == 3)    // blog
            {
                return "~/images/blog3.png";
            }

            return "~/images/forumthread.jpg";
        }

        public void init_newsfeed_page()
        {

            wrong19.Visible = false;
            noupdates.Visible = false;

            //affsbook.AffiliatesDataContext db = new affsbook.AffiliatesDataContext();
            //affsbook.Table_Affsbook_Affiliate tab = new affsbook.Table_Affsbook_Affiliate();            

            int page = 1;

            try
            {
                page = Convert.ToInt32(DropDownMain.SelectedValue);
            }
            catch (Exception ex)
            {

            }


            if (the_feed == null)
            {
                wrong19.Visible = true;
                return;
            }


            decimal count = the_feed.Count();

            decimal memcounter = Math.Ceiling(count / 7);

            nextmain.Visible = false;
            previousmain.Visible = false;

            if (count == 0)
            {
                noupdates.Visible = true;

            }

            if (page < memcounter)
            {
                nextmain.Visible = true;
            }

            if (page > 1)
            {
                previousmain.Visible = true;
            }

            List<FeedInfo> the_feed_last = new List<FeedInfo>();

            the_feed_last = the_feed.Skip(7 * (page - 1)).Take(7).ToList();


            int counter = 1;

            PanelFeed1.Visible = false;
            PanelFeed2.Visible = false;
            PanelFeed3.Visible = false;
            PanelFeed4.Visible = false;
            PanelFeed5.Visible = false;
            PanelFeed6.Visible = false;
            PanelFeed7.Visible = false;


            foreach (FeedInfo info in the_feed_last)
            {

                MyTouristBook.Table_MyTouristbook_Tourist aff = new MyTouristBook.Table_MyTouristbook_Tourist();
                aff = get_tourist(info.authoraid);

                if (aff == null)
                {
                    return;
                }

                info.destination = destination(info.destination);

                if (counter == 1)
                {

                    if (info.kind.Equals("forum"))
                    {
                        TheFeedImageButton1.ImageUrl = get_icon(1);                        
                        authorordeal1.Text = "Author:";
                        feedfrom1.Text = aff.fullname;
                        destination1.Text = (info.destination == null) ? "General" : info.destination;
                        feedsubject1.Text = info.title;
                        feedbutton1.Text = "Read Thread";
                        HiddenFieldFeedId1.Value = info.threadid.ToString();

                        //HiddenFieldType1.Value = "forum";

                        HiddenFieldAuthor1.Value = info.authoraid.ToString();
                        feedimagebutton1.ImageUrl = my_image(aff.profileimage, aff.gender);
                        PanelFeed1.Visible = true;
                    }



                    else if (info.kind.Equals("deal"))
                    {

                        TheFeedImageButton1.ImageUrl = get_icon(2);
                        authorordeal1.Text = "Deal:";
                        feedfrom1.Text = info.title;
                        destination1.Text = (info.destination == null) ? "General" : info.destination;
                        feedsubject1.Text = info.shortdesc;

                        feedbutton1.Text = "Check Deal";
                        HiddenFieldFeedId1.Value = info.dealid.ToString();
                        feedimagebutton1.ImageUrl = init_picture(info.imageurl);

                        if ((info.imageurl == null) || (info.imageurl.Equals("")))
                            feedimagebutton1.BorderWidth = 0;

                        // HiddenFieldType1.Value = "deal";

                        PanelFeed1.Visible = true;
                    }

                    else if (info.kind.Equals("blog"))
                    {
                        TheFeedImageButton1.ImageUrl = get_icon(3);
                        authorordeal1.Text = "Author:";
                        feedfrom1.Text = aff.fullname;
                        destination1.Text = (info.destination == null) ? "General" : info.destination;
                        feedsubject1.Text = info.title;

                        feedbutton1.Text = "Read Blog";
                        HiddenFieldFeedId1.Value = info.blogid.ToString();


                        HiddenFieldAuthor1.Value = info.authoraid.ToString();
                        feedimagebutton1.ImageUrl = my_image(aff.profileimage, aff.gender);
                        PanelFeed1.Visible = true;
                    }

                }



                if (counter == 2)
                {
                    if (info.kind.Equals("forum"))
                    {

                        TheFeedImageButton2.ImageUrl = get_icon(1);
                        authorordeal2.Text = "Author:";
                        feedfrom2.Text = aff.fullname;
                        destination2.Text = (info.destination == null) ? "General" : info.destination;
                        feedsubject2.Text = info.title;
                        feedbutton2.Text = "Read Thread";
                        HiddenFieldFeedId2.Value = info.threadid.ToString();

                        //HiddenFieldType1.Value = "forum";

                        HiddenFieldAuthor2.Value = info.authoraid.ToString();
                        feedimagebutton2.ImageUrl = my_image(aff.profileimage, aff.gender);
                        PanelFeed2.Visible = true;
                    }



                    else if (info.kind.Equals("deal"))
                    {

                        TheFeedImageButton2.ImageUrl = get_icon(2);
                        authorordeal2.Text = "Deal:";
                        feedfrom2.Text = info.title;
                        destination2.Text = (info.destination == null) ? "General" : info.destination;
                        feedsubject2.Text = info.shortdesc;

                        feedbutton2.Text = "Check Deal";
                        HiddenFieldFeedId2.Value = info.dealid.ToString();
                        feedimagebutton2.ImageUrl = init_picture(info.imageurl);

                        if ((info.imageurl == null) || (info.imageurl.Equals("")))
                            feedimagebutton2.BorderWidth = 0;

                        // HiddenFieldType1.Value = "deal";

                        PanelFeed2.Visible = true;
                    }

                    else if (info.kind.Equals("blog"))
                    {
                        TheFeedImageButton2.ImageUrl = get_icon(3);
                        authorordeal2.Text = "Author:";
                        feedfrom2.Text = aff.fullname;
                        destination2.Text = (info.destination == null) ? "General" : info.destination;
                        feedsubject2.Text = info.title;

                        feedbutton2.Text = "Read Blog";
                        HiddenFieldFeedId2.Value = info.blogid.ToString();


                        HiddenFieldAuthor2.Value = info.authoraid.ToString();
                        feedimagebutton2.ImageUrl = my_image(aff.profileimage, aff.gender);
                        PanelFeed2.Visible = true;
                    }
                }


                if (counter == 3)
                {
                    if (info.kind.Equals("forum"))
                    {

                        TheFeedImageButton3.ImageUrl = get_icon(1);
                        authorordeal3.Text = "Author:";
                        feedfrom3.Text = aff.fullname;
                        destination3.Text = (info.destination == null) ? "General" : info.destination;
                        feedsubject3.Text = info.title;
                        feedbutton3.Text = "Read Thread";
                        HiddenFieldFeedId3.Value = info.threadid.ToString();

                        //HiddenFieldType1.Value = "forum";

                        HiddenFieldAuthor3.Value = info.authoraid.ToString();
                        feedimagebutton3.ImageUrl = my_image(aff.profileimage, aff.gender);
                        PanelFeed3.Visible = true;
                    }



                    else if (info.kind.Equals("deal"))
                    {

                        TheFeedImageButton3.ImageUrl = get_icon(2);
                        authorordeal3.Text = "Deal:";
                        feedfrom3.Text = info.title;
                        destination3.Text = (info.destination == null) ? "General" : info.destination;
                        feedsubject3.Text = info.shortdesc;

                        feedbutton3.Text = "Check Deal";
                        HiddenFieldFeedId3.Value = info.dealid.ToString();
                        feedimagebutton3.ImageUrl = init_picture(info.imageurl);

                        if ((info.imageurl == null) || (info.imageurl.Equals("")))
                            feedimagebutton3.BorderWidth = 0;

                        // HiddenFieldType1.Value = "deal";

                        PanelFeed3.Visible = true;
                    }

                    else if (info.kind.Equals("blog"))
                    {
                        TheFeedImageButton3.ImageUrl = get_icon(3);
                        authorordeal3.Text = "Author:";
                        feedfrom3.Text = aff.fullname;
                        destination3.Text = (info.destination == null) ? "General" : info.destination;
                        feedsubject3.Text = info.title;

                        feedbutton3.Text = "Read Blog";
                        HiddenFieldFeedId3.Value = info.blogid.ToString();


                        HiddenFieldAuthor3.Value = info.authoraid.ToString();
                        feedimagebutton3.ImageUrl = my_image(aff.profileimage, aff.gender);
                        PanelFeed3.Visible = true;
                    }

                }

                if (counter == 4)
                {
                    if (info.kind.Equals("forum"))
                    {

                        TheFeedImageButton4.ImageUrl = get_icon(1);
                        authorordeal4.Text = "Author:";
                        feedfrom4.Text = aff.fullname;
                        destination4.Text = (info.destination == null) ? "General" : info.destination;
                        feedsubject4.Text = info.title;
                        feedbutton4.Text = "Read Thread";
                        HiddenFieldFeedId4.Value = info.threadid.ToString();

                        //HiddenFieldType1.Value = "forum";

                        HiddenFieldAuthor4.Value = info.authoraid.ToString();
                        feedimagebutton4.ImageUrl = my_image(aff.profileimage, aff.gender);
                        PanelFeed4.Visible = true;
                    }



                    else if (info.kind.Equals("deal"))
                    {

                        TheFeedImageButton4.ImageUrl = get_icon(2);
                        authorordeal4.Text = "Deal:";
                        feedfrom4.Text = info.title;
                        destination4.Text = (info.destination == null) ? "General" : info.destination;
                        feedsubject4.Text = info.shortdesc;

                        feedbutton4.Text = "Check Deal";
                        HiddenFieldFeedId4.Value = info.dealid.ToString();
                        feedimagebutton4.ImageUrl = init_picture(info.imageurl);

                        if ((info.imageurl == null) || (info.imageurl.Equals("")))
                            feedimagebutton4.BorderWidth = 0;

                        // HiddenFieldType1.Value = "deal";

                        PanelFeed4.Visible = true;
                    }

                    else if (info.kind.Equals("blog"))
                    {
                        TheFeedImageButton4.ImageUrl = get_icon(3);
                        authorordeal4.Text = "Author:";
                        feedfrom4.Text = aff.fullname;
                        destination4.Text = (info.destination == null) ? "General" : info.destination;
                        feedsubject4.Text = info.title;

                        feedbutton4.Text = "Read Blog";
                        HiddenFieldFeedId4.Value = info.blogid.ToString();


                        HiddenFieldAuthor4.Value = info.authoraid.ToString();
                        feedimagebutton4.ImageUrl = my_image(aff.profileimage, aff.gender);
                        PanelFeed4.Visible = true;
                    }

                }

                if (counter == 5)
                {
                    if (info.kind.Equals("forum"))
                    {

                        TheFeedImageButton5.ImageUrl = get_icon(1);
                        authorordeal5.Text = "Author:";
                        feedfrom5.Text = aff.fullname;
                        destination5.Text = (info.destination == null) ? "General" : info.destination;
                        feedsubject5.Text = info.title;
                        feedbutton5.Text = "Read Thread";
                        HiddenFieldFeedId5.Value = info.threadid.ToString();

                        //HiddenFieldType1.Value = "forum";

                        HiddenFieldAuthor5.Value = info.authoraid.ToString();
                        feedimagebutton5.ImageUrl = my_image(aff.profileimage, aff.gender);
                        PanelFeed5.Visible = true;
                    }



                    else if (info.kind.Equals("deal"))
                    {

                        TheFeedImageButton5.ImageUrl = get_icon(2);
                        authorordeal5.Text = "Deal:";
                        feedfrom5.Text = info.title;
                        destination5.Text = (info.destination == null) ? "General" : info.destination;
                        feedsubject5.Text = info.shortdesc;

                        feedbutton5.Text = "Check Deal";
                        HiddenFieldFeedId5.Value = info.dealid.ToString();
                        feedimagebutton5.ImageUrl = init_picture(info.imageurl);

                        if ((info.imageurl == null) || (info.imageurl.Equals("")))
                            feedimagebutton5.BorderWidth = 0;

                        // HiddenFieldType1.Value = "deal";

                        PanelFeed5.Visible = true;
                    }

                    else if (info.kind.Equals("blog"))
                    {
                        TheFeedImageButton5.ImageUrl = get_icon(3);
                        authorordeal5.Text = "Author:";
                        feedfrom5.Text = aff.fullname;
                        destination5.Text = (info.destination == null) ? "General" : info.destination;
                        feedsubject5.Text = info.title;

                        feedbutton5.Text = "Read Blog";
                        HiddenFieldFeedId5.Value = info.blogid.ToString();


                        HiddenFieldAuthor5.Value = info.authoraid.ToString();
                        feedimagebutton5.ImageUrl = my_image(aff.profileimage, aff.gender);
                        PanelFeed5.Visible = true;
                    }

                }

                if (counter == 6)
                {
                    if (info.kind.Equals("forum"))
                    {

                        TheFeedImageButton6.ImageUrl = get_icon(1);
                        authorordeal6.Text = "Author:";
                        feedfrom6.Text = aff.fullname;
                        destination6.Text = (info.destination == null) ? "General" : info.destination;
                        feedsubject6.Text = info.title;
                        feedbutton6.Text = "Read Thread";
                        HiddenFieldFeedId6.Value = info.threadid.ToString();

                        //HiddenFieldType1.Value = "forum";

                        HiddenFieldAuthor6.Value = info.authoraid.ToString();
                        feedimagebutton6.ImageUrl = my_image(aff.profileimage, aff.gender);
                        PanelFeed6.Visible = true;
                    }



                    else if (info.kind.Equals("deal"))
                    {

                        TheFeedImageButton6.ImageUrl = get_icon(2);
                        authorordeal6.Text = "Deal:";
                        feedfrom6.Text = info.title;
                        destination6.Text = (info.destination == null) ? "General" : info.destination;
                        feedsubject6.Text = info.shortdesc;

                        feedbutton6.Text = "Check Deal";
                        HiddenFieldFeedId6.Value = info.dealid.ToString();
                        feedimagebutton6.ImageUrl = init_picture(info.imageurl);

                        if ((info.imageurl == null) || (info.imageurl.Equals("")))
                            feedimagebutton6.BorderWidth = 0;

                        // HiddenFieldType1.Value = "deal";

                        PanelFeed6.Visible = true;
                    }

                    else if (info.kind.Equals("blog"))
                    {
                        TheFeedImageButton6.ImageUrl = get_icon(3);
                        authorordeal6.Text = "Author:";
                        feedfrom6.Text = aff.fullname;
                        destination6.Text = (info.destination == null) ? "General" : info.destination;
                        feedsubject6.Text = info.title;

                        feedbutton6.Text = "Read Blog";
                        HiddenFieldFeedId6.Value = info.blogid.ToString();


                        HiddenFieldAuthor6.Value = info.authoraid.ToString();
                        feedimagebutton6.ImageUrl = my_image(aff.profileimage, aff.gender);
                        PanelFeed6.Visible = true;
                    }

                }

                if (counter == 7)
                {
                    if (info.kind.Equals("forum"))
                    {

                        TheFeedImageButton7.ImageUrl = get_icon(1);
                        authorordeal7.Text = "Author:";
                        feedfrom7.Text = aff.fullname;
                        destination7.Text = (info.destination == null) ? "General" : info.destination;
                        feedsubject7.Text = info.title;
                        feedbutton7.Text = "Read Thread";
                        HiddenFieldFeedId7.Value = info.threadid.ToString();

                        //HiddenFieldType1.Value = "forum";

                        HiddenFieldAuthor7.Value = info.authoraid.ToString();
                        feedimagebutton7.ImageUrl = my_image(aff.profileimage, aff.gender);
                        PanelFeed7.Visible = true;
                    }



                    else if (info.kind.Equals("deal"))
                    {

                        TheFeedImageButton7.ImageUrl = get_icon(2);
                        authorordeal7.Text = "Deal:";
                        feedfrom7.Text = info.title;
                        destination7.Text = (info.destination == null) ? "General" : info.destination;
                        feedsubject7.Text = info.shortdesc;

                        feedbutton7.Text = "Check Deal";
                        HiddenFieldFeedId7.Value = info.dealid.ToString();
                        feedimagebutton7.ImageUrl = init_picture(info.imageurl);

                        if ((info.imageurl == null) || (info.imageurl.Equals("")))
                            feedimagebutton7.BorderWidth = 0;

                        // HiddenFieldType1.Value = "deal";

                        PanelFeed7.Visible = true;
                    }

                    else if (info.kind.Equals("blog"))
                    {
                        TheFeedImageButton7.ImageUrl = get_icon(3);
                        authorordeal7.Text = "Author:";
                        feedfrom7.Text = aff.fullname;
                        destination7.Text = (info.destination == null) ? "General" : info.destination;
                        feedsubject7.Text = info.title;

                        feedbutton7.Text = "Read Blog";
                        HiddenFieldFeedId7.Value = info.blogid.ToString();


                        HiddenFieldAuthor7.Value = info.authoraid.ToString();
                        feedimagebutton7.ImageUrl = my_image(aff.profileimage, aff.gender);
                        PanelFeed7.Visible = true;
                    }

                }


                counter++;

            }

            //ImageButton27.Focus();


        }

        public string destination(string city)
        {
            string country = get_country(city);
            return city + ", " + country;
        }

        protected void newsauthorclick1(object sender, ImageClickEventArgs e)
        {
            if (feedbutton1.Text.Equals("Check Deal"))      // offer
            {
                int the_offer_id = Convert.ToInt32(HiddenFieldFeedId1.Value);
                Show_Deal(the_offer_id);
                return;
            }

            string the_aid = HiddenFieldAuthor1.Value;

            Show_Profile(the_aid);
        }

        protected void newsauthorclick1(object sender, EventArgs e)
        {
            if (feedbutton1.Text.Equals("Check Deal"))      // offer
            {
                int the_offer_id = Convert.ToInt32(HiddenFieldFeedId1.Value);
                Show_Deal(the_offer_id);
                return;
            }

            string the_aid = HiddenFieldAuthor1.Value;

            Show_Profile(the_aid);


        }

        protected void newsauthorclick2(object sender, ImageClickEventArgs e)
        {
            if (feedbutton2.Text.Equals("Check Deal"))      // offer
            {
                int the_offer_id = Convert.ToInt32(HiddenFieldFeedId2.Value);
                Show_Deal(the_offer_id);
                return;
            }

            string the_aid = HiddenFieldAuthor2.Value;

            Show_Profile(the_aid);
        }

        protected void newsauthorclick2(object sender, EventArgs e)
        {
            if (feedbutton2.Text.Equals("Check Deal"))      // offer
            {
                int the_offer_id = Convert.ToInt32(HiddenFieldFeedId2.Value);
                Show_Deal(the_offer_id);
                return;
            }

            string the_aid = HiddenFieldAuthor2.Value;

            Show_Profile(the_aid);
        }

        protected void newsauthorclick3(object sender, EventArgs e)
        {
            if (feedbutton3.Text.Equals("Check Deal"))      // offer
            {
                int the_offer_id = Convert.ToInt32(HiddenFieldFeedId3.Value);
                Show_Deal(the_offer_id);
                return;
            }

            string the_aid = HiddenFieldAuthor3.Value;

            Show_Profile(the_aid);
        }

        protected void newsauthorclick3(object sender, ImageClickEventArgs e)
        {
            if (feedbutton4.Text.Equals("Check Deal"))      // offer
            {
                int the_offer_id = Convert.ToInt32(HiddenFieldFeedId3.Value);
                Show_Deal(the_offer_id);
                return;
            }

            string the_aid = HiddenFieldAuthor4.Value;

            Show_Profile(the_aid);
        }

        protected void newsauthorclick4(object sender, ImageClickEventArgs e)
        {
            if (feedbutton4.Text.Equals("Check Deal"))      // offer
            {
                int the_offer_id = Convert.ToInt32(HiddenFieldFeedId4.Value);
                Show_Deal(the_offer_id);
                return;
            }

            string the_aid = HiddenFieldAuthor4.Value;

            Show_Profile(the_aid);
        }

        protected void newsauthorclick4(object sender, EventArgs e)
        {
            if (feedbutton4.Text.Equals("Check Deal"))      // offer
            {
                int the_offer_id = Convert.ToInt32(HiddenFieldFeedId4.Value);
                Show_Deal(the_offer_id);
                return;
            }

            string the_aid = HiddenFieldAuthor4.Value;

            Show_Profile(the_aid);
        }

        protected void newsauthorclick5(object sender, ImageClickEventArgs e)
        {
            if (feedbutton4.Text.Equals("Check Deal"))      // offer
            {
                int the_offer_id = Convert.ToInt32(HiddenFieldFeedId5.Value);
                Show_Deal(the_offer_id);
                return;
            }

            string the_aid = HiddenFieldAuthor4.Value;

            Show_Profile(the_aid);
        }

        protected void newsauthorclick5(object sender, EventArgs e)
        {
            if (feedbutton5.Text.Equals("Check Deal"))      // offer
            {
                int the_offer_id = Convert.ToInt32(HiddenFieldFeedId5.Value);
                Show_Deal(the_offer_id);
                return;
            }

            string the_aid = HiddenFieldAuthor5.Value;

            Show_Profile(the_aid);
        }

        protected void newsauthorclick6(object sender, ImageClickEventArgs e)
        {
            if (feedbutton6.Text.Equals("Check Deal"))      // offer
            {
                int the_offer_id = Convert.ToInt32(HiddenFieldFeedId6.Value);
                Show_Deal(the_offer_id);
                return;
            }

            string the_aid = HiddenFieldAuthor6.Value;

            Show_Profile(the_aid);
        }

        protected void newsauthorclick6(object sender, EventArgs e)
        {
            if (feedbutton6.Text.Equals("Check Deal"))      // offer
            {
                int the_offer_id = Convert.ToInt32(HiddenFieldFeedId6.Value);
                Show_Deal(the_offer_id);
                return;
            }

            string the_aid = HiddenFieldAuthor6.Value;

            Show_Profile(the_aid);
        }

        protected void newsauthorclick7(object sender, ImageClickEventArgs e)
        {
            if (feedbutton7.Text.Equals("Check Deal"))      // offer
            {
                int the_offer_id = Convert.ToInt32(HiddenFieldFeedId7.Value);
                Show_Deal(the_offer_id);
                return;
            }

            string the_aid = HiddenFieldAuthor7.Value;

            Show_Profile(the_aid);
        }

        protected void newsauthorclick7(object sender, EventArgs e)
        {
            if (feedbutton7.Text.Equals("Check Deal"))      // offer
            {
                int the_offer_id = Convert.ToInt32(HiddenFieldFeedId7.Value);
                Show_Deal(the_offer_id);
                return;
            }

            string the_aid = HiddenFieldAuthor7.Value;

            Show_Profile(the_aid);
        }

        protected void previous_news_click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(DropDownMain.SelectedValue);
            if (page == 1)
            {
                return;
            }
            page--;
            DropDownMain.SelectedValue = page.ToString();
            init_newsfeed_page();
            feedbutton2.Focus();
        }

        protected void next_news_click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(DropDownMain.SelectedValue);
            page++;
            DropDownMain.SelectedValue = page.ToString();
            init_newsfeed_page();
            feedbutton2.Focus();
        }

        protected void news_change_click(object sender, EventArgs e)
        {
            init_newsfeed_page();
            feedbutton2.Focus();
        }

        public void Show_Deal(string the_deal)
        {
            int deal_to = 0;
            try
            {
                deal_to = Convert.ToInt32(the_deal);
            }
            catch (Exception ex)
            {
                return;
            }
            Show_Deal(deal_to);
        }

        public void Show_Deal(int the_aid)
        {
            MultiView1.ActiveViewIndex = 6;
            init_deal(the_aid);

            if (connect20.Visible == true)
            {
                connect20.Focus();
            }
            else
            {
                DealOwnerImage.Focus();
            }

        }

        public bool Is_Self_Aid(string aid)
        {
            string my_aid = get_aid(Username1.Value).ToString();
            return my_aid.Equals(aid);
        }

        public void Show_Blog(string blogId)
        {
            int blog_to = 0;
            try
            {
                blog_to = Convert.ToInt32(blogId);
            }
            catch (Exception ex)
            {
                return;
            }
            Show_Blog(blog_to);
        }

        public void Show_Blog(int the_blog_id)
        {
            init_blog(the_blog_id);

            if (connectblog.Visible)
                connectblog.Focus();
            else
                The_Image_Blog1.Focus();
        }

        public void Show_Forum(string forum_id)
        {
            int forum_to = 0;
            try
            {
                forum_to = Convert.ToInt32(forum_id);
            }
            catch (Exception ex)
            {
                return;
            }
            Show_Forum(forum_to);
        }

        public void Show_Forum(int the_thread_id)
        {
            MultiView7.ActiveViewIndex = 1;
            //init_messages();
            //MultiView4.ActiveViewIndex = 2;
            MultiView1.ActiveViewIndex = 8;
            init_thread(the_thread_id);
            reply1.Focus();


        }

        public void Show_MySelf_Profile()
        {
            profile_click(this, null);
            return;
        }

        public void Show_Profile(string the_id)
        {
            string the_aid = the_id;
            wrong4.Visible = false;
            if (Is_Self_Aid(the_aid))
            {
                profile_click(this, null);
                return;
            }
            init_profile(the_aid);
            MultiView1.ActiveViewIndex = 10;
            MultiView4.ActiveViewIndex = 0;

        }



        protected void news_button_click1(object sender, EventArgs e)
        {
            if (feedbutton1.Text.Equals("Read Thread"))      // thread
            {
                int the_thread_id = Convert.ToInt32(HiddenFieldFeedId1.Value);
                Show_Forum(the_thread_id);
            }

            else if (feedbutton1.Text.Equals("Check Deal"))      // offer
            {
                int the_offer_id = Convert.ToInt32(HiddenFieldFeedId1.Value);
                Show_Deal(the_offer_id);
                //init_offer(the_offer_id);

            }

            else if (feedbutton1.Text.Equals("Read Blog"))      // thread
            {
                int the_thread_id = Convert.ToInt32(HiddenFieldFeedId1.Value);
                Show_Blog(the_thread_id);
            }
        }

        protected void news_button_click2(object sender, EventArgs e)
        {
            if (feedbutton2.Text.Equals("Read Thread"))      // thread
            {
                int the_thread_id = Convert.ToInt32(HiddenFieldFeedId2.Value);
                Show_Forum(the_thread_id);
            }

            else if (feedbutton2.Text.Equals("Check Deal"))      // offer
            {
                int the_offer_id = Convert.ToInt32(HiddenFieldFeedId2.Value);
                Show_Deal(the_offer_id);
                //init_offer(the_offer_id);

            }

            else if (feedbutton2.Text.Equals("Read Blog"))      // thread
            {
                int the_thread_id = Convert.ToInt32(HiddenFieldFeedId2.Value);
                Show_Blog(the_thread_id);
            }
        }

        protected void news_button_click3(object sender, EventArgs e)
        {
            if (feedbutton3.Text.Equals("Read Thread"))      // thread
            {
                int the_thread_id = Convert.ToInt32(HiddenFieldFeedId3.Value);
                Show_Forum(the_thread_id);
            }

            else if (feedbutton3.Text.Equals("Check Deal"))      // offer
            {
                int the_offer_id = Convert.ToInt32(HiddenFieldFeedId3.Value);
                Show_Deal(the_offer_id);
                //init_offer(the_offer_id);

            }

            else if (feedbutton3.Text.Equals("Read Blog"))      // thread
            {
                int the_thread_id = Convert.ToInt32(HiddenFieldFeedId3.Value);
                Show_Blog(the_thread_id);
            }
        }

        protected void news_button_click4(object sender, EventArgs e)
        {
            if (feedbutton4.Text.Equals("Read Thread"))      // thread
            {
                int the_thread_id = Convert.ToInt32(HiddenFieldFeedId4.Value);
                Show_Forum(the_thread_id);
            }

            else if (feedbutton4.Text.Equals("Check Deal"))      // offer
            {
                int the_offer_id = Convert.ToInt32(HiddenFieldFeedId4.Value);
                Show_Deal(the_offer_id);
                //init_offer(the_offer_id);

            }

            else if (feedbutton4.Text.Equals("Read Blog"))      // thread
            {
                int the_thread_id = Convert.ToInt32(HiddenFieldFeedId4.Value);
                Show_Blog(the_thread_id);
            }
        }

        protected void news_button_click5(object sender, EventArgs e)
        {
            if (feedbutton5.Text.Equals("Read Thread"))      // thread
            {
                int the_thread_id = Convert.ToInt32(HiddenFieldFeedId5.Value);
                Show_Forum(the_thread_id);
            }

            else if (feedbutton5.Text.Equals("Check Deal"))      // offer
            {
                int the_offer_id = Convert.ToInt32(HiddenFieldFeedId5.Value);
                Show_Deal(the_offer_id);
                //init_offer(the_offer_id);

            }

            else if (feedbutton5.Text.Equals("Read Blog"))      // thread
            {
                int the_thread_id = Convert.ToInt32(HiddenFieldFeedId5.Value);
                Show_Blog(the_thread_id);
            }
        }

        protected void news_button_click6(object sender, EventArgs e)
        {
            if (feedbutton6.Text.Equals("Read Thread"))      // thread
            {
                int the_thread_id = Convert.ToInt32(HiddenFieldFeedId6.Value);
                Show_Forum(the_thread_id);
            }

            else if (feedbutton6.Text.Equals("Check Deal"))      // offer
            {
                int the_offer_id = Convert.ToInt32(HiddenFieldFeedId6.Value);
                Show_Deal(the_offer_id);
                //init_offer(the_offer_id);

            }

            else if (feedbutton6.Text.Equals("Read Blog"))      // thread
            {
                int the_thread_id = Convert.ToInt32(HiddenFieldFeedId6.Value);
                Show_Blog(the_thread_id);
            }
        }

        protected void news_button_click7(object sender, EventArgs e)
        {
            if (feedbutton7.Text.Equals("Read Thread"))      // thread
            {
                int the_thread_id = Convert.ToInt32(HiddenFieldFeedId7.Value);
                Show_Forum(the_thread_id);
            }

            else if (feedbutton7.Text.Equals("Check Deal"))      // offer
            {
                int the_offer_id = Convert.ToInt32(HiddenFieldFeedId7.Value);
                Show_Deal(the_offer_id);
                //init_offer(the_offer_id);

            }

            else if (feedbutton7.Text.Equals("Read Blog"))      // thread
            {
                int the_thread_id = Convert.ToInt32(HiddenFieldFeedId7.Value);
                Show_Blog(the_thread_id);
            }
        }

        protected void cconnect2(object sender, EventArgs e)
        {
            string aid = HiddenFielda2.Value;
            int the_aid = Convert.ToInt32(aid);
            cconnect(the_aid);
            int status = get_status(the_aid);
            connecta1.Text = connectButtonString(status);
            init_page();
            connecta2.Focus();
        }

        protected void cconnect3(object sender, EventArgs e)
        {
            string aid = HiddenFielda3.Value;
            int the_aid = Convert.ToInt32(aid);
            cconnect(the_aid);
            int status = get_status(the_aid);
            connecta1.Text = connectButtonString(status);
            init_page();
            connecta3.Focus();
        }

        protected void cconnect4(object sender, EventArgs e)
        {
            string aid = HiddenFielda4.Value;
            int the_aid = Convert.ToInt32(aid);
            cconnect(the_aid);
            int status = get_status(the_aid);
            connecta1.Text = connectButtonString(status);
            init_page();
            connecta4.Focus();
        }

        protected void cconnect5(object sender, EventArgs e)
        {
            string aid = HiddenFielda5.Value;
            int the_aid = Convert.ToInt32(aid);
            cconnect(the_aid);
            int status = get_status(the_aid);
            connecta1.Text = connectButtonString(status);
            init_page();
            connecta5.Focus();
        }

        protected void cconnect7(object sender, EventArgs e)
        {
            string aid = HiddenFielda7.Value;
            int the_aid = Convert.ToInt32(aid);
            cconnect(the_aid);
            int status = get_status(the_aid);
            connecta1.Text = connectButtonString(status);
            init_page();
            connecta7.Focus();
        }

        protected void cconnect6(object sender, EventArgs e)
        {
            string aid = HiddenFielda6.Value;
            int the_aid = Convert.ToInt32(aid);
            cconnect(the_aid);
            int status = get_status(the_aid);
            connecta1.Text = connectButtonString(status);
            init_page();
            connecta6.Focus();
        }

        protected void conclick1(object sender, ImageClickEventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenField1d1.Value;
            init_profile(the_aid);
        }

        protected void conclick1(object sender, EventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenField1d1.Value;
            init_profile(the_aid);
        }

        protected void conclick2(object sender, ImageClickEventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenField1d2.Value;
            init_profile(the_aid);
        }

        protected void conclick2(object sender, EventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenField1d2.Value;
            init_profile(the_aid);
        }

        protected void conclick3(object sender, ImageClickEventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenField1d3.Value;
            init_profile(the_aid);
        }

        protected void conclick3(object sender, EventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenField1d3.Value;
            init_profile(the_aid);
        }

        protected void conclick4(object sender, ImageClickEventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenField1d4.Value;
            init_profile(the_aid);
        }

        protected void conclick4(object sender, EventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenField1d4.Value;
            init_profile(the_aid);
        }

        protected void conclick5(object sender, ImageClickEventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenField1d5.Value;
            init_profile(the_aid);
        }

        protected void conclick5(object sender, EventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenField1d5.Value;
            init_profile(the_aid);
        }

        protected void conclick6(object sender, ImageClickEventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenField1d6.Value;
            init_profile(the_aid);
        }

        protected void conclick6(object sender, EventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenField1d6.Value;
            init_profile(the_aid);
        }

        protected void conclick7(object sender, ImageClickEventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenField1d7.Value;
            init_profile(the_aid);
        }

        protected void conclick7(object sender, EventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = HiddenField1d7.Value;
            init_profile(the_aid);
        }

        protected void acceptd1(object sender, EventArgs e)
        {
            if (!(accept1.Text.Equals("Accept")))
            {
                // not pending
                return;
            }

            int my_aid = get_aid(Username1.Value);
            int his_aid = Convert.ToInt32(HiddenField1d1.Value);

            accept(my_aid, his_aid);

            if (accept2.Visible)
            {
                accept2.Focus();
            }
            else
            {
                accept1.Focus();
            }
        }

        protected void acceptd2(object sender, EventArgs e)
        {
            if (!(accept2.Text.Equals("Accept")))
            {
                // not pending
                return;
            }

            int my_aid = get_aid(Username1.Value);
            int his_aid = Convert.ToInt32(HiddenField1d2.Value);

            accept(my_aid, his_aid);
            accept3.Focus();
        }

        protected void acceptd3(object sender, EventArgs e)
        {
            if (!(accept3.Text.Equals("Accept")))
            {
                // not pending
                return;
            }

            int my_aid = get_aid(Username1.Value);
            int his_aid = Convert.ToInt32(HiddenField1d3.Value);

            accept(my_aid, his_aid);
            accept4.Focus();
        }

        protected void acceptd4(object sender, EventArgs e)
        {
            if (!(accept4.Text.Equals("Accept")))
            {
                // not pending
                return;
            }

            int my_aid = get_aid(Username1.Value);
            int his_aid = Convert.ToInt32(HiddenField1d4.Value);

            accept(my_aid, his_aid);
            accept5.Focus();
        }

        protected void acceptd5(object sender, EventArgs e)
        {
            if (!(accept5.Text.Equals("Accept")))
            {
                // not pending
                return;
            }

            int my_aid = get_aid(Username1.Value);
            int his_aid = Convert.ToInt32(HiddenField1d5.Value);

            accept(my_aid, his_aid);
            accept6.Focus();
        }

        protected void acceptd6(object sender, EventArgs e)
        {
            if (!(accept6.Text.Equals("Accept")))
            {
                // not pending
                return;
            }

            int my_aid = get_aid(Username1.Value);
            int his_aid = Convert.ToInt32(HiddenField1d6.Value);

            accept(my_aid, his_aid);
            accept7.Focus();
        }

        protected void acceptd7(object sender, EventArgs e)
        {
            if (!(accept7.Text.Equals("Accept")))
            {
                // not pending
                return;
            }

            int my_aid = get_aid(Username1.Value);
            int his_aid = Convert.ToInt32(HiddenField1d7.Value);

            accept(my_aid, his_aid);
            accept7.Focus();
        }

        public void confocus()
        {
            if (accept2.Visible)
            {
                accept2.Focus();
            }
            else

            if (accept1.Visible)
            {
                accept1.Focus();
            }
            else
            {
                DropDownMyCon.Focus();
            }
        }

        public void remove(int my_aid, int his_aid)
        {
            MyTouristBook.ConnectionsDataContext db = new MyTouristBook.ConnectionsDataContext();

            //affsbook.ConnectionsDataContext db2 = new affsbook.ConnectionsDataContext();

            MyTouristBook.Table_MyTouristbook_Connection tab = new MyTouristBook.Table_MyTouristbook_Connection();
            //affsbook.Table_Affsbook_Connection tab2 = new affsbook.Table_Affsbook_Connection();

            tab = (from t in db.Table_MyTouristbook_Connections
                   where (t.aid == my_aid)
                   select t).FirstOrDefault();


            string waiting1 = remove_aid_from_list(tab.waitingconnections, his_aid.ToString());

            tab.waitingconnections = waiting1;

            db.SubmitChanges();

            init_con_page();


        }

        public void remove_con(int my_aid, int his_aid)
        {

            MyTouristBook.ConnectionsDataContext db = new MyTouristBook.ConnectionsDataContext();
            MyTouristBook.ConnectionsDataContext db2 = new MyTouristBook.ConnectionsDataContext();

            MyTouristBook.Table_MyTouristbook_Connection tab = new MyTouristBook.Table_MyTouristbook_Connection(); MyTouristBook.Table_MyTouristbook_Connection tab2 = new MyTouristBook.Table_MyTouristbook_Connection();


            tab = (from t in db.Table_MyTouristbook_Connections
                   where (t.aid == my_aid)
                   select t).FirstOrDefault();

            tab2 = (from t in db.Table_MyTouristbook_Connections
                    where (t.aid == his_aid)
                    select t).FirstOrDefault();

            string waiting1 = remove_aid_from_list(tab.waitingconnections, his_aid.ToString());
            string con1 = remove_aid_from_list(tab.connections, his_aid.ToString());
            string con2 = remove_aid_from_list(tab2.connections, my_aid.ToString());

            tab.connections = con1;
            tab2.connections = con2;


            db.SubmitChanges();
            db2.SubmitChanges();

            init_con_page();
        }


        protected void removed1(object sender, ImageClickEventArgs e)
        {
            if (accept1.Text.Equals("Connected"))
            {
                int my_aid2 = get_aid(Username1.Value);
                int his_aid2 = Convert.ToInt32(HiddenField1d1.Value);
                remove_con(my_aid2, his_aid2);
                confocus();
                return;
            }


            int my_aid = get_aid(Username1.Value);
            int his_aid = Convert.ToInt32(HiddenField1d1.Value);
            remove(my_aid, his_aid);
            confocus();
        }

        protected void removed2(object sender, ImageClickEventArgs e)
        {
            if (accept2.Text.Equals("Connected"))
            {
                int my_aid2 = get_aid(Username1.Value);
                int his_aid2 = Convert.ToInt32(HiddenField1d2.Value);
                remove_con(my_aid2, his_aid2);
                confocus();
                return;
            }


            int my_aid = get_aid(Username1.Value);
            int his_aid = Convert.ToInt32(HiddenField1d2.Value);
            remove(my_aid, his_aid);

            confocus();
        }

        protected void removed3(object sender, ImageClickEventArgs e)
        {
            if (accept3.Text.Equals("Connected"))
            {
                int my_aid2 = get_aid(Username1.Value);
                int his_aid2 = Convert.ToInt32(HiddenField1d3.Value);
                remove_con(my_aid2, his_aid2);
                confocus();
                return;
            }


            int my_aid = get_aid(Username1.Value);
            int his_aid = Convert.ToInt32(HiddenField1d3.Value);
            remove(my_aid, his_aid);

            confocus();
        }

        protected void removed4(object sender, ImageClickEventArgs e)
        {
            if (accept2.Text.Equals("Connected"))
            {
                int my_aid2 = get_aid(Username1.Value);
                int his_aid2 = Convert.ToInt32(HiddenField1d4.Value);
                remove_con(my_aid2, his_aid2);
                confocus();
                return;
            }


            int my_aid = get_aid(Username1.Value);
            int his_aid = Convert.ToInt32(HiddenField1d4.Value);
            remove(my_aid, his_aid);

            confocus();
        }

        protected void removed5(object sender, ImageClickEventArgs e)
        {
            if (accept2.Text.Equals("Connected"))
            {
                int my_aid2 = get_aid(Username1.Value);
                int his_aid2 = Convert.ToInt32(HiddenField1d5.Value);
                remove_con(my_aid2, his_aid2);
                confocus();
                return;
            }


            int my_aid = get_aid(Username1.Value);
            int his_aid = Convert.ToInt32(HiddenField1d5.Value);
            remove(my_aid, his_aid);

            confocus();
        }

        protected void removed6(object sender, ImageClickEventArgs e)
        {
            if (accept2.Text.Equals("Connected"))
            {
                int my_aid2 = get_aid(Username1.Value);
                int his_aid2 = Convert.ToInt32(HiddenField1d6.Value);
                remove_con(my_aid2, his_aid2);
                confocus();
                return;
            }


            int my_aid = get_aid(Username1.Value);
            int his_aid = Convert.ToInt32(HiddenField1d6.Value);
            remove(my_aid, his_aid);

            confocus();
        }

        protected void removed7(object sender, ImageClickEventArgs e)
        {
            if (accept2.Text.Equals("Connected"))
            {
                int my_aid2 = get_aid(Username1.Value);
                int his_aid2 = Convert.ToInt32(HiddenField1d7.Value);
                remove_con(my_aid2, his_aid2);
                confocus();
                return;
            }


            int my_aid = get_aid(Username1.Value);
            int his_aid = Convert.ToInt32(HiddenField1d7.Value);
            remove(my_aid, his_aid);

            confocus();
        }

        protected void previousd1_click(object sender, EventArgs e)
        {

        }

        protected void nextd1_Click(object sender, EventArgs e)
        {

        }

        protected void page_con_changed_click(object sender, EventArgs e)
        {

        }

        protected void text_conne_click(object sender, EventArgs e)
        {
            init_connections();
            accept1.Focus();
        }

        protected void smessagecon1(object sender, EventArgs e)
        {
            string to = HiddenField1d1.Value;
            SendMessage(to);
        }

        protected void smessagecon2(object sender, EventArgs e)
        {
            string to = HiddenField1d2.Value;
            SendMessage(to);
        }

        protected void smessagecon3(object sender, EventArgs e)
        {
            string to = HiddenField1d3.Value;
            SendMessage(to);
        }

        protected void smessagecon4(object sender, EventArgs e)
        {
            string to = HiddenField1d4.Value;
            SendMessage(to);
        }

        protected void smessagecon5(object sender, EventArgs e)
        {
            string to = HiddenField1d5.Value;
            SendMessage(to);
        }

        protected void smessagecon6(object sender, EventArgs e)
        {
            string to = HiddenField1d6.Value;
            SendMessage(to);
        }

        protected void smessagecon7(object sender, EventArgs e)
        {
            string to = HiddenField1d7.Value;
            SendMessage(to);
        }

        protected void Change_Country_Click(object sender, EventArgs e)
        {
            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.dealsDataContext db2 = new MyTouristBook.dealsDataContext();

            MyTouristBook.CountriesDataContext db4 = new MyTouristBook.CountriesDataContext();
            //MyTouristBook.Table_City tab2 = new MyTouristBook.Table_City();

            string country = DropDownCountriesDeals.SelectedItem.Text;

            DropDownCitiesDeals.Items.Clear();
            ListItem item3 = new ListItem("Select City");
            //ListItem item3 = new ListItem("Select City", "0");
            DropDownCitiesDeals.Items.Add(item3);

            if (country.Equals("Select Country"))
                return;

            var cities = (from t in db4.Table_Cities
                          where t.country.Equals(country)
                          select t.city);

            foreach (var city in cities)
            {
                ListItem item2 = new ListItem(city);
                DropDownCitiesDeals.Items.Add(item2);
            }

            info1.Focus();


        }

        protected void Change_Deal_City_Click(object sender, EventArgs e)
        {
            init_deals();
            info1.Focus();
        }

        protected void Forum_Country_Changed_Click(object sender, EventArgs e)
        {
            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.dealsDataContext db2 = new MyTouristBook.dealsDataContext();

            MyTouristBook.CountriesDataContext db4 = new MyTouristBook.CountriesDataContext();
            //MyTouristBook.Table_City tab2 = new MyTouristBook.Table_City();

            string country = DropDownCountriesForum.SelectedItem.Text;

            DropDownCitiesForum.Items.Clear();
            ListItem item3 = new ListItem("Select City");
            DropDownCitiesForum.Items.Add(item3);

            if (country.Equals("Select Country"))
                return;

            var cities = (from t in db4.Table_Cities
                          where t.country.Equals(country)
                          select t.city);

            foreach (var city in cities)
            {
                ListItem item2 = new ListItem(city);
                DropDownCitiesForum.Items.Add(item2);
            }

            readthread1.Focus();
        }

        protected void Forum_City_Changed_Click(object sender, EventArgs e)
        {
            init_forums();
            readthread1.Focus();
        }

        protected void Change_Blog_Country_Click(object sender, EventArgs e)
        {
            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.blogsDataContext db2 = new MyTouristBook.blogsDataContext();

            MyTouristBook.CountriesDataContext db4 = new MyTouristBook.CountriesDataContext();
            //MyTouristBook.Table_City tab2 = new MyTouristBook.Table_City();

            string country = DropDownCountriesBlog.SelectedItem.Text;

            DropDownCitiesBlog.Items.Clear();
            ListItem item3 = new ListItem("Select City");
            DropDownCitiesBlog.Items.Add(item3);

            if (country.Equals("Select Country"))
                return;

            var cities = (from t in db4.Table_Cities
                          where t.country.Equals(country)
                          select t.city);

            foreach (var city in cities)
            {
                ListItem item2 = new ListItem(city);
                DropDownCitiesBlog.Items.Add(item2);
            }

            readblog1.Focus();
        }

        protected void Blog_City_Changed_Click(object sender, EventArgs e)
        {
            init_blogs();
            readblog1.Focus();

        }

        protected void Feed_Country_Changed_Click(object sender, EventArgs e)
        {
            /*
            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.blogsDataContext db2 = new MyTouristBook.blogsDataContext();

            MyTouristBook.CountriesDataContext db4 = new MyTouristBook.CountriesDataContext();
            //MyTouristBook.Table_City tab2 = new MyTouristBook.Table_City();

            string country = DropDownFeedCountry.SelectedItem.Text;

            DropDownFeedCity2.Items.Clear();
            ListItem item3 = new ListItem("Select City");
            DropDownFeedCity2.Items.Add(item3);

            if (country.Equals("Select Country"))
                return;

            var cities = (from t in db4.Table_Cities
                          where t.country.Equals(country)
                          select t.city);

            foreach (var city in cities)
            {
                ListItem item2 = new ListItem(city);
                DropDownFeedCity2.Items.Add(item2);
            }

            feedbutton2.Focus();
            */
        }

        protected void Feed_City_Changed_Click(object sender, EventArgs e)
        {
            init_newsfeed();
            feedbutton2.Focus();
        }

        protected void Create_Deal_Country_Changed_Click(object sender, EventArgs e)
        {

            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.blogsDataContext db2 = new MyTouristBook.blogsDataContext();

            MyTouristBook.CountriesDataContext db4 = new MyTouristBook.CountriesDataContext();
            //MyTouristBook.Table_City tab2 = new MyTouristBook.Table_City();

            string country = DropDownDeal2Country.SelectedItem.Text;

            DropDownDeal2City.Items.Clear();
            ListItem item3 = new ListItem("Select City", "0");
            DropDownDeal2City.Items.Add(item3);

            if (country.Equals("Select Country"))
                return;

            var cities = (from t in db4.Table_Cities
                          where t.country.Equals(country)
                          orderby t.city
                          select t.city);

            foreach (var city in cities)
            {
                ListItem item2 = new ListItem(city);
                DropDownDeal2City.Items.Add(item2);
            }



            string country2 = DropDownDeal2Country.SelectedItem.Text;

            var flag_image = (from t in db4.Table_Country_Flags
                              where t.country.Equals(country2)
                              select t.flag_image).FirstOrDefault();

            //if (flag_image != null)
            //  the_deal_image2.ImageUrl = flag_image;

            DropDownDeal2City.Focus();
            //mydealshortdesc
            //Button146.Focus();

        }

        protected void Forum_Country_Create_Changed_Click(object sender, EventArgs e)
        {
            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.blogsDataContext db2 = new MyTouristBook.blogsDataContext();

            MyTouristBook.CountriesDataContext db4 = new MyTouristBook.CountriesDataContext();
            //MyTouristBook.Table_City tab2 = new MyTouristBook.Table_City();

            string country = DropDownForum2Country.SelectedItem.Text;

            DropDownForum2City.Items.Clear();
            ListItem item3 = new ListItem("Select City");
            DropDownForum2City.Items.Add(item3);

            if (country.Equals("Select Country"))
                return;

            var cities = (from t in db4.Table_Cities
                          where t.country.Equals(country)
                          select t.city);

            foreach (var city in cities)
            {
                ListItem item2 = new ListItem(city);
                DropDownForum2City.Items.Add(item2);
            }

            connect19.Focus();
        }

        protected void Blog_Country_Create_Changed_Click(object sender, EventArgs e)
        {
            MyTouristBook.TouristsDataContext db = new MyTouristBook.TouristsDataContext();
            MyTouristBook.blogsDataContext db2 = new MyTouristBook.blogsDataContext();

            MyTouristBook.CountriesDataContext db4 = new MyTouristBook.CountriesDataContext();
            //MyTouristBook.Table_City tab2 = new MyTouristBook.Table_City();

            string country = DropDownBlog2Country.SelectedItem.Text;

            DropDownBlog2City.Items.Clear();
            ListItem item3 = new ListItem("Select City", "0");
            DropDownBlog2City.Items.Add(item3);

            if (country.Equals("Select Country"))
                return;

            var cities = (from t in db4.Table_Cities
                          where t.country.Equals(country)
                          select t.city);

            foreach (var city in cities)
            {
                ListItem item2 = new ListItem(city);
                DropDownBlog2City.Items.Add(item2);
            }

            the_blog_body2.Focus();
        }

        protected void smessage2(object sender, EventArgs e)
        {
            string to = HiddenFielda2.Value;
            SendMessage(to);
        }

        protected void smessage3(object sender, EventArgs e)
        {
            string to = HiddenFielda3.Value;
            SendMessage(to);
        }

        protected void smessage4(object sender, EventArgs e)
        {
            string to = HiddenFielda4.Value;
            SendMessage(to);
        }

        protected void smessage5(object sender, EventArgs e)
        {
            string to = HiddenFielda5.Value;
            SendMessage(to);
        }

        protected void smessage6(object sender, EventArgs e)
        {
            string to = HiddenFielda6.Value;
            SendMessage(to);
        }

        protected void smessage7(object sender, EventArgs e)
        {
            string to = HiddenFielda7.Value;
            SendMessage(to);
        }

        protected void fromb2click(object sender, ImageClickEventArgs e)
        {
            wrong4.Visible = false;
            string the_aid = get_aid(HiddenFieldDest2.Value).ToString();
            init_profile(the_aid);
        }

        protected void frmautclick1(object sender, ImageClickEventArgs e)
        {
            string the_aid = HiddenFieldForumAuthorId1.Value;
            Show_Profile(the_aid);
        }

        protected void frmautclick2(object sender, ImageClickEventArgs e)
        {
            string the_aid = HiddenFieldForumAuthorId2.Value;
            Show_Profile(the_aid);
        }

        protected void frmautclick3(object sender, ImageClickEventArgs e)
        {
            string the_aid = HiddenFieldForumAuthorId3.Value;
            Show_Profile(the_aid);
        }

        protected void frmautclick4(object sender, ImageClickEventArgs e)
        {
            string the_aid = HiddenFieldForumAuthorId4.Value;
            Show_Profile(the_aid);
        }

        protected void frmautclick5(object sender, ImageClickEventArgs e)
        {

            string the_aid = HiddenFieldForumAuthorId5.Value;
            Show_Profile(the_aid);
        }

        protected void frmautclick6(object sender, ImageClickEventArgs e)
        {
            string the_aid = HiddenFieldForumAuthorId6.Value;
            Show_Profile(the_aid);
        }

        protected void frmautclick7(object sender, ImageClickEventArgs e)
        {
            string the_aid = HiddenFieldForumAuthorId1.Value;
            Show_Profile(the_aid);
        }

        protected void fblogin_click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/start.aspx");
        }

        public void init_forum_loc()
        {

            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();

            string country = DropDownCountriesForum.SelectedItem.Text;
            string city = DropDownCitiesForum.SelectedItem.Text;

            if (country.Equals("Select Country"))
            {
                Label412.Text = "Welcome to Toury Deals forums, where you can find discussions, information and ideas from tourists! Use this information wisely and you can find great deals!";                
                location_image_forum.ImageUrl = "https://www.tourist-ads.com/maintravel.jpg";
                //init_related();
                return;
            }


            var the_country = (from t in db.Table_MyTouristbook_Countries
                               where (t.country.Equals(country))
                               select t).FirstOrDefault();

            if (the_country == null)
            {
                wrong19.Visible = true;
                return;
            }


            if (city.Equals("Select City"))
            {
                string gray_circle = the_country.graylogo;

                Label412.Text = "Welcome to Toury Deals CAT forums, where you can find discussions, information and ideas from CAT tourists! Use this information wisely and you can find great deals!";                
                Label412.Text = Label412.Text.Replace("CAT", country);

                string gray_circle5 = the_country.graylogo;
                location_image_forum.ImageUrl = gray_circle5;
                return;
            }



            var the_city = (from t in db.Table_MyTouristbook_Cities
                            where (t.city.Equals(city))
                            select t).FirstOrDefault();

            if (the_city == null)
            {
                wrong10.Visible = true;
                return;
            }

            string dest = city + ", " + country;

            Label412.Text = "Welcome to Toury Deals CAT forum, where you can find discussions, information and trip ideas from CIT tourists! Use this information wisely and you can find great deals!";            
            Label412.Text = Label412.Text.Replace("CAT", dest);
            Label412.Text = Label412.Text.Replace("CIT", city);

            string gray_circle6 = the_city.graylogo;
            location_image_forum.ImageUrl = gray_circle6;
            //DropDownVertical.Focus();

            //init_related();

        }

        public void init_forum_location()
        {
            wrong10.Visible = false;
            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();

            if (DropDownCountriesForum.Items.Count == 0)
            {
                ListItem item = new ListItem("Select Country", "0");
                DropDownCountriesForum.Items.Add(item);


                var the_ver = (from t in db.Table_MyTouristbook_Countries
                               orderby t.country ascending
                               select t);

                if (the_ver == null)
                {
                    wrong10.Visible = true;
                    return;
                }

                foreach (var cat in the_ver)
                {
                    ListItem item2 = new ListItem(cat.country, cat.id.ToString());
                    DropDownCountriesForum.Items.Add(item2);

                }

            }

            if (DropDownCitiesForum.Items.Count == 0)
            {
                ListItem item = new ListItem("Select City", "0");
                DropDownCitiesForum.Items.Add(item);
            }

            string country = DropDownCountriesForum.SelectedItem.Text;

            if (!(country.Equals("Select Country")))
            {

                DropDownCitiesForum.Items.Clear();
                ListItem item = new ListItem("Select City", "0");
                DropDownCitiesForum.Items.Add(item);


                var the_cities = (from t in db.Table_MyTouristbook_Cities
                                  where t.country.Equals(country)
                                  orderby t.city ascending
                                  select t);

                if (the_cities != null)
                {


                    foreach (var cit in the_cities)
                    {
                        ListItem item2 = new ListItem(cit.city, cit.id.ToString());
                        DropDownCitiesForum.Items.Add(item2);
                    }

                }

            }

            init_forum_loc();
        }

        public void init_blog_loc()
        {
            wrong21.Visible = false;

            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();

            string country = DropDownCountriesBlog.SelectedItem.Text;
            string city = DropDownCitiesBlog.SelectedItem.Text;

            if (country.Equals("Select Country"))
            {
                Label413.Text = "Welcome to Toury Deals blogs, where you can find blogs, information, tips and ideas for tourists! Use this information wisely and you can find great deals!";               
                
                location_image_blog.ImageUrl = "https://www.tourist-ads.com/maintravel.jpg";
                //init_related();
                return;
            }


            var the_country = (from t in db.Table_MyTouristbook_Countries
                               where (t.country.Equals(country))
                               select t).FirstOrDefault();

            if (the_country == null)
            {
                wrong21.Visible = true;
                return;
            }


            if (city.Equals("Select City"))
            {
                string gray_circle = the_country.graylogo;

                Label413.Text = "Welcome to Toury Deals CAT blogs, where you can find blogs, information, tips and ideas from CAT tourists! Use this information wisely and you can find great deals!";               
                Label413.Text = Label413.Text.Replace("CAT", country);

                string gray_circle5 = the_country.graylogo;
                location_image_blog.ImageUrl = gray_circle5;
                return;
            }



            var the_city = (from t in db.Table_MyTouristbook_Cities
                            where (t.city.Equals(city))
                            select t).FirstOrDefault();

            if (the_city == null)
            {
                wrong21.Visible = true;
                return;
            }

            string dest = city + ", " + country;

            Label413.Text = "Welcome to Toury Deals CAT blogs, where you can find blogs, information, tips and ideas from CIT tourists! Use this information wisely and you can find great deals!";            
            Label413.Text = Label413.Text.Replace("CAT", dest);
            Label413.Text = Label413.Text.Replace("CIT", city);

            string gray_circle6 = the_city.graylogo;
            location_image_blog.ImageUrl = gray_circle6;
            //DropDownVertical.Focus();

            //init_related();

        }

        public void init_blog_location()
        {
            wrong21.Visible = false;
            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();

            if (DropDownCountriesBlog.Items.Count == 0)
            {
                ListItem item = new ListItem("Select Country", "0");
                DropDownCountriesBlog.Items.Add(item);


                var the_ver = (from t in db.Table_MyTouristbook_Countries
                               orderby t.country ascending
                               select t);

                if (the_ver == null)
                {
                    wrong21.Visible = true;
                    return;
                }

                foreach (var cat in the_ver)
                {
                    ListItem item2 = new ListItem(cat.country, cat.id.ToString());
                    DropDownCountriesBlog.Items.Add(item2);

                }

            }

            if (DropDownCitiesBlog.Items.Count == 0)
            {
                ListItem item = new ListItem("Select City", "0");
                DropDownCitiesBlog.Items.Add(item);
            }

            string country = DropDownCountriesBlog.SelectedItem.Text;

            if (!(country.Equals("Select Country")))
            {

                DropDownCitiesBlog.Items.Clear();
                ListItem item = new ListItem("Select City", "0");
                DropDownCitiesBlog.Items.Add(item);


                var the_cities = (from t in db.Table_MyTouristbook_Cities
                                  where t.country.Equals(country)
                                  orderby t.city ascending
                                  select t);

                if (the_cities != null)
                {


                    foreach (var cit in the_cities)
                    {
                        ListItem item2 = new ListItem(cit.city, cit.id.ToString());
                        DropDownCitiesBlog.Items.Add(item2);
                    }

                }

            }

            init_blog_loc();
        }

        public void init_deal_loc()
        {
            wrong16.Visible = false;

            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();

            string country = DropDownCountriesDeals.SelectedItem.Text;
            string city = DropDownCitiesDeals.SelectedItem.Text;

            if (country.Equals("Select Country"))
            {                
                Label414.Text = "Welcome to Toury Deals - where you can find great travel deals, attractive offers in special prices! Do not miss these opportunties and you can enjoy great travel deals!";
                location_image_deal2.ImageUrl = "https://www.tourist-ads.com/maintravel.jpg";
                //init_related();
                return;
            }


            var the_country = (from t in db.Table_MyTouristbook_Countries
                               where (t.country.Equals(country))
                               select t).FirstOrDefault();

            if (the_country == null)
            {
                wrong16.Visible = true;
                return;
            }


            if (city.Equals("Select City"))
            {
                string gray_circle = the_country.graylogo;

                Label414.Text = "Welcome to Toury Deals - CAT deals, where you can find great CAT travel deals, attractive offers in special prices! Do not miss these opportunties and you can enjoy great CAT travel deals!";                
                Label414.Text = Label414.Text.Replace("CAT", country);

                string gray_circle5 = the_country.graylogo;
                location_image_deal2.ImageUrl = gray_circle5;
                return;
            }



            var the_city = (from t in db.Table_MyTouristbook_Cities
                            where (t.city.Equals(city))
                            select t).FirstOrDefault();

            if (the_city == null)
            {
                wrong16.Visible = true;
                return;
            }

            string dest = city + ", " + country;

            Label414.Text = "Welcome to Toury Deals - CAT deals, where you can find great CIT travel deals, attractive offers in special prices! Do not miss these opportunties and you can enjoy great CIT travel deals!";            
            Label414.Text = Label414.Text.Replace("CAT", dest);
            Label414.Text = Label414.Text.Replace("CIT", city);

            string gray_circle6 = the_city.graylogo;
            location_image_deal2.ImageUrl = gray_circle6;
            //DropDownVertical.Focus();

            //init_related();

        }

        public void init_deal_location()
        {
            wrong16.Visible = false;
            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();

            if (DropDownCountriesDeals.Items.Count == 0)
            {
                ListItem item = new ListItem("Select Country", "0");
                DropDownCountriesDeals.Items.Add(item);


                var the_ver = (from t in db.Table_MyTouristbook_Countries
                               orderby t.country ascending
                               select t);

                if (the_ver == null)
                {
                    wrong16.Visible = true;
                    return;
                }

                foreach (var cat in the_ver)
                {
                    ListItem item2 = new ListItem(cat.country, cat.id.ToString());
                    DropDownCountriesDeals.Items.Add(item2);

                }

            }

            if (DropDownCitiesDeals.Items.Count == 0)
            {
                ListItem item = new ListItem("Select City", "0");
                DropDownCitiesDeals.Items.Add(item);
            }

            string country = DropDownCountriesDeals.SelectedItem.Text;

            if (!(country.Equals("Select Country")))
            {

                DropDownCitiesDeals.Items.Clear();
                ListItem item = new ListItem("Select City", "0");
                DropDownCitiesDeals.Items.Add(item);


                var the_cities = (from t in db.Table_MyTouristbook_Cities
                                  where t.country.Equals(country)
                                  orderby t.city ascending
                                  select t);

                if (the_cities != null)
                {


                    foreach (var cit in the_cities)
                    {
                        ListItem item2 = new ListItem(cit.city, cit.id.ToString());
                        DropDownCitiesDeals.Items.Add(item2);
                    }

                }

            }

            init_deal_loc();
        }


        public void init_loc()
        {

            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();

            string country = DropDownCountries.SelectedItem.Text;
            string city = DropDownCities.SelectedItem.Text;

            if (country.Equals("Select Country"))
            {                
                Label411.Text = "Welcome to Toury Deals, where you can find travel deals, travel forums, and travel blogs. Use this information wisely and you can find great travel deals!";
                location_image.ImageUrl = "https://www.tourist-ads.com/maintravel.jpg";
                //init_related();
                return;
            }


            var the_country = (from t in db.Table_MyTouristbook_Countries
                               where (t.country.Equals(country))
                               select t).FirstOrDefault();

            if (the_country == null)
            {
                wrong19.Visible = true;
                return;
            }


            if (city.Equals("Select City"))
            {
                string gray_circle = the_country.graylogo;

                Label411.Text = "Welcome to Toury Deals CAT, where you can find CAT travel deals, CAT travel forums, and CAT travel blogs. Use this information wisely and you can find great travel deals!";
                Label411.Text = Label411.Text.Replace("CAT", country);

                string gray_circle5 = the_country.graylogo;
                location_image.ImageUrl = gray_circle5;
                return;
            }



            var the_city = (from t in db.Table_MyTouristbook_Cities
                            where (t.city.Equals(city))
                            select t).FirstOrDefault();

            if (the_city == null)
            {
                wrong10.Visible = true;
                return;
            }

            string dest = city + ", " + country;

            Label411.Text = "Welcome to Toury Deals CAT, where you can find CIT travel deals, CIT travel forums, and CIT travel blogs. Use this information wisely and you can find great travel deals!";
            Label411.Text = Label411.Text.Replace("CAT", dest);
            Label411.Text = Label411.Text.Replace("CIT", city);

            string gray_circle6 = the_city.graylogo;
            location_image.ImageUrl = gray_circle6;
            //DropDownVertical.Focus();

            init_related();

        }

        public void init_location()
        {
            wrong19.Visible = false;
            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();

            if (DropDownCountries.Items.Count == 0)
            {
                ListItem item = new ListItem("Select Country", "0");
                DropDownCountries.Items.Add(item);


                var the_ver = (from t in db.Table_MyTouristbook_Countries
                               orderby t.country ascending
                               select t);

                if (the_ver == null)
                {
                    wrong19.Visible = true;
                    return;
                }

                foreach (var cat in the_ver)
                {
                    ListItem item2 = new ListItem(cat.country, cat.id.ToString());
                    DropDownCountries.Items.Add(item2);

                }

            }

            if (DropDownCities.Items.Count == 0)
            {
                ListItem item = new ListItem("Select City", "0");
                DropDownCities.Items.Add(item);
            }

            string country = DropDownCountries.SelectedItem.Text;

            if (!(country.Equals("Select Country")))
            {

                DropDownCities.Items.Clear();
                ListItem item = new ListItem("Select City", "0");
                DropDownCities.Items.Add(item);


                var the_cities = (from t in db.Table_MyTouristbook_Cities
                                  where t.country.Equals(country)
                                  orderby t.city ascending
                                  select t);

                if (the_cities != null)
                {


                    foreach (var cit in the_cities)
                    {
                        ListItem item2 = new ListItem(cit.city, cit.id.ToString());
                        DropDownCities.Items.Add(item2);
                    }

                }

            }

            init_loc();
        }

        protected void country_change(object sender, EventArgs e)
        {

            string country = DropDownCountries.SelectedItem.Text;

            DropDownCities.Items.Clear();

            init_location();

            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();


            var res = (from t in db.Table_MyTouristbook_Cities
                       where ((t.country.Equals(country)))
                       orderby t.rank descending
                       select t).FirstOrDefault();

            string val;
            bool sel = false;

            if (res == null)
            {
                val = "144";
            }

            else if (country.Equals("Select Country"))
            {
                val = "144";
                
            }

            else
            {
                val = res.id.ToString();
            }


            change_city(val);

            init_related();
            init_newsfeed();

            string country_flag = extract_flag(country);
            if (country_flag.Equals(""))
            {
                location_image.ImageUrl = "https://www.tourist-ads.com/maintravel.jpg";
            }

            else
            {
                location_image.ImageUrl = country_flag;
            }

            feedbutton1.Focus();

        }

        public string extract_flag(string country)
        {
            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();

            var res = (from t in db.Table_MyTouristbook_Countries
                       where ((t.country.Equals(country)))
                       select t).FirstOrDefault();

            if (res == null)
                return "";

            return res.graylogo;
        }

        protected void city_change(object sender, EventArgs e)
        {
            
            init_loc();            
            init_newsfeed();
            feedbutton1.Focus();

        }

        public void reset_related()
        {

            RelDest1.ImageUrl = "";
            destlink1.Text = "";
            HiddenDest1.Value = "0";
            RelDest1.BorderWidth = 0;
            RelDest1.Visible = false;


            RelDest2.ImageUrl = "";
            destlink2.Text = "";
            HiddenDest2.Value = "0";
            RelDest2.BorderWidth = 0;
            RelDest2.Visible = false;

            RelDest3.ImageUrl = "";
            destlink3.Text = "";
            HiddenDest3.Value = "0";
            RelDest3.BorderWidth = 0;
            RelDest3.Visible = false;

            RelDest4.ImageUrl = "";
            destlink4.Text = "";
            HiddenDest4.Value = "0";
            RelDest4.BorderWidth = 0;
            RelDest4.Visible = false;

            RelDest5.ImageUrl = "";
            destlink5.Text = "";
            HiddenDest5.Value = "0";
            RelDest5.BorderWidth = 0;
            RelDest5.Visible = false;


        }
        public void init_related()
        {

            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();

            string dest = DropDownCountries.SelectedItem.Text;

            if (dest.Equals("Select Country"))
            {
                //dest = "United States";                
            }

            var the_cities = (from t in db.Table_MyTouristbook_Cities
                              where ((t.country.Equals(dest)) || (dest.Equals("Select Country")))
                              orderby t.rank descending
                              select t).Take(5);


            /*var the_cities = (from t in db.Table_MyTouristbook_Cities
                              where t.country.Equals(dest)
                              orderby t.rank descending
                              select t).Take(5);
                              */

            int counter = 1;

            reset_related();

            foreach (var city in the_cities)
            {
                //affsbook.Table_Affsbook_Affiliate aff = get_affiliate((int)tab2.authoraid);

                if (counter == 1)
                {
                    RelDest3.ImageUrl = city.icon;
                    RelDest3.Width = 175;
                    RelDest3.Height = 150;
                    RelDest3.BorderWidth = 2;
                    destlink3.Text = city.city;
                    HiddenDest3.Value = city.id.ToString();
                    RelDest3.Visible = true;
                }

                if (counter == 2)
                {
                    RelDest2.ImageUrl = city.icon;
                    RelDest2.Width = 175;
                    RelDest2.Height = 150;
                    RelDest2.BorderWidth = 2;
                    destlink2.Text = city.city;
                    HiddenDest2.Value = city.id.ToString();
                    RelDest2.Visible = true;
                }

                if (counter == 3)
                {
                    RelDest4.ImageUrl = city.icon;
                    RelDest4.Width = 175;
                    RelDest4.Height = 150;
                    RelDest4.BorderWidth = 2;
                    destlink4.Text = city.city;
                    HiddenDest4.Value = city.id.ToString();
                    RelDest4.Visible = true;
                }

                if (counter == 4)
                {
                    RelDest1.ImageUrl = city.icon;
                    RelDest1.Width = 175;
                    RelDest1.Height = 150;
                    RelDest1.BorderWidth = 2;
                    destlink1.Text = city.city;
                    HiddenDest1.Value = city.id.ToString();
                    RelDest1.Visible = true;
                }

                if (counter == 5)
                {
                    RelDest5.ImageUrl = city.icon;
                    RelDest5.Width = 175;
                    RelDest5.Height = 150;
                    RelDest5.BorderWidth = 2;
                    destlink5.Text = city.city;
                    HiddenDest5.Value = city.id.ToString();
                    RelDest5.Visible = true;
                }

                counter++;
            }


        }

        public string find_country(string val)
        {
            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();

            var city = (from t in db.Table_MyTouristbook_Cities
                        where t.id.Equals(val)
                        select t).FirstOrDefault();

            if (city == null)
                return "";

            return city.country;


        }

        public void change_city(string val)
        {

            if (val.Equals("0"))
            {
                return;
            }

            string country = find_country(val);

            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();

            if (DropDownCountries.SelectedItem.Text.Equals("Select Country"))
            {
                foreach (ListItem li in DropDownCountries.Items)
                {
                    if (li.Text.Equals(country))
                    {
                        DropDownCountries.SelectedValue = li.Value;
                    }

                }

                var the_cities2 = (from t in db.Table_MyTouristbook_Cities
                                   where t.country.Equals(country)
                                   orderby t.city ascending
                                   select t);

                if (the_cities2 != null)
                {


                    foreach (var cit in the_cities2)
                    {
                        ListItem item2 = new ListItem(cit.city, cit.id.ToString());
                        DropDownCities.Items.Add(item2);
                    }

                }

            }

            string the_city = (from t in db.Table_MyTouristbook_Cities
                               where (t.id.ToString().Equals(val))
                               orderby t.city ascending
                               select t.city).FirstOrDefault();

            if (the_city == null)
            {
                return;
            }


            foreach (ListItem li in DropDownCities.Items)
            {
                if (li.Text.Equals(the_city))
                {
                    DropDownCities.SelectedValue = li.Value;
                }

            }

            city_change(this, null);

        }
        protected void RDest1_Click(object sender, ImageClickEventArgs e)
        {

            string val = HiddenDest1.Value;

            change_city(val);

        }

        protected void RDest2_Click(object sender, ImageClickEventArgs e)
        {
            string val = HiddenDest2.Value;

            change_city(val);
        }

        protected void RDest3_Click(object sender, ImageClickEventArgs e)
        {
            string val = HiddenDest3.Value;

            change_city(val);
        }

        protected void RDest4_Click(object sender, ImageClickEventArgs e)
        {
            string val = HiddenDest4.Value;

            change_city(val);
        }

        protected void RDest5_Click(object sender, ImageClickEventArgs e)
        {
            string val = HiddenDest5.Value;

            change_city(val);
        }




        protected void forum_country_change(object sender, EventArgs e)
        {

            string country = DropDownCountriesForum.SelectedItem.Text;

            init_forum_location();

            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();

            var res = (from t in db.Table_MyTouristbook_Cities
                       where ((t.country.Equals(country)))
                       orderby t.rank descending
                       select t).FirstOrDefault();

            string val;
            bool sel = false;

            if (res == null)
            {
                val = "144";
            }

            else if (country.Equals("Select Country"))
            {
                val = "144";
            }

            else
            {
                val = res.id.ToString();
            }

            change_forum_city(val);
            init_forums();

            string country_flag = extract_flag(country);
            if (country_flag.Equals(""))
            {
                location_image_forum.ImageUrl = "https://www.tourist-ads.com/maintravel.jpg";
            }

            else
            {
                location_image_forum.ImageUrl = country_flag;
            }

            readthread1.Focus();
            init_forums_related();
        }

        public void change_forum_city(string val)
        {

            if (val.Equals("0"))
            {
                return;
            }

            string country = find_country(val);

            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();

            if (DropDownCountriesForum.SelectedItem.Text.Equals("Select Country"))
            {
                foreach (ListItem li in DropDownCountriesForum.Items)
                {
                    if (li.Text.Equals(country))
                    {
                        DropDownCountriesForum.SelectedValue = li.Value;
                    }

                }

                var the_cities2 = (from t in db.Table_MyTouristbook_Cities
                                   where t.country.Equals(country)
                                   orderby t.city ascending
                                   select t);

                if (the_cities2 != null)
                {


                    foreach (var cit in the_cities2)
                    {
                        ListItem item2 = new ListItem(cit.city, cit.id.ToString());
                        DropDownCitiesForum.Items.Add(item2);
                    }

                }

            }

            string the_city = (from t in db.Table_MyTouristbook_Cities
                               where (t.id.ToString().Equals(val))
                               orderby t.city ascending
                               select t.city).FirstOrDefault();

            if (the_city == null)
            {
                return;
            }


            foreach (ListItem li in DropDownCitiesForum.Items)
            {
                if (li.Text.Equals(the_city))
                {
                    DropDownCitiesForum.SelectedValue = li.Value;
                }

            }

            forum_city_change(this, null);

            init_forums_related();
        }


        protected void forum_city_change(object sender, EventArgs e)
        {
            init_forum_loc();
            init_forums();
            readthread1.Focus();
        }

        protected void blog_country_change(object sender, EventArgs e)
        {
            string country = DropDownCountriesBlog.SelectedItem.Text;

            init_blog_location();

            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();

            var res = (from t in db.Table_MyTouristbook_Cities
                       where ((t.country.Equals(country)))
                       orderby t.rank descending
                       select t).FirstOrDefault();

            string val;
            bool sel = false;

            if (res == null)
            {
                val = "144";
            }

            else if (country.Equals("Select Country"))
            {
                val = "144";
            }

            else
            {
                val = res.id.ToString();
            }

            change_blog_city(val);
            init_blogs();

            string country_flag = extract_flag(country);
            if (country_flag.Equals(""))
            {
                location_image_blog.ImageUrl = "https://www.tourist-ads.com/maintravel.jpg";
            }

            else
            {
                location_image_blog.ImageUrl = country_flag;
            }

            init_blogs_related();            
            readblog1.Focus();

        }

        public void change_blog_city(string val)
        {

            if (val.Equals("0"))
            {
                return;
            }

            string country = find_country(val);

            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();

            if (DropDownCountriesBlog.SelectedItem.Text.Equals("Select Country"))
            {
                foreach (ListItem li in DropDownCountriesBlog.Items)
                {
                    if (li.Text.Equals(country))
                    {
                        DropDownCountriesBlog.SelectedValue = li.Value;
                    }

                }

                var the_cities2 = (from t in db.Table_MyTouristbook_Cities
                                   where t.country.Equals(country)
                                   orderby t.city ascending
                                   select t);

                if (the_cities2 != null)
                {


                    foreach (var cit in the_cities2)
                    {
                        ListItem item2 = new ListItem(cit.city, cit.id.ToString());
                        DropDownCitiesBlog.Items.Add(item2);
                    }

                }

            }

            string the_city = (from t in db.Table_MyTouristbook_Cities
                               where (t.id.ToString().Equals(val))
                               orderby t.city ascending
                               select t.city).FirstOrDefault();

            if (the_city == null)
            {
                return;
            }


            foreach (ListItem li in DropDownCitiesBlog.Items)
            {
                if (li.Text.Equals(the_city))
                {
                    DropDownCitiesBlog.SelectedValue = li.Value;
                }

            }

            blog_change(this, null);
            init_blogs_related();

        }


        protected void blog_change(object sender, EventArgs e)
        {
            init_blog_loc();
            init_blogs();
            readblog1.Focus();
        }

        protected void blog_changed(object sender, EventArgs e)
        {
            init_blog_loc();
            init_blogs();
            readblog1.Focus();
        }

        protected void deal_country_change(object sender, EventArgs e)
        {
            string country = DropDownCountriesDeals.SelectedItem.Text;

            DropDownCitiesDeals.Items.Clear();

            init_deal_location();

            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();

            var res = (from t in db.Table_MyTouristbook_Cities
                       where ((t.country.Equals(country)))
                       orderby t.rank descending
                       select t).FirstOrDefault();

            string val;
            bool sel = false;

            if (res == null)
            {
                val = "144";
            }

            else if (country.Equals("Select Country"))
            {
                val = "144";
            }

            else
            {
                val = res.id.ToString();
            }

            change_deal_city(val);
            init_deals();
            init_deal_related();

            string country_flag = extract_flag(country);
            if (country_flag.Equals(""))
            {
                location_image_deal2.ImageUrl = "https://www.tourist-ads.com/maintravel.jpg";
            }

            else
            {
                location_image_deal2.ImageUrl = country_flag;
            }


            init_deal_related();

            info1.Focus();

        }

        public void change_deal_city(string val)
        {

            if (val.Equals("0"))
            {
                return;
            }

            string country = find_country(val);

            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();

            if (DropDownCountriesDeals.SelectedItem.Text.Equals("Select Country"))
            {
                foreach (ListItem li in DropDownCountriesDeals.Items)
                {
                    if (li.Text.Equals(country))
                    {
                        DropDownCountriesDeals.SelectedValue = li.Value;
                    }

                }

                var the_cities2 = (from t in db.Table_MyTouristbook_Cities
                                   where t.country.Equals(country)
                                   orderby t.city ascending
                                   select t);

                if (the_cities2 != null)
                {


                    foreach (var cit in the_cities2)
                    {
                        ListItem item2 = new ListItem(cit.city, cit.id.ToString());
                        DropDownCitiesDeals.Items.Add(item2);
                    }

                }

            }

            string the_city = (from t in db.Table_MyTouristbook_Cities
                               where (t.id.ToString().Equals(val))
                               orderby t.city ascending
                               select t.city).FirstOrDefault();

            if (the_city == null)
            {
                return;
            }


            foreach (ListItem li in DropDownCitiesDeals.Items)
            {
                if (li.Text.Equals(the_city))
                {
                    DropDownCitiesDeals.SelectedValue = li.Value;
                }

            }

            deal_changed(this, null);
            init_deal_related();

        }

        protected void deal_changed(object sender, EventArgs e)
        {
            init_deal_loc();
            init_deals();
            info1.Focus();
        }

        public void reset_deals_related()
        {

            RelDest6.ImageUrl = "";
            destlink6.Text = "";
            HiddenDest6.Value = "0";
            RelDest6.BorderWidth = 0;
            RelDest6.Visible = false;


            RelDest7.ImageUrl = "";
            destlink7.Text = "";
            HiddenDest7.Value = "0";
            RelDest7.BorderWidth = 0;
            RelDest7.Visible = false;

            RelDest8.ImageUrl = "";
            destlink8.Text = "";
            HiddenDest8.Value = "0";
            RelDest8.BorderWidth = 0;
            RelDest8.Visible = false;

            RelDest9.ImageUrl = "";
            destlink9.Text = "";
            HiddenDest9.Value = "0";
            RelDest9.BorderWidth = 0;
            RelDest9.Visible = false;

            RelDest10.ImageUrl = "";
            destlink10.Text = "";
            HiddenDest10.Value = "0";
            RelDest10.BorderWidth = 0;
            RelDest10.Visible = false;


        }


        public void init_deal_related()
        {

            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();

            string dest = DropDownCountriesDeals.SelectedItem.Text;

            if (dest.Equals("Select Country"))
            {
                //dest = "United States";                
            }

            var the_cities = (from t in db.Table_MyTouristbook_Cities
                              where ((t.country.Equals(dest)) || (dest.Equals("Select Country")))
                              orderby t.rank descending
                              select t).Take(5);


            /*var the_cities = (from t in db.Table_MyTouristbook_Cities
                              where t.country.Equals(dest)
                              orderby t.rank descending
                              select t).Take(5);
                              */

            int counter = 1;

            reset_deals_related();

            foreach (var city in the_cities)
            {
                //affsbook.Table_Affsbook_Affiliate aff = get_affiliate((int)tab2.authoraid);

                if (counter == 1)
                {
                    RelDest8.ImageUrl = city.icon;
                    RelDest8.Width = 175;
                    RelDest8.Height = 150;
                    RelDest8.BorderWidth = 2;
                    destlink8.Text = city.city;
                    HiddenDest8.Value = city.id.ToString();
                    RelDest8.Visible = true;
                }

                if (counter == 2)
                {
                    RelDest7.ImageUrl = city.icon;
                    RelDest7.Width = 175;
                    RelDest7.Height = 150;
                    RelDest7.BorderWidth = 2;
                    destlink7.Text = city.city;
                    HiddenDest7.Value = city.id.ToString();
                    RelDest7.Visible = true;
                }

                if (counter == 3)
                {
                    RelDest9.ImageUrl = city.icon;
                    RelDest9.Width = 175;
                    RelDest9.Height = 150;
                    RelDest9.BorderWidth = 2;
                    destlink9.Text = city.city;
                    HiddenDest9.Value = city.id.ToString();
                    RelDest9.Visible = true;
                }

                if (counter == 4)
                {
                    RelDest6.ImageUrl = city.icon;
                    RelDest6.Width = 175;
                    RelDest6.Height = 150;
                    RelDest6.BorderWidth = 2;
                    destlink6.Text = city.city;
                    HiddenDest6.Value = city.id.ToString();
                    RelDest6.Visible = true;
                }

                if (counter == 5)
                {
                    RelDest10.ImageUrl = city.icon;
                    RelDest10.Width = 175;
                    RelDest10.Height = 150;
                    RelDest10.BorderWidth = 2;
                    destlink10.Text = city.city;
                    HiddenDest10.Value = city.id.ToString();
                    RelDest10.Visible = true;
                }

                counter++;
            }


        }

        protected void RDest6_Click(object sender, ImageClickEventArgs e)
        {
            string val = HiddenDest6.Value;

            change_deal_city(val);

        }

        protected void RDest7_Click(object sender, ImageClickEventArgs e)
        {
            string val = HiddenDest7.Value;

            change_deal_city(val);
        }

        protected void RDest8_Click(object sender, ImageClickEventArgs e)
        {
            string val = HiddenDest8.Value;

            change_deal_city(val);
        }

        protected void RDest9_Click(object sender, ImageClickEventArgs e)
        {
            string val = HiddenDest9.Value;

            change_deal_city(val);
        }

        protected void RDest10_Click(object sender, ImageClickEventArgs e)
        {
            string val = HiddenDest10.Value;

            change_deal_city(val);
        }

        public void reset_forums_related()
        {

            RelDest11.ImageUrl = "";
            destlink11.Text = "";
            HiddenDest11.Value = "0";
            RelDest11.BorderWidth = 0;
            RelDest11.Visible = false;


            RelDest12.ImageUrl = "";
            destlink12.Text = "";
            HiddenDest12.Value = "0";
            RelDest12.BorderWidth = 0;
            RelDest12.Visible = false;

            RelDest13.ImageUrl = "";
            destlink13.Text = "";
            HiddenDest13.Value = "0";
            RelDest13.BorderWidth = 0;
            RelDest13.Visible = false;

            RelDest14.ImageUrl = "";
            destlink14.Text = "";
            HiddenDest14.Value = "0";
            RelDest14.BorderWidth = 0;
            RelDest14.Visible = false;

            RelDest15.ImageUrl = "";
            destlink15.Text = "";
            HiddenDest15.Value = "0";
            RelDest15.BorderWidth = 0;
            RelDest15.Visible = false;


        }


        public void init_forums_related()
        {

            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();

            string dest = DropDownCountriesForum.SelectedItem.Text;

            if (dest.Equals("Select Country"))
            {
                //dest = "United States";                
            }

            var the_cities = (from t in db.Table_MyTouristbook_Cities
                              where ((t.country.Equals(dest)) || (dest.Equals("Select Country")))
                              orderby t.rank descending
                              select t).Take(5);


            /*var the_cities = (from t in db.Table_MyTouristbook_Cities
                              where t.country.Equals(dest)
                              orderby t.rank descending
                              select t).Take(5);
                              */

            int counter = 1;

            reset_forums_related();

            foreach (var city in the_cities)
            {
                //affsbook.Table_Affsbook_Affiliate aff = get_affiliate((int)tab2.authoraid);

                if (counter == 1)
                {
                    RelDest13.ImageUrl = city.icon;
                    RelDest13.Width = 175;
                    RelDest13.Height = 150;
                    RelDest13.BorderWidth = 2;
                    destlink13.Text = city.city;
                    HiddenDest13.Value = city.id.ToString();
                    RelDest13.Visible = true;
                }

                if (counter == 2)
                {
                    RelDest12.ImageUrl = city.icon;
                    RelDest12.Width = 175;
                    RelDest12.Height = 150;
                    RelDest12.BorderWidth = 2;
                    destlink12.Text = city.city;
                    HiddenDest12.Value = city.id.ToString();
                    RelDest12.Visible = true;
                }

                if (counter == 3)
                {
                    RelDest14.ImageUrl = city.icon;
                    RelDest14.Width = 175;
                    RelDest14.Height = 150;
                    RelDest14.BorderWidth = 2;
                    destlink14.Text = city.city;
                    HiddenDest14.Value = city.id.ToString();
                    RelDest14.Visible = true;
                }

                if (counter == 4)
                {
                    RelDest11.ImageUrl = city.icon;
                    RelDest11.Width = 175;
                    RelDest11.Height = 150;
                    RelDest11.BorderWidth = 2;
                    destlink11.Text = city.city;
                    HiddenDest11.Value = city.id.ToString();
                    RelDest11.Visible = true;
                }

                if (counter == 5)
                {
                    RelDest15.ImageUrl = city.icon;
                    RelDest15.Width = 175;
                    RelDest15.Height = 150;
                    RelDest15.BorderWidth = 2;
                    destlink15.Text = city.city;
                    HiddenDest15.Value = city.id.ToString();
                    RelDest15.Visible = true;
                }

                counter++;
            }


        }

        protected void RDest11_Click(object sender, ImageClickEventArgs e)
        {
            string val = HiddenDest11.Value;

            change_forum_city(val);
        }

        protected void RDest12_Click(object sender, ImageClickEventArgs e)
        {
            string val = HiddenDest12.Value;

            change_forum_city(val);
        }

        protected void RDest13_Click(object sender, ImageClickEventArgs e)
        {
            string val = HiddenDest13.Value;

            change_forum_city(val);
        }

        protected void RDest14_Click(object sender, ImageClickEventArgs e)
        {
            string val = HiddenDest14.Value;

            change_forum_city(val);
        }

        protected void RDest15_Click(object sender, ImageClickEventArgs e)
        {
            string val = HiddenDest15.Value;

            change_forum_city(val);
        }

        public void reset_blogs_related()
        {
                       
            RelDest16.ImageUrl = "";
            destlink16.Text = "";
            HiddenDest16.Value = "0";
            RelDest16.BorderWidth = 0;
            RelDest16.Visible = false;


            RelDest17.ImageUrl = "";
            destlink17.Text = "";
            HiddenDest17.Value = "0";
            RelDest17.BorderWidth = 0;
            RelDest17.Visible = false;

            RelDest18.ImageUrl = "";
            destlink18.Text = "";
            HiddenDest18.Value = "0";
            RelDest18.BorderWidth = 0;
            RelDest18.Visible = false;

            RelDest19.ImageUrl = "";
            destlink19.Text = "";
            HiddenDest19.Value = "0";
            RelDest19.BorderWidth = 0;
            RelDest19.Visible = false;

            RelDest20.ImageUrl = "";
            destlink20.Text = "";
            HiddenDest20.Value = "0";
            RelDest20.BorderWidth = 0;
            RelDest20.Visible = false;


        }


        public void init_blogs_related()
        {

            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();

            string dest = DropDownCountriesBlog.SelectedItem.Text;

            if (dest.Equals("Select Country"))
            {
                //dest = "United States";                
            }

            var the_cities = (from t in db.Table_MyTouristbook_Cities
                              where ((t.country.Equals(dest)) || (dest.Equals("Select Country")))
                              orderby t.rank descending
                              select t).Take(5);


            /*var the_cities = (from t in db.Table_MyTouristbook_Cities
                              where t.country.Equals(dest)
                              orderby t.rank descending
                              select t).Take(5);
                              */

            int counter = 1;

            reset_blogs_related();

            foreach (var city in the_cities)
            {
                //affsbook.Table_Affsbook_Affiliate aff = get_affiliate((int)tab2.authoraid);

                if (counter == 1)
                {
                    RelDest18.ImageUrl = city.icon;
                    RelDest18.Width = 175;
                    RelDest18.Height = 150;
                    RelDest18.BorderWidth = 2;
                    destlink18.Text = city.city;
                    HiddenDest18.Value = city.id.ToString();
                    RelDest18.Visible = true;
                }

                if (counter == 2)
                {
                    RelDest17.ImageUrl = city.icon;
                    RelDest17.Width = 175;
                    RelDest17.Height = 150;
                    RelDest17.BorderWidth = 2;
                    destlink17.Text = city.city;
                    HiddenDest17.Value = city.id.ToString();
                    RelDest17.Visible = true;
                }

                if (counter == 3)
                {
                    RelDest19.ImageUrl = city.icon;
                    RelDest19.Width = 175;
                    RelDest19.Height = 150;
                    RelDest19.BorderWidth = 2;
                    destlink19.Text = city.city;
                    HiddenDest19.Value = city.id.ToString();
                    RelDest19.Visible = true;
                }

                if (counter == 4)
                {
                    RelDest16.ImageUrl = city.icon;
                    RelDest16.Width = 175;
                    RelDest16.Height = 150;
                    RelDest16.BorderWidth = 2;
                    destlink16.Text = city.city;
                    HiddenDest16.Value = city.id.ToString();
                    RelDest16.Visible = true;
                }

                if (counter == 5)
                {
                    RelDest20.ImageUrl = city.icon;
                    RelDest20.Width = 175;
                    RelDest20.Height = 150;
                    RelDest20.BorderWidth = 2;
                    destlink20.Text = city.city;
                    HiddenDest20.Value = city.id.ToString();
                    RelDest20.Visible = true;
                }

                counter++;
            }


        }

        protected void RDest16_Click(object sender, ImageClickEventArgs e)
        {
            string val = HiddenDest16.Value;

            change_blog_city(val);

            
        }

        protected void RDest17_Click(object sender, ImageClickEventArgs e)
        {
            string val = HiddenDest17.Value;

            change_blog_city(val);
        }

        protected void RDest18_Click(object sender, ImageClickEventArgs e)
        {
            string val = HiddenDest18.Value;

            change_blog_city(val);
        }

        protected void RDest19_Click(object sender, ImageClickEventArgs e)
        {
            string val = HiddenDest19.Value;

            change_blog_city(val);
        }

        protected void RDest20_Click(object sender, ImageClickEventArgs e)
        {
            string val = HiddenDest20.Value;

            change_blog_city(val);
        }

        protected void offers_menu_click(object sender, EventArgs e)
        {
            deals_click(sender, e);
        }

        protected void blogs_menu_click(object sender, EventArgs e)
        {
            blogs_click(sender, e);
        }

        protected void forums_menu_click(object sender, EventArgs e)
        {
            forums_click(sender, e);
        }


  
        public void redirectfeed(string st)
        {
            if (st.Equals("~/images/forumthread.jpg"))
            {
                forums_click(this, null);
            }

            if (st.Equals("~/images/deal3.jpg"))
            {                
                deals_click(this, null);                                
            }

            if (st.Equals("~/images/blog3.png"))
            {
                blogs_click(this, null);
            }

        }

        protected void feedbutton1click(object sender, ImageClickEventArgs e)
        {
            string thetype = TheFeedImageButton1.ImageUrl;
            redirectfeed(thetype);
        }

        protected void feedbutton2click(object sender, ImageClickEventArgs e)
        {
            string thetype = TheFeedImageButton2.ImageUrl;
            redirectfeed(thetype);
        }

        protected void feedbutton3click(object sender, ImageClickEventArgs e)
        {
            string thetype = TheFeedImageButton3.ImageUrl;
            redirectfeed(thetype);
        }

        protected void feedbutton4click(object sender, ImageClickEventArgs e)
        {
            string thetype = TheFeedImageButton4.ImageUrl;
            redirectfeed(thetype);
        }

        protected void feedbutton5click(object sender, ImageClickEventArgs e)
        {
            string thetype = TheFeedImageButton5.ImageUrl;
            redirectfeed(thetype);
        }

        protected void feedbutton6click(object sender, ImageClickEventArgs e)
        {
            string thetype = TheFeedImageButton6.ImageUrl;
            redirectfeed(thetype);
        }

        protected void feedbutton7click(object sender, ImageClickEventArgs e)
        {
            string thetype = TheFeedImageButton7.ImageUrl;
            redirectfeed(thetype);
        }

        public void createad()
        {
            MultiView1.ActiveViewIndex = 4;
        }

        protected void premier_click(object sender, EventArgs e)
        {
            HiddenFieldPlan.Value = "1";
            planprice.Text = "Premier Listing - One Month - $125";
            MultiView10.ActiveViewIndex = 1;
            Button314.Focus();
            payval.Visible = false;
        }

        protected void gold_click(object sender, EventArgs e)
        {
            HiddenFieldPlan.Value = "2";
            planprice.Text = "Gold Sponsorship - One Month - $350";
            MultiView10.ActiveViewIndex = 1;
            Button314.Focus();
            payval.Visible = false;
        }

        public int calc()
        {
            int sum = 0;

            if (HiddenFieldPlan.Value.Equals("1"))
            {
                sum = 125;
            }

            else if (HiddenFieldPlan.Value.Equals("2"))
            {                
                sum = 350;
            }

            string dur = dropdownpayment1.SelectedValue;
            int duration = Convert.ToInt32(dur);

            return sum * duration;
        }
        protected void dur_click(object sender, EventArgs e)
        {
         

            
            string myplan = "";

            if (HiddenFieldPlan.Value.Equals("1"))
            {
                myplan = "Premier Listing - ";                
            }

            else if (HiddenFieldPlan.Value.Equals("2"))
            {
                myplan = "Gold Sponsorship - ";                
            }


            int amount = calc();                   

            planprice.Text = myplan + dropdownpayment1.SelectedItem.Text + " - $" + amount.ToString();

            Button314.Focus();
            
            
        }

        protected void payment_click(object sender, EventArgs e)
        {
            if (dropdownpayment2.SelectedValue.Equals("0"))
            {
                payval.Visible = true;
                Button314.Focus();
                return;
            }

            else if (dropdownpayment2.SelectedValue.Equals("1"))
            {
                int amount = calc();
                string link = @"http://www.paypal.me/adsrushx/" + amount.ToString();
                Response.Redirect(link);
            }

            else if (dropdownpayment2.SelectedValue.Equals("3"))
            {
                int amount = calc();
                planprice2.Text = planprice.Text;                
                MultiView10.ActiveViewIndex = 2;
                ccfocus();
                return;


            }

            else if (dropdownpayment2.SelectedValue.Equals("2"))
            {
                int amount = calc();
                planprice3.Text = planprice.Text;                
                MultiView10.ActiveViewIndex = 3;
                pyfocus();
                return;

            }

        }

        protected void cc_click(object sender, EventArgs e)
        {
            nocval.Visible = false;
            ccnumval.Visible = false;
            expval.Visible = false;
            cvvval.Visible = false;

            if (ccname.Text.Equals(""))
            {
                nocval.Visible = true;
                Button112.Focus();
                return;
            }

            if (ccnumber.Text.Equals(""))
            {
                ccnumval.Visible = true;
                Button112.Focus();
                return;
            }

            if (dropdownmonth.SelectedValue.Equals("0"))
            {
                expval.Visible = true;
                Button112.Focus();
                return;
            }


            if (dropdownyear.SelectedValue.Equals("0"))
            {
                expval.Visible = true;
                Button112.Focus();
                return;
            }

            if (cvv.Text.Equals(""))
            {
                cvvval.Visible = true;
                Button112.Focus();
                return;
            }

        }

        public void ccfocus()
        {
            Button112.Focus();
        }

        public void pyfocus()
        {
            Button315.Focus();
        }

        protected void payoneer_click(object sender, EventArgs e)
        {
            pyval.Visible = false;

            if (pyaccount.Text.Equals(""))
            {
                pyval.Visible = true;
                Button315.Focus();
                return;
            }

        }
    }
}