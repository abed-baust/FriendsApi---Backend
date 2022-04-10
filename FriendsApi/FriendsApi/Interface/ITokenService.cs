using FriendsApi.Models;

namespace FriendsApi.Interface
{
    public interface ITokenService
    {
        string CreateToken(AppUser user)
 ;   }
}
