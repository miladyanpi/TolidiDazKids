using AutoMapper;
using Domain;
using Dto.Enum;
using Dto.Models.DtoAbout;
using Dto.Models.DtoAdvertisement;
using Dto.Models.DtoAdvertisementSingle;
using Dto.Models.DtoBlog;
using Dto.Models.DtoBlogComment;
using Dto.Models.DtoCart;
using Dto.Models.DtoCartItem;
using Dto.Models.DtoCategory;
using Dto.Models.DtoCity;
using Dto.Models.DtoContactUs;
using Dto.Models.DtoCustomer;
using Dto.Models.DtoCustomerAddress;
using Dto.Models.DtoDepartment;
using Dto.Models.DtoFaq;
using Dto.Models.DtoFavoritUserProduct;
using Dto.Models.DtoGroupBlog;
using Dto.Models.DtoGroupQuestion;
using Dto.Models.DtoLable;
using Dto.Models.DtoOrder;
using Dto.Models.DtoOrderItem;
using Dto.Models.DtoOrderPaymentTemp;
using Dto.Models.DtoPersonel;
using Dto.Models.DtoPosition;
using Dto.Models.DtoPricingRule;
using Dto.Models.DtoProduct;
using Dto.Models.DtoProduct_CountAction_CostType;
using Dto.Models.DtoProductComment;
using Dto.Models.DtoProductFeature;
using Dto.Models.DtoProductFeatureValue;
using Dto.Models.DtoProvince;
using Dto.Models.DtoQuestion;
using Dto.Models.DtoRawProduct;
using Dto.Models.DtoRawProductStore;
using Dto.Models.DtoRawProductStore_Product;
using Dto.Models.DtoRefreshTokenEntity;
using Dto.Models.DtoRegisterCostRawProductStore;
using Dto.Models.DtoSendProductMethod;
using Dto.Models.DtoSetting;
using Dto.Models.DtoSlider;
using Dto.Models.DtoSmsOtpCode;
using Dto.Models.DtoStory;
using Dto.Models.DtoTeam;
using Dto.Models.DtoTicket;
using Dto.Models.DtoUploadFile;
using Dto.Models.DtoWallet;
using Dto.Models.DtoWalletTransaction;
using MappingProfile.FrpRoot;
using Newtonsoft.Json;
using Utility;
using static Dto.Enum.EnumConstant;
namespace MappingProfile.DtoMappingConfigs
{
    public class DtoMappingProfile : Profile
    {

        public DtoMappingProfile()
        {

            #region Category
            CreateMap<AddCategory, Category>().ConvertUsing(x => new Category
            {
                Title = x.Title,
                ParentID = x.ParentID1,
                Order = x.Order,
                Visible = x.Visible,
                JsonPicture = x.JsonPicture,
                Description = x.Description,

                IdentityCode = Guid.NewGuid(),
                RegisterDate = DateFunctions.GetDateNow(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditDate = DateFunctions.GetDateNow(),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),

            });
            CreateMap<UpdateCategory, Category>().ConvertUsing(x => new Category
            {
                ID = x.ID,
                Title = x.Title,
                ParentID = x.ParentID == 0 ? null : x.ParentID,
                Order = x.Order,
                Visible = x.Visible,
                JsonPicture = x.JsonPicture,
                Description = x.Description,

                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.GetDateNow(),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),

            });
            CreateMap<Category, UpdateCategory>().ConvertUsing(x => new UpdateCategory
            {
                ID = x.ID,
                Title = x.Title,
                ParentID1 = x.ParentID,

                Order = x.Order,
                Visible = x.Visible,
                JsonPicture = x.JsonPicture,
                Description = x.Description,

                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                ParentResultCategory = x.Parent != null ? new ResultCategory
                {
                    ID = x.Parent.ID,
                    Title = x.Parent.Title,
                    ParentID = x.Parent.ParentID,
                    Order = x.Parent.Order,
                    Visible = x.Parent.Visible,
                    JsonPicture = x.Parent.JsonPicture,
                    Description = x.Parent.Description,
                    Count = x.Parent.Categories.Count(),
                    IdentityCode = x.Parent.IdentityCode,

                } : new ResultCategory(),
                EditTime = x.EditTime,

            });
            CreateMap<Category, ResultCategory>().ConvertUsing(x => new ResultCategory
            {
                ID = x.ID,
                Title = x.Title,
                ParentID = x.ParentID,
                Order = x.Order,
                Visible = x.Visible,
                JsonPicture = x.JsonPicture,
                Description = x.Description,
                Count = x.Categories.Count(),

                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                EditTime = x.EditTime,
                ResultUploadFiles = GetResultUploadFiles(x.JsonPicture),
                ResultCategorys = x.Categories != null && x.Categories.Count > 0 ? x.Categories.Select(s => new ResultCategory
                {
                    ID = s.ID,
                    Title = s.Title,
                    ParentID = s.ParentID,
                    Order = s.Order,
                    Visible = s.Visible,
                    JsonPicture = s.JsonPicture,
                    Description = s.Description,
                    Count = s.Categories.Count(),
                    IdentityCode = s.IdentityCode,
                    ResultCategorys = s.Categories != null && s.Categories.Count > 0 ? s.Categories.Select(k => new ResultCategory
                    {
                        ID = k.ID,
                        Title = k.Title,
                        ParentID = k.ParentID,
                        Order = k.Order,
                        Visible = k.Visible,
                        JsonPicture = k.JsonPicture,
                        Description = k.Description,
                        Count = k.Categories.Count(),
                        IdentityCode = k.IdentityCode,
                    }).ToList() : new List<ResultCategory>(),
                }).ToList() : new List<ResultCategory>(),
            });

