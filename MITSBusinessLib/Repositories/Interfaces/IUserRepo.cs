using System;
using System.Collections.Generic;
using System.Text;
using MITSDataLib.Models;

namespace MITSBusinessLib.Repositories.Interfaces
{
    public interface IUserRepo
    {
        IEnumerable<Person> GetUsers();
    }
}
