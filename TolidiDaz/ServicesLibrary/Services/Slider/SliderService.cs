using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.SliderSrv
{
    public sealed class SliderService : Repository<Slider>, ISliderService
    {
        public SliderService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}