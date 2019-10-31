using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchivosBaseDeDatos.Hubs
{
    public class MainHub : Hub
    {
        public async Task CheckGroupTray()
        {
            await Clients.All.SendAsync("CheckGroupTray");
        }
    }
}
