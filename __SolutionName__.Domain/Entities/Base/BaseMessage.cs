using System.ComponentModel.DataAnnotations.Schema;

namespace __SolutionName__.Domain.Entities.Base
{
    public class BaseMessage
    {
        [NotMapped]
        public virtual List<uMessage> errorMsgList { get; set; } = [];
        [NotMapped]
        public virtual uMessage? infoMessge { get; set; }

        public void AddErrorMessage(Exception ex)
        {
            errorMsgList ??= new List<uMessage>();

            errorMsgList.Add(new uMessage
            {
                Msg = ex.Message,
                MsgType = messageType.Error
            });
        }

        public void AddErrorMessage(string message)
        {
            errorMsgList ??= new List<uMessage>();

            errorMsgList.Add(new uMessage
            {
                Msg = message,
                MsgType = messageType.Error
            });
        }

        public bool HasError => errorMsgList?.Count > 0;

    }

    public class uMessage
    {
        public int Id { get; set; }

        public string Msg { get; set; }

        public messageType MsgType { get; set; }
    }

    public enum messageType
    {
        Information,
        Warning,
        Error
    }
}
