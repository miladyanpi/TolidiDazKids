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
using Dto.Models.DtoCategoryTrait;
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
using Dto.Models.DtoProductVariant;
using Dto.Models.DtoProductVariantValue;
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
using Dto.Models.DtoTrait;
using Dto.Models.DtoTraitValue;
using Dto.Models.DtoUploadFile;
using Dto.Models.DtoWallet;
using Dto.Models.DtoWalletTransaction;
using MappingProfile.FrpRoot;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Utility;
using static Dto.Enum.EnumConstant;
namespace MappingProfile.DtoMappingConfigs
{
    public class DtoMappingProfile : Profile
    {

        public DtoMappingProfile()
        {

            #region About
            CreateMap<AddAbout, About>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateAbout, About>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
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
            #region Category
            CreateMap<AddCategory, Category>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateCategory, Category>()
                .ForMember(des => des.ParentID, s => s.MapFrom(x => x.ParentID == 0 ? null : x.ParentID))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<Category, UpdateCategory>()
                .ForMember(des => des.ParentID1, s => s.MapFrom(x => x.ParentID))
                .ForMember(des => des.ParentResultCategory, s => s.MapFrom(x =>
                x.Parent != null ? new ResultCategory
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

                } : new ResultCategory()

                ))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<Category, ResultCategory>()
                .ForMember(des => des.Count, s => s.MapFrom(x => x.Categories.Count()))
                .ForMember(des => des.ResultUploadFiles, s => s.MapFrom(x => GetResultUploadFiles(x.JsonPicture)))
                .ForMember(des => des.ResultCategorys, s => s.MapFrom(x =>
                x.Categories != null && x.Categories.Count > 0 ? x.Categories.Select(s => new ResultCategory
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
                }).ToList() : new List<ResultCategory>()))
                  .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));


            #endregion
            #region Product
            CreateMap<AddProduct, Product>()
                 .ForMember(des => des.ProductExistStatus, s => s.MapFrom(x => (int)x.ProductExistStatus))
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateProduct, Product>()
                .ForMember(des => des.ProductExistStatus, s => s.MapFrom(x => (int)x.ProductExistStatus))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<Product, UpdateProduct>()
                .ForMember(des => des.ProductExistStatus, s => s.MapFrom(x => (ProductExistStatus)x.ProductExistStatus))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)))
                .ForMember(des => des.ResultCategory, s => s.MapFrom(x => x.Category != null ? new ResultCategory
                {
                    ID = x.Category.ID,
                    Title = x.Category.Title,
                    ParentID = x.Category.ParentID,
                } : new ResultCategory()
                ));

            CreateMap<Product, ResultProduct>()
                 .ForMember(des => des.PricingType, s => s.MapFrom(x => EnumConstant.GetTitlePricingType(x.PricingType)))
                 .ForMember(des => des.PricingType2, s => s.MapFrom(x => x.PricingType))
                 .ForMember(des => des.ProductExistStatus, s => s.MapFrom(x => GetProductExistStatus(x.ProductExistStatus)))
                 .ForMember(des => des.ProductExistStatus2, s => s.MapFrom(x => (ProductExistStatus)x.ProductExistStatus))
                 .ForMember(des => des.ResultUploadFiles, s => s.MapFrom(x => GetResultUploadFiles(x.JsonPicture)))
                 .ForMember(des => des.RatingAverage, s => s.MapFrom(x => CalCulatorAverageRatingProductComment(x.ProductComments)))
                 .ForMember(des => des.ProductCommentCount, s => s.MapFrom(x => x.ProductComments != null ? x.ProductComments.Count() : 0))
                 .ForMember(des => des.ResultProductVariants, s => s.MapFrom(x => x.ProductVariants))
                 .ForMember(des => des.ResultCategory, s => s.MapFrom(x => x.Category != null ? new ResultCategory
                 {
                     ID = x.Category.ID,
                     Title = x.Category.Title,
                     ParentID = x.Category.ParentID,
                     Description = x.Category.Description,
                     Visible = x.Category.Visible,

                 } : new ResultCategory()
                ))
                .ForMember(des => des.ResultPricingRules, s => s.MapFrom(x => x.PricingRules != null && x.PricingRules.Count > 0 ? x.PricingRules.Select(k => new ResultPricingRule
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
                ))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));
                




            #endregion
            #region Setting
            CreateMap<AddSetting, Setting>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateSetting, Setting>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<ResultSetting, UpdateSetting>()
                .ForMember(des => des.CurrencyUnit, s => s.MapFrom(x => x.CurrencyUnit2))
                .ForMember(des => des.JsonTel, s => s.MapFrom(x => ConvertTelListToJson(x.ResultTels)))
                .ForMember(des => des.JsonMobile, s => s.MapFrom(x => ConvertMobileListToJson(x.ResultMobiles)));

            CreateMap<Setting, UpdateSetting>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<Setting, ResultSetting>()
                .ForMember(des => des.CurrencyUnit2, s => s.MapFrom(x => x.CurrencyUnit))
                .ForMember(des => des.CurrencyUnit, s => s.MapFrom(x => GetTitleCurrencyUnit(x.CurrencyUnit)))
                .ForMember(des => des.ResultTels, s => s.MapFrom(x => ConvertTelJsonToList(x.JsonTel)))
                .ForMember(des => des.ResultMobiles, s => s.MapFrom(x => ConvertMobileJsonToList(x.JsonMobile)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<Setting, ResultPublicSetting>()
                .ForMember(des => des.ResultTels, s => s.MapFrom(x => ConvertTelJsonToList(x.JsonTel)))
                .ForMember(des => des.ResultMobiles, s => s.MapFrom(x => ConvertMobileJsonToList(x.JsonMobile)));

            #endregion
            #region Customer
            CreateMap<AddCustomer, Customer>()
                .ForMember(des => des.BirthDate, s => s.MapFrom(x => x.BirthDate == null ? (int?)null : DateFunctions.ConvertDateStringToInt(x.BirthDate)))
                .ForMember(des => des.Gender, s => s.MapFrom(x => (int)x.Gender))
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateCustomer, Customer>()
                .ForMember(des => des.BirthDate, s => s.MapFrom(x => x.BirthDate == null ? (int?)null : DateFunctions.ConvertDateStringToInt(x.BirthDate)))
                .ForMember(des => des.Gender, s => s.MapFrom(x => (int)x.Gender))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<UpdateCustomerInfo, Customer>()
                .ForMember(des => des.BirthDate, s => s.MapFrom(x => x.BirthDate == null ? (int?)null : DateFunctions.ConvertDateStringToInt(x.BirthDate)))
                .ForMember(des => des.Gender, s => s.MapFrom(x => (int)x.Gender))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<Customer, UpdateCustomer>()
                .ForMember(des => des.BirthDate, s => s.MapFrom(x => x.BirthDate != null ? DateFunctions.ConvertDateIntToString((int)x.BirthDate) : null))
                .ForMember(des => des.Gender, s => s.MapFrom(x => (Gender)x.Gender))
                .ForMember(des => des.ResultCustomerUserInfo, s => s.MapFrom(x => new ResultCustomerUserInfo
                {
                    Mobile = x.Mobile,
                    Email = x.Email,

                }))
                .ForMember(des => des.UpdateCustomerAddresss, s => s.MapFrom(x => x.CustomerAddresss != null ? x.CustomerAddresss.Select(s => new UpdateCustomerAddress
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
                    Visible = s.Visible,
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

                }).ToList() : new List<UpdateCustomerAddress>()))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<Customer, UpdateCustomerInfo>()
                .ForMember(des => des.BirthDate, s => s.MapFrom(x => x.BirthDate != null ? DateFunctions.ConvertDateIntToString((int)x.BirthDate) : null))
                .ForMember(des => des.Gender, s => s.MapFrom(x => (Gender)x.Gender))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<Customer, ResultCustomer>()
                .ForMember(des => des.BirthDate, s => s.MapFrom(x => x.BirthDate != null ? DateFunctions.ConvertDateIntToString((int)x.BirthDate) : null))
                .ForMember(des => des.Gender, s => s.MapFrom(x => EnumConstant.GetTitleGender(x.Gender)))
                .ForMember(des => des.ImageUIrl, s => s.MapFrom(x => GetPathImage(x.JsonPicture)))
                .ForMember(des => des.CartCount, s => s.MapFrom(x => x.Carts != null ? x.Carts.Count : 0))
                .ForMember(des => des.OrderCount, s => s.MapFrom(x => x.Orders != null ? x.Orders.Count : 0))
                .ForMember(des => des.ResultCustomerUserInfo, s => s.MapFrom(x => new ResultCustomerUserInfo
                {
                    Mobile = x.Mobile,
                    Email = x.Email,

                }))
                .ForMember(des => des.ResultCustomerAddresss, s => s.MapFrom(x => x.CustomerAddresss != null ? x.CustomerAddresss.Select(s => new ResultCustomerAddress
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
                    Visible = s.Visible,
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

                }).ToList() : new List<ResultCustomerAddress>()))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultCustomer, UpdateCustomer>();


            #endregion
            #region Order
            CreateMap<AddOrder, Order>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateOrder, Order>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<Order, UpdateOrder>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultOrder, UpdateOrder>();
            CreateMap<Order, ResultOrder>()
                .ForMember(des => des.OrderStatus, s => s.MapFrom(x => GetStatusOrder(x.OrderStatus)))
                .ForMember(des => des.OrderStatus2, s => s.MapFrom(x => x.OrderStatus))
                .ForMember(des => des.PaymentStatus, s => s.MapFrom(x => GetPaymentStatus(x.PaymentStatus)))
                .ForMember(des => des.PaymentStatus2, s => s.MapFrom(x => x.PaymentStatus))
                .ForMember(des => des.ResultJsonLables, s => s.MapFrom(x => x.JsonLableTexts != null ? JsonConvert.DeserializeObject<List<ResultJsonLable>>(x.JsonLableTexts) : new List<ResultJsonLable>()))
                .ForMember(des => des.ResultSendProductMethod, s => s.MapFrom(x => x.PaymentStatus != PaymentStatus.paid ? new ResultSendProductMethod() : x.SendProductMethod != null ? new ResultSendProductMethod
                {
                    ID = x.SendProductMethod.ID,
                    Title = x.SendProductMethod.Title,
                    Description = x.SendProductMethod.Description,
                } : new ResultSendProductMethod()))
                .ForMember(des => des.ResultCustomer, s => s.MapFrom(x => x.Customer != null ? new ResultCustomer
                {
                    ID = x.Customer.ID,
                    Name = x.Customer.Name,
                    LastName = x.Customer.LastName,
                    Mobile = x.Customer.Mobile,
                    Mcode = x.Customer.Mcode,
                    Email = x.Customer.Email,
                    Gender = EnumConstant.GetTitleGender(x.Customer.Gender),
                    BirthDate = x.Customer.BirthDate != null ? DateFunctions.ConvertDateIntToString((int)x.Customer.BirthDate) : null,
                } : new ResultCustomer()))
                .ForMember(des => des.ResultCustomerAddress, s => s.MapFrom(x => GetResultCustomerAddress(x.JsonAddress)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region OrderItem
            CreateMap<AddOrderItem, OrderItem>()
                .ForMember(des => des.Quantity, s => s.MapFrom(x => 1))
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateOrderItem, OrderItem>()
                .ForMember(des => des.Quantity, s => s.MapFrom(x => 1))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<OrderItem, UpdateOrderItem>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultOrderItem, UpdateOrderItem>();
            CreateMap<OrderItem, ResultOrderItem>()
                .ForMember(des => des.FinalAmount, s => s.MapFrom(x => x.PriceAtOrder * x.Quantity))
                .ForMember(des => des.ResultProduct, s => s.MapFrom(x => x.Product != null ? new ResultProduct
                {
                    ID = x.Product.ID,
                    Title = x.Product.Title,
                    Description = x.Product.Description,
                    BasePrice = x.Product.BasePrice,
                    IdentityCode = x.Product.IdentityCode,
                    ResultUploadFiles = GetResultUploadFiles(x.Product.JsonPicture),
                } : new ResultProduct()))
                .ForMember(des => des.ResultJsonLables, s => s.MapFrom(x => x.JsonLableTexts != null ? JsonConvert.DeserializeObject<List<ResultJsonLable>>(x.JsonLableTexts) : new List<ResultJsonLable>()))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region Cart
            CreateMap<AddCart, Cart>()
                .ForMember(des => des.CartStatus, s => s.MapFrom(x => (int)x.CartStatus))
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateCart, Cart>()
                .ForMember(des => des.CartStatus, s => s.MapFrom(x => (int)x.CartStatus))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<Cart, UpdateCart>()
                .ForMember(des => des.CartStatus, s => s.MapFrom(x => (CartStatus)x.CartStatus))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultCart, UpdateCart>();
            CreateMap<Cart, ResultCart>()
                .ForMember(des => des.CartStatus, s => s.MapFrom(x => GetCartStatus(x.CartStatus)))
                .ForMember(des => des.CartStatus2, s => s.MapFrom(x => (CartStatus)x.CartStatus))
                .ForMember(des => des.TotalAmount, s => s.MapFrom(x => x.CartItems != null && x.CartItems.Count > 0 ? x.CartItems.Sum(s => s.Product.BasePrice * s.Quantity) : 0))
                .ForMember(des => des.Discount, s => s.MapFrom(x => x.CartItems != null && x.CartItems.Count > 0 ? x.CartItems.Sum(s => s.Product.Discount * s.Quantity) : 0))
                .ForMember(des => des.FinalAmount, s => s.MapFrom(x => x.CartItems != null && x.CartItems.Count > 0 ? x.CartItems.Sum(s => s.Product.BasePrice * s.Quantity) - x.CartItems.Sum(s => s.Product.Discount * s.Quantity) : 0))
                .ForMember(des => des.CountCartItems, s => s.MapFrom(x => x.CartItems != null ? x.CartItems.Count : 0))
                .ForMember(des => des.ResultJsonLables, s => s.MapFrom(x => x.JsonLableTexts != null ? JsonConvert.DeserializeObject<List<ResultJsonLable>>(x.JsonLableTexts) : new List<ResultJsonLable>()))
                .ForMember(des => des.ResultCustomer, s => s.MapFrom(x => x.Customer != null ? new ResultCustomer
                {
                    ID = x.Customer.ID,
                    Name = x.Customer.Name,
                    LastName = x.Customer.LastName,
                    Mobile = x.Customer.Mobile,
                    Mcode = x.Customer.Mcode,
                    Email = x.Customer.Email,
                    Gender = EnumConstant.GetTitleGender(x.Customer.Gender),
                    BirthDate = x.Customer.BirthDate != null ? DateFunctions.ConvertDateIntToString((int)x.Customer.BirthDate) : null,
                } : new ResultCustomer()))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region CartItem
            CreateMap<AddCartItem, CartItem>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<AddCartItem, ResultCartItem>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateCartItem, CartItem>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<CartItem, UpdateCartItem>()
                 .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultCartItem, UpdateCartItem>();
            CreateMap<CartItem, ResultCartItem>()
                .ForMember(des => des.TotalAmount, s => s.MapFrom(x => x.Product != null ? x.Product.BasePrice * x.Quantity : 0))
                .ForMember(des => des.ResultJsonLables, s => s.MapFrom(x => x.JsonLableTexts != null ? JsonConvert.DeserializeObject<List<ResultJsonLable>>(x.JsonLableTexts) : new List<ResultJsonLable>()))
                .ForMember(des => des.ResultProduct, s => s.MapFrom(x => x.Product != null ? new ResultProduct
                {
                    ID = x.Product.ID,
                    Title = x.Product.Title,
                    Description = x.Product.Description,
                    BasePrice = x.Product.BasePrice,
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
                } : new ResultProduct()))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region Slider
            CreateMap<AddSlider, Slider>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateSlider, Slider>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<Slider, UpdateSlider>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultSlider, UpdateSlider>();
            CreateMap<Slider, ResultSlider>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region Advertisement
            CreateMap<AddAdvertisement, Advertisement>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateAdvertisement, Advertisement>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<Advertisement, UpdateAdvertisement>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultAdvertisement, UpdateAdvertisement>();
            CreateMap<Advertisement, ResultAdvertisement>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region AdvertisementSingle
            CreateMap<AddAdvertisementSingle, AdvertisementSingle>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateAdvertisementSingle, AdvertisementSingle>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<AdvertisementSingle, UpdateAdvertisementSingle>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultAdvertisementSingle, UpdateAdvertisementSingle>();
            CreateMap<AdvertisementSingle, ResultAdvertisementSingle>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region GroupBlog
            CreateMap<AddGroupBlog, GroupBlog>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateGroupBlog, GroupBlog>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<GroupBlog, UpdateGroupBlog>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultGroupBlog, UpdateGroupBlog>();
            CreateMap<GroupBlog, ResultGroupBlog>()
                .ForMember(des => des.CountBlog, s => s.MapFrom(x => x.Blogs != null ? x.Blogs.Count : 0))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region Blog
            CreateMap<AddBlog, Blog>()
                 .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateBlog, Blog>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<Blog, UpdateBlog>()
                .ForMember(des => des.ResultJsonLables, s => s.MapFrom(x => x.JsonLableTexts != null ? JsonConvert.DeserializeObject<List<ResultJsonLable>>(x.JsonLableTexts) : new List<ResultJsonLable>()))
                .ForMember(des => des.ResultGroupBlog, s => s.MapFrom(x => x.GroupBlog != null ? new ResultGroupBlog
                {
                    Title = x.GroupBlog.Title,
                } : new ResultGroupBlog()))
                .ForMember(des => des.ResultTeam, s => s.MapFrom(x => x.Team != null ? new ResultTeam
                {
                    Name = x.Team.Name,
                    Title = x.Team.Title,
                    Description = x.Team.Description,
                } : new ResultTeam()))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultBlog, UpdateBlog>();
            CreateMap<Blog, ResultBlog>()
                .ForMember(des => des.ResultJsonLables, s => s.MapFrom(x => x.JsonLableTexts != null ? JsonConvert.DeserializeObject<List<ResultJsonLable>>(x.JsonLableTexts) : new List<ResultJsonLable>()))
                .ForMember(des => des.ResultGroupBlog, s => s.MapFrom(x => x.GroupBlog != null ? new ResultGroupBlog
                {
                    Title = x.GroupBlog.Title,
                } : new ResultGroupBlog()))
                .ForMember(des => des.ResultTeam, s => s.MapFrom(x => x.Team != null ? new ResultTeam
                {
                    Name = x.Team.Name,
                    Title = x.Team.Title,
                    Description = x.Team.Description,
                } : new ResultTeam()))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));


            #endregion
            #region Story
            CreateMap<AddStory, Story>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateStory, Story>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<Story, UpdateStory>()
                 .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultStory, UpdateStory>();
            CreateMap<Story, ResultStory>()
                 .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region SmsOtpCode
            CreateMap<AddSmsOtpCode, SmsOtpCode>()
                .ForMember(des => des.TimeExpired, s => s.MapFrom(x => (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)) + TimeSpan.FromMinutes(2)))
                .ForMember(des => des.DateExpired, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateSmsOtpCode, SmsOtpCode>()
                .ForMember(des => des.TimeExpired, s => s.MapFrom(x => (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)) + TimeSpan.FromMinutes(2)))
                .ForMember(des => des.DateExpired, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<SmsOtpCode, UpdateSmsOtpCode>()
                .ForMember(des => des.DateExpired, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.DateExpired)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultSmsOtpCode, UpdateSmsOtpCode>();
            CreateMap<SmsOtpCode, ResultSmsOtpCode>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region Wallet
            CreateMap<AddWallet, Wallet>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateWallet, Wallet>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<Wallet, UpdateWallet>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultWallet, UpdateWallet>();
            CreateMap<Wallet, ResultWallet>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));


            #endregion
            #region WalletTransaction
            CreateMap<AddWalletTransaction, WalletTransaction>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateWalletTransaction, WalletTransaction>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<WalletTransaction, UpdateWalletTransaction>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultWalletTransaction, UpdateWalletTransaction>();
            CreateMap<WalletTransaction, ResultWalletTransaction>()
                .ForMember(des => des.Status, s => s.MapFrom(x => GetTransactionStatuse(x.Status)))
                .ForMember(des => des.Kind, s => s.MapFrom(x => GetTransactionKind(x.Kind)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region Province
            CreateMap<AddProvince, Province>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateProvince, Province>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<Province, UpdateProvince>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultProvince, UpdateProvince>();
            CreateMap<Province, ResultProvince>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region City
            CreateMap<AddCity, City>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateCity, City>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
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
            #region CustomerAddress
            CreateMap<AddCustomerAddress, CustomerAddress>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateCustomerAddress, CustomerAddress>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<CustomerAddress, UpdateCustomerAddress>()
                .ForMember(des => des.ResultProvince, s => s.MapFrom(x => x.City != null && x.City.Province != null ? new ResultProvince
                {
                    ID = x.City.Province.ID,
                    Title = x.City.Province.Title,

                } : new ResultProvince()))
                .ForMember(des => des.ResultCity, s => s.MapFrom(x => x.City != null ? new ResultCity
                {
                    ID = x.City.ID,
                    Title = x.City.Title,
                    ResultProvince = x.City.Province != null ? new ResultProvince
                    {
                        ID = x.City.Province.ID,
                        Title = x.City.Province.Title,
                    } : new ResultProvince(),
                } : new ResultCity()))
                .ForMember(des => des.ResultCustomer, s => s.MapFrom(x => x.Customer != null ? new ResultCustomer
                {
                    ID = x.Customer.ID,
                    Name = x.Customer.Name,
                    LastName = x.Customer.LastName,
                    Mcode = x.Customer.Mcode,
                } : new ResultCustomer()))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultCustomerAddress, UpdateCustomerAddress>();
            CreateMap<CustomerAddress, ResultCustomerAddress>()
                .ForMember(des => des.ResultCustomer, s => s.MapFrom(x => x.Customer != null ? new ResultCustomer
                {
                    Name = x.Customer.Name,
                    LastName = x.Customer.LastName,
                    Mobile = x.Customer.Mobile,

                } : new ResultCustomer()))
                .ForMember(des => des.ResultCity, s => s.MapFrom(x => x.City != null ? new ResultCity
                {
                    ID = x.City.ID,
                    Title = x.City.Title,
                    ResultProvince = x.City.Province != null ? new ResultProvince
                    {
                        ID = x.City.Province.ID,
                        Title = x.City.Province.Title,
                    } : new ResultProvince(),
                } : new ResultCity()))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region SendProductMethod
            CreateMap<AddSendProductMethod, SendProductMethod>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateSendProductMethod, SendProductMethod>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<SendProductMethod, UpdateSendProductMethod>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultSendProductMethod, UpdateSendProductMethod>();
            CreateMap<SendProductMethod, ResultSendProductMethod>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region OrderPaymentTemp
            CreateMap<AddOrderPaymentTemp, OrderPaymentTemp>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateOrderPaymentTemp, OrderPaymentTemp>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<OrderPaymentTemp, UpdateOrderPaymentTemp>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));


            CreateMap<OrderPaymentTemp, SearchOrderPaymentTemp>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultOrderPaymentTemp, UpdateOrderPaymentTemp>();
            CreateMap<OrderPaymentTemp, ResultOrderPaymentTemp>()
                .ForMember(des => des.ResultJsonLables, s => s.MapFrom(x => x.JsonLableTexts != null ? JsonConvert.DeserializeObject<List<ResultJsonLable>>(x.JsonLableTexts) : new List<ResultJsonLable>()))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region RefreshTokenEntity
            CreateMap<AddRefreshTokenEntity, RefreshTokenEntity>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateRefreshTokenEntity, RefreshTokenEntity>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<RefreshTokenEntity, UpdateRefreshTokenEntity>()
                 .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultRefreshTokenEntity, UpdateRefreshTokenEntity>();
            CreateMap<RefreshTokenEntity, ResultRefreshTokenEntity>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));


            #endregion
            #region ProductFeature
            CreateMap<AddProductFeature, ProductFeature>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateProductFeature, ProductFeature>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<ProductFeature, UpdateProductFeature>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultProductFeature, UpdateProductFeature>();
            CreateMap<ProductFeature, ResultProductFeature>()
                .ForMember(des => des.ResultCategory, s => s.MapFrom(x => x.Category != null ? new ResultCategory
                {
                    ID = x.Category.ID,
                    Title = x.Category.Title,
                    ParentID = x.Category.ParentID,
                    Description = x.Category.Description,
                    Visible = x.Category.Visible,

                } : new ResultCategory()))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));


            #endregion
            #region ProductFeatureValue
            CreateMap<AddProductFeatureValue, ProductFeatureValue>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateProductFeatureValue, ProductFeatureValue>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<ProductFeatureValue, UpdateProductFeatureValue>()
                .ForMember(des => des.ResultProductFeature, s => s.MapFrom(x => x.ProductFeature != null ? new ResultProductFeature
                {
                    ID = x.ProductFeature.ID,
                    Title = x.ProductFeature.Title,
                    Visible = x.ProductFeature.Visible,

                } : new ResultProductFeature()))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultProductFeatureValue, UpdateProductFeatureValue>();
            CreateMap<ProductFeatureValue, ResultProductFeatureValue>()
                .ForMember(des => des.ResultProduct, s => s.MapFrom(x => x.Product != null ? new ResultProduct
                {
                    ID = x.Product.ID,
                    Title = x.Product.Title,
                    Description = x.Product.Description,
                    Visible = x.Product.Visible,

                } : new ResultProduct()))
                .ForMember(des => des.ResultProductFeature, s => s.MapFrom(x => x.ProductFeature != null ? new ResultProductFeature
                {
                    ID = x.ProductFeature.ID,
                    Title = x.ProductFeature.Title,
                    Visible = x.ProductFeature.Visible,

                } : new ResultProductFeature()))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region PricingRule
            CreateMap<AddPricingRule, PricingRule>()
                .ForMember(des => des.FromDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.FromDate)))
                .ForMember(des => des.ToDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.ToDate)))
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdatePricingRule, PricingRule>()
                .ForMember(des => des.FromDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.FromDate)))
                .ForMember(des => des.ToDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.ToDate)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<PricingRule, UpdatePricingRule>()
                .ForMember(des => des.RuleType, s => s.MapFrom(x => (int)x.RuleType))
                .ForMember(des => des.FromDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.FromDate)))
                .ForMember(des => des.ToDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.ToDate)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultPricingRule, UpdatePricingRule>();
            CreateMap<PricingRule, ResultPricingRule>()
                .ForMember(des => des.RuleType, s => s.MapFrom(x => EnumConstant.GetTitleRuleType(x.RuleType)))
                .ForMember(des => des.RuleType2, s => s.MapFrom(x => x.RuleType))
                .ForMember(des => des.FromDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.FromDate)))
                .ForMember(des => des.ToDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.ToDate)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region RawProduct
            CreateMap<AddRawProduct, RawProduct>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateRawProduct, RawProduct>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<RawProduct, UpdateRawProduct>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<RawProduct, ResultRawProduct>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region RawProductStore
            CreateMap<AddRawProductStore, RawProductStore>()
                 .ForMember(des => des.MessurmentType, s => s.MapFrom(x => (int)x.MessurmentType))
                 .ForMember(des => des.BuyDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.BuyDate)))
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateRawProductStore, RawProductStore>()
                .ForMember(des => des.MessurmentType, s => s.MapFrom(x => (int)x.MessurmentType))
                .ForMember(des => des.BuyDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.BuyDate)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<RawProductStore, UpdateRawProductStore>()
                .ForMember(des => des.MessurmentType, s => s.MapFrom(x => (int)x.MessurmentType))
                .ForMember(des => des.BuyDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.BuyDate)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<RawProductStore, ResultRawProductStore>()
                .ForMember(des => des.MessurmentType, s => s.MapFrom(x => Dto.Enum.EnumConstant.GetTitleMessurmentType((MessurmentType)x.MessurmentType)))
                .ForMember(des => des.MessurmentType2, s => s.MapFrom(x => (MessurmentType)x.MessurmentType))
                .ForMember(des => des.SumPrice, s => s.MapFrom(x => x.Amount * x.Price))
                .ForMember(des => des.SliceCount, s => s.MapFrom(x => x.RawProductStore_Products.Count > 0 ? (int)x.RawProductStore_Products.Sum(k => k.Count) : 0))
                .ForMember(des => des.BuyDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.BuyDate)))
                .ForMember(des => des.ResultRawProduct, s => s.MapFrom(x => new ResultRawProduct
                {
                    ID = x.RawProduct.ID,
                    Title = x.RawProduct.Title,
                    Description = x.RawProduct.Description,
                    Visible = x.RawProduct.Visible,

                }))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));


            #endregion
            #region Product_CountAction_CostType
            CreateMap<AddProduct_CountAction_CostType, Product_CountAction_CostType>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateProduct_CountAction_CostType, Product_CountAction_CostType>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<Product_CountAction_CostType, UpdateProduct_CountAction_CostType>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<Product_CountAction_CostType, ResultProduct_CountAction_CostType>()
                .ForMember(des => des.ResultPosition, s => s.MapFrom(x => x.Position != null ? new ResultPosition
                {
                    ID = x.Position.ID,
                    Title = x.Position.Title,
                } : new ResultPosition()
                ))
                .ForMember(des => des.ResultProduct, s => s.MapFrom(x => x.Product != null ? new ResultProduct
                {
                    ID = x.Product.ID,
                    Title = x.Product.Title,
                    ProductCode = x.Product.ProductCode,
                } : new ResultProduct()))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region RegisterCostRawProductStore
            CreateMap<AddRegisterCostRawProductStore, RegisterCostRawProductStore>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateRegisterCostRawProductStore, RegisterCostRawProductStore>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<RegisterCostRawProductStore, UpdateRegisterCostRawProductStore>()
                .ForMember(des => des.SumPrice, s => s.MapFrom(x => x.Count * x.Price))
                .ForMember(des => des.ResultPersonel, s => s.MapFrom(x => x.Personel != null ? new ResultPersonel
                {
                    ID = x.Personel.ID,
                    PositionID = x.Personel.PositionID,
                    Name = x.Personel.Name,
                    LastName = x.Personel.LastName,

                } : new ResultPersonel()))
                .ForMember(des => des.ResultRawProductStore_Product, s => s.MapFrom(x => x.RawProductStore_Product != null ? new ResultRawProductStore_Product
                {
                    ID = x.RawProductStore_Product.ID,
                    ProductID = x.RawProductStore_Product.ProductID,

                } : new()))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<RegisterCostRawProductStore, ResultRegisterCostRawProductStore>()
                .ForMember(des => des.SumPrice, s => s.MapFrom(x => x.Count * x.Price))
                .ForMember(des => des.ResultRawProductStore_Product, s => s.MapFrom(x => x.RawProductStore_Product != null ? new ResultRawProductStore_Product
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

                } : new ResultRawProductStore_Product()))
                .ForMember(des => des.ResultPersonel, s => s.MapFrom(x => x.Personel != null ? new ResultPersonel
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
                } : new ResultPersonel()))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region RawProductStore_Product
            CreateMap<AddRawProductStore_Product, RawProductStore_Product>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateRawProductStore_Product, RawProductStore_Product>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<RawProductStore_Product, UpdateRawProductStore_Product>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<RawProductStore_Product, ResultRawProductStore_Product>()
                .ForMember(des => des.ResultProduct, s => s.MapFrom(x => x.Product != null ? new ResultProduct
                {
                    ID = x.Product.ID,
                    Title = x.Product.Title,
                    ProductCode = x.Product.ProductCode,
                } : new()
                ))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));
            #endregion
            #region Personel
            CreateMap<AddPersonel, Personel>()
                .ForMember(des => des.BirthDate, s => s.MapFrom(x => x.BirthDate == null ? (int?)null : DateFunctions.ConvertDateStringToInt(x.BirthDate)))
                .ForMember(des => des.Gender, s => s.MapFrom(x => (int)x.Gender))
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdatePersonel, Personel>()
                .ForMember(des => des.BirthDate, s => s.MapFrom(x => x.BirthDate == null ? (int?)null : DateFunctions.ConvertDateStringToInt(x.BirthDate)))
                .ForMember(des => des.Gender, s => s.MapFrom(x => (int)x.Gender))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<Personel, UpdatePersonel>()
               .ForMember(des => des.BirthDate, s => s.MapFrom(x => x.BirthDate != null ? DateFunctions.ConvertDateIntToString((int)x.BirthDate) : null))
               .ForMember(des => des.Gender, s => s.MapFrom(x => (Gender)x.Gender))
               .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
               .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<Personel, ResultPersonel>()
              .ForMember(des => des.BirthDate, s => s.MapFrom(x => x.BirthDate != null ? DateFunctions.ConvertDateIntToString((int)x.BirthDate) : null))
              .ForMember(des => des.Gender, s => s.MapFrom(x => EnumConstant.GetTitleGender(x.Gender)))
              .ForMember(des => des.ImageUIrl, s => s.MapFrom(x => GetPathImage(x.JsonPicture)))
              .ForMember(des => des.ResultPosition, s => s.MapFrom(x => x.Position != null ? new ResultPosition
              {
                  ID = x.Position.ID,
                  Title = x.Position.Title,
              } : new()))
              .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
              .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));
            #endregion
            #region FavoritUserProduct
            CreateMap<AddFavoritUserProduct, FavoritUserProduct>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateFavoritUserProduct, FavoritUserProduct>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<FavoritUserProduct, UpdateFavoritUserProduct>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultFavoritUserProduct, UpdateFavoritUserProduct>();
            CreateMap<FavoritUserProduct, ResultFavoritUserProduct>()
                .ForMember(des => des.ResultProduct, s => s.MapFrom(x => x.Product != null ? new ResultProduct
                {
                    ID = x.Product.ID,
                    Title = x.Product.Title,
                    BaseStock = x.Product.BaseStock??0,
                    BasePrice = x.Product.BasePrice??0,
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
                } : new ResultProduct()))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region FavoritUserProduct
            CreateMap<AddFavoritUserProduct, FavoritUserProduct>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateFavoritUserProduct, FavoritUserProduct>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
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
                  BaseStock = x.Product.BaseStock??0,
                  BasePrice = x.Product.BasePrice??0,
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
                  ResultUploadFiles = GetResultUploadFiles(x.Product.JsonPicture),
                  IdentityCode = x.Product.IdentityCode,
                  RegisterDate = DateFunctions.ConvertDateIntToString(x.Product.RegisterDate),
                  RegisterTime = x.Product.RegisterTime,
                  EditDate = DateFunctions.ConvertDateIntToString(x.Product.EditDate),
                  EditTime = x.Product.EditTime,
              } : new ResultProduct()
                ));
            #endregion
            #region FavoritUserProduct
            CreateMap<AddFavoritUserProduct, FavoritUserProduct>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateFavoritUserProduct, FavoritUserProduct>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<FavoritUserProduct, UpdateFavoritUserProduct>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultFavoritUserProduct, UpdateFavoritUserProduct>();
            CreateMap<FavoritUserProduct, ResultFavoritUserProduct>()
                .ForMember(des => des.ResultProduct, s => s.MapFrom(x => x.Product != null ? new ResultProduct
                {
                    ID = x.Product.ID,
                    Title = x.Product.Title,
                    BaseStock = x.Product.BaseStock??0,
                    BasePrice = x.Product.BasePrice??0,
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

                } : new()))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region Position
            CreateMap<AddPosition, Position>()
                .ForMember(des => des.CostType, s => s.MapFrom(x => (int)x.CostType))
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdatePosition, Position>()
                .ForMember(des => des.CostType, s => s.MapFrom(x => (int)x.CostType))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<Position, UpdatePosition>()
                .ForMember(des => des.CostType, s => s.MapFrom(x => x.CostType.HasValue
                            ? (int?)x.CostType.Value
                            : null))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<Position, ResultPosition>()
                .ForMember(des => des.CostType, s => s.MapFrom(x => GetTitleCostType(x.CostType)))
                .ForMember(des => des.CostType2, s => s.MapFrom(x => x.CostType != null ? (CostType?)x.CostType : null))
                .ForMember(des => des.ResultProduct_CountAction_CostTypes, s => s.MapFrom(x => x.Product_CountAction_CostTypes != null && x.Product_CountAction_CostTypes.Count > 0 ? x.Product_CountAction_CostTypes.Select(k => new ResultProduct_CountAction_CostType
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
                }).ToList() : new List<ResultProduct_CountAction_CostType>()))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region ContactUs
            CreateMap<AddContactUs, ContactUs>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateContactUs, ContactUs>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<ContactUs, UpdateContactUs>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultContactUs, UpdateContactUs>();

            CreateMap<ContactUs, ResultContactUs>()
                .ForMember(des => des.IsRead, s => s.MapFrom(x => GetIsReadTitle(x.IsRead)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region Team
            CreateMap<AddTeam, Team>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateTeam, Team>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
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
            #region Faq
            CreateMap<AddFaq, Faq>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateFaq, Faq>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<Faq, UpdateFaq>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ResultFaq, UpdateFaq>();
            CreateMap<Faq, ResultFaq>()
                .ForMember(des => des.IsRead, s => s.MapFrom(x => GetIsReadTitle(x.IsRead)))
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
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

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
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<Question, UpdateQuestion>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));


            CreateMap<Question, ResultQuestion>()
                .ForMember(des => des.ResultGroupQuestion, s => s.MapFrom(x => x.GroupQuestion != null ? new ResultGroupQuestion
                {
                    Title = x.GroupQuestion.Title
                } : new ResultGroupQuestion()
                ))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region ProductComment
            CreateMap<AddProductComment, ProductComment>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateProductComment, ProductComment>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

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
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<Ticket, UpdateTicket>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<Ticket, ResultTicket>()
                .ForMember(des => des.TicketPriority, s => s.MapFrom(x => EnumConstant.GetTitleTicketPriority(x.TicketPriority)))
                .ForMember(des => des.ResultDepartment, s => s.MapFrom(x => x.Department != null ? new ResultDepartment
                {
                    Title = x.Department.Title
                } : new ResultDepartment()
                ))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            #endregion
            #region Department
            CreateMap<AddDepartment, Department>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateDepartment, Department>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<Department, UpdateDepartment>()
                  .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<Department, ResultDepartment>()
                  .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));
            #endregion
            #region BlogComment
            CreateMap<AddBlogComment, BlogComment>()
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
            #region Trait
            CreateMap<AddTrait, Trait>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateTrait, Trait>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<Trait, UpdateTrait>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<Trait, ResultTrait>()
                .ForMember(des => des.ResultTraitValues, s => s.MapFrom(x => x.TraitValues ))
                .ForMember(des => des.DisplayType2, s => s.MapFrom(x => x.DisplayType))
                .ForMember(des => des.DisplayType, s => s.MapFrom(x => EnumConstant.GetTitleTraitDisplayType(x.DisplayType)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));
            #endregion
            #region TraitValue
            CreateMap<AddTraitValue, TraitValue>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateTraitValue, TraitValue>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<TraitValue, UpdateTraitValue>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<TraitValue, ResultTraitValue>()
                  .ForMember(des => des.ResultTrait, s => s.MapFrom(x => x.Trait != null ? new ResultTrait
                  {
                      ID = x.Trait.ID,
                      Title = x.Trait.Title,
                      DisplayType = EnumConstant.GetTitleTraitDisplayType(x.Trait.DisplayType),
                      DisplayType2 = (int)x.Trait.DisplayType,

                  } : new ResultTrait()))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));
            #endregion
            #region ProductVariant
            CreateMap<AddProductVariant, ProductVariant>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));
            CreateMap<AddProductVariantRow, ProductVariant>()
               .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
               .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
               .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
               .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateProductVariant, ProductVariant>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<ProductVariant, UpdateProductVariant>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));
        
            CreateMap<ProductVariant, AddProductVariantRow>()
           .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
           .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ProductVariant, ResultProductVariant>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));
            #endregion
            #region ProductVariantValue
            CreateMap<AddProductVariantValue, ProductVariantValue>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateProductVariantValue, ProductVariantValue>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<ProductVariantValue, UpdateProductVariantValue>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));

            CreateMap<ProductVariantValue, ResultProductVariantValue>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));
            #endregion
            #region CategoryTrait
            CreateMap<AddCategoryTrait, CategoryTrait>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<AddUpdateCategoryTraitSelect, CategoryTrait>()
                .ForMember(des => des.RegisterTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)))
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())));

            CreateMap<UpdateCategoryTrait, CategoryTrait>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateStringToInt(GetNewDate())))
                .ForMember(des => des.EditTime, s => s.MapFrom(x => new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)));

            CreateMap<CategoryTrait, UpdateCategoryTrait>()
                .ForMember(des => des.RegisterDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.RegisterDate)))
                .ForMember(des => des.EditDate, s => s.MapFrom(x => DateFunctions.ConvertDateIntToString(x.EditDate)));


            CreateMap<CategoryTrait, ResultCategoryTrait>()
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

