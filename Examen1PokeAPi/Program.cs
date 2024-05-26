using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/pokemon", async (HttpClient httpClient) =>
{
    var pokemonResponse = await httpClient.GetFromJsonAsync<PokemonResponse>("https://pokeapi.co/api/v2/pokemon/pikachu");

    var pokemonData = new PokemonData();
    pokemonData.Name = pokemonResponse?.Name ?? "Unknown";
    pokemonData.Type = pokemonResponse != null ? string.Join(", ", pokemonResponse.Types.Select(t => t.Type.Name)) : "Unknown";
    pokemonData.SpriteUrl = pokemonResponse?.Sprites.FrontDefault ?? "No sprite available";
    pokemonData.UniqueAttacks = pokemonResponse?.Moves.Select(m => m.Move.Name).ToList() ?? new List<string> { "No moves available" };

    return Results.Ok(pokemonData);
});
//.WithName("GetPikachu")
//.WithOpenApi();

app.Run();

public class PokemonResponse
{
    public string Name { get; set; }
    public List<PokemonType> Types { get; set; }
    public PokemonSprites Sprites { get; set; }
    public List<PokemonMove> Moves { get; set; }
}

public class PokemonType
{
    public TypeDetail Type { get; set; }
}

public class TypeDetail
{
    public string Name { get; set; }
}

public class PokemonSprites
{
    [JsonPropertyName("front_default")]
    public string FrontDefault { get; set; }
}

public class PokemonMove
{
    public MoveDetail Move { get; set; }
}

public class MoveDetail
{
    public string Name { get; set; }
}

public class PokemonData
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string SpriteUrl { get; set; }
    public List<string> UniqueAttacks { get; set; }
}