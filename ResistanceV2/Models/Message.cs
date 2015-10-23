using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ResistanceV2.Models
{
    [Serializable]
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }
        public string MessageDesc { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual UserModel User { get; set; }

        public virtual ICollection<Comments> Comments { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity), DataMember]
        public DateTime? DateMessaged { get; set; }
        public int LikedCount { get; set; }
        public int Disliked { get; set; }
    }
}