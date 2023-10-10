using la_mia_pizzeria_static.Database;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Serialization;

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
        public IActionResult GetPizzas()
        {
            using(_db)
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
                return BadRequest(new { Mesage = "Nessun parametro di ricerca inserito" });
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

        [HttpGet("{id}")]
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
                    return NotFound(new { Message = "Nessun risultato trovato" });

                return Ok(foundedPizza);
            }
        }

        [HttpPost]
        public IActionResult AddPizza([FromBody] Pizza newPizza)
        {
            using(_db)
            {
                _db.Pizzas.Add(newPizza);
                _db.SaveChanges();

                return Ok();
            }
        }

        [HttpPut("{id}")]
        public IActionResult EditPizza(int id, [FromBody] Pizza editPizza)
        {
            using(_db)
            {
                Pizza foundedPizza = _db.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();
                if(foundedPizza != null)
                {
                    foundedPizza.Name = editPizza.Name;
                    foundedPizza.Description = editPizza.Description;
                    foundedPizza.Image = editPizza.Image;
                    foundedPizza.Price = editPizza.Price;
                    foundedPizza.CategoryId = editPizza.CategoryId;

                    if(editPizza.Ingredients != null)
                    {
                        if(foundedPizza.Ingredients != null)
                        {
                            foundedPizza.Ingredients.Clear();
                        }
                        else
                        {
                            foundedPizza.Ingredients = new List<Ingredient>();
                        }

                        foreach(Ingredient ingredient in editPizza.Ingredients)
                        {
                            foundedPizza.Ingredients.Add(ingredient);
                        }

                    }
                    
                    _db.SaveChanges();

                    return Ok("Pizza modificata con successo");
                }
                return BadRequest(new {Message = "Parametro di ricerca errato"});
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePizza(int id)
        {
            using(_db)
            {
                Pizza deletePizza = _db.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

                if(deletePizza != null)
                {
                    _db.Pizzas.Remove(deletePizza);
                    _db.SaveChanges();

                    return Ok();
                }

                return BadRequest(new { Message = "Parametro di ricerca errato" });
            }
        }
    }
}
