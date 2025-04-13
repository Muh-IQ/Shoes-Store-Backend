using Interfaces.DTOs;
using Interfaces.DTOs.Basket_DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Services
{
    public interface IBasketService
    {
        Task<bool> Add(Add_ChangeToBasketDTO basket);
        Task<IEnumerable<BasketItemDTO>> GetBasketPaged(int userId, int pageNumber, int pageSize);
        Task<bool> ChangeBasketItemQuantity(Add_ChangeToBasketDTO basket);
        Task<bool> DeleteAllBasketItem(int userId);

    }
}
