using Core.DTO.AccountDTO;
using Core.IServices;
using Newtonsoft.Json;

namespace ICPC_Tanta_Web.Services
{
    public class CodeforcesService : ICodeforcesService
    {
        private readonly HttpClient _httpClient;

        public CodeforcesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CodeforcesUserInfo?> GetUserInfoAsync(string handle)
        {
            try
            {
                // Build the API URL
                var url = $"https://codeforces.com/api/user.info?handles={handle}";

                // Fetch the response
                var response = await _httpClient.GetAsync(url);

                // Ensure a successful HTTP response
                if (!response.IsSuccessStatusCode)
                {
                    return null; 
                }

                // Read and deserialize the response JSON
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<CodeforcesApiResponse>(jsonResponse);

                // Return the user object
                return data?.Result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null; // Or rethrow the exception based on your use case
            }
        }
    }
}

     
