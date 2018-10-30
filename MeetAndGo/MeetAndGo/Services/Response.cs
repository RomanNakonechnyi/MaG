using MeetAndGo.Contracts;

namespace MeetAndGo.Services {
    public class Response : IResponse
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

        public Response()
        {
        }

        public Response(bool isSuccess, string errorMessage = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }
    }
}
