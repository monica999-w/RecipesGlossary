using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;
using RecipesGlossary.Model;
using RecipesGlossary.Repository;
using System.Threading.Tasks;


namespace RecipesGlossary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly RecipeRepository _recipeRepository;

        public RecipeController(RecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetAllRecipes(int page = 1, int pageSize = 20)
        {
            var recipes = await _recipeRepository.GetPaginatedRecipesAsync(page, pageSize);
            return Ok(recipes);
        }


        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Recipe>>> SearchRecipesByName(string recipeName)
        {
            if (string.IsNullOrEmpty(recipeName))
            {
                return BadRequest("Recipe name cannot be empty.");
            }

            var recipes = await _recipeRepository.SearchRecipesByNameAsync(recipeName);
            return Ok(recipes);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Recipe>>> FilterRecipesByIngredient(string ingredientName)
        {
            if (string.IsNullOrEmpty(ingredientName))
            {
                return BadRequest("Ingredient name cannot be empty.");
            }

            var recipes = await _recipeRepository.FilterRecipesByIngredientAsync(ingredientName);
            return Ok(recipes);
        }
    }
}

