using MeetAndGo.Contracts;

namespace MeetAndGo.Services
{
    class ResponseData<T> : Response, IResponseData<T> {
        public T Data { get; set; }

        public ResponseData() {

        }

        public ResponseData(T data, bool isSuccess, string errorMessage = null) : base(isSuccess, errorMessage) {
            Data = data;
            IsSuccess = isSuccess;
        } 
    }
}
