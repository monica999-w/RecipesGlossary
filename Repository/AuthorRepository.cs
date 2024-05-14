using Microsoft.AspNetCore.Http;
using Neo4j.Driver;
using RecipesGlossary.Model;

namespace RecipesGlossary.Repository
{
    public class AuthorRepository
    {
        private readonly IDriver _driver;

        public AuthorRepository(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<List<Author>> GetAllAuthorsAsync()
        {
            var authors = new List<Author>();
            IAsyncSession session = _driver.AsyncSession();
            try
            {
                IResultCursor cursor = await session.RunAsync(
                    @"MATCH (a:Author)
                  RETURN a");

                await cursor.ForEachAsync(record =>
                {
                    var authorNode = record["a"].As<INode>();
                    var author = new Author
                    {
                        Name = authorNode["name"].As<string>()
                    };
                    authors.Add(author);
                });

                return authors;
            }
            finally
            {
                await session.CloseAsync();
            }


        }
        public async Task<IEnumerable<string>> GetPublishedRecipesByAuthorNameAsync(string authorName)
        {
            var recipes = new List<string>();
            IAsyncSession session = _driver.AsyncSession();
            try
            {
                var result = await session.RunAsync(
                    @"MATCH (a:Author)-[:WROTE]->(r:Recipe)
              WHERE a.name = $authorName
              RETURN r.name as recipeName",
                    new { authorName });

                await result.ForEachAsync(record =>
                {
                    recipes.Add(record["recipeName"].As<string>());
                });

                return recipes;
            }
            finally
            {
                await session.CloseAsync();
            }
        }



    }
}
