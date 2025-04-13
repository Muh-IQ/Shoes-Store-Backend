using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    class Connection
    {
        private static string _connect = null;
        public static string connectionString { 
            get {
                if (string.IsNullOrEmpty(_connect))
                {
                    _connect = _getConnection();
                }
                return _connect;
            } 
        }
        private static string _getConnection()
        {
            var configuration = new ConfigurationBuilder()
                  .AddJsonFile(@"C:\Users\Asus\Desktop\My_ShoesStoreBackEnd-5\My_ShoesStoreBackEnd\DataAccess\dbconfig.json")  
                .Build();


            return configuration.GetConnectionString("DefaultConnection");
        }
    }
}
