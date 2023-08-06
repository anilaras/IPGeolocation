using IPLocator.Helpers;
using IPLocator.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using IPLocator.Data;
using IPLocator.Data.Repository.Concrete;
using IPLocator.Data.Repository.Abstracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IPLocator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeolocationController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IUserResponseCountRepository _userResponseCountRepository;
        private readonly IUserTokensRepository _userTokensRepository;
        public GeolocationController(ApplicationDbContext context, IUserResponseCountRepository responseCountRepository, IUserTokensRepository userTokensRepository)
        {
            this.context = context;
            _userResponseCountRepository = responseCountRepository;
            _userTokensRepository = userTokensRepository;
        }
        // GET api/<GeolocationController>/1.1.1.1/token
        [HttpGet("{ip}/{token}")]
        public string Get(string ip, string token)
        {

            var isTokenValid = context.UserTokens.Any(d => d.Token.Equals(token));
            if (!isTokenValid)
            {
                return "Not a valid token!";
            }
            else if(isTokenValid)
            {
                var user = _userTokensRepository.GetWhere(d => d.Token.Equals(token)).Select(d => d.IdentityUserId).FirstOrDefault();
                
                if(!_userResponseCountRepository.isUserHasAnyResponseBefore(user))
                {
                    _userResponseCountRepository.Create(new UserResponseCount
                    {
                        IdentityUserId = user,
                        ResponseCount = 1,
                        lastClearTime = DateTime.UtcNow
                        
                    });
                }
                else
                {
                    var userResponseCount = _userResponseCountRepository.GetWhere(d => d.IdentityUserId.Equals(user)).FirstOrDefault();
                    var userStatus = context.UserStatuses.Where(d => d.IdentityUserId.Equals(userResponseCount.IdentityUserId.ToString())).FirstOrDefault();
                    
                    if (userStatus != null && userStatus.StatusId  == 2 && (userResponseCount.ResponseCount >= 100))
                    {
                        if( DateTime.Compare(userResponseCount.lastClearTime.AddDays(1), DateTime.UtcNow) < 0 )
                        {
                            userResponseCount.ResponseCount= 0;
                            userResponseCount.lastClearTime = DateTime.UtcNow;
                        }
                        else
                        {
                            return "limit exceeded, consider to upgrade your plan to pro!";
                        }
                    }
                    else if (userStatus != null && userStatus.StatusId == 3 && (userResponseCount.ResponseCount >= 5000))
                    {
                        if (DateTime.Compare(userResponseCount.lastClearTime.AddDays(1), DateTime.UtcNow) < 0)
                        {
                            userResponseCount.ResponseCount = 0;
                            userResponseCount.lastClearTime = DateTime.UtcNow;

                        }
                        else
                        {
                            return "limit exceeded, consider to upgrade your plan to enterprise!";
                        }
                    } else if (userStatus != null && userStatus.StatusId == 4 && (userResponseCount.ResponseCount >= 10000))
                    {
                        if (DateTime.Compare(userResponseCount.lastClearTime.AddDays(1), DateTime.UtcNow) < 0)
                        {
                            userResponseCount.ResponseCount = 0;
                            userResponseCount.lastClearTime = DateTime.UtcNow;

                        }
                        else
                        {
                            return "limit exceeded, please contact with us!";
                        }
                    }
                    userResponseCount.ResponseCount++;
                    _userResponseCountRepository.Update(userResponseCount);
                }
            }
            

            var co = new IPCountryHelper(new CountryIPBlocksRepository(context) ).GetCountryCodeFromIp(ip);
            CountryLanguageCode? culturename = null;
            var culture = context.Tldlists.Where(d => d.Tld.Equals(co)).FirstOrDefault();
            if (culture != null)
            {
                culturename = context.CountryLanguageCodes.Where(d => d.TldlistId == culture.Id).FirstOrDefault();


            }
            if (culturename == null)
            {
                return "Not found";
            }
            RegionInfo region = new RegionInfo(culturename.LanguageCode);
            CultureInfo cultureInfo = CultureInfo.GetCultureInfo(culturename.LanguageCode);

            var cInfo = new
            {
                Name = cultureInfo.Name,
                EnglishName = cultureInfo.EnglishName,
                IetfLanguageTag = cultureInfo.IetfLanguageTag,
                NativeName = cultureInfo.NativeName,
                IsMetric = region.IsMetric,
                CurrencySymbol = region.CurrencySymbol,
                CurrencyEnglishName = region.CurrencyEnglishName,
                CurrencyNativeName = region.CurrencyNativeName,
                ThreeLetterISOLanguageName = cultureInfo?.ThreeLetterISOLanguageName,
                ThreeLetterWindowsLanguageName = cultureInfo?.ThreeLetterWindowsLanguageName,
                TwoLetterISOLanguageName = cultureInfo?.TwoLetterISOLanguageName,
                NumberFormat = cultureInfo?.NumberFormat,
                DateTimeFormat = cultureInfo?.DateTimeFormat,
            };

            string jsonString = JsonSerializer.Serialize(cInfo);
            

            context.Dispose();
            return jsonString;
        }
    }
}
