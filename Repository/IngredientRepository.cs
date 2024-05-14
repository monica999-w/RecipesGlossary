using Neo4j.Driver;
using RecipesGlossary.Model;

namespace RecipesGlossary.Repository
{
    public class IngredientRepository
    {
        private readonly IDriver _driver;

        public IngredientRepository(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<List<Ingredient>> GetAllIngredientsAsync()
        {
            var ingredients = new List<Ingredient>();
            IAsyncSession session = _driver.AsyncSession();
            try
            {
                IResultCursor cursor = await session.RunAsync(
                    @"MATCH (i:Ingredient)
                  RETURN i");

                await cursor.ForEachAsync(record =>
                {
                    var ingredientNode = record["i"].As<INode>();
                    var ingredient = new Ingredient
                    {
                        Name = ingredientNode["name"].As<string>()
                    };
                    ingredients.Add(ingredient);
                });

                return ingredients;
            }
            finally
            {
                await session.CloseAsync();
            }
        }
    }
}

