using MeetAndGo.Contracts;
using Newtonsoft.Json;
using Plugin.SecureStorage;
using Plugin.SecureStorage.Abstractions;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MeetAndGo.Services {
    public class HttpBase {
        private readonly ISecureStorage _secureStorage = CrossSecureStorage.Current;
        private HttpClientHandler httpClientHandler;

        protected string phone_number = "0967675519";
        protected string password = "adminadmin12344321";

        private HttpClient CreateHttpClient () {
            httpClientHandler = new HttpClientHandler () { UseCookies = false };

            var httpClient = new HttpClient ( httpClientHandler ) { BaseAddress = new Uri ( "http://dudaryrik.pythonanywhere.com/" ) };

            httpClient.DefaultRequestHeaders.Accept.Add ( new MediaTypeWithQualityHeaderValue ( "application/json" ) );

            return httpClient;
        }

        protected async Task<IResponseData<string>> GetAsync ( Uri url, bool useAuthHeader = true ) {
            using ( var httpClient = CreateHttpClient () ) {
                httpClient.DefaultRequestHeaders.Add ( "Cookie", _secureStorage.GetValue ( "csrfToken" ) + "; " + _secureStorage.GetValue ( "sessionId" ) );
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ( "Bearer", _secureStorage.GetValue ( "token" ) );

                try {
                    var resultOfAction = await httpClient.GetAsync ( url );
                    var stringContent = await resultOfAction.Content.ReadAsStringAsync ();

                    return new ResponseData<string> ( stringContent, true );
                } catch ( UnauthorizedAccessException ) {
                    // TODO Handle it
                    return null;
                } catch ( Exception ex ) {
                    return new ResponseData<string> {
                        IsSuccess = false,
                        ErrorMessage = ex.Message
                    };
                }

            }
        }

        protected async Task<IResponseData<string>> PostAsync<T> ( Uri url, T item ) {
            using ( var httpClient = CreateHttpClient () ) {
                //httpClient.DefaultRequestHeaders.Add ( "Cookie", _secureStorage.GetValue ( "csrfToken" ) + "; " + _secureStorage.GetValue ( "sessionId" ) );
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ( "Bearer", _secureStorage.GetValue ( "token" ) );

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue (
        "Basic",
        Convert.ToBase64String (
            ASCIIEncoding.ASCII.GetBytes (
                string.Format ( "{0}:{1}", phone_number, password ) ) ) );

                var json = JsonConvert.SerializeObject ( item );
                Console.WriteLine ( json );
                var content = new StringContent ( json, Encoding.UTF8, "application/json" );

                try {
                    var resultOfAction = await httpClient.PostAsync ( url, content );
                    var stringContent = await resultOfAction.Content.ReadAsStringAsync ();

                    Console.WriteLine (stringContent);

                    return new ResponseData<string> ( stringContent, true );
                } catch ( Exception ex ) {
                    return new ResponseData<string> {
                        IsSuccess = false,
                        ErrorMessage = ex.Message
                    };
                }
            }
        }

        protected async Task<IResponseData<string>> PutAsyn<T> ( Uri url, T item ) {
            using ( var httpClient = CreateHttpClient () ) {
                //httpClient.DefaultRequestHeaders.Add ( "Cookie", _secureStorage.GetValue ( "csrfToken" ) + "; " + _secureStorage.GetValue ( "sessionId" ) );
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ( "Bearer", _secureStorage.GetValue ( "token" ) );

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue (
        "Basic",
        Convert.ToBase64String (
            ASCIIEncoding.ASCII.GetBytes (
                string.Format ( "{0}:{1}", phone_number, password ) ) ) );

                var json = JsonConvert.SerializeObject ( item );
                var content = new StringContent ( json, Encoding.UTF8, "application/json" );

                try {
                    var resultOfAction = await httpClient.PutAsync ( url, content );
                    var stringContent = await resultOfAction.Content.ReadAsStringAsync ();

                    Console.WriteLine ( stringContent );

                    return new ResponseData<string> ( stringContent, true );
                } catch ( Exception ex ) {
                    return new ResponseData<string> {
                        IsSuccess = false,
                        ErrorMessage = ex.Message
                    };
                }

            }
        }

        protected async Task<IResponseData<string>> DeleteAsync ( Uri url ) {
            using ( var httpClient = CreateHttpClient () ) {
                try {
                    var resultOfAction = await httpClient.DeleteAsync ( url );
                    var stringContent = await resultOfAction.Content.ReadAsStringAsync ();

                    return new ResponseData<string> ( stringContent, true );
                } catch ( Exception ex ) {
                    return new ResponseData<string> {
                        IsSuccess = false,
                        ErrorMessage = ex.Message
                    };
                }
            }
        }

        protected async Task<IResponseData<string>> SignInAsync<T> ( Uri url, T item ) {
            using ( var httpClient = CreateHttpClient () ) {
                var json = JsonConvert.SerializeObject ( item );
                var content = new StringContent ( json, Encoding.UTF8, "application/json" );

                httpClient.DefaultRequestHeaders.Accept.Add ( new MediaTypeWithQualityHeaderValue ( "application/json" ) );
                httpClient.DefaultRequestHeaders.ExpectContinue = false;

                try {
                    HttpResponseMessage resultOfAction = null;
                    try {
                        resultOfAction = await httpClient.PostAsync ( url, content );
                    } catch ( Exception ex ) {

                    }
                    var stringContent = await resultOfAction.Content.ReadAsStringAsync ();

                    var csrfToken = Regex.Split ( resultOfAction.Headers.GetValues ( "Set-Cookie" ).FirstOrDefault (), ";\\s*" ).FirstOrDefault ();
                    var sessionId = Regex.Split ( resultOfAction.Headers.GetValues ( "Set-Cookie" ).LastOrDefault (), ";\\s*" ).FirstOrDefault ();

                    _secureStorage.SetValue ( "csrfToken", csrfToken );
                    _secureStorage.SetValue ( "sessionId", sessionId );

                    return new ResponseData<string> ( stringContent, true );
                } catch ( Exception ex ) {
                    return new ResponseData<string> {
                        IsSuccess = false,
                        ErrorMessage = ex.Message
                    };
                }
            }
        }
    }
}
