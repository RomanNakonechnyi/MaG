using Acr.UserDialogs;
using MeetAndGo.Contracts;
using MeetAndGo.Controls.Models;
using MeetAndGo.Models;
using MeetAndGo.Models.Enums;
using Newtonsoft.Json;
using Plugin.SecureStorage;
using Plugin.SecureStorage.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MeetAndGo.Services {
    public class ApiService : HttpBase {
        private const string hostUri = "http://dudaryrik.pythonanywhere.com/";

        public async Task<IResponseData<SignInModel>> SignInAsync () {
            var refBuilder = new StringBuilder ( hostUri + "rest-auth/login/" );

            var uri = new Uri ( refBuilder.ToString () );
            var jsonContent = await SignInAsync ( uri, new CredentialsModel ( phone_number, password ) );

            if ( jsonContent.IsSuccess ) {
                var signInInfo = JsonConvert.DeserializeObject<SignInModel> ( jsonContent.Data );
                return new ResponseData<SignInModel> ( signInInfo, true );
            }

            return new ResponseData<SignInModel> ( null, false, jsonContent.ErrorMessage );
        }

        public async Task<IResponseData<List<EventModel>>> GetEventListAsync ( Location startLocation, Location endLocation ) {
            var refBuilder = new StringBuilder ( hostUri );
            refBuilder.Append ( "search/" );
            refBuilder.Append ( $"?end_lat={endLocation.Latitude}" +
                                $"&end_lon={endLocation.Longitude}" +
                                $"&start_lat={startLocation.Latitude}" +
                                $"&start_lon={startLocation.Longitude}" );

            var uri = new Uri ( refBuilder.ToString () );
            var jsonContent = await GetAsync ( uri );

            if ( jsonContent.IsSuccess ) {
                var eventList = JsonConvert.DeserializeObject<List<EventModel>> ( jsonContent.Data );
                return new ResponseData<List<EventModel>> ( eventList, true );
            }

            return new ResponseData<List<EventModel>> ( null, false, jsonContent.ErrorMessage );
        }

        public async Task<IResponseData<EventModel>> GetEventDetailsAsync ( int eventId ) {
            var refBuilder = new StringBuilder ( hostUri );
            refBuilder.Append ( $"events/{eventId.ToString ()}" );

            var uri = new Uri ( refBuilder.ToString () );
            var jsonContent = await GetAsync ( uri );

            if ( jsonContent.IsSuccess ) {
                var eventModel = JsonConvert.DeserializeObject<EventModel> ( jsonContent.Data );
                return new ResponseData<EventModel> ( eventModel, true );
            }

            return new ResponseData<EventModel> ( null, false, jsonContent.ErrorMessage );
        }

        public async Task<IResponseData<List<Location>>> GetEventPointsAsync ( EventModel eventModel ) {
            var refBuilder = new StringBuilder ( hostUri + "points/" );

            var uri = new Uri ( refBuilder.ToString () );
            var jsonContent = await GetAsync ( uri );

            if ( jsonContent.IsSuccess ) {
                var points = JsonConvert.DeserializeObject<List<Location>> ( jsonContent.Data );
                return new ResponseData<List<Location>> ( points, true );
            }

            return new ResponseData<List<Location>> ( null, false, jsonContent.ErrorMessage );
        }

        public async Task<IResponseData<EventModel>> CreateNewEventAsync ( EventModel eventModel ) {
            var refBuilder = new StringBuilder ( hostUri + "events/" );

            var uri = new Uri ( refBuilder.ToString () );
            var response = await PostAsync ( uri, eventModel );

            if ( response.IsSuccess ) {
                eventModel = JsonConvert.DeserializeObject<EventModel> ( response.Data );
                return new ResponseData<EventModel> ( eventModel, true );
            }

            return new ResponseData<EventModel> ( null, false, response.ErrorMessage );
        }

        public async Task AddDirectionPointsAsync ( int id, EventModel eventModel ) {
            var points = eventModel.Direction;

            for ( int position = 0; position < points.Count; position++ ) {
                var loc = $"{points.ElementAt ( position ).Latitude},{points.ElementAt ( position ).Longitude}";

                var point = new {
                    start_point = points.ElementAt ( position ).Name,
                    location = loc,
                    route = id,
                    sort_index = position + 1
                };

                var refBuilder = new StringBuilder ( hostUri + "points/" );

                var uri = new Uri ( refBuilder.ToString () );
                var response = await PostAsync ( uri, point );
            }
        }

        public async Task<IResponseData<EventModel>> ChangeEventAsync ( EventModel eventModel ) {
            var refBuilder = new StringBuilder ( hostUri + $"events/{eventModel.Id}" );

            var uri = new Uri ( refBuilder.ToString () );
            var jsonContent = await GetAsync ( uri );

            if ( jsonContent.IsSuccess ) {
                var changedEvent = JsonConvert.DeserializeObject<EventModel> ( jsonContent.Data );
                return new ResponseData<EventModel> ( changedEvent, true );
            }

            return new ResponseData<EventModel> ( null, false, jsonContent.ErrorMessage );
        }

        public async Task RemoveUserFromEventAsync ( UserModel user, EventModel eventModel ) {
            var refBuilder = new StringBuilder ( hostUri );
            refBuilder.Append ( $"events/{eventModel.Id.ToString ()}/remove/?phone_number={user.PhoneNumber.ToString ()}" );

            var uri = new Uri ( refBuilder.ToString () );
            var jsonContent = await GetAsync ( uri );
        }

        public async Task LeaveEventAsync ( EventModel eventModel ) {
            var refBuilder = new StringBuilder ( hostUri );
            refBuilder.Append ( $"events/{eventModel.Id.ToString ()}/remove/" );

            var uri = new Uri ( refBuilder.ToString () );
            var jsonContent = await GetAsync ( uri );
        }

        public async Task<IResponseData<EventModel>> JoinEventAsync ( EventModel eventModel ) {
            var refBuilder = new StringBuilder ( hostUri );
            refBuilder.Append ( $"events/{eventModel.Id.ToString ()}/join" );

            var uri = new Uri ( refBuilder.ToString () );
            var jsonContent = await GetAsync ( uri );

            if ( jsonContent.IsSuccess ) {
                eventModel = JsonConvert.DeserializeObject<EventModel> ( jsonContent.Data );
                return new ResponseData<EventModel> ( eventModel, true );
            }

            return new ResponseData<EventModel> ( null, false, jsonContent.ErrorMessage );
        }

        public async Task DeleteEventAsync ( EventModel eventModel ) {
            var refBuilder = new StringBuilder ( hostUri );
            refBuilder.Append ( $"events/{eventModel.Id.ToString ()}" );

            var uri = new Uri ( refBuilder.ToString () );
            var jsonContent = await DeleteAsync ( uri );
        }

        public async Task<IResponseData<UserStatus>> GetUserStatusAsync ( UserModel userModel ) {
            var refBuilder = new StringBuilder ( hostUri );
            refBuilder.Append ( $"users/{userModel.Id}/state/" );

            var uri = new Uri ( refBuilder.ToString () );
            var jsonContent = await GetAsync ( uri );

            if ( jsonContent.IsSuccess ) {
                var data = JsonConvert.DeserializeObject<Dictionary<string, int>> ( jsonContent.Data );
                var value = data["status"];

                switch ( value ) {
                    case 0:
                        return new ResponseData<UserStatus> ( UserStatus.User, true );
                    case 1:
                        return new ResponseData<UserStatus> ( UserStatus.Member, true );
                    case 2:
                        return new ResponseData<UserStatus> ( UserStatus.Organizer, true );
                }
            }

            return new ResponseData<UserStatus> ( 0, false, jsonContent.ErrorMessage );
        }

        public async Task<IResponseData<UserModel>> GetUserAsync ( int id ) {
            var refBuilder = new StringBuilder ( hostUri );
            refBuilder.Append ( $"users/{id}" );

            var uri = new Uri ( refBuilder.ToString () );
            var jsonContent = await GetAsync ( uri );

            if ( jsonContent.IsSuccess ) {
                var user = JsonConvert.DeserializeObject<UserModel> ( jsonContent.Data );
                return new ResponseData<UserModel> ( user, true );
            }

            return new ResponseData<UserModel> ( null, false, jsonContent.ErrorMessage );
        }

        public async Task VoteForUserAsync ( VoteModel voteModel ) {
            var refBuilder = new StringBuilder ( hostUri );
            refBuilder.Append ( $"votes/" );

            var uri = new Uri ( refBuilder.ToString () );
            var jsonContent = await PostAsync ( uri, voteModel );
        }
    }
}
