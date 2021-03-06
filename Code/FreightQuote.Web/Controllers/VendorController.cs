﻿using FreightQuote.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FreightQuote.Web.Controllers
{
    public class VendorController : BaseController
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
        public ActionResult GetVendorList()
        {
            return Json(db.Venders.Select(x =>
                  new
                  {
                      VendorId = x.VenderId,
                      Name = x.Name,
                      Address = x.Address,
                      PhoneNumber = x.PhoneNumber,
                      Email = x.Email,
                      IsActive = x.IsActive
                  }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateVender(int venderId, bool IsActive)
        {
            Vender vender = db.Venders.Where(x => x.VenderId == venderId).SingleOrDefault();
            vender.IsActive = IsActive;
            db.SaveChanges();
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}