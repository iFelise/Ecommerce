using LIBRARY.Shared.DTO.CategoryDTO;
using LIBRARY.Shared.DTO.ProductDTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FRONT_END.Service
{
    public class CategoryService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public CategoryService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            _httpClient = new HttpClient(handler);
            _httpClient.Timeout = TimeSpan.FromSeconds(30);

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
            };

            _baseUrl = ConfigService.ApiBaseUrl;
            Debug.WriteLine($"Base URL: {_baseUrl}");
        }

        public async Task<List<CategoryResponseDto>> GetCategoriesAsync()
        {
            try
            {
                var endpoint = $"{_baseUrl}/category";
                Debug.WriteLine($"Fetching categories from: {endpoint}");

                var response = await _httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Response JSON: {json}");

                    var categories = JsonSerializer.Deserialize<List<CategoryResponseDto>>(json, _jsonSerializerOptions);
                    return categories ?? new List<CategoryResponseDto>();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Error Response: {response.StatusCode} - {errorContent}");
                    throw new HttpRequestException($"Error fetching categories: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"HTTP Request Error: {ex.Message}");
                string errorMessage = GetFriendlyErrorMessage(ex);
                await ShowErrorAlert("Error", errorMessage);
                throw;
            }
            catch (TaskCanceledException ex)
            {
                Debug.WriteLine($"Timeout Error: {ex.Message}");
                string errorMessage = "La solicitud tardó demasiado. Verifique su conexión a internet.";
                await ShowErrorAlert("Error", errorMessage);
                throw new HttpRequestException(errorMessage, ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"General Exception: {ex.Message}");
                string errorMessage = $"Error inesperado: {ex.Message}";
                await ShowErrorAlert("Error", errorMessage);
                throw new HttpRequestException(errorMessage, ex);
            }
        }

        public async Task<CategoryResponseDto?> GetCategoryAsyncById(int id)
        {
            try
            {
                var endpoint = $"{_baseUrl}/category/{id}";
                var response = await _httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<CategoryResponseDto>(json, _jsonSerializerOptions);
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error fetching category with ID {id}: {response.StatusCode} - {errorContent}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                await ShowErrorAlert("Error", ex is HttpRequestException ? GetFriendlyErrorMessage((HttpRequestException)ex) : ex.Message);
                throw new HttpRequestException("Error fetching category", ex);
            }
        }

        public async Task<List<ProdCategoryResponseDto>> GetProdCategoriesByCategoryIdAsync(int categoryId)
        {
            try
            {
                var endpoint = $"{_baseUrl}/category/{categoryId}/prodcategories";
                var response = await _httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<ProdCategoryResponseDto>>(json, _jsonSerializerOptions)
                           ?? new List<ProdCategoryResponseDto>();
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error fetching product categories: {response.StatusCode} - {errorContent}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                await ShowErrorAlert("Error", ex is HttpRequestException ? GetFriendlyErrorMessage((HttpRequestException)ex) : ex.Message);
                throw new HttpRequestException("Error fetching product categories", ex);
            }
        }

        public async Task<List<ProductResponseDto>> GetProductsByProdCategoryIdAsync(int prodCategoryId)
        {
            try
            {
                var endpoint = $"{_baseUrl}/prodcategory/{prodCategoryId}/products";
                var response = await _httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<ProductResponseDto>>(json, _jsonSerializerOptions)
                           ?? new List<ProductResponseDto>();
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error fetching products: {response.StatusCode} - {errorContent}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                await ShowErrorAlert("Error", ex is HttpRequestException ? GetFriendlyErrorMessage((HttpRequestException)ex) : ex.Message);
                throw new HttpRequestException("Error fetching products", ex);
            }
        }

        public async Task<bool> AddCategoryAsync(CreateCategoryDto category)
        {
            try
            {
                var endpoint = $"{_baseUrl}/category";
                var json = JsonSerializer.Serialize(category, _jsonSerializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Debug.WriteLine($"Sending POST to: {endpoint}");
                Debug.WriteLine($"Request JSON: {json}");

                var response = await _httpClient.PostAsync(endpoint, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Error Response: {response.StatusCode} - {errorContent}");
                    throw new HttpRequestException($"Error creating category: {response.StatusCode} - {errorContent}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                await ShowErrorAlert("Error", ex is HttpRequestException ? GetFriendlyErrorMessage((HttpRequestException)ex) : ex.Message);
                throw new HttpRequestException("Error creating category", ex);
            }
        }

        public async Task<bool> UpdateCategoryAsync(UpdateCategoryDto category)
        {
            try
            {
                var endpoint = $"{_baseUrl}/category/{category.Id}";
                var json = JsonSerializer.Serialize(category, _jsonSerializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Debug.WriteLine($"Sending PUT to: {endpoint}");
                Debug.WriteLine($"Request JSON: {json}");

                var response = await _httpClient.PutAsync(endpoint, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Error Response: {response.StatusCode} - {errorContent}");
                    throw new HttpRequestException($"Error updating category: {response.StatusCode} - {errorContent}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                await ShowErrorAlert("Error", ex is HttpRequestException ? GetFriendlyErrorMessage((HttpRequestException)ex) : ex.Message);
                throw new HttpRequestException("Error updating category", ex);
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                var endpoint = $"{_baseUrl}/category/{id}";
                Debug.WriteLine($"Sending DELETE to: {endpoint}");

                var response = await _httpClient.DeleteAsync(endpoint);

                if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Error Response: {response.StatusCode} - {errorContent}");
                    throw new HttpRequestException($"Error deleting category: {response.StatusCode} - {errorContent}");
                }

                return response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NoContent;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
                await ShowErrorAlert("Error", ex is HttpRequestException ? GetFriendlyErrorMessage((HttpRequestException)ex) : ex.Message);
                throw new HttpRequestException("Error deleting category", ex);
            }
        }

        private string GetFriendlyErrorMessage(HttpRequestException ex)
        {
            if (ex.Message.Contains("certificate") || ex.Message.Contains("SSL"))
            {
                return "Error de certificado SSL. Verifique la configuración del servidor.";
            }
            else if (ex.Message.Contains("connection") || ex.Message.Contains("refused"))
            {
                return "No se puede conectar al servidor. Verifique que el servidor esté ejecutándose.";
            }
            else if (ex.Message.Contains("timeout"))
            {
                return "La conexión tardó demasiado. Verifique su conexión a internet.";
            }
            return $"Error de conexión: {ex.Message}";
        }

        private async Task ShowErrorAlert(string title, string message)
        {
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert(title, message, "OK");
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}