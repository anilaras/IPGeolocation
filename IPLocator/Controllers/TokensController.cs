using IPLocator.Helpers;
using IPLocator.Data;
using IPLocator.Helpers;
using IPLocator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IPLocator.Controllers
{
    [Authorize]
    public class TokensController : Controller
    {
        ApplicationDbContext dbContext;
        public TokensController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }


        public string GenerateToken()
        {
            var token = UtilsHelper.Hash(Guid.NewGuid().ToString());

            var a = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userTokenCount = dbContext.UserTokens.Where(x => x.IdentityUserId.Equals(Guid.Parse(a))).Select(d => d.Token).Count();
            if (userTokenCount > 1)
            {
                return "you can't generate more token"!;
            }
            dbContext.UserTokens.Add(new UserTokens
            {
                IdentityUserId = Guid.Parse(a),
                Token = token,
            });
            dbContext.SaveChanges();
            return token;
        }

        public string DeleteToken(string token)
        {
            //Not implemented
            return token;
        }

        public string GetUserToken()
        {
            var a = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userToken = dbContext.UserTokens.Where(x => x.IdentityUserId.Equals(Guid.Parse(a))).Select(d=> d.Token).FirstOrDefault();

            if (string.IsNullOrEmpty(userToken))
            {
                return "NOTOKEN";
            }
            return userToken;
        }

        public string RefreshUserToken()
        {
            var a = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userToken = dbContext.UserTokens.Where(x => x.IdentityUserId.Equals(Guid.Parse(a))).FirstOrDefault();
            var newToken = UtilsHelper.Hash(Guid.NewGuid().ToString());
            userToken.Token = newToken;
            dbContext.UserTokens.Update(userToken);
            dbContext.SaveChanges();
            return newToken;
        }
    }
}
