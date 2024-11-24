public partial class MainViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    [ObservableProperty]
    private ObservableCollection<Client> clients;

    [ObservableProperty]
    private Client selectedClient;

    [ObservableProperty]
    private bool isBusy;

    public MainViewModel(ApiService apiService)
    {
        _apiService = apiService;
        Clients = new ObservableCollection<Client>();
    }

    [RelayCommand]
    private async Task LoadClientsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            var clientList = await _apiService.GetClientsAsync();
            var sortedClients = clientList.OrderBy(c => c.Name).ToList();
            
            Clients.Clear();
            foreach (var client in sortedClients)
            {
                Clients.Add(client);
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}
