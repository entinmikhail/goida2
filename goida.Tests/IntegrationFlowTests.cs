using System.Net.Http.Headers;
using System.Net.Http.Json;
using goida.Dtos;
using Xunit;

namespace goida.Tests;

public class IntegrationFlowTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public IntegrationFlowTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Registration_login_upload_and_hr_list_workflow()
    {
        var register = new RegisterRequest("user1@example.com", "Password123!", "Ирина Иванова");
        var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", register);
        registerResponse.EnsureSuccessStatusCode();

        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new LoginRequest(register.Email, register.Password));
        loginResponse.EnsureSuccessStatusCode();
        var loginPayload = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(loginPayload);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginPayload!.Token);

        using var form = new MultipartFormDataContent();
        var pdfBytes = System.Text.Encoding.UTF8.GetBytes("fake pdf content");
        var fileContent = new ByteArrayContent(pdfBytes);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
        form.Add(fileContent, "file", "extract.pdf");

        var uploadResponse = await _client.PostAsync("/api/me/upload-extract", form);
        uploadResponse.EnsureSuccessStatusCode();

        var hrLoginResponse = await _client.PostAsJsonAsync("/api/auth/login", new LoginRequest("hr@example.com", "ChangeMe123!"));
        hrLoginResponse.EnsureSuccessStatusCode();
        var hrPayload = await hrLoginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(hrPayload);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", hrPayload!.Token);
        var listResponse = await _client.GetAsync("/api/hr/candidates?sort=name");
        listResponse.EnsureSuccessStatusCode();
    }
}
