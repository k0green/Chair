using System.Text;
using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Chair.Controllers
{
    [ApiController]
    [Route("dropbox")]
    //[Authorize]
    public class DropboxController : ControllerBase
    {
        private string _clientId = "uhh7kn61531ty3m";//"qk43v7ea6h3g93k";
        private string _clientSecret = "k278w12kluoybk5";//"jou4b2pcu2vyrjh";
        private string _redirectUri = "http://localhost:3000";
        private string _token = "sl.Bx6TwKa4aeE-PHGJ0gD2oVy6pAVfHI9OWAP1jNdU4Nq4lRv-JNSMLPt-pcxw7D7Houf8Es2ruEmneE-6pHNzS4tOS267MoJfy8_wfpb8ReUaE4d7Rd8kt4aOeO5s_bO4s0Elinxtyrqu6pg";

        /*public async Task<string> GetAccessToken()
        {
            var response = await DropboxOAuth2Helper.(_clientId, _clientSecret, _refreshToken);
            _refreshToken = response.RefreshToken;
            return response.AccessToken;
        }*/
        
        [HttpGet("trr")]
        public async Task<string> GetAccessTokenT()
        {
            var code = "vzeOurfhHYQAAAAAAAAATbdJCTTF712BSgUBq3KRX-U";
            var response = await DropboxOAuth2Helper.ProcessCodeFlowAsync(
                code,
                _clientId,
                _clientSecret,
                _redirectUri
            );

            return response.AccessToken;
        }
        
        [HttpPost("test")]
        public async Task<string> GetAccessToken(string code)
        {
            code = "vzeOurfhHYQAAAAAAAAATbdJCTTF712BSgUBq3KRX-U";
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.dropbox.com/oauth2/token");

                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}")));

                request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "code", code },
                    { "grant_type", "authorization_code" },
                    { "redirect_uri", _redirectUri }
                });

                var response = await httpClient.SendAsync(request);
                var jsonResult = await response.Content.ReadAsStringAsync();

                return JObject.Parse(jsonResult)["access_token"].ToString();
            }
        }
        
        [HttpGet]
        public async Task<DropboxClient> GetDropboxClient()
        {
            var config = new DropboxClientConfig();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.dropboxapi.com/oauth2/token");
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(_clientId + ":" + _clientSecret)));
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", _token }
            });

            var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            var jsonResult = JObject.Parse(result);
            _token = jsonResult.Value<string>("access_token");

            return new DropboxClient(_token, config);
        }

        [HttpPost]
        public async Task Upload(IFormFile file)
        {
            using (var dbx = new DropboxClient(_token))
            {
                using (var stream = file.OpenReadStream())
                {
                    var updated = await dbx.Files.UploadAsync(
                        "/"+file.FileName,
                        WriteMode.Overwrite.Instance,
                        body: stream);
                }
            }
        }

        [HttpPost]
        [Route("2")]
        public async Task<IActionResult> Upload2(IFormFile formFile)
        {
                string url = "";
                using (var dbx = new DropboxClient(_token))
                {
                    string folder = @"/CourseApplication";
                    using (var mem = new MemoryStream())
                    {
                        await formFile.CopyToAsync(mem);
                        mem.Seek(0, SeekOrigin.Begin);
                        var updates = await dbx.Files.UploadAsync(folder + "/" + formFile.FileName, WriteMode.Overwrite.Instance,
                            body: mem);
                        var tx = await dbx.Sharing.CreateSharedLinkWithSettingsAsync(folder + "/" + formFile.FileName);
                        url = tx.Url;
                        url = url.Replace("dl=0", "raw=1");
                    }
                }
                HttpContext.Response.Cookies.Append("photoUrl", url);
                return Ok(url);
        }

        [HttpGet("filename:string")]
        public async Task<byte[]> Download(string filename)
        {
            using (var dbx = new DropboxClient(_token))
            {
                using (var response = await dbx.Files.DownloadAsync("/"+filename))
                {
                    return await response.GetContentAsByteArrayAsync();
                }
            }
        }
        
        [HttpDelete("filename:string")]
        public async Task<IActionResult> Delete2(string fileName)
        {
            string url = "";
            using (var dbx = new DropboxClient(_token))
            {
                string folder = @"/CourseApplication";
                using (var mem = new MemoryStream())
                {
                    mem.Seek(0, SeekOrigin.Begin);
                    var updates = await dbx.Files.DeleteV2Async(folder + "/" + fileName);
                }
            }
            return Ok();
        }
    }
}