using System;
using System.Collections.Generic;

namespace UPMS.Common
{
    public static class StringHelper
    {
        /// <summary>
        /// 将数字字符串转为decimal
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static decimal GetDecimal(this string strValue)
        {
            decimal reInt = 0;
            decimal.TryParse(strValue, out reInt);
            return reInt;
        }

        /// <summary>
        /// 将数字字符串转为int
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static int GetInt(this string strValue)
        {
            int reInt = 0;
            int.TryParse(strValue, out reInt);
            return reInt;
        }

        public static int GetInt(this object oValue)
        {
            int reInt = 0;
            try
            {
                reInt = Convert.ToInt32(oValue);
            }
            catch
            {
                reInt = 0;
            }
            return reInt;
        }

        /// <summary>
        /// 将字符串按照字符speater分割为List<string>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="speater"></param>
        /// <param name="toLower"></param>
        /// <returns></returns>
        public static List<string> GetStrList(this string str, char speater, bool toLower)
        {
            List<string> list = new List<string>();
            string[] ss = str.Split(speater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != speater.ToString())
                {
                    string strVal = s;
                    if (toLower)
                    {
                        strVal = s.ToLower();
                    }
                    list.Add(strVal);
                }

            }
            return list;
        }

        /// <summary>
        /// 将字符串按照,分割为数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] GetStrArray(this string str)
        {
            return str.Split(new char[] { ',' });
        }
    }
}
