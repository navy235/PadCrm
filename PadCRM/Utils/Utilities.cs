using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.IO;
using System.Globalization;
using System.Web.UI;

namespace PadCRM.Utils
{
    public class Utilities
    {

        public static IList<SelectListItem> GetSelectListData<T>(IEnumerable<T> entities, Func<T, object> funcToGetValue, Func<T, object> funcToGetText, bool addDefaultSelectItem = true, bool defaultValueisZero = false)
        {
            var eList = entities
                   .Select(x => new SelectListItem
                   {
                       Value = funcToGetValue(x).ToString(),
                       Text = funcToGetText(x).ToString()
                   }).ToList();

            if (addDefaultSelectItem)
                eList.Insert(0, new SelectListItem { Selected = true, Text = "请选择", Value = defaultValueisZero ? "0" : "" });

            return eList;
        }

        public static IList<SelectListItem> GetSelectListData<T>(IEnumerable<T> entities, Func<T, object> funcToGetValue, Func<T, object> funcToGetText, List<int> SeletdValues, bool addDefaultSelectItem = true)
        {
            var list = GetSelectListData(entities, funcToGetValue, funcToGetText, addDefaultSelectItem);

            foreach (var item in list)
            {
                if (SeletdValues.Contains(Convert.ToInt32(item.Value)))
                {
                    item.Selected = true;
                }
            }
            return list;
        }

        public static IList<SelectListItem> GetSelectListData<T>(IEnumerable<T> entities, Func<T, object> funcToGetValue, Func<T, object> funcToGetText, int value, bool addDefaultSelectItem = true)
        {
            var list = GetSelectListData(entities, funcToGetValue, funcToGetText, addDefaultSelectItem);

            foreach (var item in list)
            {
                if (item.Value != "")
                {

                    if (value == Convert.ToInt32(item.Value))
                    {
                        item.Selected = true;
                    }
                }
            }
            return list;
        }


        public static SelectList CreateSelectList<T>(IEnumerable<T> entities, Func<T, object> funcToGetValue, Func<T, object> funcToGetText, bool addDefaultSelectItem = true)
        {
            return new SelectList(GetSelectListData(entities, funcToGetValue, funcToGetText, addDefaultSelectItem), "Value", "Text");

        }

        public static string GetInnerMostException(Exception ex)
        {
            return ex.GetBaseException().Message;
        }

        public static List<int> GetIdList(string Ids)
        {
            var list = new List<int>();
            if (!string.IsNullOrEmpty(Ids))
            {
                list = Ids.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            }
            return list;
        }

        public static int GetCascadingId(string Ids)
        {
            int id = 0;
            if (!string.IsNullOrEmpty(Ids))
            {
                id = Convert.ToInt32(Ids.Split(',').Last());
            }
            return id;
        }

        public static int GetMaxCode(int code, int level)
        {
            var codeStr = code.ToString();
            var maxLength = codeStr.Length;
            codeStr = codeStr.Substring(0, (level + 1) * 2);
            var needLength = maxLength - codeStr.Length;
            if (needLength > 0)
            {
                for (var i = 0; i < needLength; i++)
                {
                    codeStr += "9";
                }
            }
            return Convert.ToInt32(codeStr);
        }
        public static int GetRootCode(int code, int level)
        {
            var codeStr = code.ToString();
            var maxLength = codeStr.Length;
            codeStr = codeStr.Substring(0, 2);
            var needLength = maxLength - codeStr.Length;
            if (needLength > 0)
            {
                for (var i = 0; i < needLength; i++)
                {
                    codeStr += "0";
                }
            }
            return Convert.ToInt32(codeStr);
        }


        public static int GetMaxCode(int code)
        {
            var codeStr = code.ToString();
            var maxLength = codeStr.Length;
            var lastNotEquelZeroIndex = 0;
            for (var i = maxLength - 1; i >= 0; i--)
            {
                if (Convert.ToInt32(codeStr.Substring(i, 1)) != 0)
                {
                    lastNotEquelZeroIndex = i;
                    break;
                }
            }
            if (lastNotEquelZeroIndex % 2 == 0)
            {
                lastNotEquelZeroIndex += 1;
            }
            if (lastNotEquelZeroIndex < maxLength - 1)
            {
                codeStr = codeStr.Substring(0, lastNotEquelZeroIndex + 1);
                var needLength = maxLength - lastNotEquelZeroIndex - 1;
                for (var i = 0; i < needLength; i++)
                {
                    codeStr += "9";
                }
            }
            return Convert.ToInt32(codeStr);
        }

