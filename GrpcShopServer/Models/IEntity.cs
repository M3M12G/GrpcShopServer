using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcShopServer.Models
{
    public interface IEntity
    {
        int Id { get; set; }
    }
}
