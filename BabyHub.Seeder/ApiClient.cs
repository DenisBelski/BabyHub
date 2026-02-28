using BabyHub.Application.Contracts.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

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
