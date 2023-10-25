using ClientSaleApi.Models.Products;
using ClientSaleApi.Models.Roles;
using ClientSaleApi.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Newtonsoft.Json;
using System.Security.Policy;
using System.Text;
using X.PagedList;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;

namespace ClientSaleApi.Areas.Admin.Controllers
{
    public abstract class BaseController<T, TList, TInput> : Controller
    where T : class
    where TList : class, new()
    where TInput : class, new()

    {
        private readonly HttpClient _httpClient;

        public BaseController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Client");
        }

        public string GetFromCookie(string name)
        {
            var Name = string.Empty;
            if (Request.Cookies.TryGetValue(name, out var value))
            {
                Name = value;
            }
            return Name;
        }

        protected async Task<T?> GetThongTinModelAsync(string baseUrl)
        {
            var response = await _httpClient.GetAsync(baseUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<T>();
            }
            else
            {
                return null;
            }
        }

        protected async Task<ListViewModel<T>> GetDanhSachModelAsync(string baseUrl, SearchViewModel searchModel)
        {
            string url = $"{baseUrl}?search={searchModel.Search}&skipCount={searchModel.SkipCount}&maxResultCount={searchModel.MaxResultCount}";
            if (!string.IsNullOrEmpty(searchModel.Sorting))
            {
                url += $"&sorting={searchModel.Sorting}";
            }
            ListViewModel<T> list;
			var response = await _httpClient.GetAsync(url);
			if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                list = JsonConvert.DeserializeObject<ListViewModel<T>>(content) ?? new ListViewModel<T> { Items = new List<T>(), TotalCount = 0 };
            }
            else
            {
                list = new ListViewModel<T> { Items = new List<T>(), TotalCount = 0 };
            }
            return list;
        }

        public async Task<IActionResult> Index(string baseUrl, SearchViewModel searchModel, int? page, string? message)
        {
            int pageSize = searchModel.MaxResultCount;
            int pageNumber = (page ?? 1);
            searchModel.SkipCount = (pageNumber - 1) * pageSize;
            var list = await GetDanhSachModelAsync(baseUrl, searchModel);
            var items = new StaticPagedList<T>(list.Items, pageNumber, pageSize, list.TotalCount);
            var viewModel = new IndexViewModel<T>
            {
                Items = items,
                PagingModel = new PagingViewModel
                {
                    Page = pageNumber,
                    PageSize = pageSize,
                    TotalCount = list.TotalCount,
                },
                Message = message ?? ""
            };
            return View(viewModel);
        }

        public async Task<IActionResult> CapNhatTrangThaiId(string baseUrl, int page, int maxResultCount)
        {
            var result = new ApiResponse();
            var response = await _httpClient.PutAsync(baseUrl, null);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<ApiResponse>();
            }
            return RedirectToAction("Index", new { Page = page, MaxResultCount = maxResultCount, Message = result.Message });
        }

        public async Task<IActionResult> XoaId(string baseUrl)
        {
            var result = new ApiResponse();
            var response = await _httpClient.DeleteAsync(baseUrl);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<ApiResponse>();
            }
            return RedirectToAction("Index", new { Message = result.Message });
        }

        protected string EncryptPasswordSHA256(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var data = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(data);
                var encryptedPassword = Convert.ToBase64String(hash);
                return encryptedPassword;
            }
        }

        private async Task<ApiResponse> SendRequestAsync<TInputModel>(string baseUrl, HttpMethod method, TInputModel input) where TInputModel : class
        {
            var result = new ApiResponse();
            var form = new MultipartFormDataContent();
            var properties = input.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.Name == "imageFile")
                {
                    var file = property.GetValue(input) as IFormFile;
                    if (file != null)
                    {
                        form.Add(new StreamContent(file.OpenReadStream()), property.Name, file.FileName);
                    }
                }
                else
                {
                    var value = property.GetValue(input);
                    form.Add(new StringContent(value?.ToString() ?? string.Empty), property.Name);
                }
            }

            var request = new HttpRequestMessage(method, baseUrl) { Content = form };
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<ApiResponse>();
            }
            return result;
        }


        public async Task<IActionResult> Them(string baseUrl, T input)
        {
            var result = await SendRequestAsync(baseUrl, HttpMethod.Post, input);
            return RedirectToAction("Index", new { Message = result.Message });
        }


        public async Task<IActionResult> CapNhat(string baseUrl, T input)
        {
            var result = await SendRequestAsync(baseUrl, HttpMethod.Put, input);
            return RedirectToAction("Index", new { Message = result.Message });
        }

    }
}



