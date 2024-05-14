using Microsoft.AspNetCore.Mvc;
using RecipesGlossary.Model;
using RecipesGlossary.Repository;

namespace RecipesGlossary.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly AuthorRepository _authorRepository;

        public AuthorController(AuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

       [HttpGet("{authorName}")]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetOtherRecipesByAuthor(string authorName )
        {
            var recipes = await _authorRepository.GetPublishedRecipesByAuthorNameAsync(authorName);
            return Ok(recipes);
        }

    }
}
