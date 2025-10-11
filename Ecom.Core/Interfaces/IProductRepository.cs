using Ecom.Core.DTO;
using Ecom.Core.Entities.Product;
using Ecom.Core.Sharing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        //For Future Product Specific Methods

        //Get Products Sorted
        Task<IEnumerable<ProductDTO>> GetAllAsync(ProductParams productParams);
        //For adding productDTO
        Task<bool> AddAsync(AddProductDTO productDTO);
        Task<bool> UpdateAsync(UpdateProductDTO UpdateProductDTO);

        Task DeleteAsync(Product product);
    }
}
