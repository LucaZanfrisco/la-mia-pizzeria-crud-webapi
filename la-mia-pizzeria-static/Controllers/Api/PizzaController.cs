using la_mia_pizzeria_static.Database;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_static.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PizzaController : ControllerBase
    {
        private PizzeriaContext _db;

        public PizzaController(PizzeriaContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetPizzass()
        {
          using (_db)
            {
                List<Pizza> pizzas = _db.Pizzas.Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).ToList();
                
                return Ok(pizzas);
            }  
        }

        [HttpGet]
        public IActionResult GetPizzaByName(string? name)
        {
            if(name == null)
            {
                return BadRequest(new {Mesage = "Nessun parametro di ricerca inserito"});
            }

            using(_db)
            {
                List<Pizza> foundedPizza = _db.Pizzas
                    .Where(pizza => pizza.Name.ToLower().Contains(name.ToLower()))
                    .Include(pizza => pizza.Category)
                    .Include(pizza => pizza.Ingredients)
                    .ToList();

                return Ok(foundedPizza);
            }
        }

        [HttpGet]
        public IActionResult GetPizzaById(int id)
        {
            using(_db)
            {
                Pizza foundedPizza = _db.Pizzas
                    .Where(pizza => pizza.Id == id)
                    .Include(pizza => pizza.Category)
                    .Include(pizza => pizza.Ingredients)
                    .FirstOrDefault();

                if(foundedPizza == null)
                    return NotFound(new {Message = "Nessun risultato trovato"});
                
                return Ok(foundedPizza);
            }
        }

    }
}
