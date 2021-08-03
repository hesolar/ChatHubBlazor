using BlazorApp.Server.Data.Model.minesweeperLogic;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Data.Hubs {
    public class GameHub:Hub<MinesweeperLogic> {
        public const string HubUrl = "/game";
    }
}
