namespace IntegrationTests;

public class IntegrationTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public IntegrationTests(TestWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_ReturnsHelloWorld()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync("/");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Hello World!", content);
    }

    [Theory]
    [InlineData("1234567890", true)]
    [InlineData("0987654321", true)]
    [InlineData("5555555555", true)]
    [InlineData("12345", false)]
    [InlineData("abcdefghij", false)]
    [InlineData("123-456-7890", false)]
    public async Task ValidatePhoneNumber_ReturnsExpected(string phoneNumber, bool isValid)
    {
        // Act
        var response = await _client.GetAsync($"/validatephonenumber?phoneNumber={phoneNumber}");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(isValid.ToString().ToLower(), content);
    }

    [Fact]
    public async Task ValidateSpanishDni_ReturnsValid()
    {
        // Arrange
        var dni = "12345678Z";

        // Act
        var response = await _client.GetAsync($"/validatespanishdni?dni={dni}");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("false", content);
    }

    [Fact]
    public async Task ReturnColorCode_ReturnsRgba()
    {
        // Arrange
        var color = "red";

        // Act
        var response = await _client.GetAsync($"/returncolorcode?color={color}");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("\"#FF0000\"", content);
    }

    [Fact]
    public async Task GetJoke_ReturnsJoke()
    {
        // Act
        var response = await _client.GetAsync("/getjoke");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content); // Log the response content
        Assert.False(string.IsNullOrEmpty(content));
        Assert.Contains("setup", content);
        Assert.Contains("punchline", content);
    }

    [Fact]
    public async Task MoviesByTitle_ReturnsGuardiansOfTheGalaxyVol2()
    {
        // Arrange
        var director = "James Gunn";

        // Act
        var response = await _client.GetAsync($"/moviesbytitle?director={director}");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content); // Log the response content
        Assert.Contains("Guide to the Galaxy with James Gunn", content);
    }


        [Fact]
        public async Task ParseUrl_ReturnsComponents()
        {
            var response = await _client.GetAsync("/parseurl?someurl=https://example.com:8080/path?query=1#fragment");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("\"protocol\":\"https\"", content);
            Assert.Contains("\"host\":\"example.com\"", content);
            Assert.Contains("\"port\":8080", content);
            Assert.Contains("\"path\":\"/path\"", content);
            Assert.Contains("\"queryString\":\"?query=1\"", content);
        }

        [Fact]
        public async Task ListFiles_ReturnsFiles()
        {
            var response = await _client.GetAsync("/listfiles");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrEmpty(content));
        }

        [Fact]
        public async Task CalculateMemoryConsumption_ReturnsMemoryInGB()
        {
            var response = await _client.GetAsync("/calculatememoryconsumption");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrEmpty(content));
        }

        [Fact]
        public async Task RandomEuropeanCountry_ReturnsCountry()
        {
            var response = await _client.GetAsync("/randomeuropeancountry");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrEmpty(content));
        }

}
