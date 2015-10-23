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
    public class Comments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }
        public string Comment { get; set; }
        [JsonIgnore]
        public virtual Message Message2 { get; set; }
        [ForeignKey("UserModel")]
        public int UserId { get; set; }
        [JsonIgnore]
        public virtual UserModel UserModel { get; set; }
        [ForeignKey("Message2")]
        public int MessageId { get; set; }
        public DateTime DateCommented { get; set; }
    }
}