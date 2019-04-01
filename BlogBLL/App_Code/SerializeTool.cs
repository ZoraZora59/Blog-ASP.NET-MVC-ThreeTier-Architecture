using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace NewBeeBlog.App_Code
{
    public class SerializeTool
    {
        public void Serialize<T>(object o )//序列化
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            string filePath = HttpContext.Current.Server.MapPath("~/config/blogconfig.xml");
            Stream s = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read);
            xs.Serialize(s, o);//将model序列化
            s.Close();//关闭流，防止被占用
        }

        public T DeSerialize<T>()//反序列化
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            string filePath = HttpContext.Current.Server.MapPath("~/config/blogconfig.xml");
            //获取配置文件的绝对路径
            Stream s = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            s.Position = 0;
            T t = (T)xs.Deserialize(s);
            s.Close();//关闭流
            return t;
        }
            

            
            
        }
    }
