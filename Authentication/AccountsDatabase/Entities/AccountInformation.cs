using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsDatabase.Entities
{
    public class AccountInformation
    {
        [Key]
        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }
    }
}
