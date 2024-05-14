using Neo4j.Driver;
using RecipesGlossary.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var neo4jUri = builder.Configuration["Neo4j:Uri"];
var neo4jUsername = builder.Configuration["Neo4j:Username"];
var neo4jPassword = builder.Configuration["Neo4j:Password"];
builder.Services.AddSingleton<IDriver>(provider => GraphDatabase.Driver(neo4jUri, AuthTokens.Basic(neo4jUsername, neo4jPassword)));
builder.Services.AddScoped<RecipeRepository>();
builder.Services.AddScoped<IngredientRepository>();
builder.Services.AddScoped<AuthorRepository>();


// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Apply CORS policy
app.UseCors("AllowLocalhost3000");

app.UseAuthorization();

app.MapControllers();

app.Run();
