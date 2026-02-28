using BabyHub.Seeder;

var baseUrl = Environment.GetEnvironmentVariable("API_URL")
    ?? args.FirstOrDefault()
    ?? "https://localhost:7047";

Console.WriteLine($"Seeding 100 patients to {baseUrl}...\n");

var client = new ApiClient(baseUrl);
var patients = PatientFaker.Generate(100);

int successCount = 0;
int failCount = 0;

foreach (var patient in patients)
{
    var (success, message) = await client.CreatePatientAsync(patient);

    if (success)
    {
        successCount++;
        Console.WriteLine($"[OK]   {patient.FamilyName} -> {message}");
    }
    else
    {
        failCount++;
        Console.WriteLine($"[FAIL] {patient.FamilyName} -> {message}");
    }
}

Console.WriteLine($"\nDone. Success: {successCount}, Failed: {failCount}");