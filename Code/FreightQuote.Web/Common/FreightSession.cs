using FreightQuote.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreightQuote.Web
{
    public class FreightSession
    {
        public FreightSession()
        { 

        }

        public static FreightSession Current
        {
            get
            {
                FreightSession session = (FreightSession)HttpContext.Current.Session["_FreigtSession_"];
                if (session == null)
                {
                    session = new FreightSession();
                    HttpContext.Current.Session["_FreigtSession_"] = session;
                }
                return session;
            } 
        
        }
        public User User { get; set; }

    }
}