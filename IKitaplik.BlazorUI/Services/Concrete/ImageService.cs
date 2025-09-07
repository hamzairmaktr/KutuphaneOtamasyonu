using Core.Entities;
using IKitaplik.BlazorUI.Cosntant;
using IKitaplik.BlazorUI.Responses;
using IKitaplik.BlazorUI.Services.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs;
using IKitaplik.Entities.DTOs.BookDTOs;
using IKitaplik.Entities.DTOs.ImagesDTOs;
using IKitaplik.Entities.Enums;
using System.Net.Http.Headers;
using System.Text.Json;

namespace IKitaplik.BlazorUI.Services.Concrete
{
    public class ImageService : IImageService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
        public ImageService(HttpClient httpClient, IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
            _httpClient.BaseAddress = new Uri(Settings.apiUrl);
        }
        public async Task<Response> Delete(int id)
        {
            string token = await _authService.GetToken() ?? "";
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var res = await _httpClient.PostAsJsonAsync("image/delete", new DeleteDto { Id = id });
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response>(content, _jsonOptions)!;
            }
            else
            {
                return await Task.FromException<Response>(new Exception("Login Olunamadı"));
            }
        }

        public async Task<Response<List<Image>>> GetAll(ImageType? type = null, int relationshipId = 0)
        {
            string token = await _authService.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                if (type is not null && relationshipId > 0)
                {
                    var res = await _httpClient.GetAsync($"image/getall?type{type}&{relationshipId}");
                    var content = await res.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Response<List<Image>>>(content, _jsonOptions)!;
                }
                else
                {
                    var res = await _httpClient.GetAsync($"image/getall");
                    var content = await res.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Response<List<Image>>>(content, _jsonOptions)!;
                }
            }
            else
            {
                return await Task.FromException<Response<List<Image>>>(new Exception("Login Olunamadı"));
            }
        }

        public async Task<Response<Image>> Upload(ImageUploadDto imageUploadDto)
        {
            string token = await _authService.GetToken() ?? "";
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                using var content = new MultipartFormDataContent();

                var fileStream = imageUploadDto.File.OpenReadStream();
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(imageUploadDto.File.ContentType);

                content.Add(fileContent, "File", imageUploadDto.File.Name);
                content.Add(new StringContent(((int)imageUploadDto.ImageType).ToString()), "ImageType");
                content.Add(new StringContent(imageUploadDto.RelationshipId.ToString()), "RelationshipId");

                var res = await _httpClient.PostAsync("image/add", content);
                var responseContent = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<Image>>(responseContent, _jsonOptions)!;
            }
            else
            {
                return await Task.FromException<Response<Image>>(new Exception("Login Olunamadı"));
            }
        }

    }
}
