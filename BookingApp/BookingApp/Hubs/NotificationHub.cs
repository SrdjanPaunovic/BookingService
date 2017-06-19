using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;

namespace BookingApp.Hubs
{
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    using BookingApp.Models;

    using Microsoft.Ajax.Utilities;

    [HubName("notifications")]
    public class NotificationHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

        private static Dictionary<string, string> subscribed = new Dictionary<string, string>();
      //  private  List<Accomodation> needToApprove = new List<Accomodation>();
        private static List<Accomodation> ApprovedList = new List<Accomodation>();
        List<Accomodation> accList = new List<Accomodation>();

        private BAContext db = new BAContext();

        private Thread thread;

        public NotificationHub()
        {
            ApprovedList = this.db.Accomodations.Where(x => x.Approved).ToList();
        }

        public static void UpdateList(List<Accomodation> list )
        {
            ApprovedList = list;
        }
        public void Subscribe(string username, string role)
        {
            if (subscribed.Count == 0)
            {
                subscribed.Add(Context.ConnectionId, username);
                Groups.Add(Context.ConnectionId, role);
                this.thread = new Thread(ThreadLoop);
                this.thread.Start();
            }
            else
            {
                subscribed.Add(Context.ConnectionId, username);
                Groups.Add(Context.ConnectionId, role);
            }
        }

        private void ThreadLoop()
        {
            while (true)
            {
                Thread.Sleep(1000);
                this.CheckAccomodations();
            }
        }

        public void CheckAccomodations()
        {
            var needToApprove = this.db.Accomodations.Where(x => x.Approved == false);

            Clients.Group("Admin").checkForApproveAcc(needToApprove);
            //    Clients.Client(Context.ConnectionId).checkAccomodations(accList);

            if (ApprovedList.Count > 0)
            {
                this.accList.Clear();
                accList.AddRange(ApprovedList.Where(x => x.Approved).ToList());

                foreach (var acc in accList)
                {
                    AppUser user = this.db.AppUsers.FirstOrDefault(x => x.Id == acc.AppUser_Id);
                    if (user != null)
                    {
                        if (subscribed.ContainsValue(user.UserName))
                        {
                            Clients.Client(subscribed.FirstOrDefault(x => x.Value == user.UserName).Key).getApprovedAcc(acc);
                            ApprovedList.Remove(acc);
                        }
                    }
                    
                }
            }
        }

        public void Hello()
        {
            Clients.Client(Context.ConnectionId).hello("Hello from client");
            Clients.All.hello("Hello from ALL");
            hubContext.Clients.Group("Admin").hello("Hello from Admin");
        }

        public static void Notify(int clickCount)
        {
            hubContext.Clients.Group("Admins").clickNotification($"Clicks: {clickCount}");
        }

       
        public override Task OnDisconnected(bool stopCalled)
        {
            subscribed.Remove(Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }
    }
}