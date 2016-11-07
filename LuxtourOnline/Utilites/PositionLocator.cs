using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;

namespace LuxtourOnline.Utilites
{
    public static class LocationMaster
    {
        public static readonly string Site = "http://www.freegeoip.net/xml/";

        public static string GetIp()
        {
            string ip = HttpContext.Current.Request.UserHostAddress;
            return ip;
        }

        public static LocationModel GetLocation(string ip)
        {
            try
            {
                XmlDocument doc = new XmlDocument();

                string getdetails = Site + ip;

                doc.Load(getdetails);

                return new LocationModel(doc);
            }
            catch
            {
                return null;
            }
        }

    }

    public class LocationModel
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }

        public string City { get; set; }
        public string TimeZone { get; set; }

        public LocationModel()
        {

        }

        public LocationModel(XmlDocument doc)
        {
            XmlNodeList cc = doc.GetElementsByTagName("CountryCode");
            CountryCode = cc[0].InnerText;

            XmlNodeList cn = doc.GetElementsByTagName("CountryName");
            CountryName = cn[0].InnerText;

            XmlNodeList rc = doc.GetElementsByTagName("RegionCode");
            RegionCode = rc[0].InnerText;

            XmlNodeList rn = doc.GetElementsByTagName("RegionName");
            RegionName = rn[0].InnerText;

            XmlNodeList city = doc.GetElementsByTagName("City");
            City = city[0].InnerText;

            XmlNodeList tz = doc.GetElementsByTagName("TimeZone");
            TimeZone = tz[0].InnerText;
        }
    }
}