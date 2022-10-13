using Microsoft.CodeAnalysis.CSharp.Syntax;
using MoneyManager.Models;
using MoneyManager.Models.DTOs;

namespace MoneyManager.Utility
{
    public static class ConverToDTO
    {
        public static UserDTO ConverToUserDTO(this ApplicationUser user) => new UserDTO() { FullName = $"{user.FirstName} {user.LastName}"};
        public static ItemDTO ConverToItemsDTO(this Item item) => new ItemDTO()
        {
            Id = item.Id,
            Type = item.Type,
            Name = item.Name,
            Price = item.Price,
            TransactionDate = item.TransactionDate,
        };
        public static ItemDetailsDTO ConverToItemDetailsDTO(this Item item)
        {
            var detailsDTO = new ItemDetailsDTO();

            if (item is Income)
            {
                Income income = item as Income;
                detailsDTO.Id = income.Id;
                detailsDTO.Name = income.Name;
                detailsDTO.Price = income.Price;
                detailsDTO.Category = income.IncomeCategory;
                detailsDTO.Type = income.Type;
                detailsDTO.TransactionDate = income.TransactionDate;
            }
            else
            {
                Outcome outcome = item as Outcome;
                detailsDTO.Id = outcome.Id;
                detailsDTO.Name = outcome.Name;
                detailsDTO.Price = outcome.Price;
                detailsDTO.Category = outcome.OutcomeCategory;
                detailsDTO.Type = outcome.Type;
                detailsDTO.TransactionDate = outcome.TransactionDate;
            }

            return detailsDTO;
        }
    }
}
