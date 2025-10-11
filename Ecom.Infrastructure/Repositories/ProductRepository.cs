using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly IImageManagementService imageManagementService;
        public ProductRepository(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService) : base(context)
        {
            this.context = context;
            this.mapper = mapper;
            this.imageManagementService = imageManagementService;
        }

        public async Task<bool> AddAsync(AddProductDTO productDTO)
        {
            if (productDTO is null)
                return false;

            var product = mapper.Map<Product>(productDTO);
            await context.Products.AddAsync(product);
             await context.SaveChangesAsync();

            //Handling Images
            var ImagePath = await imageManagementService
                .AddImageAsync(productDTO.Photos, product.Name);

            //Manual Mapping
            var photo = ImagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = product.Id,
            }).ToList();
            await context.Photos.AddRangeAsync(photo);
            await context.SaveChangesAsync();
            return true;

        }


        public async Task<bool> UpdateAsync(UpdateProductDTO UpdateProductDTO)
        {
            if (UpdateProductDTO is null)
                return false;
            var FindProduct = await context.Products
                .Include(m => m.Category)
                .Include(m => m.Photos)
                .FirstOrDefaultAsync(m => m.Id == UpdateProductDTO.Id);
            if (FindProduct is null)
                return false;
            //Map the fields
            mapper.Map(UpdateProductDTO, FindProduct);

            //Handling Images
            var FindPhotos = await context.Photos.Where(m => m.ProductId == UpdateProductDTO.Id).ToListAsync();

            foreach (var item in FindPhotos)
            {
                imageManagementService.DeleteImageAsync(item.ImageName);
            }
            context.Photos.RemoveRange(FindPhotos);

            //Add New Photos
            var ImagePath = await imageManagementService
                .AddImageAsync(UpdateProductDTO.Photos, FindProduct.Name);
            //Manual Mapping New Photo
            var photo = ImagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = UpdateProductDTO.Id,
            }).ToList();
            await context.Photos.AddRangeAsync(photo);
            await context.SaveChangesAsync();

            return true;

        }

        public async Task DeleteAsync(Product product)
        {
            //Delete Product Photos from Folder
            var Photo = await context.Photos.Where(m => m.ProductId == product.Id).ToListAsync();
            foreach (var item in Photo)
            {
                imageManagementService.DeleteImageAsync(item.ImageName);
            }
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }
    }
}