        public static string RenderPartialToString(ControllerContext context, string partialViewName, ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            var viewEngineResult = ViewEngines.Engines.FindPartialView(context, partialViewName);

            if (viewEngineResult.View != null)
            {
                var stringBuilder = new StringBuilder();
                using (var stringWriter = new StringWriter(stringBuilder))
                {
                    using (var output = new HtmlTextWriter(stringWriter))
                    {
                        ViewContext viewContext = new ViewContext(context, viewEngineResult.View, viewData, tempData, output);
                        viewEngineResult.View.Render(viewContext, output);
                    }
                }

                return stringBuilder.ToString();
            }

            //return string.Empty;
            throw new FileNotFoundException("The view cannot be found", partialViewName);
        }

        public static DateTime GetDataFromChineseDate(int year, int month, int day, bool isLeap)
        {
            ChineseCalendarInfo ccInfo = ChineseCalendarInfo.FromLunarDate(year, month, day, isLeap);
            return ccInfo.SolarDate;
        }

        public static string ConvertToChineseYearStyle(int year)
        {
            var strNum = "〇一二三四五六七八九十";
            var yearStr = year.ToString();
            var result = string.Empty;
            for (var i = 0; i < yearStr.Length; i++)
            {
                var index = Convert.ToInt32(yearStr.Substring(i, 1));
                result += strNum.Substring(index, 1);
            }
            return result + "年";
        }

        public static string ConvertToChineseMonthStyle(int Month, bool isRunYue = false)
        {
            var szText = "正二三四五六七八九十";
            var strMonth = string.Empty;
            if (Month <= 10)
            {
                strMonth = szText.Substring(Month - 1, 1);
            }
            else if (Month == 11) strMonth = "十一";
            else strMonth = "十二";
            if (isRunYue)
            {
                strMonth = "润" + strMonth;
            }
            return strMonth + "月";
        }


        public static string ConvertToChineseDayStyle(int Day)
        {
            var dayStr = string.Empty;
            var szText1 = "初十廿三";
            var szText2 = "一二三四五六七八九十";
            if ((Day != 20) && (Day != 30))
            {
                dayStr = szText1.Substring((Day - 1) / 10, 1) + szText2.Substring((Day - 1) % 10, 1);
            }
            else if (Day != 20)
            {
                dayStr = szText1.Substring(Day / 10, 1) + "十";
            }
            else
            {
                dayStr = "二十";
            }
            return dayStr;
        }


        /// <summary>
        /// 把由2013/1/1格式表示的日期转成二〇一三年正月初一
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public static string GetLunarString(DateTime time)
        {
            string str = ConvertToChineseYearStyle(time.Year) +
                ConvertToChineseMonthStyle(time.Month) +
                ConvertToChineseDayStyle(time.Day);
            return str;
        }

        /// <summary>
        /// 把由2013/1/1格式表示的日期转成正月初一
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public static string GetLunarStringOnlyMonthDay(DateTime time)
        {
            string str = ConvertToChineseMonthStyle(time.Month) +
                ConvertToChineseDayStyle(time.Day);
            return str;
        }


        public static string GetCurrentLunarYear()
        {
            ChineseLunisolarCalendar clc = new System.Globalization.ChineseLunisolarCalendar();
            string Luneryear = ConvertToChineseYearStyle(clc.GetYear(DateTime.Now));
            return Luneryear;
        }

        public static string NoPermissionStr = "<script type='text/javascript'>alert('你没有权限访问该页面');window.history.go(-1);</script>";


        public static string GetCurrentDayString()
        {

            var szText = "日一二三四五六";
            var time = DateTime.Now;
            return time.Year + "年" + time.Month + "月" + time.Day + "日星期" + szText.Substring((int)time.DayOfWeek, 1);
        }

        public static int GetMonthDayCount(int year, int month)
        {
            var list = new Dictionary<int, int>();
            list.Add(1, 31);
            list.Add(2, 28);
            list.Add(3, 31);
            list.Add(4, 30);
            list.Add(5, 31);
            list.Add(6, 30);
            list.Add(7, 31);
            list.Add(8, 31);
            list.Add(9, 30);
            list.Add(10, 31);
            list.Add(11, 30);
            list.Add(12, 31);
            var day = list.First(x => x.Key == month).Value;
            if (year % 4 == 0)
            {
                day++;
            }
            return day;
        }

      
    }
}