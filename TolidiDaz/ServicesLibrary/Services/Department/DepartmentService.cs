using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.DepartmentSrv
{
    public sealed class DepartmentService : Repository<Department>, IDepartmentService
    {
        public DepartmentService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}