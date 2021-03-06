﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Collections.Specialized;

namespace PathToSuccess.PhoneSync
{
    public static class InfoSender
    {
        public static string Url { get; private set; }
        public static readonly string FilePath = "WebServerUrl.txt";
        public static void Initialize()
        {
            GetUrlFromFile();
        }

        private static string ToJson(Models.TimeBinding tb)
        {
            string json = "";

            json += "{" +
                "\"description\" : \"" + tb.Step.Description + "\",\n" +
                "\"importance\" : \"" + tb.Step.ImportanceName + "\",\n" +
                "\"start_time\" : \"" + tb.Time.ToShortTimeString() + "\",\n" +
                "\"end_time\" : \"" + tb.Time.AddHours(1).ToShortTimeString() + "\",\n" +
                "\"id\" : \"" + tb.Id
                + "\"\n}";

            return json;
        }

        private static string GetTimeTableJson()
        {
            string json = "";

            json +=
                "{\"days\" : [\n";

            DateTime dateCounter = DateTime.Now;
            string day = "";
            for (int i = 0; i < 30; i++)
            {
                var tbs = Models.TimeBinding.GetTBofDay(dateCounter.Day, dateCounter.Month, dateCounter.Year,BL.Application.CurrentTree);
                day = "";
   
                day += "{";
                day += "   \"day\" : \"" + dateCounter.Day.ToString() + "\",\n" +
                    "   \"month\" : \"" + dateCounter.Month.ToString() + "\",\n" +
                    "   \"year\" : \"" + dateCounter.Year.ToString() + "\",\n" +
                    "   \"steps\" : [\n";

                foreach (var tb in tbs)
                {
                    day += "      " + ToJson(tb) + ",";
                }

                if (day.Last() == ',')
                    day = day.Remove(day.Length - 1); //remove redundant comma

                day += "   ]\n";
                day += "},";

                json += day;
                dateCounter = dateCounter.AddDays(1);
            }

            if (json.Last() == ',')
                json = json.Remove(json.Length - 1); //remove redundant comma

            json += "]\n}";

            return json;
        }

        public static bool Send()
        {
            try
            {
                TrySend();
            }
            catch
            {
                BL.Log.Add("Couldn't connect to server at" + DateTime.Now.ToString());
                return false;
            }
            return true;
        }

        private static void TrySend()
        {
            string json = GetTimeTableJson();
            var request = (HttpWebRequest)WebRequest.Create(Url);
            request.ContentType = "text/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            string result = "";
            using (WebClient wc = new WebClient())
            {
                //var postData = new NameValueCollection()
                //{
                //    {"user", "stub"},
                //    {"password_hash", "0"},
                //    {"jsondata", json },
                //    {"referer", "desktop"}
                //};
                var postData = new NameValueCollection()
                {
                    {"user", BL.Application.CurrentUser.Login},
                    {"password_hash", BL.Application.CurrentUser.Password.ToString()},
                    {"jsondata", json },
                    {"referer", "desktop"}
                };
                result = Encoding.UTF8.GetString(wc.UploadValues(Url, postData));
            }
            

            //var response = (HttpWebResponse)request.GetResponse();
            //string result = "";
            //using (var streamReader = new StreamReader(response.GetResponseStream()))
            //{
            //    result = streamReader.ReadToEnd();
            //}

            PathToSuccess.BL.Log.Add(result);
        }

        private static void GetUrlFromFile()
        {
            var file = File.Open(FilePath, FileMode.Open);
            var bytes = new byte[1024];
            file.Read(bytes, 0, (int)file.Length);
            Url = System.Text.Encoding.UTF8.GetString(bytes);
            Url = Url.Trim('\0');
        }
    }
}
