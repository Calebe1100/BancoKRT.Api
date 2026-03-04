using System;
using BancoKRT.Api.Domain.Accounts;

namespace BancoKRT.Api.Application.Accounts
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public string HolderName { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public AccountStatus Status { get; set; }
    }
}
