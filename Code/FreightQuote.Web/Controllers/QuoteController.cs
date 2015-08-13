using FreightQuote.Data;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.Data.Entity;
using System.Threading.Tasks;
namespace FreightQuote.Web.Controllers
{
    public class QuoteController : BaseController
    {
        //
        // GET: /Vendor/
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// get Vendor list
        /// </summary>
        /// <returns></returns>
        public ActionResult GetQuoteList()
        {
            return Json(db.Quotes.Select(x =>
                  new
                  {
                      QuoteId = x.QuoteId,
                      ReferenceNo = x.ReferenceNo,
                      PickupLocation = x.PickupLocation,
                      DeliveryLocation = x.DeliveryLocation,
                      ShipDate = x.ShipDate,
                      CreationDate = x.CreationDate,
                      Description = x.Description,
                      Comments = x.Comments,
                      VenderName = x.Vender != null ? x.Vender.Name : "",
                      VenderId = x.Vender != null ? x.Vender.VenderId.ToString() : "",
                      Status = x.Status
                  }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            ViewData["StatusList"] =
                new SelectList(new[] { "Open", "Send", "Received", "Shipped", "Completed" }
                .Select(x => new { value = x, text = x }),
                "value", "text", "Open");

            Quote quote = new Quote();
            quote.ShipDate = System.DateTime.Now;
            return View(quote);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Quote quote)
        {
            try
            {
                Quote oldQuote = db.Quotes.Where(x => x.ReferenceNo == quote.ReferenceNo).SingleOrDefault();
                if (oldQuote == null)
                {
                    quote.CreationDate = System.DateTime.Now;
                    db.Quotes.Add(quote);
                    db.SaveChanges();

                    MailMessage Msg = new MailMessage();

                    Msg.From = new MailAddress(ConfigurationManager.AppSettings["Email"]);

                    StreamReader reader = new StreamReader(Server.MapPath("~/SendMail.html"));
                    string readFile = reader.ReadToEnd();
                    string StrContent = "";
                    StrContent = readFile;
                    //Here replace the name with [MyName]
                    StrContent = StrContent.Replace("[Reference]", quote.ReferenceNo);
                    StrContent = StrContent.Replace("[PickUpLocation]", quote.PickupLocation);
                    StrContent = StrContent.Replace("[DeliveryLocation]", quote.DeliveryLocation);
                    StrContent = StrContent.Replace("[ShipDate]", quote.ShipDate.ToShortDateString());
                    StrContent = StrContent.Replace("[Description]", quote.Description);
                    StrContent = StrContent.Replace("[Comments]", quote.Comments);
                    Msg.Subject = string.Format("Request Quote – Reference# {0}", quote.ReferenceNo);
                    Msg.Body = StrContent.ToString();
                    Msg.IsBodyHtml = true;

                    // your remote SMTP server IP.
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = ConfigurationManager.AppSettings["MailServer"];
                    System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                    NetworkCred.UserName = ConfigurationManager.AppSettings["Email"];
                    NetworkCred.Password = ConfigurationManager.AppSettings["Password"];
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = int.Parse(ConfigurationManager.AppSettings["MailPort"]);
                    smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSSLEnabled"].ToString());

                    //List<Vender> vendorList = db.Venders.Where(x => x.IsActive == true).ToList();
                    //foreach (var item in vendorList)
                    //{                        
                    //    Msg.To.Add(item.Email);
                    //    smtp.Send(Msg);
                    //}

                    Msg.To.Add(new MailAddress(ConfigurationManager.AppSettings["ToEmail"]));
                    //smtp.Send(Msg);
                    await smtp.SendMailAsync(Msg);

                    return RedirectToAction("List");
                }
                else
                {
                    ViewData["StatusList"] =
              new SelectList(new[] { "Open", "Quote Send", "Quote Received", "Shipped", "Completed" }
              .Select(x => new { value = x, text = x }),
              "value", "text");

                    return View(quote);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Edit(int id)
        {
            Quote quote = db.Quotes.Where(x => x.QuoteId == id).SingleOrDefault();
            ViewData["StatusList"] =
                new SelectList(new[] { "Open", "Quote Send", "Quote Received", "Shipped", "Completed" }
                .Select(x => new { value = x, text = x }),
                "value", "text", quote.Status);

            ViewData["VenderList"] = new SelectList(db.Venders, "VendorId", "Name", quote.VenderId);
            return View(quote);
        }

        public ActionResult EditingInline_Update([DataSourceRequest] DataSourceRequest request, Quote quote)
        {
            db.Quotes.Attach(quote);
            var entry = db.Entry(quote);
            entry.State = EntityState.Modified;
            db.SaveChanges();

            return Json(ModelState.ToDataSourceResult(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(Quote quote)
        {
            db.Quotes.Attach(quote);
            var entry = db.Entry(quote);
            entry.State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("List");
        }

        public ActionResult Remove([DataSourceRequest] DataSourceRequest request, int QuoteId)
        {
            Quote quote = db.Quotes.Where(x => x.QuoteId == QuoteId).SingleOrDefault();
            db.Quotes.Remove(quote);
            db.SaveChanges();
            return Json(ModelState.ToDataSourceResult(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetVendors()
        {
            var vendorList = db.Venders.Select(x => new { Name = x.Name, VenderId = x.VenderId });
            return Json(vendorList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsReferenceNoAvailable(string referenceNo, string InitialReferenceNo)
        {
            if (referenceNo.ToLower() != InitialReferenceNo.ToLower())
            {
                Quote quote = db.Quotes.Where(x => x.ReferenceNo == referenceNo).SingleOrDefault();
                if (quote == null)
                    return Json(true, JsonRequestBehavior.AllowGet);
                else
                    return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
    }
}