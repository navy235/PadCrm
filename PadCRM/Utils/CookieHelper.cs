using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Configuration;
using Maitonn.Core;

namespace PadCRM.Utils
{
    public class CookieHelper
    {

        #region  Nomarl Cookies
        /// <summary>
        /// 会员登录Cookies
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="email">Email</param>
        /// <param name="nickName">昵称</param>
        /// <param name="avtarImgUrl">头像地址</param>
        /// <param name="sex">性别</param>
        /// <param name="loginCount">登录次数</param>
        /// <param name="pwd">密码</param>
        /// <param name="cookiedate">记住</param>
        public static void LoginCookieSave(string uid,
            string email,
            string nickName,
            string avtarUrl,
            string groupID,
            string Status,
            string mt,
            string pwd,
            string remember,
            string message)
        {

            System.Web.HttpCookie cookie = new System.Web.HttpCookie(ConfigurationManager.AppSettings["CookieName"]);
            cookie.Values.Add("UID", CheckHelper.Escape(uid));
            cookie.Values.Add("NickName", CheckHelper.Escape(nickName));
            cookie.Values.Add("Email", CheckHelper.Escape(email));
            cookie.Values.Add("AvtarUrl", avtarUrl);
            cookie.Values.Add("GroupID", groupID);
            cookie.Values.Add("Status", Status);
            cookie.Values.Add("MT", mt);
            cookie.Values.Add("PWD", CheckHelper.StrToSHA1(pwd));
            cookie.Values.Add("Message", message);

            var valueStr = "UID=" + uid
                        + "NickName=" + nickName
                        + "Email=" + email
                        + "AvtarUrl=" + avtarUrl
                        + "GroupID=" + groupID
                        + "Status=" + Status
                        + "MT=" + mt
                        + "PWD=" + CheckHelper.StrToSHA1(pwd)
                        + "Message=" + message;
            valueStr = valueStr.ToLower();
            var secretStr = CheckHelper.StrToSHA1(valueStr);

            cookie.Values.Add("Secret", secretStr);

            switch (remember)
            {
                case "1":
                    cookie.Expires = DateTime.Now.AddDays(1); break;
                case "2":
                    cookie.Expires = DateTime.Now.AddDays(2); break;
                case "3":
                    cookie.Expires = DateTime.Now.AddYears(1); break;
            }

            cookie.Domain = ConfigurationManager.AppSettings["LocalDomain"];
            HttpContext.Current.Response.AppendHeader("P3P: CP", "CURa ADMa DEVa PSAo PSDo OUR BUS UNI PUR INT DEM STA PRE COM NAV OTC NOI DSP COR");
            HttpContext.Current.Response.AppendCookie(cookie);

            //保存常驻COOKIE
            //WinCookieSave(email, nid);

            //保存登录痕迹
            //WinLoginTraceCookieSave(uid);
        }


        public static bool notModify()
        {
            var cookieName = ConfigurationManager.AppSettings["CookieName"];
            string UID = GetCookie(cookieName, "UID");
            string NickName = GetCookie(cookieName, "NickName");
            string Email = GetCookie(cookieName, "Email");
            string AvtarUrl = GetCookie(cookieName, "AvtarUrl");
            string GroupID = GetCookie(cookieName, "GroupID");
            string Status = GetCookie(cookieName, "Status");
            string MT = GetCookie(cookieName, "MT");
            string PWD = GetCookie(cookieName, "PWD");
            string Message = GetCookie(cookieName, "Message");
            string Secret = GetCookie(cookieName, "Secret");
            var valueStr = "UID=" + UID
                      + "NickName=" + NickName
                      + "Email=" + Email
                      + "AvtarUrl=" + AvtarUrl
                      + "GroupID=" + GroupID
                      + "Status=" + Status
                      + "MT=" + MT
                      + "PWD=" + PWD
                      + "Message=" + Message;
            valueStr = valueStr.ToLower();
            var secretStr = CheckHelper.StrToSHA1(valueStr);
            return Secret == secretStr;
        }

