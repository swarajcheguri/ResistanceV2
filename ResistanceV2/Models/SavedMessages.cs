using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ResistanceV2.Models
{
    public class SavedMessages
    { [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SavedMessageId { get; set; }
        public virtual Message Message { get; set; }
        [ForeignKey("UserModel")]
        public int UserId { get; set; }
        [JsonIgnore]
        public virtual UserModel UserModel { get; set; }
        [ForeignKey("Message")]
        public int MessageId { get; set; }
    }
}