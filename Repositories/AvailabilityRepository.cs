using Microsoft.EntityFrameworkCore;
using Repositories.Base;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories
{
    public class AvailabilityRepository : GenericRepository<ConsultantsAvailability>
    {
        public AvailabilityRepository() { }

    }
}
