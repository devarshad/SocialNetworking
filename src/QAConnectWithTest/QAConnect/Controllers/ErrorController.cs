﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QAConnect.Controllers
{
    [AllowAnonymous]
    public class ErrorController : BaseController
    {
        public ActionResult BadRequest(string message)
        {
            return View(message);
        }

        public ActionResult Forbidden(string message)
        {
            return View(message);
        }

        public ActionResult Index(string message)
        {
            return View(message);
        }

        public ActionResult PageNotFound(string message)
        {
            return View(message);
        }

        public ActionResult ServerError(string message)
        {
            return View(message);
        }

        public ActionResult Unauthorized(string message)
        {
            return View(message);
        }
    }
}