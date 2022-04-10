using FriendsApi.DTOs;
using FriendsApi.Helpers;
using FriendsApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FriendsApi.Interface
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByNameAsync(string userName);
        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
        Task<MemberDto> GetMemberAsync(string userName);

    }
}
