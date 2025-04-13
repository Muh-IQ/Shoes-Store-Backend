using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;

namespace Business.Cloudinary_Service
{
    class Credentials
    {
        private static string _cloudName = string.Empty;
        private static string _apiSecretKey = string.Empty;
        private static string _apiKey  = string.Empty;

        public static Account GetAccount()
        {
            if (string.IsNullOrEmpty(_cloudName) || string.IsNullOrEmpty(_apiSecretKey) || string.IsNullOrEmpty(_apiKey) )
            {
                FillCredentials();
            }

            return new Account(_cloudName, _apiKey, _apiSecretKey);
        }

        private static void FillCredentials()
        {
            // get from json
            _cloudName = "dt4938k0j";
            _apiSecretKey = "Bj9_iZkdmqnT_BggtdyTB7o_-cw";
            _apiKey = "588516926714133";
        }
    }
}
