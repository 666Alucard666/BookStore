using BLL.Abstractions.ServiceInterfaces;
using Core.DTO_Models;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreUI.Controllers;

[Authorize]
[ApiController]
[Route("api/person")]
[Produces("application/json")]
public class PersonController : Controller
{
    private readonly IPersonService _personService;
    private readonly IUserService _userService;

    public PersonController(IPersonService personService, IUserService userService)
    {
        _personService = personService;
        _userService = userService;
    }
    
    [HttpGet("GetWorkerById")]
    public async Task<ActionResult<Worker>> GetWorkerById([FromQuery]Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest("Wrong user");
        }

        var b = await _personService.GetWorkerById(userId);
        if (b == null)
        {
            return BadRequest("None workers with this userId");
        }
        return Ok(b);
    }
    
    [HttpGet("GetCustomerById")]
    public async Task<ActionResult<Customer>> GetCustomerById([FromQuery]Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest("Wrong user");
        }

        var b = await _personService.GetCustomerById(userId);
        if (b == null)
        {
            return BadRequest("None customers with this userId");
        }
        return Ok(b);
    }
    
    [HttpGet("GetWorkersByShop")]
    public async Task<ActionResult<Worker>> GetWorkersByShop([FromQuery]Guid shopId)
    {
        if (shopId == Guid.Empty)
        {
            return BadRequest("Wrong shop");
        }

        var b = await _personService.GetWorkersByShop(shopId);
        if (b == null || !b.Any())
        {
            return BadRequest("None workers with this userId");
        }
        return Ok(b);
    }
    
    [HttpPut("UpdatePersons")]
    public async Task<ActionResult<bool>> UpdatePersons([FromBody]UpdateModel updateModel)
    {
        if (updateModel is null)
        {
            return BadRequest("Wrong data");
        }

        bool workRes = true, customerRes = true, deleteRes = true, createWork = true;

        if (updateModel.WorkersToUpdate is not null && updateModel.WorkersToUpdate.Any())
        {
            workRes = await _personService.UpdateWorkers(updateModel.WorkersToUpdate);
        }

        if (updateModel.CustomersToUpdate is not null && updateModel.CustomersToUpdate.Any())
        {
            customerRes = await _personService.UpdateCustomer(updateModel.CustomersToUpdate);
        }

        if (updateModel.WorkerToCreate is not null)
        {
            createWork = await _userService.RegisterAsync(updateModel.WorkerToCreate);
        }

        if (updateModel.WorkersToDelete is not null && updateModel.WorkersToDelete.Any())
        {
            foreach (var id in updateModel.WorkersToDelete)
            {
                deleteRes = await _personService.DeleteWorkerById(id);
            }
        }

        if (!workRes && !customerRes && !deleteRes && !createWork)
        {
            return BadRequest("None workers with this userId");
        }
        return Ok(true);
    }
}