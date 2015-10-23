using ResistanceV2.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using ResistanceV2.Models;
using System.Web.Security;
using Microsoft.AspNet.SignalR;








namespace ResistanceV2.Controllers
{




    public class FeedController : Controller
    {

        private ResistanceDBContext db = new ResistanceDBContext();

        //// GET: /Comments/
        //[Route("/Feed/Index")]
        public ActionResult Feed()
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]

        public void SaveMessage(string msg)
        {
            Message message = new Message();
            message.MessageDesc = msg;
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            string UserName = authCookie["userName"].ToString();

            UserModel matchUser = new UserModel(UserName);
            message.UserId = matchUser.UserId;
            message.DateMessaged = DateTime.UtcNow;
            db.Message.Add(message);
            db.SaveChanges();
            FeedContent messages = new FeedContent();
            messages.DateMessaged = message.DateMessaged;
            messages.UserName = matchUser.UserName;
            messages.MessageDesc = message.MessageDesc;
            messages.UserId = message.UserId;
            messages.MessageId = message.MessageId;
            messages.liked = message.LikedCount;
            messages.disliked = message.Disliked;
            List<CommentContent> commentContent = new List<CommentContent>();
            messages.Comments = new List<CommentContent>();
            var myhubContext = GlobalHost.ConnectionManager.GetHubContext<MyHub1>();
            myhubContext.Clients.All.NewMessage(messages);

        }

        public string GetFeed()
        {

            var messages = from entity in db.Message
                           select new
                           {
                               Comments = from cmt in entity.Comments
                                          select
                                          new
                                          {
                                              UserName = cmt.UserModel.UserName,
                                              MessageId = cmt.MessageId,
                                              Comment = cmt.Comment,
                                              DateCommented = cmt.DateCommented

                                          },
                               DateMessaged = entity.DateMessaged,
                               UserName = entity.User.UserName,
                               MessageDesc = entity.MessageDesc,
                               UserId = entity.UserId,
                               MessageId=entity.MessageId,
                               liked=entity.LikedCount,
            disliked=entity.Disliked


                           };
            string data = JsonConvert.SerializeObject(messages.ToList(), Formatting.Indented);
            return data;

        }
        public string GetSavedMessages()
        {

             
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            string UserName = authCookie["userName"].ToString();
            UserModel matchUser = new UserModel(UserName);
            var user =   db.User.Where(i => i.UserId == matchUser.UserId);

            UserModel userContent = db.User.First(i => i.UserId == matchUser.UserId);
            List<Message> msgs = new List<Message>();
            foreach(SavedMessages smsg in userContent.SavedMessages)
            {
                msgs.Add(smsg.Message);

            }

            var messages = from entity in msgs
                           select new
                           {
                               Comments = from cmt in entity.Comments
                                          select
                                          new
                                          {
                                              UserName = cmt.UserModel.UserName,
                                              MessageId = cmt.MessageId,
                                              Comment = cmt.Comment,
                                              DateCommented = cmt.DateCommented

                                          },
                               DateMessaged = entity.DateMessaged,
                               UserName = entity.User.UserName,
                               MessageDesc = entity.MessageDesc,
                               UserId = entity.UserId,
                               MessageId = entity.MessageId,
                               liked = entity.LikedCount,
                               disliked = entity.Disliked


                           };                      
                          
            string data = JsonConvert.SerializeObject(messages.ToList(), Formatting.Indented);
            return data;

        



           
        }
        [HttpPost]
        public void CommentSave(string cmt, int msgid)
        {
            Comments comments = new Comments();
            comments.Comment = cmt;
            comments.MessageId = msgid;
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            string UserName = authCookie["userName"].ToString();
            UserModel matchUser = new UserModel(UserName);
            comments.DateCommented = DateTime.UtcNow;
            comments.UserId = matchUser.UserId;
            db.Comment.Add(comments);

            db.SaveChanges();
            CommentContent comment = new CommentContent();
            comment.UserName = UserName;
            comment.MessageId = comments.MessageId;
            comment.Comment = comments.Comment;
            comment.DateCommented = comments.DateCommented;
            var myhubContext = GlobalHost.ConnectionManager.GetHubContext<MyHub1>();
            myhubContext.Clients.All.NewComment(comment);

        }

        [HttpPost]
        public void SaveMessageToUser( string msgid)
        {
            
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            string UserName = authCookie["userName"].ToString();
            UserModel matchUser = new UserModel(UserName);
            SavedMessages savemessage = new SavedMessages();
            savemessage.MessageId = Convert.ToInt32(msgid);
            savemessage.UserId = matchUser.UserId;
            db.SavedMessages.Add(savemessage);
            db.SaveChanges();
           

        }


        [HttpPost]
        public void Like(string msgid)
        {



            int msgId=Convert.ToInt32 (msgid);
            Message message = db.Message.First(i => i.MessageId == msgId);
            message.LikedCount += 1;            
            db.SaveChanges();
            FeedContent messages = new FeedContent();
            messages.MessageId = message.MessageId;
            messages.DateMessaged = message.DateMessaged;
            messages.UserName = message.User.UserName;
            messages.MessageDesc = message.MessageDesc;
            messages.UserId = message.UserId;
            messages.liked = message.LikedCount;
            messages.disliked = message.Disliked;
            var myhubContext = GlobalHost.ConnectionManager.GetHubContext<MyHub1>();
            myhubContext.Clients.All.NewLike(messages);

        }
        [HttpPost]
        public void DisLike(string msgid)
        {



            int msgId = Convert.ToInt32(msgid);
            Message message = db.Message.First(i => i.MessageId == msgId);
            message.Disliked += 1;
            db.SaveChanges();
            FeedContent messages = new FeedContent();
            messages.MessageId = message.MessageId;
            messages.DateMessaged = message.DateMessaged;
            messages.UserName = message.User.UserName;
            messages.MessageDesc = message.MessageDesc;
            messages.UserId = message.UserId;
            messages.liked = message.LikedCount;
            messages.disliked = message.Disliked;
            var myhubContext = GlobalHost.ConnectionManager.GetHubContext<MyHub1>();
            myhubContext.Clients.All.NewDislike(messages);

        }


    }
}