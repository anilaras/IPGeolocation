using IPLocator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IPLocator.Controllers
{
    public class PlanController : Controller
    {

        public PlanController()
        {
        }

        public IActionResult Index()
        {
           return View(); 
        }
       
    }
}
