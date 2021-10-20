namespace ZoomAPI.Controllers
{
	using Microsoft.IdentityModel.Tokens;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using RestSharp;
	using System;
	using System.Net;
	using System.Text;
	using System.Web.Mvc;

	public class HomeController : Controller
	{
		// GET: Home
		public ActionResult Index()
		{
			return View();
		}

		public string CreateZoomMeeting()
		{
			string email = "이메일 주소를 입력하세요.";
			var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
			var now = DateTime.UtcNow;
			// API Secret
			var apiSecret = "API Secret을 입력하세요.";
			byte[] symmetricKey = Encoding.ASCII.GetBytes(apiSecret);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				// API Key
				Issuer = "API Key를 입력하세요.",
				Expires = now.AddSeconds(300),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			var tokenString = tokenHandler.WriteToken(token);

			var client = new RestClient($"https://api.zoom.us/v2/users/{email}/meetings");
			var request = new RestRequest(Method.POST);
			request.RequestFormat = DataFormat.Json;

			request.AddJsonBody(new { topic = "Meeting with Zoom", duration = "10", start_time = "2021-10-21T05:00:00", type = "2" });

			request.AddHeader("authorization", $"Bearer {tokenString}");
			IRestResponse restResponse = client.Execute(request);
			HttpStatusCode statusCode = restResponse.StatusCode;
			int numericStatusCode = (int)statusCode;
			var jObject = JObject.Parse(restResponse.Content);

			return JsonConvert.SerializeObject(new { StartUrl = (string)jObject["start_url"], JoinUrl = (string)jObject["join_url"], Code = Convert.ToString(numericStatusCode) });
		}
	}
}