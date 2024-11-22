var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

// Define the root endpoint that returns "Hello World!"
app.MapGet("/", () => "Hello World!");

// Define an endpoint to calculate the number of days between two dates
app.MapGet("/DaysBetweenDates", (DateTime startDate, DateTime endDate) =>
{
    var daysBetween = (endDate - startDate).TotalDays;
    return Results.Ok(daysBetween);
});

// Define an endpoint to validate a phone number
app.MapGet("/validatephonenumber", (string phoneNumber) =>
{
    var isValid = phoneNumber.Length == 10 && phoneNumber.All(char.IsDigit);
    return Results.Ok(isValid);
});

// Define an endpoint to validate a Spanish DNI
app.MapGet("/validatespanishdni", (string dni) =>
{
    bool isValid = false;
    if (dni.Length == 9)
    {
        // Check the length
        string numbers = dni.Substring(0, 8);
        string lastCharacter = dni.Substring(8, 1);
        int number;
        if (int.TryParse(numbers, out number))
        {
            //int remainder = number %
        }
    }
    return Results.Ok(isValid);
});

// Define an endpoint to return the color code for a given color name
app.MapGet("/returncolorcode", async (string color) =>
{
    // Read the colors.json file
    var colorsJson = await File.ReadAllTextAsync("colors.json");
    var colors = JsonSerializer.Deserialize<List<Color>>(colorsJson);

    // Find the color in the list
    var colorInfo = colors?.FirstOrDefault(c => c.Name.Equals(color, StringComparison.OrdinalIgnoreCase));

    if (colorInfo != null)
    {
        return Results.Ok(colorInfo.Code.HEX);
    }
    else
    {
        return Results.NotFound("Color not found");
    }
});

// Define an endpoint to get a random joke from the joke API
app.MapGet("/getjoke", async () =>
{
    using var httpClient = new HttpClient();
    var response = await httpClient.GetStringAsync("https://official-joke-api.appspot.com/random_joke");
    return Results.Ok(response);
});

// Define an endpoint to get movies by a given director
app.MapGet("/moviesbytitle", async (string director) =>
{
    using var httpClient = new HttpClient();
    var apiKey = "f4602c0"; // Replace with your OMDB API key
    var response = await httpClient.GetStringAsync($"http://www.omdbapi.com/?apikey={apiKey}&s={director}&type=movie");
    return Results.Ok(response);
});

// Define an endpoint to parse a URL and return its components
app.MapGet("/parseurl", (string someurl) =>
{
    var uri = new Uri(someurl);
    var result = new
    {
        Protocol = uri.Scheme,
        Host = uri.Host,
        Port = uri.Port,
        Path = uri.AbsolutePath,
        QueryString = uri.Query,
        Hash = uri.Fragment
    };
    return Results.Ok(result);
});

// Define an endpoint to list files in the current directory
app.MapGet("/listfiles", () =>
{
    var currentDirectory = Directory.GetCurrentDirectory();
    var files = Directory.GetFiles(currentDirectory);
    return Results.Ok(files);
});

// Define an endpoint to calculate the memory consumption of the process in GB
app.MapGet("/calculatememoryconsumption", () =>
{
    var process = Process.GetCurrentProcess();
    var memoryInBytes = process.WorkingSet64;
    var memoryInGB = Math.Round(memoryInBytes / (1024.0 * 1024.0 * 1024.0), 2);
    return Results.Ok(memoryInGB);
});

// Define an endpoint to return a random European country and its ISO code
app.MapGet("/randomeuropeancountry", () =>
{
    var countries = new[]
    {
        new { Name = "Germany", ISOCode = "DE" },
        new { Name = "France", ISOCode = "FR" },
        new { Name = "Italy", ISOCode = "IT" },
        new { Name = "Spain", ISOCode = "ES" },
        new { Name = "United Kingdom", ISOCode = "GB" },
        new { Name = "Netherlands", ISOCode = "NL" },
        new { Name = "Belgium", ISOCode = "BE" },
        new { Name = "Sweden", ISOCode = "SE" },
        new { Name = "Norway", ISOCode = "NO" },
        new { Name = "Denmark", ISOCode = "DK" },
        // Add more countries as needed
    };

    var random = new Random();
    var randomCountry = countries[random.Next(countries.Length)];
    return Results.Ok(randomCountry);
});

app.UseHttpsRedirection();

// ADD NEW ENDPOINTS HERE

app.Run();

// Needed to be able to access this type from the MinimalAPI.Tests project.
public partial class Program
{ }
