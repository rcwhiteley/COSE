using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServiceModel
{
    public enum AccountAuthorization
    {
        Banned,
        Warned,
        User,
        Moderator,
        GameMaster,
        Admin
    }
}
