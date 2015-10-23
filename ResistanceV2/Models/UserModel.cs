using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ResistanceV2.DataContext;


namespace ResistanceV2.Models
{       
    public class UserModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage="Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "User Name")]
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
        [JsonIgnore]
        public virtual ICollection<SavedMessages> SavedMessages { get; set; }

        
        public bool IsValid(string _username, string _password)
        {
            ResistanceDBContext _db = new ResistanceDBContext();
            var userEntity = _db.User.ToList();
            UserModel matchUser = userEntity.Find(x => (x.UserName == _username) && (x.Password == _password));
            if (matchUser != null && matchUser.Name != null)
            {
                return true;

            }
            else {
                return false;
            }
        }
        public UserModel() { 
        }
        public UserModel(string UserName) {
            ResistanceDBContext _db = new ResistanceDBContext();
            var userEntity = _db.User.ToList();
            UserModel temp = userEntity.Find(x => (x.UserName == UserName));
            if (temp != null)
            {
                this.UserId = temp.UserId;
                this.Name = temp.Name;
                this.Password = temp.Password;
                this.UserName = temp.UserName;
                this.SavedMessages = temp.SavedMessages;
            }

        }
    }
}