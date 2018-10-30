using System;
using System.Collections.Generic;
using System.Text;

namespace MeetAndGo.Contracts
{
    public interface IResponseData<T> : IResponse
    {
        T Data { get; set; }
    }
}
