using System;
using System.Collections.Generic;
using System.Text;

namespace MeetAndGo.Contracts
{
    public interface IResponse
    {
        bool IsSuccess { get; set; }
        string ErrorMessage { get; set; }
    }
}
