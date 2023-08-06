using IPLocator.Data.Repository.Abstracts;
using IPLocator.Models;
using System.Net;
using System.Text.RegularExpressions;

namespace IPLocator.Helpers
{
    public class IPCountryHelper
    {
        ICountryIPBlocksRepository _repoCountryIPBlock;
        private enum IPClass
        {
            A = 0, B = 1, C = 2, D = 3
        }

        public IPCountryHelper(ICountryIPBlocksRepository countryIPBlocksRepository)
        {
            _repoCountryIPBlock = countryIPBlocksRepository;
        }

        public static bool IsValidIP(string addr)
        {
            //create our match pattern
            string pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.
([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
            //create our Regular Expression object
            Regex check = new Regex(pattern);
            //boolean variable to hold the status
            bool valid = false;
            //check to make sure an ip address was provided
            if (addr == "")
            {
                //no address provided so return false
                valid = false;
            }
            else
            {
                //address provided so use the IsMatch Method
                //of the Regular Expression object
                valid = check.IsMatch(addr, 0);
            }
            //return the results
            return valid;
        }

        public static bool IsInRange(string ipAddress, string CIDRmask)
        {
            string[] parts = CIDRmask.Split('/');

            int IP_addr = BitConverter.ToInt32(IPAddress.Parse(ipAddress).GetAddressBytes(), 0);
            int CIDR_addr = BitConverter.ToInt32(IPAddress.Parse(parts[0]).GetAddressBytes(), 0);
            int CIDR_mask = IPAddress.HostToNetworkOrder(-1 << (32 - int.Parse(parts[1])));

            return ((IP_addr & CIDR_mask) == (CIDR_addr & CIDR_mask));
        }

        public string GetCountryCodeFromIp(string ip)
        {
            var parsedIP = ip.Split('.');
            var ParsedA = parsedIP[(int)IPClass.A];
            var ParsedB = parsedIP[(int)IPClass.B];
            var ParsedC = parsedIP[(int)IPClass.C];
            var ParsedD = parsedIP[(int)IPClass.D];

            var AClassCount = _repoCountryIPBlock.GetWhere(d => d.Aclass == ParsedA).Count();
            var BClassCount = _repoCountryIPBlock.GetWhere(d => d.Aclass == ParsedA).Where(d => d.Bclass == ParsedB).Count();
            var CClassCount = _repoCountryIPBlock.GetWhere(d => d.Aclass == ParsedA).Where(d => d.Bclass == ParsedB).Where(d => d.Cclass == ParsedC).Count();
            var DClassCount = _repoCountryIPBlock.GetWhere(d => d.Aclass == ParsedA).Where(d => d.Bclass == ParsedB).Where(d => d.Cclass == ParsedC).Where(d => d.Cclass == ParsedD).Count();

            List<CountryIPBlock> CiP = new List<CountryIPBlock>();
            if (AClassCount > 1 && BClassCount > 1 && CClassCount > 1)
            {
                CiP = _repoCountryIPBlock.GetAll().Where(d => d.Aclass == ParsedA && d.Bclass == ParsedB && d.Cclass == ParsedC).ToList();
            }
            else if (AClassCount > 1 && BClassCount > 1 && CClassCount <= 1)
            {
                CiP = _repoCountryIPBlock.GetAll().Where(d => d.Aclass == ParsedA && d.Bclass == ParsedB).ToList();
            }
            else if (AClassCount > 1 && BClassCount <= 1)
            {
                CiP = _repoCountryIPBlock.GetAll().Where(d => d.Aclass == ParsedA).ToList();
            }

            foreach (var item in CiP)
            {
                if (IsInRange(ip, item.Cidrmask))
                    return item.CountryCode;
            }


            return "";
        }
    }
}
