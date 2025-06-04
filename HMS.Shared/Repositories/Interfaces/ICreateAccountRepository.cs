using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Shared.Repositories.Interfaces
{
    public interface ICreateAccountRepository
    {
        /// <summary>
        /// Interface for LogInRepository which does the following things:
        /// Makes the connection with the database in order to get information about the user
        /// useful for the login and for creating a new account.
        /// </summary>

        /// <summary>
        /// Creates a user account with the given information and adds it to the database.
        /// </summary>
        /// <param name="model_for_creating_user_account">The "model" for creating an account - domain.</param>
        /// <returns> 1 if the user account was created, 0 otherwise.</returns>
        /// <exception cref="AuthenticationException">Throws an exception if the user already exists
        /// or if there was a database error.</exception>
        Task<Patient> createAccount(PatientCreateDto model_for_creating_user_account);

    }
}
