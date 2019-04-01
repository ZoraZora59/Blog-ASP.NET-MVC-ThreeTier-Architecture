using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace NewBeeBlog.App_Code
{
    public class md5tool
    {
        public static string GetMD5(string mystring)
        {
            //MD5加密算法
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.Unicode.GetBytes(mystring);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2string = null;
            for(int i = 0; i < targetData.Length; i++)
            {
                byte2string += targetData[i].ToString();
            }
            return byte2string;
        }
    }
}