            #endregion
            #region Product
            CreateMap<AddProduct, Product>().ConvertUsing(x => new Product
            {
                Title = x.Title,
                Count = x.Count,
                Price = x.Price,
                CategoryID = x.CategoryID,
                Visible = x.Visible,
                JsonPicture = x.JsonPicture,
                ShortDescription = x.ShortDescription,
                Description = x.Description,
                Discount = x.Discount,
                Brand = x.Brand,
                ProductCode = x.ProductCode,
                ProductExistStatus = (int)x.ProductExistStatus,
                SkuCode = x.SkuCode,

                IdentityCode = Guid.NewGuid(),
                RegisterDate = DateFunctions.GetDateNow(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditDate = DateFunctions.GetDateNow(),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),

            });
            CreateMap<UpdateProduct, Product>().ConvertUsing(x => new Product
            {
                ID = x.ID,
                Title = x.Title,
                Count = x.Count,
                Price = x.Price,
                CategoryID = x.CategoryID,
                Visible = x.Visible,
                JsonPicture = x.JsonPicture,
                ShortDescription = x.ShortDescription,
                Description = x.Description,
                Discount = x.Discount,
                Brand = x.Brand,
                ProductCode = x.ProductCode,
                ProductExistStatus = (int)x.ProductExistStatus,
                SkuCode = x.SkuCode,
                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.GetDateNow(),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),

            }); ;
            CreateMap<Product, UpdateProduct>().ConvertUsing(x => new UpdateProduct
            {
                ID = x.ID,
                Title = x.Title,
                Count = x.Count,
                Price = x.Price,
                CategoryID = x.CategoryID,
                Visible = x.Visible,
                JsonPicture = x.JsonPicture,
                ShortDescription = x.ShortDescription,
                Description = x.Description,
                Discount = x.Discount,
                Brand = x.Brand,
                ProductCode = x.ProductCode,
                ProductExistStatus = (ProductExistStatus)x.ProductExistStatus,
                SkuCode = x.SkuCode,
                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                EditTime = x.EditTime,
                ResultCategory = x.Category != null ? new ResultCategory
                {
                    ID = x.Category.ID,
                    Title = x.Category.Title,
                    ParentID = x.Category.ParentID,
                } : new ResultCategory()


            });
            CreateMap<Product, ResultProduct>().ConvertUsing(x => new ResultProduct
            {
                ID = x.ID,
                Title = x.Title,
                Count = x.Count,
                Price = x.Price,
                CategoryID = x.CategoryID,
                Visible = x.Visible,
                JsonPicture = x.JsonPicture,
                ShortDescription = x.ShortDescription,
                Description = x.Description,
                Discount = x.Discount,
                Brand = x.Brand,
                ProductCode = x.ProductCode,
                ViewCount = x.ViewCount,
                ProductExistStatus = GetProductExistStatus(x.ProductExistStatus),
                ProductExistStatus2 = (ProductExistStatus)x.ProductExistStatus,
                SkuCode = x.SkuCode,
                ResultUploadFiles = GetResultUploadFiles(x.JsonPicture),
                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                EditTime = x.EditTime,
                RatingAverage = CalCulatorAverageRatingProductComment(x.ProductComments),
                ProductCommentCount = x.ProductComments != null ? x.ProductComments.Count() : 0,
                ResultCategory = x.Category != null ? new ResultCategory
                {
                    ID = x.Category.ID,
                    Title = x.Category.Title,
                    ParentID = x.Category.ParentID,
                    Description = x.Category.Description,
                    Visible = x.Category.Visible,

                } : new ResultCategory(),
                ResultPricingRules = x.PricingRules != null && x.PricingRules.Count > 0 ? x.PricingRules.Select(k => new ResultPricingRule
                {
                    ID = k.ID,
                    ProductID = k.ProductID,
                    Price = k.Price,
                    MinQuantity = k.MinQuantity,
                    RoleID = k.RoleID,
                    RuleType = EnumConstant.GetTitleRuleType(k.RuleType),
                    RuleType2 = k.RuleType,
                    Title = k.Title,
                    FromDate = DateFunctions.ConvertDateIntToString(k.FromDate),
                    ToDate = DateFunctions.ConvertDateIntToString(k.ToDate),
                    Visible = k.Visible,

                    RegisterTime = k.RegisterTime,
                    EditTime = k.EditTime,
                    RegisterDate = DateFunctions.ConvertDateIntToString(k.RegisterDate),
                    EditDate = DateFunctions.ConvertDateIntToString(k.EditDate),

                }).ToList() : new List<ResultPricingRule>()
            });
            #endregion
            #region Setting
            CreateMap<AddSetting, Setting>().ConvertUsing(x => new Setting
            {
                Name = x.Name,
                CountShowRecord = x.CountShowRecord,
                JsonPicture = x.JsonPicture,
                UserName = x.UserName,
                Password = x.Password,
                PhoneSender = x.PhoneSender,
                CurrencyUnit = x.CurrencyUnit,
                Email = x.Email,
                JsonTel = x.JsonTel,
                JsonMobile = x.JsonMobile,
                Address = x.Address,

                IdentityCode = Guid.NewGuid(),
                RegisterDate = DateFunctions.GetDateNow(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditDate = DateFunctions.GetDateNow(),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),

            });
            CreateMap<UpdateSetting, Setting>().ConvertUsing(x => new Setting
            {
                ID = x.ID,
                Name = x.Name,
                CountShowRecord = x.CountShowRecord,
                JsonPicture = x.JsonPicture,
                UserName = x.UserName,
                Password = x.Password,
                PhoneSender = x.PhoneSender,
                CurrencyUnit = x.CurrencyUnit,
                Email = x.Email,
                JsonTel = x.JsonTel,
                JsonMobile = x.JsonMobile,

                Address = x.Address,
                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.GetDateNow(),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),

            });
            CreateMap<ResultSetting, UpdateSetting>().ConvertUsing(x => new UpdateSetting
            {
                ID = x.ID,
                Name = x.Name,
                CountShowRecord = x.CountShowRecord,
                JsonPicture = x.JsonPicture,
                UserName = x.UserName,
                Password = x.Password,
                PhoneSender = x.PhoneSender,
                CurrencyUnit = x.CurrencyUnit2,
                Email = x.Email,
                JsonTel = ConvertTelListToJson(x.ResultTels),
                JsonMobile = ConvertMobileListToJson(x.ResultMobiles),
                Address = x.Address,
                IdentityCode = x.IdentityCode,
                RegisterDate = x.RegisterDate,
                RegisterTime = x.RegisterTime,
                EditDate = x.EditDate,
                EditTime = x.EditTime,

            });
            CreateMap<Setting, UpdateSetting>().ConvertUsing(x => new UpdateSetting
            {
                ID = x.ID,
                Name = x.Name,
                CountShowRecord = x.CountShowRecord,
                JsonPicture = x.JsonPicture,
                UserName = x.UserName,
                Password = x.Password,
                PhoneSender = x.PhoneSender,
                CurrencyUnit = x.CurrencyUnit,
                Email = x.Email,
                JsonTel = x.JsonTel,
                JsonMobile = x.JsonMobile,
                Address = x.Address,

                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                EditTime = x.EditTime,

            });
            CreateMap<Setting, ResultSetting>().ConvertUsing(x => new ResultSetting
            {
                ID = x.ID,
                Name = x.Name,
                CountShowRecord = x.CountShowRecord,
                JsonPicture = x.JsonPicture,
                UserName = x.UserName,
                Password = x.Password,
                PhoneSender = x.PhoneSender,
                CurrencyUnit2 = x.CurrencyUnit,
                CurrencyUnit = GetTitleCurrencyUnit(x.CurrencyUnit),
                Email = x.Email,
                JsonTel = x.JsonTel,
                ResultTels = ConvertTelJsonToList(x.JsonTel),
                JsonMobile = x.JsonMobile,
                ResultMobiles = ConvertMobileJsonToList(x.JsonMobile),
                Address = x.Address,
                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                EditTime = x.EditTime,

            });
            CreateMap<Setting, ResultPublicSetting>().ConvertUsing(x => new ResultPublicSetting
            {
                ID = x.ID,
                Name = x.Name,
                CountShowRecord = x.CountShowRecord,
                JsonPicture = x.JsonPicture,
                Email = x.Email,
                JsonTel = x.JsonTel,
                ResultTels = ConvertTelJsonToList(x.JsonTel),
                JsonMobile = x.JsonMobile,
                ResultMobiles = ConvertMobileJsonToList(x.JsonMobile),
                Address = x.Address,

            });
            #endregion
            #region Customer
            CreateMap<AddCustomer, Customer>().ConvertUsing(x => new Customer
            {
                Name = x.Name,
                LastName = x.LastName,
                BirthDate = x.BirthDate != null ? DateFunctions.ConvertDateStringToInt(x.BirthDate) : null,
                Mobile = x.Mobile,
                Mcode = x.Mcode,
                Email = x.Email,
                Gender = (int)x.Gender,
                JsonPicture = x.JsonPicture,
                Visible = x.Visible,
                JsonLableTexts = x.JsonLableTexts,
                IdentityCode = Guid.NewGuid(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<UpdateCustomer, Customer>().ConvertUsing(x => new Customer
            {
                ID = x.ID,
                Name = x.Name,
                LastName = x.LastName,
                BirthDate = x.BirthDate != null ? DateFunctions.ConvertDateStringToInt(x.BirthDate) : null,
                Mobile = x.Mobile,
                Mcode = x.Mcode,
                Email = x.Email,
                Gender = (int)x.Gender,
                JsonPicture = x.JsonPicture,
                Visible = x.Visible,
                JsonLableTexts = x.JsonLableTexts,
                RegisterTime = x.RegisterTime,
                IdentityCode = x.IdentityCode,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<UpdateCustomerInfo, Customer>().ConvertUsing(x => new Customer
            {
                ID = x.ID,
                Name = x.Name,
                LastName = x.LastName,
                BirthDate = x.BirthDate != null ? DateFunctions.ConvertDateStringToInt(x.BirthDate) : null,
                Mobile = x.Mobile,
                Mcode = x.Mcode,
                Email = x.Email,
                Gender = (int)x.Gender,
                Visible = x.Visible,
                JsonLableTexts = x.JsonLableTexts,
                IdentityCode = x.IdentityCode,

                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<Customer, UpdateCustomer>().ConvertUsing(x => new UpdateCustomer
            {
                ID = x.ID,
                Name = x.Name,
                LastName = x.LastName,
                BirthDate = x.BirthDate != null ? DateFunctions.ConvertDateIntToString((int)x.BirthDate) : null,
                Mobile = x.Mobile,
                Mcode = x.Mcode,
                Email = x.Email,
                Gender = (Gender)x.Gender,
                JsonPicture = x.JsonPicture,
                Visible = x.Visible,
                Slug = x.Slug,
                JsonLableTexts = x.JsonLableTexts,
                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),

                ResultCustomerUserInfo = new ResultCustomerUserInfo
                {
                    Mobile = x.Mobile,
                    Email = x.Email,

                },
                UpdateCustomerAddresss = x.CustomerAddresss != null ? x.CustomerAddresss.Select(s => new UpdateCustomerAddress
                {
                    ID = s.ID,
                    Address = s.Address,
                    BuildingUnit = s.BuildingUnit,
                    ProvinceID = s.City.ProvinceID,
                    CityID = s.CityID,
                    CustomerID = s.CustomerID,
                    Default = s.Default,
                    Plaque = s.Plaque,
                    PostalCode = s.PostalCode,
                    Description = s.Description,
                    Visible = x.Visible,
                    RegisterTime = s.RegisterTime,
                    EditTime = s.EditTime,
                    IdentityCode = s.IdentityCode,
                    RegisterDate = DateFunctions.ConvertDateIntToString(s.RegisterDate),
                    EditDate = DateFunctions.ConvertDateIntToString(s.EditDate),

                    ResultProvince = new ResultProvince
                    {
                        ID = s.City.Province.ID,
                        Title = s.City.Province.Title,

                    },
                    ResultCity = s.City != null ? new ResultCity
                    {
                        ID = s.City.ID,
                        Title = s.City.Title,

                    } : new ResultCity()

                }).ToList() : new List<UpdateCustomerAddress>(),

            });
            CreateMap<Customer, UpdateCustomerInfo>().ConvertUsing(x => new UpdateCustomerInfo
            {
                ID = x.ID,
                Name = x.Name,
                LastName = x.LastName,
                BirthDate = x.BirthDate != null ? DateFunctions.ConvertDateIntToString((int)x.BirthDate) : null,
                Mobile = x.Mobile,
                Mcode = x.Mcode,
                Email = x.Email,
                Gender = (Gender)x.Gender,
                Visible = x.Visible,
                Slug = x.Slug,
                JsonLableTexts = x.JsonLableTexts,
                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),


            });
            CreateMap<Customer, ResultCustomer>().ConvertUsing(x => new ResultCustomer
            {
                ID = x.ID,
                Name = x.Name,
                LastName = x.LastName,
                BirthDate = x.BirthDate != null ? DateFunctions.ConvertDateIntToString((int)x.BirthDate) : null,
                Mobile = x.Mobile,
                Mcode = x.Mcode,
                Email = x.Email,
                Gender = EnumConstant.GetTitleGender(x.Gender),
                JsonPicture = x.JsonPicture,
                ImageUIrl = GetPathImage(x.JsonPicture),
                Visible = x.Visible,
                Slug = x.Slug,
                CartCount = x.Carts != null ? x.Carts.Count : 0,
                OrderCount = x.Orders != null ? x.Orders.Count : 0,

                RegisterTime = x.RegisterTime,

                ResultCustomerUserInfo = new ResultCustomerUserInfo
                {
                    Mobile = x.Mobile,
                    Email = x.Email,

                },
                ResultCustomerAddresss = x.CustomerAddresss != null ? x.CustomerAddresss.Select(s => new ResultCustomerAddress
                {
                    ID = s.ID,
                    Address = s.Address,
                    BuildingUnit = s.BuildingUnit,
                    CityID = s.CityID,
                    CustomerID = s.CustomerID,
                    Default = s.Default,
                    Plaque = s.Plaque,
                    PostalCode = s.PostalCode,
                    Description = s.Description,
                    Visible = x.Visible,
                    RegisterTime = s.RegisterTime,
                    EditTime = s.EditTime,
                    IdentityCode = s.IdentityCode,
                    RegisterDate = DateFunctions.ConvertDateIntToString(s.RegisterDate),
                    EditDate = DateFunctions.ConvertDateIntToString(s.EditDate),
                    ResultCity = s.City != null ? new ResultCity
                    {
                        ID = s.City.Province.ID,
                        Title = s.City.Province.Title,
                        ResultProvince = s.City.Province != null ? new ResultProvince
                        {
                            ID = s.City.Province.ID,
                            Title = s.City.Province.Title,
                        } : new ResultProvince(),
                    } : new ResultCity(),

                }).ToList() : new List<ResultCustomerAddress>(),
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                IdentityCode = x.IdentityCode,

            });
            CreateMap<ResultCustomer, UpdateCustomer>();


            #endregion
            #region Order
            CreateMap<AddOrder, Order>().ConvertUsing(x => new Order
            {
                CustomerID = x.CustomerID,
                OrderStatus = x.OrderStatus,
                PaymentStatus = x.PaymentStatus,
                Discount = x.Discount,
                FinalAmount = x.FinalAmount,
                TotalAmount = x.TotalAmount,
                OrderCode = x.OrderCode,
                CardPen = x.CardPen,
                RefId = x.RefId,
                ResNum = x.ResNum,
                SendProductMethodID = x.SendProductMethodID,
                JsonAddress = x.JsonAddress,
                StatusDescription = x.StatusDescription,
                Visible = x.Visible,
                IdentityCode = Guid.NewGuid(),
                JsonLableTexts = x.JsonLableTexts,
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),


            });
            CreateMap<UpdateOrder, Order>().ConvertUsing(x => new Order
            {
                ID = x.ID,
                CustomerID = x.CustomerID,
                OrderStatus = x.OrderStatus,
                PaymentStatus = x.PaymentStatus,
                Discount = x.Discount,
                FinalAmount = x.FinalAmount,
                TotalAmount = x.TotalAmount,
                OrderCode = x.OrderCode,
                CardPen = x.CardPen,
                RefId = x.RefId,
                ResNum = x.ResNum,
                Visible = x.Visible,
                SendProductMethodID = x.SendProductMethodID,
                JsonAddress = x.JsonAddress,
                StatusDescription = x.StatusDescription,

                JsonLableTexts = x.JsonLableTexts,
                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<Order, UpdateOrder>().ConvertUsing(x => new UpdateOrder
            {
                ID = x.ID,
                CustomerID = x.CustomerID,
                OrderStatus = x.OrderStatus,
                PaymentStatus = x.PaymentStatus,
                Discount = x.Discount,
                FinalAmount = x.FinalAmount,
                TotalAmount = x.TotalAmount,
                OrderCode = x.OrderCode,
                CardPen = x.CardPen,
                RefId = x.RefId,
                ResNum = x.ResNum,
                Visible = x.Visible,
                SendProductMethodID = x.SendProductMethodID,
                JsonAddress = x.JsonAddress,
                StatusDescription = x.StatusDescription,

                JsonLableTexts = x.JsonLableTexts,
                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
            });
            CreateMap<ResultOrder, UpdateOrder>();
            CreateMap<Order, ResultOrder>().ConvertUsing(x => new ResultOrder
            {
                ID = x.ID,
                CustomerID = x.CustomerID,
                OrderStatus = GetStatusOrder(x.OrderStatus),
                OrderStatus2 = x.OrderStatus,
                PaymentStatus = GetPaymentStatus(x.PaymentStatus),
                PaymentStatus2 = x.PaymentStatus,
                Discount = x.Discount,
                FinalAmount = x.FinalAmount,
                TotalAmount = x.TotalAmount,
                OrderCode = x.OrderCode,
                CardPen = x.CardPen,
                RefId = x.RefId,
                ResNum = x.ResNum,
                Visible = x.Visible,
                SendProductMethodID = x.SendProductMethodID,
                JsonAddress = x.JsonAddress,
                StatusDescription = x.StatusDescription,

                IdentityCode = x.IdentityCode,
                ResultJsonLables = x.JsonLableTexts != null ? JsonConvert.DeserializeObject<List<ResultJsonLable>>(x.JsonLableTexts) : new List<ResultJsonLable>(),
                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                ResultSendProductMethod = x.PaymentStatus != PaymentStatus.paid ? new ResultSendProductMethod() : x.SendProductMethod != null ? new ResultSendProductMethod
                {
                    ID = x.SendProductMethod.ID,
                    Title = x.SendProductMethod.Title,
                    Description = x.SendProductMethod.Description,
                } : new ResultSendProductMethod(),
                ResultCustomer = x.Customer != null ? new ResultCustomer
                {
                    ID = x.Customer.ID,
                    Name = x.Customer.Name,
                    LastName = x.Customer.LastName,
                    Mobile = x.Customer.Mobile,
                    Mcode = x.Customer.Mcode,
                    Email = x.Customer.Email,
                    Gender = EnumConstant.GetTitleGender(x.Customer.Gender),
                    BirthDate = x.Customer.BirthDate != null ? DateFunctions.ConvertDateIntToString((int)x.Customer.BirthDate) : null,
                } : new ResultCustomer(),
                ResultCustomerAddress = GetResultCustomerAddress(x.JsonAddress),

            });

            #endregion
            #region OrderItem
            CreateMap<AddOrderItem, OrderItem>().ConvertUsing(x => new OrderItem
            {

                OrderID = x.OrderID,
                ProductID = x.ProductID,
                Quantity = 1, //x.Quantity,
                PriceAtOrder = x.PriceAtOrder,

                Visible = x.Visible,
                IdentityCode = Guid.NewGuid(),
                JsonLableTexts = x.JsonLableTexts,
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<UpdateOrderItem, OrderItem>().ConvertUsing(x => new OrderItem
            {
                ID = x.ID,
                OrderID = x.OrderID,
                ProductID = x.ProductID,
                Quantity = 1, //x.Quantity,
                PriceAtOrder = x.PriceAtOrder,

                Visible = x.Visible,

                JsonLableTexts = x.JsonLableTexts,
                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<OrderItem, UpdateOrderItem>().ConvertUsing(x => new UpdateOrderItem
            {
                ID = x.ID,
                OrderID = x.OrderID,
                ProductID = x.ProductID,
                Quantity = x.Quantity,
                PriceAtOrder = x.PriceAtOrder,

                Visible = x.Visible,

                JsonLableTexts = x.JsonLableTexts,
                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
            });
            CreateMap<ResultOrderItem, UpdateOrderItem>();
            CreateMap<OrderItem, ResultOrderItem>().ConvertUsing(x => new ResultOrderItem
            {
                ID = x.ID,
                OrderID = x.OrderID,
                ProductID = x.ProductID,
                Quantity = x.Quantity,
                PriceAtOrder = x.PriceAtOrder,
                FinalAmount = x.PriceAtOrder * x.Quantity,

                Visible = x.Visible,
                ResultProduct = x.Product != null ? new ResultProduct
                {
                    ID = x.Product.ID,
                    Title = x.Product.Title,
                    Description = x.Product.Description,
                    Price = x.Product.Price,
                    IdentityCode = x.Product.IdentityCode,
                    ResultUploadFiles = GetResultUploadFiles(x.Product.JsonPicture),
                } : new ResultProduct(),
                ResultJsonLables = x.JsonLableTexts != null ? JsonConvert.DeserializeObject<List<ResultJsonLable>>(x.JsonLableTexts) : new List<ResultJsonLable>(),
                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),

            });

            #endregion
            #region Cart
            CreateMap<AddCart, Cart>().ConvertUsing(x => new Cart
            {
                CustomerID = x.CustomerID,
                Visible = x.Visible,
                CartStatus = (int)x.CartStatus,
                SendProductMethodID = x.SendProductMethodID,
                JsonLableTexts = x.JsonLableTexts,
                IdentityCode = Guid.NewGuid(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<UpdateCart, Cart>().ConvertUsing(x => new Cart
            {
                ID = x.ID,
                CustomerID = x.CustomerID,
                SendProductMethodID = x.SendProductMethodID,
                Visible = x.Visible,
                CartStatus = (int)x.CartStatus,

                JsonLableTexts = x.JsonLableTexts,
                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<Cart, UpdateCart>().ConvertUsing(x => new UpdateCart
            {
                ID = x.ID,
                CustomerID = x.CustomerID,
                SendProductMethodID = x.SendProductMethodID,
                Visible = x.Visible,
                CartStatus = (CartStatus)x.CartStatus,

                JsonLableTexts = x.JsonLableTexts,
                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
            });
            CreateMap<ResultCart, UpdateCart>();
            CreateMap<Cart, ResultCart>().ConvertUsing(x => new ResultCart
            {
                ID = x.ID,
                CustomerID = x.CustomerID,
                SendProductMethodID = x.SendProductMethodID,
                Visible = x.Visible,
                Slug = x.Slug,
                CartStatus = GetCartStatus(x.CartStatus),
                CartStatus2 = (CartStatus)x.CartStatus,
                TotalAmount = x.CartItems != null && x.CartItems.Count > 0 ? x.CartItems.Sum(s => s.Product.Price * s.Quantity) : 0,
                Discount = x.CartItems != null && x.CartItems.Count > 0 ? x.CartItems.Sum(s => s.Product.Discount * s.Quantity) : 0,
                FinalAmount = x.CartItems != null && x.CartItems.Count > 0 ? x.CartItems.Sum(s => s.Product.Price * s.Quantity) - x.CartItems.Sum(s => s.Product.Discount * s.Quantity) : 0,
                CountCartItems = x.CartItems != null ? x.CartItems.Count : 0,

                ResultJsonLables = x.JsonLableTexts != null ? JsonConvert.DeserializeObject<List<ResultJsonLable>>(x.JsonLableTexts) : new List<ResultJsonLable>(),
                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                IdentityCode = x.IdentityCode,
                ResultCustomer = x.Customer != null ? new ResultCustomer
                {
                    ID = x.Customer.ID,
                    Name = x.Customer.Name,
                    LastName = x.Customer.LastName,
                    Mobile = x.Customer.Mobile,
                    Mcode = x.Customer.Mcode,
                    Email = x.Customer.Email,
                    Gender = EnumConstant.GetTitleGender(x.Customer.Gender),
                    BirthDate = x.Customer.BirthDate != null ? DateFunctions.ConvertDateIntToString((int)x.Customer.BirthDate) : null,
                } : new ResultCustomer(),

            });

            #endregion
            #region CartItem
            CreateMap<AddCartItem, CartItem>().ConvertUsing(x => new CartItem
            {

                CartID = x.CartID,
                ProductID = x.ProductID,
                Quantity = x.Quantity,
                Visible = x.Visible,
                IdentityCode = Guid.NewGuid(),
                JsonLableTexts = x.JsonLableTexts,
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<AddCartItem, ResultCartItem>().ConvertUsing(x => new ResultCartItem
            {

                CartID = x.CartID,
                ProductID = x.ProductID,
                Quantity = x.Quantity,
                Visible = x.Visible,

                JsonLableTexts = x.JsonLableTexts,
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = GetNewDate(),
                EditDate = GetNewDate(),
            });
            CreateMap<UpdateCartItem, CartItem>().ConvertUsing(x => new CartItem
            {
                ID = x.ID,
                CartID = x.CartID,
                ProductID = x.ProductID,
                Quantity = x.Quantity,
                Visible = x.Visible,

                JsonLableTexts = x.JsonLableTexts,
                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<CartItem, UpdateCartItem>().ConvertUsing(x => new UpdateCartItem
            {
                ID = x.ID,
                CartID = x.CartID,
                ProductID = x.ProductID,
                Quantity = x.Quantity,
                Visible = x.Visible,

                JsonLableTexts = x.JsonLableTexts,
                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
            });
            CreateMap<ResultCartItem, UpdateCartItem>();
            CreateMap<CartItem, ResultCartItem>().ConvertUsing(x => new ResultCartItem
            {
                ID = x.ID,
                CartID = x.CartID,
                ProductID = x.ProductID,
                Quantity = x.Quantity,
                TotalAmount = x.Product != null ? x.Product.Price * x.Quantity : 0,
                Visible = x.Visible,

                ResultJsonLables = x.JsonLableTexts != null ? JsonConvert.DeserializeObject<List<ResultJsonLable>>(x.JsonLableTexts) : new List<ResultJsonLable>(),
                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                ResultProduct = x.Product != null ? new ResultProduct
                {
                    ID = x.Product.ID,
                    Title = x.Product.Title,
                    Description = x.Product.Description,
                    Price = x.Product.Price,
                    Discount = x.Product.Discount,
                    ProductCode = x.Product.ProductCode,
                    IdentityCode = x.Product.IdentityCode,
                    ResultUploadFiles = GetResultUploadFiles(x.Product.JsonPicture),
                    ResultPricingRules = x.Product.PricingRules != null && x.Product.PricingRules.Count > 0 ? x.Product.PricingRules.Select(k => new ResultPricingRule
                    {
                        ID = k.ID,
                        ProductID = k.ProductID,
                        Price = k.Price,
                        MinQuantity = k.MinQuantity,
                        RoleID = k.RoleID,
                        RuleType = EnumConstant.GetTitleRuleType(k.RuleType),
                        RuleType2 = k.RuleType,
                        Title = k.Title,
                        FromDate = DateFunctions.ConvertDateIntToString(k.FromDate),
                        ToDate = DateFunctions.ConvertDateIntToString(k.ToDate),
                        Visible = k.Visible,

                        RegisterTime = k.RegisterTime,
                        EditTime = k.EditTime,
                        RegisterDate = DateFunctions.ConvertDateIntToString(k.RegisterDate),
                        EditDate = DateFunctions.ConvertDateIntToString(k.EditDate),

                    }).ToList() : new List<ResultPricingRule>()
                } : new ResultProduct()
            });

            #endregion
            #region Slider
            CreateMap<AddSlider, Slider>().ConvertUsing(x => new Slider
            {
                Title = x.Title,
                Url = x.Url,
                JsonPicture = x.JsonPicture,
                Visible = x.Visible,
                Description = x.Description,
                IdentityCode = Guid.NewGuid(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<UpdateSlider, Slider>().ConvertUsing(x => new Slider
            {
                ID = x.ID,
                Title = x.Title,
                Url = x.Url,
                JsonPicture = x.JsonPicture,
                Visible = x.Visible,
                Description = x.Description,


                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<Slider, UpdateSlider>().ConvertUsing(x => new UpdateSlider
            {
                ID = x.ID,
                Title = x.Title,
                Url = x.Url,
                JsonPicture = x.JsonPicture,
                Visible = x.Visible,
                Description = x.Description,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
            });
            CreateMap<ResultSlider, UpdateSlider>();
            CreateMap<Slider, ResultSlider>().ConvertUsing(x => new ResultSlider
            {
                ID = x.ID,
                Title = x.Title,
                Url = x.Url,
                JsonPicture = x.JsonPicture,
                Visible = x.Visible,
                Description = x.Description,


                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),

            });

            #endregion
            #region Advertisement
            CreateMap<AddAdvertisement, Advertisement>().ConvertUsing(x => new Advertisement
            {
                Title = x.Title,
                SiteUrl = x.SiteUrl,
                SiteName = x.SiteName,
                JsonPicture = x.JsonPicture,
                Visible = x.Visible,
                Description = x.Description,
                IdentityCode = Guid.NewGuid(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<UpdateAdvertisement, Advertisement>().ConvertUsing(x => new Advertisement
            {
                ID = x.ID,
                Title = x.Title,
                SiteUrl = x.SiteUrl,
                SiteName = x.SiteName,
                JsonPicture = x.JsonPicture,
                Visible = x.Visible,
                Description = x.Description,


                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<Advertisement, UpdateAdvertisement>().ConvertUsing(x => new UpdateAdvertisement
            {
                ID = x.ID,
                Title = x.Title,
                SiteUrl = x.SiteUrl,
                SiteName = x.SiteName,
                JsonPicture = x.JsonPicture,
                Visible = x.Visible,
                Description = x.Description,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
            });
            CreateMap<ResultAdvertisement, UpdateAdvertisement>();
            CreateMap<Advertisement, ResultAdvertisement>().ConvertUsing(x => new ResultAdvertisement
            {
                ID = x.ID,
                Title = x.Title,
                SiteUrl = x.SiteUrl,
                SiteName = x.SiteName,
                JsonPicture = x.JsonPicture,
                Visible = x.Visible,
                Description = x.Description,


                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),

            });

            #endregion
            #region AdvertisementSingle
            CreateMap<AddAdvertisementSingle, AdvertisementSingle>().ConvertUsing(x => new AdvertisementSingle
            {
                Title = x.Title,
                SiteUrl = x.SiteUrl,
                SiteName = x.SiteName,
                JsonPicture = x.JsonPicture,
                Visible = x.Visible,
                Description = x.Description,
                IdentityCode = Guid.NewGuid(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<UpdateAdvertisementSingle, AdvertisementSingle>().ConvertUsing(x => new AdvertisementSingle
            {
                ID = x.ID,
                Title = x.Title,
                SiteUrl = x.SiteUrl,
                SiteName = x.SiteName,
                JsonPicture = x.JsonPicture,
                Visible = x.Visible,
                Description = x.Description,


                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<AdvertisementSingle, UpdateAdvertisementSingle>().ConvertUsing(x => new UpdateAdvertisementSingle
            {
                ID = x.ID,
                Title = x.Title,
                SiteUrl = x.SiteUrl,
                SiteName = x.SiteName,
                JsonPicture = x.JsonPicture,
                Visible = x.Visible,
                Description = x.Description,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
            });
            CreateMap<ResultAdvertisementSingle, UpdateAdvertisementSingle>();
            CreateMap<AdvertisementSingle, ResultAdvertisementSingle>().ConvertUsing(x => new ResultAdvertisementSingle
            {
                ID = x.ID,
                Title = x.Title,
                SiteUrl = x.SiteUrl,
                SiteName = x.SiteName,
                JsonPicture = x.JsonPicture,
                Visible = x.Visible,
                Description = x.Description,


                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),

            });

            #endregion
            #region GroupBlog
            CreateMap<AddGroupBlog, GroupBlog>().ConvertUsing(x => new GroupBlog
            {
                Title = x.Title,
                Visible = x.Visible,
                IdentityCode = Guid.NewGuid(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<UpdateGroupBlog, GroupBlog>().ConvertUsing(x => new GroupBlog
            {
                ID = x.ID,
                Title = x.Title,
                Visible = x.Visible,

                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<GroupBlog, UpdateGroupBlog>().ConvertUsing(x => new UpdateGroupBlog
            {
                ID = x.ID,
                Title = x.Title,
                Visible = x.Visible,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
            });
            CreateMap<ResultGroupBlog, UpdateGroupBlog>();
            CreateMap<GroupBlog, ResultGroupBlog>().ConvertUsing(x => new ResultGroupBlog
            {
                ID = x.ID,
                Title = x.Title,
                Visible = x.Visible,
                CountBlog = x.Blogs != null ? x.Blogs.Count : 0,
                IdentityCode = x.IdentityCode,
                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),


            });
            #endregion
            #region Blog
            CreateMap<AddBlog, Blog>().ConvertUsing(x => new Blog
            {
                Title = x.Title,
                GroupBlogID = x.GroupBlogID,
                TeamID = x.TeamID,
                StudyDuration = x.StudyDuration,
                NumberOfVisits = x.NumberOfVisits,
                JsonLableTexts = x.JsonLableTexts,
                JsonPicture = x.JsonPicture,
                Visible = x.Visible,
                Description = x.Description,
                IdentityCode = Guid.NewGuid(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<UpdateBlog, Blog>().ConvertUsing(x => new Blog
            {
                ID = x.ID,
                Title = x.Title,
                GroupBlogID = x.GroupBlogID,
                TeamID = x.TeamID,
                StudyDuration = x.StudyDuration,
                NumberOfVisits = x.NumberOfVisits,
                JsonLableTexts = x.JsonLableTexts,
                JsonPicture = x.JsonPicture,
                Visible = x.Visible,
                Description = x.Description,


                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<Blog, UpdateBlog>().ConvertUsing(x => new UpdateBlog
            {
                ID = x.ID,
                Title = x.Title,
                GroupBlogID = x.GroupBlogID,
                TeamID = x.TeamID,
                StudyDuration = x.StudyDuration,
                NumberOfVisits = x.NumberOfVisits,
                JsonLableTexts = x.JsonLableTexts,
                ResultJsonLables = x.JsonLableTexts != null ? JsonConvert.DeserializeObject<List<ResultJsonLable>>(x.JsonLableTexts) : new List<ResultJsonLable>(),
                JsonPicture = x.JsonPicture,

                Visible = x.Visible,
                Description = x.Description,
                ResultGroupBlog = x.GroupBlog != null ? new ResultGroupBlog
                {
                    Title = x.GroupBlog.Title,
                } : new ResultGroupBlog(),
                ResultTeam = x.Team != null ? new ResultTeam
                {
                    Name = x.Team.Name,
                    Title = x.Team.Title,
                    Description = x.Team.Description,
                } : new ResultTeam(),
                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
            });
            CreateMap<ResultBlog, UpdateBlog>();
            CreateMap<Blog, ResultBlog>().ConvertUsing(x => new ResultBlog
            {
                ID = x.ID,
                Title = x.Title,
                GroupBlogID = x.GroupBlogID,
                TeamID = x.TeamID,
                StudyDuration = x.StudyDuration,
                NumberOfVisits = x.NumberOfVisits,
                ResultJsonLables = x.JsonLableTexts != null ? JsonConvert.DeserializeObject<List<ResultJsonLable>>(x.JsonLableTexts) : new List<ResultJsonLable>(),
                Visible = x.Visible,
                Description = x.Description,
                JsonPicture = x.JsonPicture,
                IdentityCode = x.IdentityCode,
                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                ResultGroupBlog = x.GroupBlog != null ? new ResultGroupBlog
                {
                    Title = x.GroupBlog.Title,
                } : new ResultGroupBlog(),
                ResultTeam = x.Team != null ? new ResultTeam
                {
                    Name = x.Team.Name,
                    Title = x.Team.Title,
                    Description = x.Team.Description,
                } : new ResultTeam(),

            });

            #endregion
            #region BlogComment
            CreateMap<AddBlogComment, BlogComment>()
                .ForMember(des => des.IdentityCode, s => s.MapFrom(x => Guid.NewGuid()))
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));
            CreateMap<UpdateBlogComment, BlogComment>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<BlogComment, UpdateBlogComment>()
                  .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));


            CreateMap<BlogComment, ResultBlogComment>()
                .ForMember(des => des.IsVerifiedBuyer, s => s.MapFrom(x => x.IsVerifiedBuyer
            ? "<span class='inline-flex items-center px-2 py-1 rounded-full text-xs font-semibold bg-green-100 text-green-800'>✓ بله</span>"
           : "<span class='inline-flex items-center px-2 py-1 rounded-full text-xs font-semibold bg-red-100 text-red-800'>✕ خیر</span>"))
                .ForMember(des => des.Status, s => s.MapFrom(x => EnumConstant.GetTitleCommentStatus(x.Status)))
                .ForMember(des => des.Status2, s => s.MapFrom(x => x.Status))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)))
                .ForMember(des => des.ResultBlog, s => s.MapFrom(x => x.Blog != null ? new ResultBlog
                {
                    Title = x.Blog.Title
                } : new ResultBlog()
                ))

            .ForMember(des => des.ResultCustomer, s => s.MapFrom(x => x.Customer != null ? new ResultCustomer
            {
                Name = x.Customer.Name
            } : new ResultCustomer()
                ));


            #endregion
            #region Story
            CreateMap<AddStory, Story>().ConvertUsing(x => new Story
            {
                Title = x.Title,
                Url = x.Url,
                JsonPicture = x.JsonPicture,
                JsonVideo = x.JsonVideo,
                Visible = x.Visible,
                Description = x.Description,
                IdentityCode = Guid.NewGuid(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<UpdateStory, Story>().ConvertUsing(x => new Story
            {
                ID = x.ID,
                Title = x.Title,
                Url = x.Url,
                JsonPicture = x.JsonPicture,
                JsonVideo = x.JsonVideo,
                Visible = x.Visible,
                Description = x.Description,

                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<Story, UpdateStory>().ConvertUsing(x => new UpdateStory
            {
                ID = x.ID,
                Title = x.Title,
                Url = x.Url,
                JsonPicture = x.JsonPicture,
                JsonVideo = x.JsonVideo,
                Visible = x.Visible,
                Description = x.Description,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
            });
            CreateMap<ResultStory, UpdateStory>();
            CreateMap<Story, ResultStory>().ConvertUsing(x => new ResultStory
            {
                ID = x.ID,
                Title = x.Title,
                Url = x.Url,
                JsonPicture = x.JsonPicture,
                JsonVideo = x.JsonVideo,
                Visible = x.Visible,
                Description = x.Description,


                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),

            });

            #endregion
            #region SmsOtpCode
            CreateMap<AddSmsOtpCode, SmsOtpCode>().ConvertUsing(x => new SmsOtpCode
            {
                Code = x.Code,
                PhoneNumber = x.PhoneNumber,
                Visible = x.Visible,
                IdentityCode = Guid.NewGuid(),
                TimeExpired = (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)) + TimeSpan.FromMinutes(2),
                DateExpired = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<UpdateSmsOtpCode, SmsOtpCode>().ConvertUsing(x => new SmsOtpCode
            {
                ID = x.ID,
                Code = x.Code,
                PhoneNumber = x.PhoneNumber,
                Visible = x.Visible,

                TimeExpired = (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)) + TimeSpan.FromMinutes(2),
                DateExpired = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<SmsOtpCode, UpdateSmsOtpCode>().ConvertUsing(x => new UpdateSmsOtpCode
            {
                ID = x.ID,
                Code = x.Code,
                PhoneNumber = x.PhoneNumber,
                Visible = x.Visible,


                TimeExpired = x.TimeExpired,
                DateExpired = DateFunctions.ConvertDateIntToString(x.DateExpired),
                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
            });
            CreateMap<ResultSmsOtpCode, UpdateSmsOtpCode>();
            CreateMap<SmsOtpCode, ResultSmsOtpCode>().ConvertUsing(x => new ResultSmsOtpCode
            {
                ID = x.ID,
                Code = x.Code,
                PhoneNumber = x.PhoneNumber,
                Visible = x.Visible,


                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),

            });

            #endregion
            #region Wallet
            CreateMap<AddWallet, Wallet>().ConvertUsing(x => new Wallet
            {
                Currency = x.Currency,
                Balance = x.Balance,
                CustomerID = x.CustomerID,
                GiftCredit = x.GiftCredit,

                Visible = x.Visible,

                IdentityCode = Guid.NewGuid(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<UpdateWallet, Wallet>().ConvertUsing(x => new Wallet
            {
                ID = x.ID,
                Currency = x.Currency,
                Balance = x.Balance,
                CustomerID = x.CustomerID,
                GiftCredit = x.GiftCredit,
                Visible = x.Visible,

                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<Wallet, UpdateWallet>().ConvertUsing(x => new UpdateWallet
            {
                ID = x.ID,
                Currency = x.Currency,
                Balance = x.Balance,
                CustomerID = x.CustomerID,
                GiftCredit = x.GiftCredit,
                Visible = x.Visible,
                Slug = x.Slug,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
            });
            CreateMap<ResultWallet, UpdateWallet>();
            CreateMap<Wallet, ResultWallet>().ConvertUsing(x => new ResultWallet
            {
                ID = x.ID,
                Currency = x.Currency,
                Balance = x.Balance,
                CustomerID = x.CustomerID,
                GiftCredit = x.GiftCredit,
                Visible = x.Visible,
                Slug = x.Slug,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),

            });

            #endregion
            #region WalletTransaction
            CreateMap<AddWalletTransaction, WalletTransaction>().ConvertUsing(x => new WalletTransaction
            {
                WalletID = x.WalletID,
                Currency = x.Currency,
                Amount = x.Amount,
                AfterBalance = x.AfterBalance,
                ExternalReference = x.ExternalReference,
                RefID = x.RefID,
                Status = x.Status,
                Description = x.Description,
                Kind = x.Kind,
                IdentityCode = x.IdentityCode,
                Visible = x.Visible,

                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<UpdateWalletTransaction, WalletTransaction>().ConvertUsing(x => new WalletTransaction
            {
                ID = x.ID,
                Currency = x.Currency,
                WalletID = x.WalletID,
                Amount = x.Amount,
                AfterBalance = x.AfterBalance,
                ExternalReference = x.ExternalReference,
                RefID = x.RefID,
                Status = x.Status,
                Description = x.Description,
                Kind = x.Kind,
                IdentityCode = x.IdentityCode,
                Visible = x.Visible,

                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<WalletTransaction, UpdateWalletTransaction>().ConvertUsing(x => new UpdateWalletTransaction
            {
                ID = x.ID,
                Currency = x.Currency,
                WalletID = x.WalletID,
                Amount = x.Amount,
                AfterBalance = x.AfterBalance,
                ExternalReference = x.ExternalReference,
                RefID = x.RefID,
                Status = x.Status,
                Description = x.Description,
                Kind = x.Kind,
                IdentityCode = x.IdentityCode,
                Visible = x.Visible,
                Slug = x.Slug,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
            });
            CreateMap<ResultWalletTransaction, UpdateWalletTransaction>();
            CreateMap<WalletTransaction, ResultWalletTransaction>().ConvertUsing(x => new ResultWalletTransaction
            {
                ID = x.ID,
                Currency = x.Currency,
                WalletID = x.WalletID,
                Amount = x.Amount,
                AfterBalance = x.AfterBalance,
                ExternalReference = x.ExternalReference,
                RefID = x.RefID,
                Status = GetTransactionStatuse(x.Status),
                Description = x.Description,
                Kind = GetTransactionKind(x.Kind),
                IdentityCode = x.IdentityCode,
                Visible = x.Visible,
                Slug = x.Slug,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),

            });

            #endregion
            #region Province
            CreateMap<AddProvince, Province>().ConvertUsing(x => new Province
            {
                Title = x.Title,
                Visible = x.Visible,
                IdentityCode = Guid.NewGuid(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<UpdateProvince, Province>().ConvertUsing(x => new Province
            {
                ID = x.ID,
                Title = x.Title,
                Visible = x.Visible,

                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<Province, UpdateProvince>().ConvertUsing(x => new UpdateProvince
            {
                ID = x.ID,
                Title = x.Title,
                Visible = x.Visible,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
            });
            CreateMap<ResultProvince, UpdateProvince>();
            CreateMap<Province, ResultProvince>().ConvertUsing(x => new ResultProvince
            {
                ID = x.ID,
                Title = x.Title,
                Visible = x.Visible,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),

            });

            #endregion
            #region CustomerAddress
            CreateMap<AddCustomerAddress, CustomerAddress>().ConvertUsing(x => new CustomerAddress
            {
                Address = x.Address,
                PostalCode = x.PostalCode,
                BuildingUnit = x.BuildingUnit,
                CityID = x.CityID,

                CustomerID = x.CustomerID,
                Plaque = x.Plaque,
                Default = x.Default,

                Visible = x.Visible,
                Description = x.Description,
                IdentityCode = Guid.NewGuid(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<UpdateCustomerAddress, CustomerAddress>().ConvertUsing(x => new CustomerAddress
            {
                ID = x.ID,
                Address = x.Address,
                PostalCode = x.PostalCode,
                BuildingUnit = x.BuildingUnit,
                CityID = x.CityID,
                CustomerID = x.CustomerID,
                Plaque = x.Plaque,
                Default = x.Default,
                Visible = x.Visible,
                Description = x.Description,


                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),


            });
            CreateMap<CustomerAddress, UpdateCustomerAddress>().ConvertUsing(x => new UpdateCustomerAddress
            {
                ID = x.ID,
                Address = x.Address,
                PostalCode = x.PostalCode,
                BuildingUnit = x.BuildingUnit,
                CityID = x.CityID,
                CustomerID = x.CustomerID,
                Plaque = x.Plaque,
                Default = x.Default,
                Visible = x.Visible,
                Description = x.Description,
                IdentityCode = x.IdentityCode,
                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                ResultProvince = x.City != null && x.City.Province != null ? new ResultProvince
                {
                    ID = x.City.Province.ID,
                    Title = x.City.Province.Title,

                } : new ResultProvince(),
                ResultCity = x.City != null ? new ResultCity
                {
                    ID = x.City.ID,
                    Title = x.City.Title,
                    ResultProvince = x.City.Province != null ? new ResultProvince
                    {
                        ID = x.City.Province.ID,
                        Title = x.City.Province.Title,
                    } : new ResultProvince(),
                } : new ResultCity(),
                ResultCustomer = x.Customer != null ? new ResultCustomer
                {
                    ID = x.Customer.ID,
                    Name = x.Customer.Name,
                    LastName = x.Customer.LastName,
                    Mcode = x.Customer.Mcode,
                } : new ResultCustomer(),
            });
            CreateMap<ResultCustomerAddress, UpdateCustomerAddress>();
            CreateMap<CustomerAddress, ResultCustomerAddress>().ConvertUsing(x => new ResultCustomerAddress
            {
                ID = x.ID,
                Address = x.Address,
                PostalCode = x.PostalCode,
                BuildingUnit = x.BuildingUnit,
                CityID = x.CityID,
                CustomerID = x.CustomerID,
                Plaque = x.Plaque,
                Default = x.Default,
                Visible = x.Visible,
                Description = x.Description,
                IdentityCode = x.IdentityCode,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                ResultCustomer = x.Customer != null ? new ResultCustomer
                {
                    Name = x.Customer.Name,
                    LastName = x.Customer.LastName,
                    Mobile = x.Customer.Mobile,

                } : new ResultCustomer(),
                ResultCity = x.City != null ? new ResultCity
                {
                    ID = x.City.ID,
                    Title = x.City.Title,
                    ResultProvince = x.City.Province != null ? new ResultProvince
                    {
                        ID = x.City.Province.ID,
                        Title = x.City.Province.Title,
                    } : new ResultProvince(),
                } : new ResultCity(),
            });

            #endregion
            #region SendProductMethod
            CreateMap<AddSendProductMethod, SendProductMethod>().ConvertUsing(x => new SendProductMethod
            {
                Title = x.Title,
                Description = x.Description,
                Visible = x.Visible,
                IdentityCode = Guid.NewGuid(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<UpdateSendProductMethod, SendProductMethod>().ConvertUsing(x => new SendProductMethod
            {
                ID = x.ID,
                Title = x.Title,
                Description = x.Description,
                Visible = x.Visible,

                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<SendProductMethod, UpdateSendProductMethod>().ConvertUsing(x => new UpdateSendProductMethod
            {
                ID = x.ID,
                Title = x.Title,
                Description = x.Description,
                Visible = x.Visible,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
            });
            CreateMap<ResultSendProductMethod, UpdateSendProductMethod>();
            CreateMap<SendProductMethod, ResultSendProductMethod>().ConvertUsing(x => new ResultSendProductMethod
            {
                ID = x.ID,
                Title = x.Title,
                Description = x.Description,
                Visible = x.Visible,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),

            });

            #endregion
            #region OrderPaymentTemp
            CreateMap<AddOrderPaymentTemp, OrderPaymentTemp>().ConvertUsing(x => new OrderPaymentTemp
            {
                ResNum = x.ResNum,
                Token = x.Token,
                Amount = x.Amount,
                Visible = x.Visible,
                JsonLableTexts = x.JsonLableTexts,
                IdentityCode = Guid.NewGuid(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<UpdateOrderPaymentTemp, OrderPaymentTemp>().ConvertUsing(x => new OrderPaymentTemp
            {
                ID = x.ID,
                ResNum = x.ResNum,
                Token = x.Token,
                Amount = x.Amount,
                Visible = x.Visible,

                JsonLableTexts = x.JsonLableTexts,
                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<OrderPaymentTemp, UpdateOrderPaymentTemp>().ConvertUsing(x => new UpdateOrderPaymentTemp
            {
                ID = x.ID,
                ResNum = x.ResNum,
                Token = x.Token,
                Amount = x.Amount,
                Visible = x.Visible,


                JsonLableTexts = x.JsonLableTexts,
                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
            });
            CreateMap<OrderPaymentTemp, SearchOrderPaymentTemp>().ConvertUsing(x => new SearchOrderPaymentTemp
            {
                ID = x.ID,
                ResNum = x.ResNum,
                Token = x.Token,
                Amount = x.Amount,
                Visible = x.Visible,


                JsonLableTexts = x.JsonLableTexts,
                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
            });
            CreateMap<ResultOrderPaymentTemp, UpdateOrderPaymentTemp>();
            CreateMap<OrderPaymentTemp, ResultOrderPaymentTemp>().ConvertUsing(x => new ResultOrderPaymentTemp
            {
                ID = x.ID,
                ResNum = x.ResNum,
                Token = x.Token,
                Amount = x.Amount,
                Visible = x.Visible,
                Slug = x.Slug,

                ResultJsonLables = x.JsonLableTexts != null ? JsonConvert.DeserializeObject<List<ResultJsonLable>>(x.JsonLableTexts) : new List<ResultJsonLable>(),
                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),

            });

            #endregion
            #region RefreshTokenEntity
            CreateMap<AddRefreshTokenEntity, RefreshTokenEntity>().ConvertUsing(x => new RefreshTokenEntity
            {
                Token = x.Token,
                UserId = x.UserId,
                DeviceId = x.DeviceId,
                ExpiryDate = x.ExpiryDate,
                CreatedAt = x.CreatedAt,
                IsRevoked = x.IsRevoked,
                ReplacedByToken = x.ReplacedByToken,
                RemoteIpAddress = x.RemoteIpAddress,

                JsonLableTexts = x.JsonLableTexts,
                Visible = x.Visible,
                IdentityCode = Guid.NewGuid(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<UpdateRefreshTokenEntity, RefreshTokenEntity>().ConvertUsing(x => new RefreshTokenEntity
            {
                ID = x.ID,
                Token = x.Token,
                UserId = x.UserId,
                DeviceId = x.DeviceId,
                ExpiryDate = x.ExpiryDate,
                CreatedAt = x.CreatedAt,
                IsRevoked = x.IsRevoked,
                ReplacedByToken = x.ReplacedByToken,
                RemoteIpAddress = x.RemoteIpAddress,
                JsonLableTexts = x.JsonLableTexts,
                Visible = x.Visible,

                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<RefreshTokenEntity, UpdateRefreshTokenEntity>().ConvertUsing(x => new UpdateRefreshTokenEntity
            {
                ID = x.ID,
                Token = x.Token,
                UserId = x.UserId,
                DeviceId = x.DeviceId,
                ExpiryDate = x.ExpiryDate,
                CreatedAt = x.CreatedAt,
                IsRevoked = x.IsRevoked,
                ReplacedByToken = x.ReplacedByToken,
                RemoteIpAddress = x.RemoteIpAddress,
                JsonLableTexts = x.JsonLableTexts,
                Visible = x.Visible,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
            });
            CreateMap<ResultRefreshTokenEntity, UpdateRefreshTokenEntity>();
            CreateMap<RefreshTokenEntity, ResultRefreshTokenEntity>().ConvertUsing(x => new ResultRefreshTokenEntity
            {
                ID = x.ID,
                Token = x.Token,
                UserId = x.UserId,
                DeviceId = x.DeviceId,
                ExpiryDate = x.ExpiryDate,
                CreatedAt = x.CreatedAt,
                IsRevoked = x.IsRevoked,
                ReplacedByToken = x.ReplacedByToken,
                RemoteIpAddress = x.RemoteIpAddress,
                JsonLableTexts = x.JsonLableTexts,
                Visible = x.Visible,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),


            });

            #endregion
            #region ProductFeature
            CreateMap<AddProductFeature, ProductFeature>().ConvertUsing(x => new ProductFeature
            {
                Title = x.Title,
                CategoryID = x.CategoryID,
                Visible = x.Visible,
                IdentityCode = Guid.NewGuid(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<UpdateProductFeature, ProductFeature>().ConvertUsing(x => new ProductFeature
            {
                ID = x.ID,
                CategoryID = x.CategoryID,
                Title = x.Title,
                Visible = x.Visible,
                IdentityCode = x.IdentityCode,

                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<ProductFeature, UpdateProductFeature>().ConvertUsing(x => new UpdateProductFeature
            {
                ID = x.ID,
                Title = x.Title,
                CategoryID = x.CategoryID,

                Visible = x.Visible,
                IdentityCode = x.IdentityCode,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
            });
            CreateMap<ResultProductFeature, UpdateProductFeature>();
            CreateMap<ProductFeature, ResultProductFeature>().ConvertUsing(x => new ResultProductFeature
            {
                ID = x.ID,
                CategoryID = x.CategoryID,
                Title = x.Title,
                Visible = x.Visible,

                IdentityCode = x.IdentityCode,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),

                ResultCategory = x.Category != null ? new ResultCategory
                {
                    ID = x.Category.ID,
                    Title = x.Category.Title,
                    ParentID = x.Category.ParentID,
                    Description = x.Category.Description,
                    Visible = x.Category.Visible,

                } : new ResultCategory()

            });

            #endregion
            #region ProductFeatureValue
            CreateMap<AddProductFeatureValue, ProductFeatureValue>().ConvertUsing(x => new ProductFeatureValue
            {
                ProductFeatureID = x.ProductFeatureID,
                ProductID = x.ProductID,
                Value = x.Value,
                Visible = x.Visible,
                IdentityCode = Guid.NewGuid(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<UpdateProductFeatureValue, ProductFeatureValue>().ConvertUsing(x => new ProductFeatureValue
            {
                ID = x.ID,
                ProductFeatureID = x.ProductFeatureID,
                ProductID = x.ProductID,
                Value = x.Value,
                Visible = x.Visible,
                IdentityCode = x.IdentityCode,
                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<ProductFeatureValue, UpdateProductFeatureValue>().ConvertUsing(x => new UpdateProductFeatureValue
            {
                ID = x.ID,
                ProductFeatureID = x.ProductFeatureID,
                ProductID = x.ProductID,
                Value = x.Value,
                Visible = x.Visible,
                IdentityCode = x.IdentityCode,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),

                ResultProductFeature = x.ProductFeature != null ? new ResultProductFeature
                {
                    ID = x.ProductFeature.ID,
                    Title = x.ProductFeature.Title,
                    Visible = x.ProductFeature.Visible,

                } : new ResultProductFeature(),
            });
            CreateMap<ResultProductFeatureValue, UpdateProductFeatureValue>();
            CreateMap<ProductFeatureValue, ResultProductFeatureValue>().ConvertUsing(x => new ResultProductFeatureValue
            {
                ID = x.ID,
                ProductFeatureID = x.ProductFeatureID,
                ProductID = x.ProductID,
                Value = x.Value,
                Visible = x.Visible,
                IdentityCode = x.IdentityCode,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),

                ResultProduct = x.Product != null ? new ResultProduct
                {
                    ID = x.Product.ID,
                    Title = x.Product.Title,
                    Description = x.Product.Description,
                    Visible = x.Product.Visible,

                } : new ResultProduct(),
                ResultProductFeature = x.ProductFeature != null ? new ResultProductFeature
                {
                    ID = x.ProductFeature.ID,
                    Title = x.ProductFeature.Title,
                    Visible = x.ProductFeature.Visible,

                } : new ResultProductFeature(),

            });

            #endregion
            #region PricingRule
            CreateMap<AddPricingRule, PricingRule>().ConvertUsing(x => new PricingRule
            {
                ProductID = x.ProductID,
                Price = x.Price,
                MinQuantity = x.MinQuantity,
                RoleID = x.RoleID,
                RuleType = x.RuleType,
                Title = x.Title,
                FromDate = DateFunctions.ConvertDateStringToInt(x.FromDate),
                ToDate = DateFunctions.ConvertDateStringToInt(x.ToDate),
                Visible = x.Visible,
                IdentityCode = Guid.NewGuid(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<UpdatePricingRule, PricingRule>().ConvertUsing(x => new PricingRule
            {
                ID = x.ID,
                ProductID = x.ProductID,
                Price = x.Price,
                MinQuantity = x.MinQuantity,
                RoleID = x.RoleID,
                RuleType = x.RuleType,
                Title = x.Title,
                FromDate = DateFunctions.ConvertDateStringToInt(x.FromDate),
                ToDate = DateFunctions.ConvertDateStringToInt(x.ToDate),
                Visible = x.Visible,

                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<PricingRule, UpdatePricingRule>().ConvertUsing(x => new UpdatePricingRule
            {
                ID = x.ID,
                ProductID = x.ProductID,
                Price = x.Price,
                MinQuantity = x.MinQuantity,
                RoleID = x.RoleID,
                RuleType = (int)x.RuleType,
                Title = x.Title,
                FromDate = DateFunctions.ConvertDateIntToString(x.FromDate),
                ToDate = DateFunctions.ConvertDateIntToString(x.ToDate),
                Visible = x.Visible,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
            });
            CreateMap<ResultPricingRule, UpdatePricingRule>();
            CreateMap<PricingRule, ResultPricingRule>().ConvertUsing(x => new ResultPricingRule
            {
                ID = x.ID,
                ProductID = x.ProductID,
                Price = x.Price,
                MinQuantity = x.MinQuantity,
                RoleID = x.RoleID,
                RuleType = EnumConstant.GetTitleRuleType(x.RuleType),
                RuleType2 = x.RuleType,
                Title = x.Title,
                FromDate = DateFunctions.ConvertDateIntToString(x.FromDate),
                ToDate = DateFunctions.ConvertDateIntToString(x.ToDate),
                Visible = x.Visible,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),

            });

            #endregion
            #region RawProduct
            CreateMap<AddRawProduct, RawProduct>().ConvertUsing(x => new RawProduct
            {
                Title = x.Title,
                Visible = x.Visible,
                Description = x.Description,

                IdentityCode = Guid.NewGuid(),
                RegisterDate = DateFunctions.GetDateNow(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditDate = DateFunctions.GetDateNow(),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),

            });
            CreateMap<UpdateRawProduct, RawProduct>().ConvertUsing(x => new RawProduct
            {
                ID = x.ID,
                Title = x.Title,
                Visible = x.Visible,
                Description = x.Description,

                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.GetDateNow(),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),

            }); ;
            CreateMap<RawProduct, UpdateRawProduct>().ConvertUsing(x => new UpdateRawProduct
            {
                ID = x.ID,
                Title = x.Title,

                Visible = x.Visible,
                Description = x.Description,

                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                EditTime = x.EditTime,

            });
            CreateMap<RawProduct, ResultRawProduct>().ConvertUsing(x => new ResultRawProduct
            {
                ID = x.ID,
                Title = x.Title,
                Visible = x.Visible,
                Description = x.Description,

                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                EditTime = x.EditTime,
            });
            #endregion
            #region RawProductStore
            CreateMap<AddRawProductStore, RawProductStore>().ConvertUsing(x => new RawProductStore
            {
                RawProductID = x.RawProductID,
                MessurmentType = (int)x.MessurmentType,
                Amount = x.Amount,
                Price = x.Price,
                Visible = x.Visible,
                BuyDate = DateFunctions.ConvertDateStringToInt(x.BuyDate),
                IdentityCode = Guid.NewGuid(),
                RegisterDate = DateFunctions.GetDateNow(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditDate = DateFunctions.GetDateNow(),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),

            });
            CreateMap<UpdateRawProductStore, RawProductStore>().ConvertUsing(x => new RawProductStore
            {
                ID = x.ID,
                RawProductID = x.RawProductID,
                MessurmentType = (int)x.MessurmentType,
                Amount = x.Amount,
                Price = x.Price,
                Visible = x.Visible,
                BuyDate = DateFunctions.ConvertDateStringToInt(x.BuyDate),

                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.GetDateNow(),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),

            }); ;
            CreateMap<RawProductStore, UpdateRawProductStore>().ConvertUsing(x => new UpdateRawProductStore
            {
                ID = x.ID,
                RawProductID = x.RawProductID,
                MessurmentType = (int)x.MessurmentType,
                Amount = x.Amount,
                Price = x.Price,
                Visible = x.Visible,
                BuyDate = DateFunctions.ConvertDateIntToString(x.BuyDate),

                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                EditTime = x.EditTime,

            });
            CreateMap<RawProductStore, ResultRawProductStore>().ConvertUsing(x => new ResultRawProductStore
            {
                ID = x.ID,
                RawProductID = x.RawProductID,
                MessurmentType = Dto.Enum.EnumConstant.GetTitleMessurmentType((MessurmentType)x.MessurmentType),
                MessurmentType2 = (MessurmentType)x.MessurmentType,
                Amount = x.Amount,
                Price = x.Price,
                SumPrice = x.Amount * x.Price,
                SliceCount = x.RawProductStore_Products.Count > 0 ? (int)x.RawProductStore_Products.Sum(k => k.Count) : 0,
                Visible = x.Visible,
                BuyDate = DateFunctions.ConvertDateIntToString(x.BuyDate),

                ResultRawProduct = new ResultRawProduct
                {
                    ID = x.RawProduct.ID,
                    Title = x.RawProduct.Title,
                    Description = x.RawProduct.Description,
                    Visible = x.RawProduct.Visible,

                },
                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                EditTime = x.EditTime,

            });
            #endregion
            #region Product_CountAction_CostType
            CreateMap<AddProduct_CountAction_CostType, Product_CountAction_CostType>().ConvertUsing(x => new Product_CountAction_CostType
            {
                PositionID = x.PositionID,
                ProductID = x.ProductID,
                CountAction = x.CountAction,
                Price = x.Price,
                Visible = x.Visible,

                IdentityCode = Guid.NewGuid(),
                RegisterDate = DateFunctions.GetDateNow(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditDate = DateFunctions.GetDateNow(),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),

            });
            CreateMap<UpdateProduct_CountAction_CostType, Product_CountAction_CostType>().ConvertUsing(x => new Product_CountAction_CostType
            {
                ID = x.ID,
                PositionID = x.PositionID,
                ProductID = x.ProductID,
                CountAction = x.CountAction,
                Price = x.Price,
                Visible = x.Visible,

                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.GetDateNow(),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),

            }); ;
            CreateMap<Product_CountAction_CostType, UpdateProduct_CountAction_CostType>().ConvertUsing(x => new UpdateProduct_CountAction_CostType
            {
                ID = x.ID,
                PositionID = x.PositionID,
                ProductID = x.ProductID,
                CountAction = x.CountAction,
                Price = x.Price,
                Visible = x.Visible,

                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                EditTime = x.EditTime,

            });
            CreateMap<Product_CountAction_CostType, ResultProduct_CountAction_CostType>().ConvertUsing(x => new ResultProduct_CountAction_CostType
            {
                ID = x.ID,
                PositionID = x.PositionID,
                ProductID = x.ProductID,
                CountAction = x.CountAction,
                Price = x.Price,
                Visible = x.Visible,
                ResultPosition = x.Position != null ? new ResultPosition
                {
                    ID = x.Position.ID,
                    Title = x.Position.Title,
                } : new ResultPosition(),
                ResultProduct = x.Product != null ? new ResultProduct
                {
                    ID = x.Product.ID,
                    Title = x.Product.Title,
                    ProductCode = x.Product.ProductCode,
                } : new ResultProduct(),

                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                EditTime = x.EditTime,
            });
            #endregion
            #region RegisterCostRawProductStore
            CreateMap<AddRegisterCostRawProductStore, RegisterCostRawProductStore>().ConvertUsing(x => new RegisterCostRawProductStore
            {
                RawProductStore_ProductID = x.RawProductStore_ProductID,
                PersonelID = x.PersonelID,
                Count = x.Count,
                Price = x.Price,
                Visible = x.Visible,

                IdentityCode = Guid.NewGuid(),
                RegisterDate = DateFunctions.GetDateNow(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditDate = DateFunctions.GetDateNow(),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),

            });
            CreateMap<UpdateRegisterCostRawProductStore, RegisterCostRawProductStore>().ConvertUsing(x => new RegisterCostRawProductStore
            {
                ID = x.ID,
                RawProductStore_ProductID = x.RawProductStore_ProductID,
                PersonelID = x.PersonelID,

                Count = x.Count,
                Price = x.Price,
                Visible = x.Visible,

                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.GetDateNow(),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),

            }); ;
            CreateMap<RegisterCostRawProductStore, UpdateRegisterCostRawProductStore>().ConvertUsing(x => new UpdateRegisterCostRawProductStore
            {
                ID = x.ID,
                RawProductStore_ProductID = x.RawProductStore_ProductID,
                PersonelID = x.PersonelID,

                Count = x.Count,
                Price = x.Price,
                SumPrice = x.Count * x.Price,
                Visible = x.Visible,
                ResultPersonel = x.Personel != null ? new ResultPersonel
                {
                    ID = x.Personel.ID,
                    PositionID = x.Personel.PositionID,
                    Name = x.Personel.Name,
                    LastName = x.Personel.LastName,


                } : new ResultPersonel(),
                ResultRawProductStore_Product = x.RawProductStore_Product != null ? new ResultRawProductStore_Product
                {
                    ID = x.RawProductStore_Product.ID,
                    ProductID = x.RawProductStore_Product.ProductID,

                } : new(),
                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                EditTime = x.EditTime,

            });
            CreateMap<RegisterCostRawProductStore, ResultRegisterCostRawProductStore>().ConvertUsing(x => new ResultRegisterCostRawProductStore
            {
                ID = x.ID,
                RawProductStore_ProductID = x.RawProductStore_ProductID,
                PersonelID = x.PersonelID,

                Count = x.Count,
                Price = x.Price,
                SumPrice = x.Count * x.Price,
                Visible = x.Visible,
                ResultRawProductStore_Product = x.RawProductStore_Product != null ? new ResultRawProductStore_Product
                {
                    ID = x.RawProductStore_Product.ID,
                    RawProductStoreID = x.RawProductStore_Product.RawProductStoreID,
                    ProductID = x.RawProductStore_Product.ProductID,
                    Count = x.RawProductStore_Product.Count,
                    Visible = x.RawProductStore_Product.Visible,
                    ResultProduct = x.RawProductStore_Product != null || x.RawProductStore_Product.Product != null ? new ResultProduct
                    {
                        ID = x.RawProductStore_Product.Product.ID,
                        Title = x.RawProductStore_Product.Product.Title,
                        ProductCode = x.RawProductStore_Product.Product.ProductCode,
                    } : new ResultProduct(),

                } : new ResultRawProductStore_Product(),
                ResultPersonel = x.Personel != null ? new ResultPersonel
                {
                    ID = x.Personel.ID,
                    Name = x.Personel.Name,
                    LastName = x.Personel.LastName,
                    BirthDate = x.Personel.BirthDate != null ? DateFunctions.ConvertDateIntToString((int)x.Personel.BirthDate) : null,
                    Mobile = x.Personel.Mobile,
                    Gender = EnumConstant.GetTitleGender(x.Personel.Gender),
                    JsonPicture = x.Personel.JsonPicture,
                    ImageUIrl = GetPathImage(x.Personel.JsonPicture),
                    Visible = x.Personel.Visible,
                    Address = x.Personel.Address,
                    ResultPosition = x.Personel.Position != null ? new ResultPosition
                    {
                        ID = x.Personel.Position.ID,
                        Title = x.Personel.Position.Title,
                    } : new(),
                } : new ResultPersonel(),
                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                EditTime = x.EditTime,
            });
            #endregion
            #region RawProductStore_Product
            CreateMap<AddRawProductStore_Product, RawProductStore_Product>().ConvertUsing(x => new RawProductStore_Product
            {
                RawProductStoreID = x.RawProductStoreID,
                ProductID = x.ProductID,
                Count = x.Count,
                Visible = x.Visible,

                IdentityCode = Guid.NewGuid(),
                RegisterDate = DateFunctions.GetDateNow(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditDate = DateFunctions.GetDateNow(),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),

            });
            CreateMap<UpdateRawProductStore_Product, RawProductStore_Product>().ConvertUsing(x => new RawProductStore_Product
            {
                ID = x.ID,
                RawProductStoreID = x.RawProductStoreID,
                ProductID = x.ProductID,
                Count = x.Count,
                Visible = x.Visible,

                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.GetDateNow(),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),

            }); ;
            CreateMap<RawProductStore_Product, UpdateRawProductStore_Product>().ConvertUsing(x => new UpdateRawProductStore_Product
            {
                ID = x.ID,
                RawProductStoreID = x.RawProductStoreID,
                ProductID = x.ProductID,
                Count = x.Count,
                Visible = x.Visible,

                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                EditTime = x.EditTime,

            });
            CreateMap<RawProductStore_Product, ResultRawProductStore_Product>().ConvertUsing(x => new ResultRawProductStore_Product
            {
                ID = x.ID,
                RawProductStoreID = x.RawProductStoreID,
                ProductID = x.ProductID,
                Count = x.Count,
                Visible = x.Visible,
                ResultProduct = x.Product != null ? new ResultProduct
                {
                    ID = x.Product.ID,
                    Title = x.Product.Title,
                    ProductCode = x.Product.ProductCode,
                } : new(),
                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                EditTime = x.EditTime,

            });
            #endregion
            #region Personel
            CreateMap<AddPersonel, Personel>().ConvertUsing(x => new Personel
            {
                PositionID = x.PositionID,
                Name = x.Name,
                LastName = x.LastName,
                BirthDate = x.BirthDate != null ? DateFunctions.ConvertDateStringToInt(x.BirthDate) : null,
                Mobile = x.Mobile,
                Gender = (int)x.Gender,
                JsonPicture = x.JsonPicture,
                Visible = x.Visible,
                JsonLableTexts = x.JsonLableTexts,
                Address = x.Address,
                Description = x.Description,
                IdentityCode = Guid.NewGuid(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<UpdatePersonel, Personel>().ConvertUsing(x => new Personel
            {
                ID = x.ID,
                PositionID = x.PositionID,
                Name = x.Name,
                LastName = x.LastName,
                BirthDate = x.BirthDate != null ? DateFunctions.ConvertDateStringToInt(x.BirthDate) : null,
                Mobile = x.Mobile,
                Gender = (int)x.Gender,
                JsonPicture = x.JsonPicture,
                Visible = x.Visible,
                JsonLableTexts = x.JsonLableTexts,
                Address = x.Address,
                Description = x.Description,

                RegisterTime = x.RegisterTime,
                IdentityCode = x.IdentityCode,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<Personel, UpdatePersonel>().ConvertUsing(x => new UpdatePersonel
            {
                ID = x.ID,
                PositionID = x.PositionID,
                Name = x.Name,
                LastName = x.LastName,
                BirthDate = x.BirthDate != null ? DateFunctions.ConvertDateIntToString((int)x.BirthDate) : null,
                Mobile = x.Mobile,
                Gender = (Gender)x.Gender,
                JsonPicture = x.JsonPicture,
                Visible = x.Visible,
                Slug = x.Slug,
                JsonLableTexts = x.JsonLableTexts,
                Address = x.Address,
                Description = x.Description,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),



            });
            CreateMap<Personel, ResultPersonel>().ConvertUsing(x => new ResultPersonel
            {
                ID = x.ID,
                PositionID = x.PositionID,
                Name = x.Name,
                LastName = x.LastName,
                BirthDate = x.BirthDate != null ? DateFunctions.ConvertDateIntToString((int)x.BirthDate) : null,
                Mobile = x.Mobile,
                Gender = EnumConstant.GetTitleGender(x.Gender),
                JsonPicture = x.JsonPicture,
                ImageUIrl = GetPathImage(x.JsonPicture),
                Visible = x.Visible,
                Slug = x.Slug,
                Address = x.Address,
                Description = x.Description,
                ResultPosition = x.Position != null ? new ResultPosition
                {
                    ID = x.Position.ID,
                    Title = x.Position.Title,
                } : new(),

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),

            });
            CreateMap<ResultPersonel, UpdatePersonel>();


            #endregion
            #region FavoritUserProduct
            CreateMap<AddFavoritUserProduct, FavoritUserProduct>().ConvertUsing(x => new FavoritUserProduct
            {
                CustomerID = x.CustomerID,
                ProductID = x.ProductID,
                Visible = x.Visible,
                IdentityCode = Guid.NewGuid(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<UpdateFavoritUserProduct, FavoritUserProduct>().ConvertUsing(x => new FavoritUserProduct
            {
                ID = x.ID,
                CustomerID = x.CustomerID,
                ProductID = x.ProductID,
                Visible = x.Visible,

                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<FavoritUserProduct, UpdateFavoritUserProduct>().ConvertUsing(x => new UpdateFavoritUserProduct
            {
                ID = x.ID,
                CustomerID = x.CustomerID,
                ProductID = x.ProductID,
                Visible = x.Visible,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
            });
            CreateMap<ResultFavoritUserProduct, UpdateFavoritUserProduct>();
            CreateMap<FavoritUserProduct, ResultFavoritUserProduct>().ConvertUsing(x => new ResultFavoritUserProduct
            {
                ID = x.ID,
                CustomerID = x.CustomerID,
                ProductID = x.ProductID,
                Visible = x.Visible,
                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),

                ResultProduct = x.Product != null ? new ResultProduct
                {
                    ID = x.Product.ID,
                    Title = x.Product.Title,
                    Count = x.Product.Count,
                    Price = x.Product.Price,
                    CategoryID = x.Product.CategoryID,
                    Visible = x.Product.Visible,
                    JsonPicture = x.Product.JsonPicture,
                    ShortDescription = x.Product.ShortDescription,
                    Description = x.Product.Description,
                    Discount = x.Product.Discount,
                    Brand = x.Product.Brand,
                    ProductCode = x.Product.ProductCode,
                    ProductExistStatus = GetProductExistStatus(x.Product.ProductExistStatus),
                    ProductExistStatus2 = (ProductExistStatus)x.Product.ProductExistStatus,
                    SkuCode = x.Product.SkuCode,
                    ResultUploadFiles = GetResultUploadFiles(x.Product.JsonPicture),
                    IdentityCode = x.Product.IdentityCode,
                    RegisterDate = DateFunctions.ConvertDateIntToString(x.Product.RegisterDate),
                    RegisterTime = x.RegisterTime,
                    EditDate = DateFunctions.ConvertDateIntToString(x.Product.EditDate),
                    EditTime = x.Product.EditTime,
                    ResultCategory = x.Product.Category != null ? new ResultCategory
                    {
                        ID = x.Product.Category.ID,
                        Title = x.Product.Category.Title,
                        ParentID = x.Product.Category.ParentID,
                        Description = x.Product.Category.Description,
                        Visible = x.Product.Category.Visible,

                    } : new ResultCategory(),

                } : new()

            });

            #endregion
            #region Position
            CreateMap<AddPosition, Position>().ConvertUsing(x => new Position
            {
                Title = x.Title,
                CostType = (int)x.CostType,
                Price = x.Price,
                Visible = x.Visible,
                Description = x.Description,

                IdentityCode = Guid.NewGuid(),
                RegisterDate = DateFunctions.GetDateNow(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditDate = DateFunctions.GetDateNow(),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),

            });
            CreateMap<UpdatePosition, Position>().ConvertUsing(x => new Position
            {
                ID = x.ID,
                Title = x.Title,
                CostType = (int)x.CostType,
                Price = x.Price,
                Visible = x.Visible,
                Description = x.Description,

                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.GetDateNow(),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),

            }); ;
            CreateMap<Position, UpdatePosition>().ConvertUsing(x => new UpdatePosition
            {
                ID = x.ID,
                Title = x.Title,
                CostType = x.CostType.HasValue
                            ? (int)x.CostType.Value
                            : null,
                Price = x.Price,
                Visible = x.Visible,
                Description = x.Description,

                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                EditTime = x.EditTime,

            });
            CreateMap<Position, ResultPosition>().ConvertUsing(x => new ResultPosition
            {
                ID = x.ID,
                Title = x.Title,
                CostType = GetTitleCostType(x.CostType),
                CostType2 = x.CostType != null ? (CostType)x.CostType : null,
                Price = x.Price,
                Visible = x.Visible,
                Description = x.Description,
                ResultProduct_CountAction_CostTypes = x.Product_CountAction_CostTypes != null && x.Product_CountAction_CostTypes.Count > 0 ? x.Product_CountAction_CostTypes.Select(k => new ResultProduct_CountAction_CostType
                {
                    ID = k.ID,
                    PositionID = k.PositionID,
                    ProductID = k.ProductID,
                    CountAction = k.CountAction,
                    Price = k.Price,
                    Visible = k.Visible,
                    ResultPosition = k.Position != null ? new ResultPosition
                    {
                        ID = k.Position.ID,
                        Title = k.Position.Title,
                    } : new ResultPosition(),
                    ResultProduct = k.Product != null ? new ResultProduct
                    {
                        ID = k.Product.ID,
                        Title = k.Product.Title,
                    } : new ResultProduct(),
                }).ToList() : new List<ResultProduct_CountAction_CostType>(),

                IdentityCode = x.IdentityCode,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                RegisterTime = x.RegisterTime,
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                EditTime = x.EditTime,
            });
            #endregion
            #region FavoritUserProduct
            CreateMap<AddFavoritUserProduct, FavoritUserProduct>().ConvertUsing(x => new FavoritUserProduct
            {
                ProductID = x.ProductID,
                CustomerID = x.CustomerID,
                JsonLableTexts = x.JsonLableTexts,
                Visible = x.Visible,
                IdentityCode = Guid.NewGuid(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<UpdateFavoritUserProduct, FavoritUserProduct>().ConvertUsing(x => new FavoritUserProduct
            {
                ID = x.ID,
                ProductID = x.ProductID,
                CustomerID = x.CustomerID,
                JsonLableTexts = x.JsonLableTexts,
                Visible = x.Visible,


                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<FavoritUserProduct, UpdateFavoritUserProduct>().ConvertUsing(x => new UpdateFavoritUserProduct
            {
                ID = x.ID,
                ProductID = x.ProductID,
                CustomerID = x.CustomerID,
                JsonLableTexts = x.JsonLableTexts,
                Visible = x.Visible,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
            });
            CreateMap<ResultFavoritUserProduct, UpdateFavoritUserProduct>();
            CreateMap<FavoritUserProduct, ResultFavoritUserProduct>().ConvertUsing(x => new ResultFavoritUserProduct
            {
                ID = x.ID,
                ProductID = x.ProductID,
                CustomerID = x.CustomerID,
                JsonLableTexts = x.JsonLableTexts,
                Visible = x.Visible,


                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
                ResultProduct = x.Product != null ? new ResultProduct
                {
                    ID = x.Product.ID,
                    Title = x.Product.Title,
                    Count = x.Product.Count,
                    Price = x.Product.Price,
                    CategoryID = x.Product.CategoryID,
                    Visible = x.Product.Visible,
                    JsonPicture = x.Product.JsonPicture,
                    ShortDescription = x.Product.ShortDescription,
                    Description = x.Product.Description,
                    Discount = x.Product.Discount,
                    Brand = x.Product.Brand,
                    ProductCode = x.Product.ProductCode,
                    ProductExistStatus = GetProductExistStatus(x.Product.ProductExistStatus),
                    ProductExistStatus2 = (ProductExistStatus)x.Product.ProductExistStatus,
                    SkuCode = x.Product.SkuCode,
                    ResultUploadFiles = GetResultUploadFiles(x.Product.JsonPicture),
                    IdentityCode = x.Product.IdentityCode,
                    RegisterDate = DateFunctions.ConvertDateIntToString(x.Product.RegisterDate),
                    RegisterTime = x.Product.RegisterTime,
                    EditDate = DateFunctions.ConvertDateIntToString(x.Product.EditDate),
                    EditTime = x.Product.EditTime,

                    ResultCategory = x.Product.Category != null ? new ResultCategory
                    {
                        ID = x.Product.Category.ID,
                        Title = x.Product.Category.Title,
                        ParentID = x.Product.Category.ParentID,
                        Description = x.Product.Category.Description,
                        Visible = x.Product.Category.Visible,

                    } : new ResultCategory(),
                    ResultPricingRules = x.Product.PricingRules != null && x.Product.PricingRules.Count > 0 ? x.Product.PricingRules.Select(k => new ResultPricingRule
                    {
                        ID = k.ID,
                        ProductID = k.ProductID,
                        Price = k.Price,
                        MinQuantity = k.MinQuantity,
                        RoleID = k.RoleID,
                        RuleType = EnumConstant.GetTitleRuleType(k.RuleType),
                        RuleType2 = k.RuleType,
                        Title = k.Title,
                        FromDate = DateFunctions.ConvertDateIntToString(k.FromDate),
                        ToDate = DateFunctions.ConvertDateIntToString(k.ToDate),
                        Visible = k.Visible,

                        RegisterTime = k.RegisterTime,
                        EditTime = k.EditTime,
                        RegisterDate = DateFunctions.ConvertDateIntToString(k.RegisterDate),
                        EditDate = DateFunctions.ConvertDateIntToString(k.EditDate),

                    }).ToList() : new List<ResultPricingRule>()
                } : new ResultProduct(),


            });

            #endregion
            #region ContactUs
            CreateMap<AddContactUs, ContactUs>().ConvertUsing(x => new ContactUs
            {
                FullName = x.FullName,
                Subject = x.Subject,
                Message = x.Message,
                Email = x.Email,
                Visible = x.Visible,
                Mobile = x.Mobile,
                IsRead = x.IsRead,
                IpAddress = x.IpAddress,
                IdentityCode = Guid.NewGuid(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<UpdateContactUs, ContactUs>().ConvertUsing(x => new ContactUs
            {
                ID = x.ID,
                FullName = x.FullName,
                Subject = x.Subject,
                Message = x.Message,
                Email = x.Email,
                Visible = x.Visible,
                Mobile = x.Mobile,
                IsRead = x.IsRead,
                IpAddress = x.IpAddress,

                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<ContactUs, UpdateContactUs>().ConvertUsing(x => new UpdateContactUs
            {
                ID = x.ID,
                FullName = x.FullName,
                Subject = x.Subject,
                Message = x.Message,
                Email = x.Email,
                Visible = x.Visible,
                Mobile = x.Mobile,
                IsRead = x.IsRead,
                IpAddress = x.IpAddress,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
            });
            CreateMap<ResultContactUs, UpdateContactUs>();
            CreateMap<ContactUs, ResultContactUs>().ConvertUsing(x => new ResultContactUs
            {
                ID = x.ID,
                FullName = x.FullName,
                Subject = x.Subject,
                Message = x.Message,
                Email = x.Email,
                Mobile = x.Mobile,
                Visible = x.Visible,
                IsRead = GetIsReadTitle(x.IsRead),
                IpAddress = x.IpAddress,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),


            });
            #endregion
            #region Faq
            CreateMap<AddFaq, Faq>().ConvertUsing(x => new Faq
            {
                FullName = x.FullName,
                Subject = x.Subject,
                Message = x.Message,
                Visible = x.Visible,
                Mobile = x.Mobile,
                IsRead = x.IsRead,
                IpAddress = x.IpAddress,

                IdentityCode = Guid.NewGuid(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),
            });
            CreateMap<UpdateFaq, Faq>().ConvertUsing(x => new Faq
            {
                ID = x.ID,
                FullName = x.FullName,
                Subject = x.Subject,
                Message = x.Message,
                Visible = x.Visible,
                Mobile = x.Mobile,
                IsRead = x.IsRead,
                IpAddress = x.IpAddress,

                RegisterTime = x.RegisterTime,
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateStringToInt(GetNewDate()),

            });
            CreateMap<Faq, UpdateFaq>().ConvertUsing(x => new UpdateFaq
            {
                ID = x.ID,
                FullName = x.FullName,
                Subject = x.Subject,
                Message = x.Message,
                Visible = x.Visible,
                Mobile = x.Mobile,
                IsRead = x.IsRead,
                IpAddress = x.IpAddress,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),
            });
            CreateMap<ResultFaq, UpdateFaq>();
            CreateMap<Faq, ResultFaq>().ConvertUsing(x => new ResultFaq
            {
                ID = x.ID,
                FullName = x.FullName,
                Subject = x.Subject,
                Message = x.Message,
                Mobile = x.Mobile,
                Visible = x.Visible,
                IsRead = GetIsReadTitle(x.IsRead),
                IpAddress = x.IpAddress,

                RegisterTime = x.RegisterTime,
                EditTime = x.EditTime,
                RegisterDate = DateFunctions.ConvertDateIntToString(x.RegisterDate),
                EditDate = DateFunctions.ConvertDateIntToString(x.EditDate),


            });
            #endregion







           
            #region About
            CreateMap<AddAbout, About>()
                .ForMember(des => des.IdentityCode, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(Guid.NewGuid().ToString())))
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateAbout, About>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<About, UpdateAbout>()
                .ForMember(des => des.ResultUploadFiles, s => s.MapFrom(x => GetResultUploadFiles(x.JsonPicture)))
                  .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));


            CreateMap<About, ResultAbout>()
                  .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));
            #endregion
            #region City
            CreateMap<AddCity, City>()
                .ForMember(des => des.IdentityCode, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(Guid.NewGuid().ToString())))
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateCity, City>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<City, UpdateCity>()
               .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
               .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)))
               .ForMember(des => des.ResultProvince, s => s.MapFrom(x => x.Province != null ? new ResultProvince
               {
                   ID = x.Province.ID,
                   Title = x.Province.Title,

               } : new ResultProvince()
                ));
            CreateMap<City, ResultCity>()
              .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
              .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)))
              .ForMember(des => des.ResultProvince, s => s.MapFrom(x => x.Province != null ? new ResultProvince
              {
                  ID = x.Province.ID,
                  Title = x.Province.Title,

              } : new ResultProvince()
                ));
            #endregion
            #region Team
            CreateMap<AddTeam, Team>()
                .ForMember(des => des.IdentityCode, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(Guid.NewGuid().ToString())))
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateTeam, Team>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<Team, UpdateTeam>()
                  .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));


            CreateMap<Team, ResultTeam>()
                .ForMember(des => des.ResultUploadFiles, s => s.MapFrom(x => GetResultUploadFiles(x.JsonPictures)))
                  .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));
            #endregion
            #region Position
            CreateMap<AddPosition, Position>()
                .ForMember(des => des.IdentityCode, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(Guid.NewGuid().ToString())))
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdatePosition, Position>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<Position, UpdatePosition>()
               .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
               .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<Position, ResultPosition>()
              .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
              .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));
            #endregion
            #region FavoritUserProduct
            CreateMap<AddFavoritUserProduct, FavoritUserProduct>()
                .ForMember(des => des.IdentityCode, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(Guid.NewGuid().ToString())))
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateFavoritUserProduct, FavoritUserProduct>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<FavoritUserProduct, UpdateFavoritUserProduct>()
               .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
               .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<FavoritUserProduct, ResultFavoritUserProduct>()
              .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
              .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)))
              .ForMember(des => des.ResultProduct, s => s.MapFrom(x => x.Product != null ? new ResultProduct
              {
                  ID = x.Product.ID,
                  Title = x.Product.Title,
                  Count = x.Product.Count,
                  Price = x.Product.Price,
                  CategoryID = x.Product.CategoryID,
                  Visible = x.Product.Visible,
                  JsonPicture = x.Product.JsonPicture,
                  ShortDescription = x.Product.ShortDescription,
                  Description = x.Product.Description,
                  Discount = x.Product.Discount,
                  Brand = x.Product.Brand,
                  ProductCode = x.Product.ProductCode,
                  ProductExistStatus = GetProductExistStatus(x.Product.ProductExistStatus),
                  ProductExistStatus2 = (ProductExistStatus)x.Product.ProductExistStatus,
                  SkuCode = x.Product.SkuCode,
                  ResultUploadFiles = GetResultUploadFiles(x.Product.JsonPicture),
                  IdentityCode = x.Product.IdentityCode,
                  RegisterDate = DateFunctions.ConvertDateIntToString(x.Product.RegisterDate),
                  RegisterTime = x.Product.RegisterTime,
                  EditDate = DateFunctions.ConvertDateIntToString(x.Product.EditDate),
                  EditTime = x.Product.EditTime,
              } : new ResultProduct()
                ));
            #endregion
            #region ContactUs
            CreateMap<AddContactUs, ContactUs>()
                .ForMember(des => des.IdentityCode, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(Guid.NewGuid().ToString())))
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateContactUs, ContactUs>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<ContactUs, UpdateContactUs>()
                  .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));


            CreateMap<ContactUs, ResultContactUs>()

                .ForMember(des => des.IsRead, s => s.MapFrom(x => x.IsRead == true ? ("خوانده شده") : ("خوانده نشده")))
                .ForMember(des => des.IsRead, s => s.MapFrom(x => x.IsRead))
                  .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));
            #endregion
            #region Faq
            CreateMap<AddFaq, Faq>()
                .ForMember(des => des.IdentityCode, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(Guid.NewGuid().ToString())))
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateFaq, Faq>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<Faq, UpdateFaq>()
                  .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));


            CreateMap<Faq, ResultFaq>()

                .ForMember(des => des.IsRead, s => s.MapFrom(x => x.IsRead == true ? ("خوانده شده") : ("خوانده نشده")))
                .ForMember(des => des.IsRead, s => s.MapFrom(x => x.IsRead))
                  .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));
            #endregion
            #region GroupQuestion
            CreateMap<AddGroupQuestion, GroupQuestion>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));
            CreateMap<UpdateGroupQuestion, GroupQuestion>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<GroupQuestion, UpdateGroupQuestion>()
                  .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));


            CreateMap<GroupQuestion, ResultGroupQuestion>()
                  .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));
            #endregion
            #region Question
            CreateMap<AddQuestion, Question>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));
            CreateMap<UpdateQuestion, Question>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<Question, UpdateQuestion>()
                  .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));


            CreateMap<Question, ResultQuestion>()
                  .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)))
                .ForMember(des => des.ResultGroupQuestion, s => s.MapFrom(x => x.GroupQuestion != null ? new ResultGroupQuestion
                {
                    Title = x.GroupQuestion.Title
                } : new ResultGroupQuestion()
                ));

            #endregion
            #region ProductComment
            CreateMap<AddProductComment, ProductComment>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));
            CreateMap<UpdateProductComment, ProductComment>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<ProductComment, UpdateProductComment>()
                  .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));


            CreateMap<ProductComment, ResultProductComment>()
                .ForMember(des => des.IsVerifiedBuyer, s => s.MapFrom(x => x.IsVerifiedBuyer
         ? "<span class='inline-flex items-center px-2 py-1 rounded-full text-xs font-semibold bg-green-100 text-green-800'>✓ بله</span>"
        : "<span class='inline-flex items-center px-2 py-1 rounded-full text-xs font-semibold bg-red-100 text-red-800'>✕ خیر</span>"))
                .ForMember(des => des.Status, s => s.MapFrom(x => EnumConstant.GetTitleCommentStatus(x.Status)))
                .ForMember(des => des.Status2, s => s.MapFrom(x => x.Status))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)))
                .ForMember(des => des.ResultProduct, s => s.MapFrom(x => x.Product != null ? new ResultProduct
                {
                    Title = x.Product.Title
                } : new ResultProduct()
                ))

            .ForMember(des => des.ResultCustomer, s => s.MapFrom(x => x.Customer != null ? new ResultCustomer
            {
                Name = x.Customer.Name
            } : new ResultCustomer()
                ));
            #endregion
            #region Ticket
            CreateMap<AddTicket, Ticket>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));
            CreateMap<UpdateTicket, Ticket>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<Ticket, UpdateTicket>()
                  .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));


            CreateMap<Ticket, ResultTicket>()
                  .ForMember(des => des.TicketPriority, s => s.MapFrom(x => EnumConstant.GetTitleTicketPriority(x.TicketPriority)))
                  .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)))
                .ForMember(des => des.ResultDepartment, s => s.MapFrom(x => x.Department != null ? new ResultDepartment
                {
                    Title = x.Department.Title
                } : new ResultDepartment()
                ));

            #endregion
            #region Department
            CreateMap<AddDepartment, Department>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));
            CreateMap<UpdateDepartment, Department>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<Department, UpdateDepartment>()
                  .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));


            CreateMap<Department, ResultDepartment>()
                  .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));
            #endregion
        }


        private float CalCulatorAverageRatingProductComment(ICollection<ProductComment> productComments)
        {
            if (productComments is null)
                return 0;
            var count = productComments.Count(s => s.Rating.HasValue);
            if (count == 0)
                return 0;
            var ratingAverage = count == 0
                ? 0m
                : Math.Round(
                    (decimal)(productComments.Sum(s => s.Rating) ?? 0) / count,
                    2,
                    MidpointRounding.AwayFromZero);
            return (float)ratingAverage;
        }
        private string GetIsReadTitle(bool isRead)
        {
            switch (isRead)
            {
                case true:
                    return "<span style=\"color:green\">خوانده شده</span>";
                case false:
                    return "<span style=\"color:red\">خوانده نشده</span>";
            }
        }

        private List<TelJson> ConvertTelJsonToList(string? jsonTel)
        {

            return jsonTel != null ? JsonConvert.DeserializeObject<List<TelJson>>(jsonTel) : new List<TelJson>();

        }
        private List<MobileJson> ConvertMobileJsonToList(string? jsonMobile)
        {

            return jsonMobile != null ? JsonConvert.DeserializeObject<List<MobileJson>>(jsonMobile) : new List<MobileJson>();

        }
        private string? ConvertTelListToJson(List<TelJson> TelLists)
        {

            return TelLists.Count > 0 ? JsonConvert.SerializeObject(TelLists) : null;

        }
        private string? ConvertMobileListToJson(List<MobileJson> MobileLists)
        {

            return MobileLists.Count > 0 ? JsonConvert.SerializeObject(MobileLists) : null;

        }
        private string GetTextJsonLableTexts(string jsonLableTexts)
        {
            string Texts = string.Empty;
            string lables = jsonLableTexts != null ? JsonConvert.DeserializeObject<string>(jsonLableTexts) : string.Empty;
            foreach (var label in lables)
            {
                Texts += label;
            }
            return Texts;
        }
        private ResultCustomerAddress GetResultCustomerAddress(string json)
        {

            ResultCustomerAddress model = json != null ? JsonConvert.DeserializeObject<ResultCustomerAddress>(json) : new ResultCustomerAddress();
            return model;
        }
        private string? GetPathImage(string? JsonPicture)
        {
            string? path = string.Empty;
            if (JsonPicture != null)
            {
                var result = JsonConvert.DeserializeObject<List<ResultUploadFile>>(JsonPicture)!;
                return result.Count > 0 ? ApiLink.ftpRootPublicHtmlPath + result[0].PathFileName : string.Empty;

            }
            return string.Empty;

        }
        private List<ResultUploadFile>? GetResultUploadFiles(string? JsonPicture)
        {
            return JsonPicture != null ? JsonConvert.DeserializeObject<List<ResultUploadFile>>(JsonPicture) : new List<ResultUploadFile>();
        }
        public string GetStatusOrder(OrderStatus orderStatus)
        {
            switch (orderStatus)
            {
                case OrderStatus.canceled:
                    return "لغو شده";
                case OrderStatus.pending:
                    return "درحال بررسی";
                case OrderStatus.delivered:
                    return "تحویل داده شده";
                case OrderStatus.shipped:
                    return "ارسال شده";
                case OrderStatus.PendingPayment:
                    return "در انتظار پرداخت";
                default:
                    return "نامشخص";

            }
        }
        public string GetPaymentStatus(PaymentStatus paymentStatus)
        {
            switch (paymentStatus)
            {
                case PaymentStatus.paid:
                    return "پرداخت موفق";
                case PaymentStatus.failed:
                    return "پرداخت ناموفق";
                case PaymentStatus.PendingPayment:
                    return "در انتظار پرداخت";
                case PaymentStatus.Expired:
                    return "مهلت پرداخت تمام شده";
                case PaymentStatus.Refunded:
                    return "برگشت وجه";
                default:
                    return "نامشخص";

            }
        }
        public string GetTransactionKind(TransactionKind transactionKind)
        {
            switch (transactionKind)
            {
                case TransactionKind.Adjustment:
                    return "اصلاح دستی ادمین";
                case TransactionKind.Refund:
                    return "برگشت وجه";
                case TransactionKind.Deposit:
                    return "واریز کیف پول";
                case TransactionKind.Purchase:
                    return "پرداخت خرید از طریق درگاه";
                case TransactionKind.Withdrawal:
                    return "برداشت از کیف پول";
                case TransactionKind.Fee:
                    return "کارمزد";
                default:
                    return "نامشخص";

            }
        }
        public string GetCartStatus(int cartStatus)
        {
            switch (cartStatus)
            {
                case (int)CartStatus.active:
                    return "باز";
                case (int)CartStatus.checked_out:
                    return "پرداخت شده و بسته شده";
                case (int)CartStatus.abandoned:
                    return "رها شده و مدت زیادی گذشته";
                default:
                    return "نامشخص";
            }
        }
        private string GetProductExistStatus(int? productExistStatus)
        {
            switch (productExistStatus)
            {
                case (int)ProductExistStatus.Existent:
                    return "موجود";
                case (int)ProductExistStatus.Nonexistent:
                    return "ناموجود";
                default:
                    return "نامشخص";

            }
        }

        public string GetTransactionStatuse(TransactionStatus transactionStatus)
        {
            switch (transactionStatus)
            {
                case TransactionStatus.Pending:
                    return "درحال انتظار";
                case TransactionStatus.Cancelled:
                    return "لغو شده";
                case TransactionStatus.Reversed:
                    return "برگشت داده شده";
                case TransactionStatus.Failed:
                    return "ناموفق";
                case TransactionStatus.Completed:
                    return "تکمیل شده";
                default:
                    return "نامشخص";

            }
        }
        private string GetNewDate()
        {
            return PersianDateExtensionMethods.GetMiladiToPersianDate(DateTime.Now);
        }
    }
}

