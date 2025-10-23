using AutoMapper;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IMapper mapper;
        private readonly IConnectionMultiplexer redis;
        private readonly IImageManagementService imageManagementService;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IEmailServices emailServices;
        private readonly IGenerateTokenService token;

        public ICategoryRepository CategoryRepository { get; }

        public IProductRepository ProductRepository { get; }

        public IPhotoRepository PhotoRepository { get; }

        public ICustomerBasketRepository CustomerBasketRepository { get; }

        public IAuth Auth { get; }

        public UnitOfWork(AppDbContext context, IImageManagementService imageManagementService, IMapper mapper,
                          IConnectionMultiplexer redis, UserManager<AppUser> userManager,
                          SignInManager<AppUser> signInManager, IEmailServices emailServices, IGenerateTokenService token)
        {
            _context = context;
            this.imageManagementService = imageManagementService;
            this.mapper = mapper;
            this.redis = redis;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailServices = emailServices;
            this.token = token;
            CategoryRepository = new CategoryRepository(_context);
            ProductRepository = new ProductRepository(_context, mapper, imageManagementService);
            PhotoRepository = new PhotoRepository(_context);
            CustomerBasketRepository = new CustomerBasketRepository(redis);
            Auth = new AuthRepository(userManager, emailServices, signInManager ,token);
        }
    }
}