        /// <summary>
        /// 设置省份cookie
        /// </summary>
        /// <param name="province"></param>
        public static void SetProvinceCookie(string province)
        {
            System.Web.HttpCookie cookie = new System.Web.HttpCookie(ConfigurationManager.AppSettings["ProvinceName"]);
            cookie.Values.Add("province", CheckHelper.Escape(province));
            cookie.Expires = DateTime.Now.AddDays(7);
            cookie.Domain = ConfigurationManager.AppSettings["LocalDomain"];
            HttpContext.Current.Response.AppendHeader("P3P: CP", "CURa ADMa DEVa PSAo PSDo OUR BUS UNI PUR INT DEM STA PRE COM NAV OTC NOI DSP COR");
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 清除常驻的COOKIE
        /// </summary>
        public static void ClearProvinceCookie()
        {
            System.Web.HttpCookie cookie = new System.Web.HttpCookie(ConfigurationManager.AppSettings["ProvinceName"]);
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.Domain = ConfigurationManager.AppSettings["LocalDomain"];
            HttpContext.Current.Response.Cookies.Add(cookie);
        }


        /// <summary>
        /// 清除常驻的COOKIE
        /// </summary>
        public static void ClearCookie()
        {
            System.Web.HttpCookie cookie = new System.Web.HttpCookie(ConfigurationManager.AppSettings["CookieName"]);
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.Domain = ConfigurationManager.AppSettings["LocalDomain"];
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        /// <summary>
        /// 重置COOKIE Message
        /// </summary>
        public static void ClearCookieMessage()
        {
            System.Web.HttpCookie cookie = HttpContext.Current.Request.Cookies[ConfigurationManager.AppSettings["CookieName"]];
            cookie.Values["Message"] = "0";
            cookie.Expires = DateTime.Now.AddDays(1);
            cookie.Domain = ConfigurationManager.AppSettings["LocalDomain"];
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public static string GetCookie(string cookieName, string key)
        {
            if (HttpContext.Current.Request.Cookies[cookieName] == null || HttpContext.Current.Request.Cookies[cookieName].Values[key] == null)
            {
                return string.Empty;
            }
            return CheckHelper.StrUrlDecode(HttpContext.Current.Request.Cookies[cookieName].Values[key]);
        }

        #endregion

        public static string UID
        {
            get
            {
                string _uid = GetCookie(ConfigurationManager.AppSettings["CookieName"], "uid");
                if (_uid == "") { return string.Empty; }
                if (_uid.Contains(",")) { ClearCookie(); return string.Empty; }
                if (!notModify()) { return string.Empty; }
                return CheckHelper.UnEscape(_uid).Replace("'", "").ToLower();
            }
        }

        public static int MemberID
        {
            get
            {
                int Uid;
                Int32.TryParse(UID, out Uid);
                return Uid;
            }
        }

        public static string NickName
        {
            get
            {
                if (!notModify()) { return string.Empty; }
                return CheckHelper.UnEscape(GetCookie(ConfigurationManager.AppSettings["CookieName"], "NickName"));
            }
        }

        public static string AvtarUrl
        {
            get
            {
                if (!notModify()) { return string.Empty; }
                return CheckHelper.UnEscape(GetCookie(ConfigurationManager.AppSettings["CookieName"], "AvtarUrl"));
            }
        }

        public static string GroupID
        {
            get
            {
                if (!notModify()) { return string.Empty; }
                return CheckHelper.UnEscape(GetCookie(ConfigurationManager.AppSettings["CookieName"], "GroupID"));
            }
        }

        public static string Email
        {
            get
            {
                if (!notModify()) { return string.Empty; }
                return CheckHelper.UnEscape(GetCookie(ConfigurationManager.AppSettings["CookieName"], "Email"));
            }
        }

        public static string Status
        {
            get
            {
                if (!notModify()) { return string.Empty; }
                return CheckHelper.UnEscape(GetCookie(ConfigurationManager.AppSettings["CookieName"], "Status"));
            }
        }

        public static string MemberType
        {
            get
            {
                if (!notModify()) { return string.Empty; }
                return CheckHelper.UnEscape(GetCookie(ConfigurationManager.AppSettings["CookieName"], "MT"));
            }
        }


        public static string PWD
        {
            get
            {
                if (!notModify()) { return string.Empty; }
                return CheckHelper.UnEscape(GetCookie(ConfigurationManager.AppSettings["CookieName"], "PWD"));
            }
        }

        public static string Message
        {
            get
            {
                if (!notModify()) { return string.Empty; }
                return CheckHelper.UnEscape(GetCookie(ConfigurationManager.AppSettings["CookieName"], "Message"));
            }
        }

        public static string MT
        {
            get
            {
                if (!notModify()) { return string.Empty; }
                return CheckHelper.UnEscape(GetCookie(ConfigurationManager.AppSettings["CookieName"], "MT"));
            }
        }
        public static bool IsLogin
        {
            get
            {
                string uid = UID;
                //如果UID为空,表示未登陆
                if (uid.Length <= 0)
                {
                    return false;
                }
                int Uid;
                Int32.TryParse(uid, out Uid);
                if (Uid > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool CheckPermission(string permission)
        {
            var permissionStr = MT.Split(',').ToList();

            if (permissionStr.Count <= 0)
            {
                return false;
            }
            var dic = new Dictionary<string, bool>();

            foreach (var pair in permissionStr)
            {
                var pairlist = pair.Split('|').ToList();
                var key = pairlist[0];
                var value = Convert.ToBoolean(pairlist[1]);
                dic.Add(key, value);
            }
            var result = dic.Single(x => x.Key == permission).Value;
            return result;
        }

        public static int GetDepartmentID()
        {
            var MessageStr = Message.Split(',').ToList();

            if (MessageStr.Count <= 0)
            {
                return 0;
            }
            var dic = new Dictionary<string, int>();

            foreach (var pair in MessageStr)
            {
                var pairlist = pair.Split('|').ToList();
                var key = pairlist[0];
                var value = Convert.ToInt32(pairlist[1]);
                dic.Add(key, value);
            }
            var result = dic.Single(x => x.Key == "DepartmentID").Value;
            return result;
        }

        public static string Province
        {
            get
            {
                var province = CheckHelper.UnEscape(GetCookie(ConfigurationManager.AppSettings["ProvinceName"], "province"));
                if (string.IsNullOrEmpty(province))
                {
                    SetProvinceCookie("quanguo");
                    province = "quanguo";
                }
                return province;
            }
        }
    }
}