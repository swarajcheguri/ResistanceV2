using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading;
using System.Threading.Tasks;
using ResistanceV2.Models;
namespace ResistanceV2
{
    public class MyHub1 : Hub
    {
      public MyHub1()
        {
            // Create a Long running task to do an infinite loop which will keep sending the server time
            // to the clients every 3 seconds.
            var taskTimer = Task.Factory.StartNew(async () =>
                {
                    while(true)
                    {
                        string timeNow = DateTime.Now.ToString();
                        //Sending the server time to all the connected clients on the client method SendServerTime()
                        Clients.All.SendServerTime(timeNow);
                        //Delaying by 3 seconds.
                        await Task.Delay(3000);
                    }
                }, TaskCreationOptions.LongRunning
                );
        }
        public void HelloServer()
        {
            Clients.All.hello("Hello message to all clients");
        }
        public void NewMessage(FeedContent msg)
        {
            Clients.All.NewMessage("Hello message to all clients");
        }
        public void NewComment(CommentContent comments)
        {
            Clients.All.NewComment("Hello message to all clients");
        }
        public void NewLike(FeedContent msg)
        {
            Clients.All.NewLike("Hello message to all clients");
        }
        
        public void NewDislike(FeedContent msg)
        {
            Clients.All.NewDislike("Hello message to all clients");
        }
    }
}