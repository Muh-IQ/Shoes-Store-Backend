using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;

namespace ShoesStore_Project.Extention
{
    public class Utility
    {
        public static Dictionary<int, int> ConvertSizeQuantityToDictionary(string value)
        {
            Dictionary<int, int> sizeQuantitiesObj;
            try
            {
                sizeQuantitiesObj = JsonConvert.DeserializeObject<Dictionary<int, int>>(value);

            }
            catch (Exception ex)
            {
                throw new Exception("Error parsing SizeQuantities: " + ex.Message);
            }
            return sizeQuantitiesObj;
        }
    }
}
