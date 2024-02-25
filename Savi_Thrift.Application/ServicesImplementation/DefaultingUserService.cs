using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;

namespace Savi_Thrift.Application.ServicesImplementation
{
	public class DefaultingUserService: IDefaultingUserService
	{
        private readonly IUnitOfWork _unitOfWork;
        public DefaultingUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
