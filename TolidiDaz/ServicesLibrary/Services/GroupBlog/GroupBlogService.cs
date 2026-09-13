using DAL.Context;
using Domain;

namespace ServicesLibrary.Services.GroupBlogSrv
{
    public sealed class GroupBlogService : Repository<GroupBlog>, IGroupBlogService
    {
        public GroupBlogService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

    }
}