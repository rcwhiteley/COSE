using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServiceModel
{
    public enum AuthenticationResponse
    {
        InvalidCredentials,
        OK,
        Banned
    }
}