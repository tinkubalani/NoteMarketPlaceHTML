using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;

namespace NotesMarketplace.Password_Encryption
{
    public class CheckInternet
    {
        public static bool IsConnectedToInternet()
        {
            string host = "smtp.gmail.com";
            bool result = false;
            Ping p = new Ping();
            try
            {
                PingReply reply = p.Send(host, 587);
                if (reply.Status == IPStatus.Success)
                    return true;
            }
            catch { }
            return result;
        }
    }
}