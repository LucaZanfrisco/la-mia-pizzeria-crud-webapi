using la_mia_pizzeria_static.Database;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Mvc;

namespace la_mia_pizzeria_static.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            using(PizzeriaContext db = new PizzeriaContext())
            {
                List<Pizza> pizzas = db.Pizzas.ToList();
                return View("Index", pizzas);
            }
            
        }
    }
}
