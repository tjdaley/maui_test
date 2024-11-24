public partial class LoginViewModel : ObservableObject
{
    private readonly ApiService _apiService;
    private readonly INavigation _navigation;

    [ObservableProperty]
    private string username;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string twoFactorCode;

    [ObservableProperty]
    private string errorMessage;

    [ObservableProperty]
    private bool isBusy;

    public LoginViewModel(ApiService apiService, INavigation navigation)
    {
        _apiService = apiService;
        _navigation = navigation;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            var response = await _apiService.AuthenticateAsync(
                Username, Password, TwoFactorCode);

            if (response.Success)
            {
                await _navigation.PushAsync(new MainPage());
            }
            else
            {
                ErrorMessage = response.ErrorMessage;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
