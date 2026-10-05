using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class MessageEntity
    {
        public int Messages_Id { get; set; }
        public Nullable<int> Conversation_Id { get; set; }
        public Nullable<int> Sender_Id { get; set; }
        public string Message_Type { get; set; }
        public string Message1 { get; set; }
        public string Attachment_Thumb_Url { get; set; }
        public string Attachment_Url { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Guid { get; set; }
        public Nullable<System.DateTime> Deleted_Date { get; set; }
    }
}
