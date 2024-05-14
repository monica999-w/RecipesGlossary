using Neo4j.Driver;
using RecipesGlossary.Controllers;
using RecipesGlossary.Model;
using Neo4j.Driver.Internal.Types;

namespace RecipesGlossary.Repository
{

    public class RecipeRepository
    {
        private readonly IDriver _driver;

        public RecipeRepository(IDriver driver)
        {
            _driver = driver;
        }


        public async Task<List<Recipe>> GetPaginatedRecipesAsync(int page, int pageSize)
        {
            var recipes = new List<Recipe>();
            IAsyncSession session = _driver.AsyncSession();
            try
            {
                int skip = (page - 1) * pageSize;
                IResultCursor cursor = await session.RunAsync(
                    $@"MATCH (r:Recipe)
               RETURN r.id as id, r.name as name, r.skillLevel as skillLevel,r.description as description,r.preparationTime as preparationTime,r.cookingTime as cookingTime
               ORDER BY r.name ASC
               SKIP $skip
               LIMIT $limit",
                    new { skip, limit = pageSize });

               
                var records = await cursor.ToListAsync();

                foreach (var record in records)
                {
                    var recipe = new Recipe
                    {
                        Id = record["id"].As<string>(),
                        Name = record["name"].As<string>(),
                        SkillLevel = record["skillLevel"].As<string>(),
                        Description = record["description"].As<string>(),
                        PreparationTime = record["preparationTime"].As<int>(),
                        CookingTime = record["cookingTime"].As<int>(),
                        Ingredients = new List<Ingredient>(),
                        Author = null 
                    };

                    
                    var ingredientsCursor = await session.RunAsync(
                        $@"MATCH (r:Recipe)-[:CONTAINS_INGREDIENT]->(i:Ingredient)
                   WHERE r.id = $recipeId
                   RETURN i.name as name",
                        new { recipeId = recipe.Id });

                    var ingredientsRecords = await ingredientsCursor.ToListAsync();
                    foreach (var ingredientRecord in ingredientsRecords)
                    {
                        var ingredient = new Ingredient
                        {
                            Name = ingredientRecord["name"].As<string>()
                        };
                        recipe.Ingredients.Add(ingredient);
                    }

                   
                    var authorCursor = await session.RunAsync(
                        $@"MATCH (r:Recipe)<-[:WROTE]-(a:Author)
                   WHERE r.id = $recipeId
                   RETURN a.name as name",
                        new { recipeId = recipe.Id });

                    var authorRecords = await authorCursor.ToListAsync();
                    if (authorRecords.Any())
                    {
                        recipe.Author = new Author
                        {
                            Name = authorRecords.First()["name"].As<string>()
                        };
                    }

                    recipes.Add(recipe);
                }

                return recipes;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

 
        public async Task<List<Recipe>> SearchRecipesByNameAsync(string recipeName)
        {
            var recipes = new List<Recipe>();
            IAsyncSession session = _driver.AsyncSession();
            try
            {
                IResultCursor cursor = await session.RunAsync(
                    $@"MATCH (r:Recipe)
                   WHERE toLower(r.name) CONTAINS toLower('{recipeName}')
                   RETURN r.id as id, r.name as name, r.description as description, r.preparationTime as preparationTime, r.cookingTime as cookingTime, r.skillLevel as skillLevel");

                var records = await cursor.ToListAsync();

                foreach (var record in records)
                {
                    var recipe = new Recipe
                    {
                        Id = record["id"].As<string>(),
                        Name = record["name"].As<string>(),
                        SkillLevel = record["skillLevel"].As<string>(),
                        Description = record["description"].As<string>(),
                        PreparationTime = record["preparationTime"].As<int>(),
                        CookingTime = record["cookingTime"].As<int>(),
                        Ingredients = new List<Ingredient>(),
                        Author = null
                    };
                    
                    var ingredientsCursor = await session.RunAsync(
                        $@"MATCH (r:Recipe)-[:CONTAINS_INGREDIENT]->(i:Ingredient)
                   WHERE r.id = $recipeId
                   RETURN i.name as name",
                        new { recipeId = recipe.Id });

                    var ingredientsRecords = await ingredientsCursor.ToListAsync();
                    foreach (var ingredientRecord in ingredientsRecords)
                    {
                        var ingredient = new Ingredient
                        {
                            Name = ingredientRecord["name"].As<string>()
                        };
                        recipe.Ingredients.Add(ingredient);
                    }

                    
                    var authorCursor = await session.RunAsync(
                        $@"MATCH (r:Recipe)<-[:WROTE]-(a:Author)
                   WHERE r.id = $recipeId
                   RETURN a.name as name",
                        new { recipeId = recipe.Id });

                    var authorRecords = await authorCursor.ToListAsync();
                    if (authorRecords.Any())
                    {
                        recipe.Author = new Author
                        {
                            Name = authorRecords.First()["name"].As<string>()
                        };
                    }
                    recipes.Add(recipe);
                };

                return recipes;
            }
            finally
            {
                await session.CloseAsync();
            }
        }


        public async Task<List<Recipe>> FilterRecipesByIngredientAsync(string ingredientName)
        {
            var recipes = new List<Recipe>();
            IAsyncSession session = _driver.AsyncSession();
            try
            {
                IResultCursor cursor = await session.RunAsync(
                    $@"MATCH (r:Recipe)-[:CONTAINS_INGREDIENT]->(i:Ingredient {{name: '{ingredientName}'}})
                    RETURN r.id as id, r.name as name, r.description as description, r.preparationTime as preparationTime, r.cookingTime as cookingTime, r.skillLevel as skillLevel");

                var records = await cursor.ToListAsync();
                foreach (var record in records)
                {
                    var recipe = new Recipe
                    {
                        Id = record["id"].As<string>(),
                        Name = record["name"].As<string>(),
                        SkillLevel = record["skillLevel"].As<string>(),
                        Description = record["description"].As<string>(),
                        PreparationTime = record["preparationTime"].As<int>(),
                        CookingTime = record["cookingTime"].As<int>(),
                        Ingredients = new List<Ingredient>(),
                        Author = null
                    };
                    var ingredientsCursor = await session.RunAsync(
                        $@"MATCH (r:Recipe)-[:CONTAINS_INGREDIENT]->(i:Ingredient)
                   WHERE r.id = $recipeId
                   RETURN i.name as name",
                        new { recipeId = recipe.Id });

                    var ingredientsRecords = await ingredientsCursor.ToListAsync();
                    foreach (var ingredientRecord in ingredientsRecords)
                    {
                        var ingredient = new Ingredient
                        {
                            Name = ingredientRecord["name"].As<string>()
                        };
                        recipe.Ingredients.Add(ingredient);
                    }

                    
                    var authorCursor = await session.RunAsync(
                        $@"MATCH (r:Recipe)<-[:WROTE]-(a:Author)
                   WHERE r.id = $recipeId
                   RETURN a.name as name",
                        new { recipeId = recipe.Id });

                    var authorRecords = await authorCursor.ToListAsync();
                    if (authorRecords.Any())
                    {
                        recipe.Author = new Author
                        {
                            Name = authorRecords.First()["name"].As<string>()
                        };
                    }
                    recipes.Add(recipe);
                };

                return recipes;
            }
            finally
            {
                await session.CloseAsync();
            }
        }


    }
}
