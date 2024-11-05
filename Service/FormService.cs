using BlazorDeneme.Core;
namespace BlazorDeneme.Service
{
    public class FormService
    {
        private readonly HttpClient _httpClient;
        public FormService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task SubmitForm(FormData formData)
        {
            await _httpClient.PostAsJsonAsync("https://localhost:5092/api/form/formdata", formData);
        }
        public async Task<List<FormData>> GetData()
        {
            var response = await _httpClient.GetAsync("https://localhost:5092/api/form/get-form-data");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<FormData>>();
            }
            else
            {
                Console.WriteLine($"Hata: {response.StatusCode} - {response.ReasonPhrase}");
                return new List<FormData>(); // Boş bir liste döndür
            }
        }
    }
}