using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Local.Infrastructure.Repository
{
    public interface IRepositoryItem
    {
        public string Id { get; set; }
    }
}
