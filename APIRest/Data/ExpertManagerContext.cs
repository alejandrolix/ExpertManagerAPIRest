using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.Context
{
    public class ExpertManagerContext : DbContext
    {
        public ExpertManagerContext(DbContextOptions<ExpertManagerContext> options) : base(options)
        {
        }
    }
}
