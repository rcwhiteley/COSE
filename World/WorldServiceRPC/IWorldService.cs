using System;
using System.Collections.Generic;
using System.Text;
using WorldServiceContracts.Net;

namespace WorldServiceContracts
{
    public class WorldService
    {
        ILoginAuthorizationService LoginAuthorizationService { get; set; }
    }
}
