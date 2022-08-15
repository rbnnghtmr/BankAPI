using Microsoft.EntityFrameworkCore;
using BankAPI.Data;
using BankAPI.Data.BankModels;

namespace BankAPI.Services;

public class AccountService{
    private readonly BankContext _context;

    public AccountService(BankContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Account>> GetAll()
    {
        return await _context.Accounts.ToListAsync();
    }

    public async Task<Account?> GetById(int id)
    {
        return await _context.Accounts.FindAsync(id);
    }

     public async Task<Account> Create(AccountModel newAccountModel)
    {
        Account newAccount = new Account();
        newAccount.AccountType=newAccountModel.AccountType;
        newAccount.ClientId=newAccountModel.ClientId;
        newAccount.Balance=newAccountModel.Balance;
        _context.Accounts.Add(newAccount);
        await _context.SaveChangesAsync();
        
        return newAccount;
        
    }

    public async Task Update(int id, AccountModel account)
    {
        var existingAccount = await GetById(id);

        if(existingAccount is not null)
        {
            existingAccount.AccountType = account.AccountType;
            existingAccount.ClientId = account.ClientId;
            existingAccount.Balance = account.Balance;

           await _context.SaveChangesAsync();
        }
        
    }

    public async Task Delete(int id)
    {
        var accountToDelete = await GetById(id);

        if(accountToDelete is not null)
        {
            _context.Accounts.Remove(accountToDelete);
            await _context.SaveChangesAsync();
        }
        
    }

   /* public Client? ExistingIDClient(int id)
    {
        return _context.Clients.Find(id);
    }
    */
}