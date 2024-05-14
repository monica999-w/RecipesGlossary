namespace RecipesGlossary.Model
{
    public class Recipe
    {
        public string? Id { get; set; }
        public string ?Name { get; set; }
        public string? Description { get; set; }
        public int CookingTime { get; set; }
        public int PreparationTime { get; set; }
        public Author? Author { get; set; }
        public List<Ingredient>? Ingredients { get; set; }
        public string? SkillLevel { get; set; }

        
    }
}
