using System.Collections.ObjectModel;

namespace Models.Utils
{
    public class Response
    {
        public object Result { get; internal set; }

        private readonly IList<string> messages = [];

        public bool IsOk { get; internal set; }

        public string Message { get; internal set; }

        public Exception? Exception { get; internal set; }

        public IEnumerable<string> Validates { get; }

        public Response()
        {
            IsOk = true;
            Exception = null;
            Message = "Operation completed successfully";
            Validates = new ReadOnlyCollection<string>(messages);
        }

        public Response(object entity) : this()
        {
            Result = entity;
        }

        public Response(object entity, string Message) : this(Message)
        {
            Result = entity;
        }

        public Response(string MessageParam) : this()
        {
            Message = MessageParam;
        }

        public Response(bool isOk, string Message) : this(Message)
        {
            IsOk = isOk;
        }

        public Response(Exception erro) : this()
        {
            IsOk = false;
            Exception = erro;
        }

        public Response AddError(string message)
        {
            IsOk = false;
            Message = "Params invalid";
            messages.Add(message);
            return this;
        }
    }
}
