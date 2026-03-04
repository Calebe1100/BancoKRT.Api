using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BancoKRT.Api.Application.Accounts;
using Microsoft.AspNetCore.Mvc;

namespace BancoKRT.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<AccountDto>>> GetAll(CancellationToken cancellationToken)
        {
            var accounts = await _accountService.GetAllAsync(cancellationToken);
            return Ok(accounts);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AccountDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var account = await _accountService.GetByIdAsync(id, cancellationToken);
            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        [HttpPost]
        public async Task<ActionResult<AccountDto>> Create([FromBody] CreateAccountRequest request, CancellationToken cancellationToken)
        {
            var created = await _accountService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<AccountDto>> Update(Guid id, [FromBody] UpdateAccountRequest request, CancellationToken cancellationToken)
        {
            var updated = await _accountService.UpdateAsync(id, request, cancellationToken);
            if (updated == null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _accountService.DeleteAsync(id, cancellationToken);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
