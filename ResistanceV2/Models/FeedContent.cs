using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResistanceV2.Models
{
    

    public class FeedContent
    {
       public DateTime ? DateMessaged;
       public string UserName;
       public string MessageDesc;
       public int UserId;
       public List<CommentContent> Comments;
       public int MessageId;
       public int disliked;
            public int liked;

    }
}