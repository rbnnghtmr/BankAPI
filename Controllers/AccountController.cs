using Microsoft.AspNetCore.Mvc;
using BankAPI.Services;
using BankAPI.Data.BankModels;

namespace BankAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase{
    private readonly AccountService _service;
    private readonly ClientService _serviceC;
    public AccountController(AccountService service, ClientService clientService)
    {
        _service = service;
        _serviceC = clientService;
        
    }

    [HttpGet]
    public async Task<IEnumerable<Account>> Get()
    {
        return await _service.GetAll();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Account>> GetById(int id)
    {
        var account = await _service.GetById(id);
        if(account is null )
            return AccountNotFound(id);
        
        return account;
    }

    public ClientService GetServiceC()
    {
        return  _serviceC;
    }

    [HttpPost]
    public async Task<IActionResult> Create (AccountModel account)
    {
        
        if(account.ClientId is not null)
       {
            int numId = (int)account.ClientId;
            var clientToSearch = await _serviceC.GetId(numId);
            if (clientToSearch is null)
                 return BadRequest();
        }
       
        await _service.Create(account);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, AccountModel account)
    {
        if(id != account.Id)
            return BadRequest(new { message = $"El ID{id} de la URL no coincide con el ID{account.Id} del cuerpo de la solciitud."});
        
        var accountToUpdate = await _service.GetById(id);
        
        if(accountToUpdate is not null)
        {
            if(accountToUpdate.ClientId != account.ClientId)
            {
                int numId = (int)account.ClientId;
                var clientToSearch = await _serviceC.GetId(numId);
                if (clientToSearch is null)
                    return BadRequest();
               
            }
            await _service.Update(id, account);
            return NoContent();
        }
        else
           return AccountNotFound(id);
        
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var accountToDelete = await _service.GetById(id);
        if(accountToDelete is not null)
        {
            await _service.Delete(id);
            return Ok();
        }
        else
            return AccountNotFound(id);
    }
    
    public NotFoundObjectResult AccountNotFound(int id)
    {
        return NotFound(new { message = $"La cuenta con ese ID = {id} no existe."});
    }

}