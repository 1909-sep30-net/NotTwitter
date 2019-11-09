using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using NotTwitter.API.Models;
using Microsoft.AspNetCore.Http;

namespace NotTwitter.API.Services
{
	public class AuthInfoService
	{
		private readonly HttpClient _client;

		private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

		public AuthInfoService(HttpClient client)
		{
			_client = client ?? throw new ArgumentNullException(nameof(client));
		}

		public async Task<string> GetUserEmailAsync(HttpRequest currentRequest)
		{
			var request = new HttpRequestMessage(HttpMethod.Get, "https://nottwitter.auth0.com/userinfo");
			var authHeader = currentRequest.Headers["Authorization"].ToString();
			request.Headers.Add("Authorization", authHeader);
			using var response = await _client.SendAsync(request);
			response.EnsureSuccessStatusCode();
			using var responseStream = await response.Content.ReadAsStreamAsync();
			var info = await JsonSerializer.DeserializeAsync<AuthUserInfo>(responseStream, _jsonOptions);
			return info.Email;
		}
	}
}