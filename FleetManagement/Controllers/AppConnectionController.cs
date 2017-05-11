using System;
using System.Web.Mvc;

namespace FleetManagement.Controllers
{
    public class AppConnectionController : Controller
    {
        //
        // GET: /AppConnection/

        public ActionResult Index()
        {
            return View();

        }

        [HttpPost]
        public string LogIn(LogPass logpass)
        {
            String response = "";
            try
            {
                int userid = logpass.CheckLogin();

                //if correct - generate temporary user_id (id+gandom)
                if (userid != -1)
                {
                    string temp_userid = logpass.Login + Guid.NewGuid().ToString("N");

                    response = "{\"error\":\"false\",\"user_id\":\"" + temp_userid + "\"}";

                    // put/apdate temp_userid to BD TemporaryID (user_id, temporary_id)
                    //То для того щоб не можна було при пості координат користувача знаючи просто логін/id засрати БД =)
                }
                else
                    if (logpass.CheckExist())
                    response = "{\"error\":\"true\", \"message\":\"Wrong Login\"}";
                else
                    response = "{\"error\":\"true\", \"message\":\"Wrong Password\"}";


            }
            catch (Exception ex)
            {
                response = "{\"error\":\"true\", \"message\":\"Bad request\"}";
            }
            return response;
        }

        [HttpPost]
        public string Registrate(LogPass logpass)
        {
            String response = "";
            try
            {
                if (!logpass.CheckExist())
                {
                    string temp_userid = logpass.Login + Guid.NewGuid().ToString("N");

                    response = "{\"error\":\"false\",\"login\":\"" + temp_userid + "\"}";
                }
                else
                    response = "{\"error\":\"true\", \"message\":\"User already exist\"}";


            }
            catch (Exception ex)
            {
                response = "{\"error\":\"true\", \"message\":\"Bad request\"}";
            }
            return response;
        }

        [HttpPost]
        public string SendLocation(UserLocationPoint user_location)
        {
            //Put coordinates to BD by user_Temporary_Id

            String response = "";
            try
            {
                // Put 'user_location' data intu DB
                response = "{\"error\":\"false\", \"message\":\"Data received\"}";
            }
            catch (Exception ex)
            {
                response = "{\"error\":\"true\", \"message\":\"Bad request\"}";
            }
            return response;

        }

    }

    public class LogPass
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public int CheckLogin()
        {
            //Check code
            if (Login == "Max" && Login == Password)
            {
                return 1;
            }
            else
                return -1;
        }
        public bool CheckExist()
        {
            //Check code
            if (Login == "Max")
                return true;
            else
                return false;

        }
    }
    public class UserLocationPoint
    {
        public string User_id { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Time { get; set; }

    }
}
