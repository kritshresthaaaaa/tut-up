using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tutor.Infrastructure.Configuration
{
    public interface ISeeder
    {
        Task SeedDataAsync();
    }
}
