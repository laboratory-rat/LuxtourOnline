using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace LuxtourOnline.Utilites
{


    public static class IdGenerator
    {
        static readonly string[] Salts = 
        {
            "QCXvDzWCIK2DLOtgpUKK",
            "UTpV5AF3c9rEXpakExcp",
            "kurDvMxBb8RTzpHfQVWj",
            "RW1EUv5igWYe65r3aF9u",
            "lTxQvFFVn8FGVrnZEKDv",
            "nvTNwQ8h7yqJF7X1ABah",
            "1Fc6h5RDwXL9XQv6XlMw",
            "okMy6T5jaRp9tti05Rv7",
            "Yx9CtkPn7BxOmHoJ9haU",
            "PvuRoxTFfM6toCRZkMPL",
            "UDo4LpYTJi52M9A6S0Go",
            "USi1GSg5uovgfhgladZG",
            "YhNXyo4VeXG6az2cJF1v",
            "ouTbBq9MKQAt687VITvg",
            "z8ucIChBMQB7tOmrFaJV",
            "BuNWf9m1CyHn2BN6NGzG",
            "dqdyalpeOMhMxrYlAP8j",
            "KbYIhsGQe1QTFMZgAhVu",
            "l7HsTxz1SU8ULqZZXaLI",
            "d8NDyEdVbVyg9xM5asLf",
            "UgshCyfR7e5Uhltawa1q",
            "AlRZcIwY0NxBn0ZkQTuD",
            "NjykUKPhUnEHg3JSYyIV",
            "6ssdpSu8W4m97xDuONhd",
            "alg4alnu2Ac437IaKqW8",
            "fbUysdu5DFQmI21jwde2",
            "Gwz44cdPyMpKke2cARIR",
            "bFO5aVHLm9FiNnOBzJHv",
            "OJRowEe5AH2kOltpwtDZ",
            "OpauI7Xn2JmdB29j2icE",
            "W0McQ8xwtKcxn4PNFXLI",
            "ehvKFgiRmlvhUJc6ZWTy",
            "qS2JVOuP8ilNyoGPoMts",
            "527HjGl2sQmylpROPFgg",
            "iDKvIxCke5ps3SSAZOQT",
            "NgqmsTFBsNvMGRovu4P8",
            "YxTZ0J3uPycWNkz8Cnnn",
            "qASTYXlYFMnDGEei3qeX",
            "zIdHm80Lp1iiJKAMgNYc",
            "ZVC6ejuuNKPnfbHyHF2I",
            "YPnBv92IYP5VSXQyzmG8",
            "JrsrOKbbMSjGUjaJfJlY",
            "jKxjKyfokuIDG16k8KJS",
            "4WI3yLOGt2iOjXfoC3EG",
            "A2b8Ky74h7DzqOT4qp5i",
            "38ZBD8Df6nCQPFLg9uTp",
            "AeOnrRFrrroZSfq8KRK9",
            "C3dmgisgRY3hidUp3yHE",
            "XFxxrDsYtjRpeW1Qzugp",
            "Zkp4dhh0ZbNftzqUXvDL",
            "mXQ6YBEMffR5dp9UZU8V",
            "v9G7SoTdICzl1wfxlAnI",
            "G0ia4pMu5m11WLZ0MkxH",
            "HmomqXkBGiZSUI6rX7Fo",
            "rkC00ydxx0h1c3r0uAna",
            "tG1Cxbv7fdWw6cUw1mJE",
            "ENJWNq84lpTdmT5M4UmF",
            "JBY8qsaDSa8OmTSDgLy6",
            "ggrbbFd9d0loYMJjPueH",
            "yJyqoohLJfHtwnReYCZ7",
            "CYkg8foqVomX0OpslSop",
            "ADOWqu4ZEqICXVbjPVzc",
            "iF50VJX5nhap5qfnQv1F",
            "R3nqeygqH5XOr10kmiZK",
        };

        public static string GenerateIdMd5()
        {
            var rand = new Random();
            int index = rand.Next(0, Salts.Length - 1);

            return GenerateIdMd5(Salts[index]);
        }

        public static string GenerateIdMd5(string salt)
        {
            string baseData = DateTime.Now.Ticks.ToString() + salt;

            return baseData.GetHashCode().ToString();
        }

        public static string GenerateId()
        {
            var rand = new Random();
            return GenerateId(Salts[rand.Next(0, Salts.Length - 1)]);
        }

        public static string GenerateId(string salt)
        {

            string now = DateTime.Now.Ticks.ToString();
            string text = $"{now}{salt}{now}";

            byte[] bytes = Encoding.Unicode.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString.ToLower();
        }
    }
}