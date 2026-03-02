using BabyHub.Application.Contracts.Patients;
using System.Net.Http.Json;

namespace BabyHub.Seeder
{
    public class ApiClient
    {
        private readonly HttpClient _client;

        public ApiClient(string baseUrl)
        {
            _client = new HttpClient { BaseAddress = new Uri(baseUrl) };
        }

        public async Task<(bool success, string message)> CreatePatientAsync(PatientCreateDto dto)
        {
            var response = await _client.PostAsJsonAsync("/api/patients", dto);
            if (response.IsSuccessStatusCode)
            {
                var id = await response.Content.ReadFromJsonAsync<Guid>();
                return (true, id.ToString());
            }

            var error = await response.Content.ReadAsStringAsync();
            return (false, error);
        }
    }
}