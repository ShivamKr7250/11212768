using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace AverageCalculatorApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NumbersController : ControllerBase
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private const int WindowSize = 10;
        private static List<int> window = new List<int>();

        [HttpGet("{numberid}")]
        public async Task<IActionResult> GetNumbers(string numberid)
        {
            if (!IsValidId(numberid))
                return BadRequest(new { error = "Invalid number ID" });

            var newNumbers = await FetchNumbersFromTestServer(numberid);
            if (newNumbers == null)
                return StatusCode(500, new { error = "Failed to fetch numbers" });

            var previousWindow = new List<int>(window);
            UpdateWindow(newNumbers);

            var response = new
            {
                numbers = newNumbers,
                windowPrevState = previousWindow,
                windowCurrState = window,
                avg = window.Any() ? window.Average() : 0
            };

            return Ok(response);
        }

        private static bool IsValidId(string id)
        {
            var validIds = new[] { "primes", "f", "e", "r" };
            return validIds.Contains(id);
        }

        private static async Task<List<int>> FetchNumbersFromTestServer(string numberid)
        {
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJNYXBDbGFpbXMiOnsiZXhwIjoxNzE5ODEyMTEwLCJpYXQiOjE3MTk4MTE4MTAsImlzcyI6IkFmZm9yZG1lZCIsImp0aSI6IjQ3N2Q4NWQzLTE0NGEtNGMyNi04YjVlLTM0ZmUyNDAzZjE5YiIsInN1YiI6InN1cHNoaXY3MjUwQGdtYWlsLmNvbSJ9LCJjb21wYW55TmFtZSI6Ik1NTURVIiwiY2xpZW50SUQiOiI0NzdkODVkMy0xNDRhLTRjMjYtOGI1ZS0zNGZlMjQwM2YxOWIiLCJjbGllbnRTZWNyZXQiOiJ6YVN2UGNEdHF3Uk9QV0F0Iiwib3duZXJOYW1lIjoiU2hpdmFtIEt1bWFyIiwib3duZXJFbWFpbCI6InN1cHNoaXY3MjUwQGdtYWlsLmNvbSIsInJvbGxObyI6IjExMjEyNzY4In0.2379XTb0SbYJQDlJu8owxnkoEvNvs_PGus16Eh7I6ko");
                var response = await httpClient.GetAsync($"http://20.244.56.144/test/{numberid}"); // Replace with the actual API URL
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var numbersResponse = JsonConvert.DeserializeObject<NumbersResponse>(content);
                return numbersResponse.Numbers;
            }
            catch
            {
                return null;
            }
        }

        private static void UpdateWindow(List<int> newNumbers)
        {
            foreach (var num in newNumbers)
            {
                if (!window.Contains(num))
                {
                    if (window.Count >= WindowSize)
                    {
                        window.RemoveAt(0);
                    }
                    window.Add(num);
                }
            }
        }

        private class NumbersResponse
        {
            public List<int> Numbers { get; set; }
        }
    }
}
