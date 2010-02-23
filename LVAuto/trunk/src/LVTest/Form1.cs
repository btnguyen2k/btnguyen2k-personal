using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Collections;
using System.IO;

namespace LVTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://login.linhvuong.zooz.vn/login.aspx?server=7");
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)";
            request.Referer = "http://linhvuong.zooz.vn/news/";
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream input = response.GetResponseStream();
            MemoryStream memStream = new MemoryStream();
            byte[] buff = new byte[1024];
            int bytesRead = input.Read(buff, 0, 1024);
            while ( bytesRead > 0 ) {
                bytesRead = input.Read(buff, 0, 1024);
                memStream.Write(buff, 0, bytesRead);
            }
            string str = System.Text.Encoding.UTF8.GetString(memStream.ToArray());
            Hashtable result = LVAuto.LVWeb.ParseHeader.Parse(str, false);
            textLog.AppendText(str);
        }

        public static Hashtable LoginForm()
        {
            string header = "GET http://login.linhvuong.zooz.vn/login.aspx?server=" + 7 + " HTTP/1.1\n";
            header += "Host: login.linhvuong.zooz.vn\n";
            header += "User-Agent: Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)\n";
            header += "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8\n";
            header += "Accept-Language: en-us,en;q=0.5\n";
            //header += "Accept-Encoding: gzip,deflate\n";
            header += "Accept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.7\n";
            header += "Keep-Alive: 0\n";
            header += "Proxy-Connection: close\n";
            header += "Referer: http://linhvuong.zooz.vn/news/\n";
            header += "Content-Type: application/x-www-form-urlencoded\n";
            header += "\n";
            return null;
        }
    }
}
