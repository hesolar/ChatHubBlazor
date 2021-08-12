using Microsoft.AspNetCore.Components;
using Server.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Pages.BoardServiceContent {
    public class ScoreBoard: ComponentBase {

    [Parameter] public Partida[] partidas { get; set; }

    }
}
