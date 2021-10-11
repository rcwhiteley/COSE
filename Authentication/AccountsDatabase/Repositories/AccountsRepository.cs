using AccountsDatabase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsDatabase.Repositories
{
    public class AccountsRepository : IDisposable
    {
        private AuthenticationDbContext dbContext = new AuthenticationDbContext();
        public AccountsRepository(AuthenticationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(AccountInformation account)
        {
            dbContext.Accounts.Add(account);
            dbContext.SaveChanges();
        }

        public void Delete(AccountInformation account)
        {
            dbContext.Accounts.Remove(dbContext.Accounts.Find(account.Username));
            dbContext.SaveChanges();
        }

        public void Update(AccountInformation account)
        {
            var dbAccount = dbContext.Accounts.Find(account.Username);
            dbAccount.Username = account.Username;
            dbAccount.Password = account.Password;
            dbAccount.Email = account.Email;
            dbContext.SaveChanges();
        }

        public AccountInformation Find(string Username)
        {
            return dbContext.Accounts.Find(Username);
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
