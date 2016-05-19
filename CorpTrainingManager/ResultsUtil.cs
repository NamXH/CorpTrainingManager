using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Net;
using System.Collections.Generic;

namespace CorpTrainingManager
{
	public static class ResultsUtil
	{

		private static async Task<HttpResponseMessage> MakeServerPostRequest(string url, StringContent content){
			HttpClient client = new HttpClient ();
			client.MaxResponseContentBufferSize = 256000;

			var uri = new Uri (string.Format (url));

			HttpResponseMessage response = null;
			try{

				response = await client.PostAsync (uri, content);

			}catch (WebException e){
				if(e.Response==null)
					throw new WebException ("Error connecting to the server: " + url +" Possible Internet problems");

				throw new WebException ("Error connecting to the server: " + url +" Status code: " +((HttpWebResponse)e.Response).StatusCode);
			}
			return response;

		}

		private static async Task<HttpResponseMessage> MakeServerGetRequest(string url){
			HttpClient client = new HttpClient ();
			client.MaxResponseContentBufferSize = 256000;

			var uri = new Uri (string.Format (url));

			HttpResponseMessage response = null;
			try{

				response = await client.GetAsync (uri);

			}catch (WebException e){
				if(e.Response==null)
					throw new WebException ("Error connecting to the server: " + url +" Possible Internet problems");

				throw new WebException ("Error connecting to the server: " + url +" Status code: " +((HttpWebResponse)e.Response).StatusCode);
			}
			return response;

		}

		public static async Task<IList<User>> GetUsersListAsync(){
			
			IList<User> usersList;

			HttpResponseMessage response = null;

			response = await MakeServerGetRequest (Globals.USERS_URL);

			var content = await response.Content.ReadAsStringAsync ();

			usersList = JsonConvert.DeserializeObject<IList<User>> (content);

			return usersList;
			
		}

		public static async Task<IList<UserResults>> GetUserResultsAsync(int userId){

			IList<UserResults> userResults;

			HttpResponseMessage response = null;

			response = await MakeServerGetRequest (Globals.USERS_URL + userId);

			var content = await response.Content.ReadAsStringAsync ();

			userResults = JsonConvert.DeserializeObject<IList<UserResults>> (content);

			return userResults;

		}


	}
}

