using Interfaces.DTOs;
using Interfaces.DTOs.Basket_DTOs;
using Interfaces.Repository;
using Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class BasketService(IBasket repo) : IBasketService
    {
        public async Task<bool> Add(Add_ChangeToBasketDTO basket)
        {
            _ValidateOfAdd_Change(basket);

            return await repo.Add(basket);
        }


        public async Task<IEnumerable<BasketItemDTO>> GetBasketPaged(int userId, int pageNumber, int pageSize)
        {
            _ValidateOfGetPaged(userId, pageNumber, pageSize);
            return await repo.GetBasketPaged(userId, pageNumber, pageSize);
        }

        public async Task<bool> ChangeBasketItemQuantity(Add_ChangeToBasketDTO basket)
        {
            _ValidateOfAdd_Change(basket);

            return await repo.ChangeBasketItemQuantity(basket);
        }

        private bool _ValidateOfAdd_Change(Add_ChangeToBasketDTO basket)
        {
            if (basket == null) 
            {
                throw new ArgumentNullException("basket is null");

            }
            if (basket.Quantity < 1 || basket.ProductID < 1 || basket.UserID < 1 )
            {
                throw new ArgumentNullException("props of basket are null");
            }
            else
            { 
                return true;
            }
        }

        private bool _ValidateOfGetPaged(int userId, int pageNumber, int pageSize)
        {
            if (userId < 1 || pageNumber < 1 || pageSize < 1)
            {
                throw new ArgumentNullException("props are null");
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> DeleteAllBasketItem(int userId)
        {
            if (userId < 1)
            {
                throw new ArgumentNullException("userId is null");
            }
            return await repo.DeleteAllBasketItem(userId);
        }
    }
}
