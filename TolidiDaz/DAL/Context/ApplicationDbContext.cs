using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Domain.Mapping;
using System.Reflection.Emit;
using System.Reflection;
using System.Security.Principal;

namespace DAL.Context
{
    public class ApplicationDbContext :  IdentityDbContext<Account>, IUnitOfWork
    {

        #region CreateDatabaeTablesList

        public DbSet<Category>? Category { get; set; }
        public DbSet<Product>? Product { get; set; }
        public DbSet<Province>? Province { get; set; }
        public DbSet<City>? City { get; set; }
        public DbSet<Customer>? Customer { get; set; }
        public DbSet<CustomerAddress>? CustomerAddress { get; set; }
        public DbSet<Reminder>? Reminder { get; set; }
        public DbSet<ReminderEvent>? ReminderEvent { get; set; }
        public DbSet<Setting>? Setting { get; set; }
        public DbSet<SmsLog>? SmsLog { get; set; }
        public DbSet<Story>? Story { get; set; }
        public DbSet<Slider>? Slider { get; set; }
        public DbSet<Advertisement>? Advertisement { get; set; }
        public DbSet<AdvertisementSingle>? AdvertisementSingle { get; set; }
        public DbSet<Cart>? Cart { get; set; }
        public DbSet<CartItem>? CartItem { get; set; }
        public DbSet<Order>? Order { get; set; }
        public DbSet<OrderItem>? OrderItem { get; set; }
        public DbSet<SmsOtpCode>? SmsOtpCode { get; set; }
        public DbSet<Wallet>? Wallet { get; set; }
        public DbSet<WalletTransaction>? WalletTransaction { get; set; }
        public DbSet<SendProductMethod>? SendProductMethod { get; set; }
        public DbSet<OrderPaymentTemp>? OrderPaymentTemp { get; set; }
        public DbSet<RefreshTokenEntity>? RefreshTokenEntity { get; set; }
        public DbSet<ProductFeature>? ProductFeature { get; set; }
        public DbSet<ProductFeatureValue>? ProductFeatureValue { get; set; }
        public DbSet<PricingRule>? PricingRule { get; set; }
        public DbSet<RawProduct>? RawProduct { get; set; }
        public DbSet<RawProductStore>? RawProductStore { get; set; }
        public DbSet<Position>? Position { get; set; }
        public DbSet<Product_CountAction_CostType>? Product_CountAction_CostType { get; set; }
        public DbSet<RegisterCostRawProductStore>? RegisterCostRawProductStore { get; set; }
        public DbSet<RawProductStore_Product>? RawProductStore_Product { get; set; }
        public DbSet<Personel>? Personel { get; set; }
        public DbSet<FavoritUserProduct>? FavoritUserProduct { get; set; }
        public DbSet<ContactUs>? ContactUs { get; set; }
        public DbSet<Team>? Team { get; set; }
        public DbSet<About>? About { get; set; }
        public DbSet<Faq>? Faq { get; set; }
        public DbSet<ProductComment>? ProductComment { get; set; }
        public DbSet<GroupQuestion>? GroupQuestion { get; set; }
        public DbSet<Question>? Question { get; set; }
        public DbSet<Ticket>? Ticket { get; set; }

        #endregion
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                  .UseSqlServer(ConnectionString.Value)
                  .UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //base.OnModelCreating(builder);
            base.OnModelCreating(builder);
              builder.Seed();
           // builder.HasDefaultSchema("dbo");
            //builder.HasDefaultSchema("tolidiayhan_user");
            builder.ApplyConfiguration(new AccountMap());
            builder.ApplyConfiguration(new CustomerMap());
            builder.ApplyConfiguration(new SmsLogMap());
            builder.ApplyConfiguration(new CategoryMap());
            builder.ApplyConfiguration(new ProductMap());
            builder.ApplyConfiguration(new CartItemMap());
            builder.ApplyConfiguration(new CartMap());
            builder.ApplyConfiguration(new OrderItemMap());
            builder.ApplyConfiguration(new OrderMap());
            builder.ApplyConfiguration(new BlogMap());
            builder.ApplyConfiguration(new CityMap());
            builder.ApplyConfiguration(new CustomerAddressMap());
            builder.ApplyConfiguration(new SmsLogMap());
            builder.ApplyConfiguration(new WalletMap());
            builder.ApplyConfiguration(new WalletTransactionMap());
            builder.ApplyConfiguration(new RefreshTokenEntityMap());
            builder.ApplyConfiguration(new ProductFeatureMap());
            builder.ApplyConfiguration(new ProductFeatureValueMap());
            builder.ApplyConfiguration(new PricingRuleMap());
            builder.ApplyConfiguration(new Product_CountAction_CostTypeMap());
            builder.ApplyConfiguration(new RawProductStoreMap());
            builder.ApplyConfiguration(new RegisterCostRawProductStoreMap());
            builder.ApplyConfiguration(new FavoritUserProductMap());
            builder.ApplyConfiguration(new PersonelMap());
            builder.ApplyConfiguration(new ProductCommentMap());
            builder.ApplyConfiguration(new BlogCommentMap());
            builder.ApplyConfiguration(new TicketMap());
        }
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken);
        }
        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
    }
}