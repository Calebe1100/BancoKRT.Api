using System.ComponentModel.DataAnnotations;
using BancoKRT.Api.Domain.Accounts;

namespace BancoKRT.Api.Application.Accounts
{
    public class CreateAccountRequest
    {
        [Required]
        [MaxLength(200)]
        public string HolderName { get; set; } = string.Empty;

        [Required]
        [MaxLength(11)]
        [MinLength(11)]
        public string Cpf { get; set; } = string.Empty;

        public AccountStatus Status { get; set; } = AccountStatus.Ativa;
    }
}
