  using Microsoft.AspNetCore.Mvc;
    using RecipesGlossary.Model;
    using RecipesGlossary.Repository;
    using System.Collections.Generic;
    using System.Threading.Tasks;

namespace RecipesGlossary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IngredientRepository _ingredientRepository;

        public IngredientController(IngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ingredient>>> GetAllIngredients()
        {
            var ingredients = await _ingredientRepository.GetAllIngredientsAsync();
            return Ok(ingredients);
        }

       
        [HttpGet("{ingredientName}")]
        public async Task<ActionResult<Ingredient>> GetIngredientByName(string ingredientName)
        {
            var ingredients = await _ingredientRepository.GetAllIngredientsAsync();
            var ingredient = ingredients.FirstOrDefault(i => i.Name.ToLower() == ingredientName.ToLower());
            if (ingredient == null)
            {
                return NotFound();
            }
            return Ok(ingredient);
        }
    }

}
