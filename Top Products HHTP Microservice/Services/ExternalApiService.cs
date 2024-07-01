using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using Top_Products_HHTP_Microservice.Model;

public class ExternalApiService
{
    private const string BaseUrl = "http://20.244.56.144";
    private const string Token = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJNYXBDbGFpbXMiOnsiZXhwIjoxNzE5ODE0NjQwLCJpYXQiOjE3MTk4MTQzNDAsImlzcyI6IkFmZm9yZG1lZCIsImp0aSI6IjQ3N2Q4NWQzLTE0NGEtNGMyNi04YjVlLTM0ZmUyNDAzZjE5YiIsInN1YiI6InN1cHNoaXY3MjUwQGdtYWlsLmNvbSJ9LCJjb21wYW55TmFtZSI6Ik1NTURVIiwiY2xpZW50SUQiOiI0NzdkODVkMy0xNDRhLTRjMjYtOGI1ZS0zNGZlMjQwM2YxOWIiLCJjbGllbnRTZWNyZXQiOiJ6YVN2UGNEdHF3Uk9QV0F0Iiwib3duZXJOYW1lIjoiU2hpdmFtIEt1bWFyIiwib3duZXJFbWFpbCI6InN1cHNoaXY3MjUwQGdtYWlsLmNvbSIsInJvbGxObyI6IjExMjEyNzY4In0.sto6KyvS5MBABS4d7EanzTI_r2FbC1D7vhtSBJI07o8";

    public async Task<List<Product>> GetTopProducts(string companyName, string categoryName, int top, int minPrice, int maxPrice)
    {
        var client = new RestClient(BaseUrl);
        var request = new RestRequest($"/products/companies/{companyName}/categories/{categoryName}/products", Method.Get);
        request.AddParameter("top", top);
        request.AddParameter("minPrice", minPrice);
        request.AddParameter("maxPrice", maxPrice);
        request.AddHeader("Authorization", Token);

        var response = await client.ExecuteAsync<List<Product>>(request);
        return response.Data;
    }

    public async Task<Product> GetProductDetails(string companyName, string categoryName, string productId)
    {
        var client = new RestClient(BaseUrl);
        var request = new RestRequest($"/products/companies/{companyName}/categories/{categoryName}/products/{productId}", Method.Get);
        request.AddHeader("Authorization", Token);

        var response = await client.ExecuteAsync<Product>(request);
        return response.Data;
    }
}
