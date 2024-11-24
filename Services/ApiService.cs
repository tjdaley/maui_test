public class ApiService
{
    private readonly HttpClient _client;
    private string _token;

    public ApiService()
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri("https://your-api-base-url")
        };
    }

    public async Task<AuthResponse> AuthenticateAsync(string username, string password, string twoFactorCode)
    {
        try
        {
            var request = new AuthRequest
            {
                Username = username,
                Password = password,
                TwoFactorCode = twoFactorCode
            };

            var response = await _client.PostAsJsonAsync("auth/login", request);
            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (authResponse?.Success == true)
                {
                    _token = authResponse.Token;
                    _client.DefaultRequestHeaders.Authorization = 
                        new AuthenticationHeaderValue("Bearer", _token);
                    return authResponse;
                }
            }
            
            return new AuthResponse 
            { 
                Success = false, 
                ErrorMessage = "Authentication failed" 
            };
        }
        catch (Exception ex)
        {
            return new AuthResponse 
            { 
                Success = false, 
                ErrorMessage = ex.Message 
            };
        }
    }

    public async Task<List<Client>> GetClientsAsync()
    {
        try
        {
            var response = await _client.GetFromJsonAsync<List<Client>>("clients");
            return response ?? new List<Client>();
        }
        catch
        {
            return new List<Client>();
        }
    }
}
