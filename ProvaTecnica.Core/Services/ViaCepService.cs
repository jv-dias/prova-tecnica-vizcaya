using Microsoft.Extensions.Caching.Memory;
using ProvaTecnica.Core.DTOs;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ProvaTecnica.Core.Services
{
	public class ViaCepService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IMemoryCache _cache;

		public ViaCepService(IHttpClientFactory httpClientFactory, IMemoryCache cache)
		{
			_httpClientFactory = httpClientFactory;
			_cache = cache;
		}

		public async Task<ViaCepResponse?> ConsultarCepAsync(string cep)
		{
			if (_cache.TryGetValue(cep, out ViaCepResponse? cachedResponse))
			{
				return cachedResponse;
			}
			
			var httpClient = _httpClientFactory.CreateClient();
			try
			{
				var response = await httpClient.GetFromJsonAsync<ViaCepResponse>($"https://viacep.com.br/ws/{cep}/json/");

				var cacheOptions = new MemoryCacheEntryOptions()
					.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

				_cache.Set(cep, response, cacheOptions);

				return response;
			}
			catch (HttpRequestException)
			{
				return null;
			}
		}
	}
}