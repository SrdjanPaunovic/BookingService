using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;

namespace BookingApp.Hubs
{
    using System.Threading.Tasks;

    using BookingApp.Models;

    [HubName("notifications")]
    public class NotificationHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

        private static Dictionary<string, string> subscribed = new Dictionary<string, string>();

        private BAContext db = new BAContext();


        public void Subscribe(string username, string role)
        {
            subscribed.Add(Context.ConnectionId, username);
            Groups.Add(Context.ConnectionId, role);
        }

        public void Completed()
        {
            for (int i = 0; i < 10; i++)
            {
                this.CheckAccomodations();
            }
        }

        public void CheckAccomodations()
        {
            var accList = this.db.Accomodations.Where(x => x.Approved == false).ToList();

            Clients.Client(Context.ConnectionId).checkAccomodations(accList);
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

    }
}