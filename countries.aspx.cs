using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyTouristBook
{
    public partial class countries : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DropDownListCountry.Items.Clear();
                DropDownListCountry3.Items.Clear();
                DropDownListCountry.Items.Clear();

                MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();

                var countries = (from t in db.Table_Countries
                                 select t.country);

                foreach(var country in countries)
                {
                    ListItem item = new ListItem(country);
                    DropDownListCountry2.Items.Add(item);
                    DropDownListCountry3.Items.Add(item);
                    DropDownListCountry.Items.Add(item);
                }

            }
        }

        protected void submit_click(object sender, EventArgs e)
        {
            string txt = TextBox1.Text;
            string[] lst = txt.Split(new Char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            Array.Sort(lst);

            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();            

            foreach (string country in lst)
            {
                MyTouristBook.Table_Country tab = new MyTouristBook.Table_Country();                

                var the_country = (from t in db.Table_Countries
                                where (t.country.Equals(country))
                                select t.country).FirstOrDefault();

                if (the_country!=null)
                {
                    continue;
                }


                int max2 = 0;

                try
                {

                    max2 = (from t in db.Table_Countries
                            select t.id).Max();
                }
                catch (Exception ex)
                {

                }

                // get referral aid and username

                tab.id = max2 + 1;
                tab.country = country;
                db.Table_Countries.InsertOnSubmit(tab);
                db.SubmitChanges();
            }



        }

        protected void submit2_click(object sender, EventArgs e)
        {
            string the_country = DropDownListCountry.SelectedItem.Text;

            string txt = TextBox2.Text;
            string[] lst = txt.Split(new Char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            Array.Sort(lst);

            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();

            foreach (string city in lst)
            {
                
                MyTouristBook.Table_City tab = new MyTouristBook.Table_City();

                var the_city    = (from t in db.Table_Cities
                                   where (t.city.Equals(city))
                                   select t.country).FirstOrDefault();

                if (the_city != null)
                {
                    continue;
                }


                int max2 = 0;

                try
                {

                    max2 = (from t in db.Table_Cities
                            select t.id).Max();
                }
                catch (Exception ex)
                {

                }

                // get referral aid and username

                tab.id = max2 + 1;
                tab.country = the_country;
                tab.city = city;
                db.Table_Cities.InsertOnSubmit(tab);

                db.SubmitChanges();
            }


        }

        protected void clear_click(object sender, EventArgs e)
        {
            TextBox2.Text = "";
        }

        public bool blogwelcome(string city)
        {

            if (city.Equals("Barcelona"))
                return false;

            MyTouristBook.blogsDataContext db = new MyTouristBook.blogsDataContext();
            MyTouristBook.blogsDataContext db2 = new MyTouristBook.blogsDataContext();

            MyTouristBook.Table_MyTouristbook_Blog tab = new MyTouristBook.Table_MyTouristbook_Blog();
            MyTouristBook.Table_MyTouristbook_Blog tab2 = new MyTouristBook.Table_MyTouristbook_Blog();


            tab = (from t in db.Table_MyTouristbook_Blogs
                   where (t.blog_id == 4)
                   select t).FirstOrDefault();


            int max2 = 0;

            try
            {

                max2 = (from t in db2.Table_MyTouristbook_Blogs
                        select t.id).Max();
            }

            catch (Exception ex)
            {
                max2 = 0;
            }

            int? max3 = 0;

            try
            {

                max3 = (from t in db2.Table_MyTouristbook_Blogs
                        select t.blog_id).Max();
            }

            catch (Exception ex)
            {
                max3 = 0;
            }

            if (max3 == null)
                max3 = 0;

            tab2.id = max2 + 1;
            tab2.blog_id = max3 + 1;
            tab2.startdate = DateTime.Now;

            tab2.authorusername = tab.authorusername;
            tab2.authoraid = tab.authoraid;

            tab2.title = tab.title.Replace("Barcelona", city);
            tab2.body = tab.body.Replace("Barcelona", city);


            tab2.active = 1;
            tab2.imageurl = "";
            tab2.priority = 0;
            tab2.featured = 0;
            tab2.sponsored = 0;
            tab2.featured = 0;
            tab2.popular = 0;

            tab2.dest1 = city;

            db2.Table_MyTouristbook_Blogs.InsertOnSubmit(tab2);
            

            try
            {
                db2.SubmitChanges();
            }
            catch (Exception ex)
            {
                return false;
            }


            return true;
        }

        public bool dealwelcome(string city)
        {

            if (city.Equals("Barcelona"))
                return false;

            MyTouristBook.dealsDataContext db = new MyTouristBook.dealsDataContext();
            MyTouristBook.dealsDataContext db2 = new MyTouristBook.dealsDataContext();

            MyTouristBook.Table_MyTouristbook_Deal tab = new MyTouristBook.Table_MyTouristbook_Deal();
            MyTouristBook.Table_MyTouristbook_Deal tab2 = new MyTouristBook.Table_MyTouristbook_Deal();


            tab = (from t in db.Table_MyTouristbook_Deals
                   where (t.id == 2)
                   select t).FirstOrDefault();


            int max2 = 0;

            try
            {

                max2 = (from t in db2.Table_MyTouristbook_Deals
                        select t.id).Max();
            }

            catch (Exception ex)
            {
                max2 = 0;
            }

    
            tab2.id = max2 + 1;            
            tab2.date = DateTime.Now;
            tab2.active = 1;
            tab2.owneraid = tab.owneraid;
            tab2.ownerusername = tab.ownerusername;                                  

            tab2.title = tab.title.Replace("Barcelona", city);
            tab2.description=tab.description.Replace("Barcelona", city);
            tab2.shortdescription=tab.shortdescription.Replace("Barcelona", city);
            tab2.pricing = tab.pricing;
            tab2.dest1 = city;

            tab2.dealurl = "";
            tab2.orderurl = "";
            tab2.country = tab.country;
            tab2.city = tab.city;

            MyTouristBook.CountriesDataContext db3 = new MyTouristBook.CountriesDataContext();
            MyTouristBook.Table_Country_Flag tab3 = new MyTouristBook.Table_Country_Flag();

            MyTouristBook.CountriesDataContext db4 = new MyTouristBook.CountriesDataContext();
            MyTouristBook.Table_City tab4 = new MyTouristBook.Table_City();

            tab4 = (from t in db4.Table_Cities
                    where (t.city.Equals(city))
                    select t).FirstOrDefault();

            string country = tab4.country;

            tab3 = (from t in db3.Table_Country_Flags
                   where (t.country.Equals(country))
                   select t).FirstOrDefault();

            tab2.imageurl = "";

            if (tab3 != null)
                tab2.imageurl = tab3.flag_image;                


            tab2.rating = 5;           
            
            tab2.priority = 0;
            tab2.featured = 0;
            tab2.sponsored = 0;
            tab2.featured = 0;
            tab2.popular = 0;
            tab2.relatedofferid = 0;

            db2.Table_MyTouristbook_Deals.InsertOnSubmit(tab2);            


            try
            {
                db2.SubmitChanges();
            }
            catch (Exception ex)
            {
                return false;
            }


            return true;
        }


        public bool forumwelcome (string city)
        {

            if (city.Equals("Barcelona"))
                return false;

            MyTouristBook.ForumsDataContext db = new MyTouristBook.ForumsDataContext();
            MyTouristBook.ForumsDataContext db2 = new MyTouristBook.ForumsDataContext();
            MyTouristBook.Table_MyTouristbook_Forum_Thread tab = new MyTouristBook.Table_MyTouristbook_Forum_Thread();
            MyTouristBook.Table_MyTouristbook_Forum_Thread tab2 = new MyTouristBook.Table_MyTouristbook_Forum_Thread();


            tab = (from t in db.Table_MyTouristbook_Forum_Threads
                   where (t.thread_id == 3)
                   select t).FirstOrDefault();


            int max2 = 0;

            try
            {

                max2 = (from t in db2.Table_MyTouristbook_Forum_Threads
                        select t.id).Max();
            }

            catch (Exception ex)
            {
                max2 = 0;
            }

            int? max3 = 0;

            try
            {

                max3 = (from t in db2.Table_MyTouristbook_Forum_Threads
                        select t.thread_id).Max();
            }

            catch (Exception ex)
            {
                max3 = 0;
            }

            if (max3 == null)
                max3 = 0;

            tab2.id = max2 + 1;
            tab2.thread_id = max3 + 1;

            tab2.autherusername = tab.autherusername;
            tab2.authoraid = tab.authoraid;
            tab2.replyusername = tab.replyusername;
            tab2.the_reply_date = tab.the_reply_date;

            tab2.replyaid = tab.replyaid;
            tab2.views = 0;
            
            tab2.startdate = DateTime.Now;
            

            tab2.subject = tab.subject.Replace("Barcelona",city);
            tab2.body = tab.body.Replace("Barcelona", city);
            tab2.dest1 = city;
            tab2.replynumber = 0;
            tab2.replies = 0;            

            db2.Table_MyTouristbook_Forum_Threads.InsertOnSubmit(tab2);

            try
            {
                db2.SubmitChanges();
            }
            catch (Exception ex)
            {
                return false;
            }


            return true;
        }


        protected void submit3_click(object sender, EventArgs e)
        {

            /*
             * 
             */


            string the_country = DropDownListCountry2.SelectedItem.Text;
            TextBox3.Text = "";
            /*

            string txt = TextBox2.Text;
            string[] lst = txt.Split(new Char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            Array.Sort(lst);    */

            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();
            MyTouristBook.Table_Country tab = new MyTouristBook.Table_Country();
            MyTouristBook.Table_City tab2 = new MyTouristBook.Table_City();

            var tab3 = (from t in db.Table_Cities
                        where t.country.Equals(the_country)
                        select t.city);

            foreach (var city in tab3)
            {

                /*

                var the_cities = (from t in db.Table_Cities
                                  where (t.country.Equals(the_country))
                                  select t.city); */

                if (city == null)
                {
                    continue;
                }

                bool result= blogwelcome(city);

                if (result==true)                
                    TextBox3.Text += city + " Done! \n";
                else if (result == false)
                    TextBox3.Text += city + " Error! \n";


                /*
                int max2 = 0;

                try
                {

                    max2 = (from t in db.Table_Cities
                            select t.id).Max();
                }
                catch (Exception ex)
                {

                }

                // get referral aid and username

                tab.id = max2 + 1;
                tab.country = the_country;
                tab.city = city;
                db.Table_Cities.InsertOnSubmit(tab);

                db.SubmitChanges(); */
            }

        }

        protected void submit4_click(object sender, EventArgs e)
        {
            string the_country = DropDownListCountry3.SelectedItem.Text;
            TextBox4.Text = "";
            /*

            string txt = TextBox2.Text;
            string[] lst = txt.Split(new Char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            Array.Sort(lst);    */

            MyTouristBook.CountriesDataContext db = new MyTouristBook.CountriesDataContext();
            MyTouristBook.Table_Country tab = new MyTouristBook.Table_Country();
            MyTouristBook.Table_City tab2 = new MyTouristBook.Table_City();

            var tab3 = (from t in db.Table_Cities
                        where t.country.Equals(the_country)
                        select t.city);

            foreach (var city in tab3)
            {

                /*

                var the_cities = (from t in db.Table_Cities
                                  where (t.country.Equals(the_country))
                                  select t.city); */

                if (city == null)
                {
                    continue;
                }

                bool result = dealwelcome(city);

                if (result == true)
                    TextBox4.Text += city + " Done! \n";
                else if (result == false)
                    TextBox4.Text += city + " Error! \n";


                /*
                int max2 = 0;

                try
                {

                    max2 = (from t in db.Table_Cities
                            select t.id).Max();
                }
                catch (Exception ex)
                {

                }

                // get referral aid and username

                tab.id = max2 + 1;
                tab.country = the_country;
                tab.city = city;
                db.Table_Cities.InsertOnSubmit(tab);

                db.SubmitChanges(); */
            }


        }
    }
}