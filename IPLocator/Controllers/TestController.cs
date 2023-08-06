using IPLocator.Data;
using IPLocator.Helpers;
using IPLocator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace IPLocator.Controllers
{
    public class TestController : Controller
    {
        int _threadCount = 0;
        Object _lock = new object();
        private readonly ApplicationDbContext context;

        public TestController(ApplicationDbContext context)
        {
            this.context = context;
        }
        
        public string Index(string ip)
        {
            return ip;
        }

        [Authorize]
        public string genarateTLDTable()
        {
            List<string> list = new List<string>();
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo culture in cultures)
            {
                RegionInfo region = new RegionInfo(culture.Name);
                string tld = region.Name.ToLower();

                if (tld != null)
                {
                    context.Tldlists.Add(new Tldlist{ Tld = tld, Name = region.EnglishName, LanguageCode = culture.TwoLetterISOLanguageName, Language = culture.Name });
                    context.SaveChanges();
                }
                else
                {
                    continue;
                }
                //region.EnglishName;
                //culture.Parent.EnglishName;
                //region.TwoLetterISORegionName;
                //region.ThreeLetterISORegionName;
                //culture.LCID;

            }
            list.Sort();
            context.Dispose();
            return "";
        }

        [Authorize]
        public string genarateTable()
        {
            List<string> list = new List<string>();
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo culture in cultures)
            {
                RegionInfo region = new RegionInfo(culture.Name);
                string tld = region.Name.ToLower();
                var tldId = context.Tldlists.Where(d => d.Tld.Equals(tld)).FirstOrDefault();

                if (tldId != null)
                {
                    context.CountryLanguageCodes.Add(new CountryLanguageCode { TldlistId = tldId.Id, LanguageCode = culture.Name });
                    context.SaveChanges();
                }
                else
                {
                    continue;
                }
                //region.EnglishName;
                //culture.Parent.EnglishName;
                //region.TwoLetterISORegionName;
                //region.ThreeLetterISORegionName;
                //culture.LCID;

            }
            list.Sort();
            context.Dispose();
            return "";
        }

        public string ImportTLDFiles(string path)
        {

            if (String.IsNullOrEmpty(path))
                return "path null";

            var context = new ApplicationDbContext(null);

            foreach (string line in System.IO.File.ReadLines(path))
            {

                var parsedClass = line.Split(',');

                Tldlist listtld = new Tldlist
                {
                    Tld = parsedClass[1],
                    Name = parsedClass[0],
                    Language = null,
                    LanguageCode = null
                };


                context.Tldlists.Add(listtld);
                context.SaveChanges();
            }
            context.Dispose();
            return "OK";
        }


        public string ImportFiles(string path)
        {

            if (String.IsNullOrEmpty(path))
                return "path null";

            var fileList = Directory.GetFiles(path);
            var inserList = new List<CountryIPBlock>();
            foreach (var file in fileList)
            {
                foreach (string line in System.IO.File.ReadLines(file))
                {
                    //while (true)
                    //{
                    //    if (_threadCount > 50)
                    //    {
                    //        Thread.Sleep(200);
                    //        continue;
                    //    }

                    //    break;
                    //}
                    var parsedClass = line.Split('.');

                    var a = new CountryIPBlock
                    {
                        Cidrmask = line,
                        CountryCode = Path.GetFileNameWithoutExtension(file),
                        Aclass = parsedClass[0],
                        Bclass = parsedClass[1],
                        Cclass = parsedClass[2],
                        Dclass = parsedClass[3].Split('/')[0],
                        Subnet = parsedClass[3].Split('/')[1],
                    };
                    inserList.Add(a);
                    //StartTheThread(a);
                    //context.CountryIpblock.Add(a);
                    //context.SaveChanges();
                }
            }
            context.AddRange(inserList);
            context.SaveChanges();
            return "OK";
        }

        public void CreateIpBlock(CountryIPBlock addItem)
        {
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer("Server=ARAS;Database=IPLocator;Trusted_Connection=True;MultipleActiveResultSets=true");

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    context.CountryIpblocks.Add(addItem);
                    context.SaveChanges();
                    context.Dispose();
                }
            }
            catch
            {

            }

            _threadCount--;
        }

        public Thread StartTheThread(CountryIPBlock addItem)
        {
            lock (_lock)
                _threadCount++;

            var t = new Thread(() => CreateIpBlock(addItem));
            t.Start();

            return t;
        }
    }
}
