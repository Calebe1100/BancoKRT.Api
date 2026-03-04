using System;

namespace BancoKRT.Api.Domain.Accounts
{
    public class Account
    {
        public Guid Id { get; private set; }
        public string HolderName { get; private set; }
        public string Cpf { get; private set; }
        public AccountStatus Status { get; private set; }

        public Account(Guid id, string holderName, string cpf, AccountStatus status)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            HolderName = holderName;
            Cpf = cpf;
            Status = status;
        }

        public void Update(string holderName, string cpf, AccountStatus status)
        {
            HolderName = holderName;
            Cpf = cpf;
            Status = status;
        }
    }
}
