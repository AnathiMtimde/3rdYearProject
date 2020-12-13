using DGSappSem2s.Models;
using PayFast;
using PayFast.AspNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DGSappSem2.Models;

namespace DGSappSem2.Controllers
{
    public class RegistrationPayment : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Payment
        public ActionResult Index()
        {
            return View();
        }
    }
}