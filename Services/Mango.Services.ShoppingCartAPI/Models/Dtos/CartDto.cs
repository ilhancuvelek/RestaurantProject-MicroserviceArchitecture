namespace Mango.Services.ShoppingCartAPI.Models.Dtos
{
    public class CartDto
    {
        public CartHeaderDto CartHeaderDto { get; set; }
        public IEnumerable<CartDetailsDto> CartDetailsDto { get; set; }
    }
}
