using __SolutionName__.Domain.Entities.Base;
using System.Net;

namespace __SolutionName__.Domain.Exceptions
{
    public class BusinessException : ApplicationException
    {
        public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.BadRequest;
        public readonly BaseMessage message = new();

        public BusinessException(string message) : base(message)
        {
            var umessage = new uMessage { Msg = message, MsgType = messageType.Error };
            this.message.errorMsgList.Add(umessage);
        }

        public BusinessException(IEnumerable<string> errors, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest) : base(string.Join("; ", errors))
        {
            HttpStatusCode = httpStatusCode;

            foreach (var error in errors)
            {
                var umessage = new uMessage { Msg = error, MsgType = messageType.Error };
                message.errorMsgList.Add(umessage);
            }
        }

        public BusinessException(string error, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest) : base(error)
        {
            HttpStatusCode = httpStatusCode;
            var umessage = new uMessage { Msg = error, MsgType = messageType.Error };
            message.errorMsgList.Add(umessage);
        }

        public BusinessException(string infoMessage, bool isInfo = true, HttpStatusCode httpStatusCode = HttpStatusCode.Accepted) : base(infoMessage)
        {
            HttpStatusCode = httpStatusCode;
            message.infoMessge = new uMessage { Msg = infoMessage, MsgType = messageType.Information };
        }

        public BusinessException(uMessage uMessage, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest) : base(uMessage.Msg)
        {
            HttpStatusCode = httpStatusCode;
            message.errorMsgList.Add(uMessage);
        }
    }
}
