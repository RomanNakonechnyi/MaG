namespace MeetAndGo.Controls {
    interface IValidatable<T> {
        bool IsValid ( T value );
        string Message { get; }
    }
}