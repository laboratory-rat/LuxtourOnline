using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuxtourOnline.Utilites
{
    public static class AppRandom
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

    public static class AppConsts
    {
        public static List<string> ImageExtensions = new List<string>() { "png", "jpg", "jpeg" };
        public static List<string> Langs = new List<string>() { "en", "uk", "ru" };
    }
}