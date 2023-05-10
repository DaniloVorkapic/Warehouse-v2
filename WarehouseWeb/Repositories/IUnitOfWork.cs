using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WarehouseWeb.Repositories
{
  public  interface IUnitOfWork:IDisposable
    {
        int commit();
        IDbContextTransaction myTransaction();
    }
}
