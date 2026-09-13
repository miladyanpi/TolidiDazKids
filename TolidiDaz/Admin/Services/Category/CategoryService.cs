using Dto.Models.DtoCategory;
using Dto.Models.DtoProduct;
using Dto.Models.ResponseApi;
using RestSharp;
using System.ComponentModel;

namespace Admin.Services.Category
{
    public class CategoryService
        (IRootApi<ResponseApiEntities<ResultProduct>> RootApiProducts,
        IRootApi<ResponseApiEntities<ResultCategory>> _RootApiResultCategorys)
    {
        [DisplayName("دسته بندی سطح 3")]
        public int? CategoryID { get; set; }
        public AddCategory? addCategory { get; set; } = new() { Visible = true };

        public List<ResultCategory>? ResultCategorys1 { get; set; } = new List<ResultCategory>();
        public List<ResultCategory>? ResultCategorys2 { get; set; } = new List<ResultCategory>();
        public List<ResultCategory>? ResultCategorys3 { get; set; } = new List<ResultCategory>();
        public List<ResultProduct> ResultProducts { get; set; } = new();
        public event Action? OnChange = null;
        public bool AfterLoaded { get; set; } = false;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public async Task<List<ResultCategory>> GetListDate(int? parentId = null)
        {

            var data = await _RootApiResultCategorys.RunMethodApi($"Categorys/Public?ParentID={null}", null, method: Method.Get);
            if (data != null)
                return data.Entities.ToList();
            else
                return new List<ResultCategory>();

        }

        private List<ResultCategory> GetCategoryParent(ResultCategory result)
        {
            return result.ResultCategorys.Where(s => s.ParentID == result.ID).ToList();
        }

        public async Task GetInitialData()
        {
            ResultCategorys1 = await GetListDate();
            if (ResultCategorys1.Count > 0)
            {
                var x1 = ResultCategorys1.FirstOrDefault();
                addCategory.ParentID1 = x1.ID;
                if (x1.ResultCategorys.Count > 0)
                    ResultCategorys2 = GetCategoryParent(x1);
                if (ResultCategorys2.Count > 0)
                {
                    var x2 = ResultCategorys2.FirstOrDefault();
                    addCategory.ParentID2 = x2.ID;

                    if (x2.ResultCategorys.Count > 0)
                    {
                        ResultCategorys3 = GetCategoryParent(x2);
                        CategoryID = ResultCategorys3.Count > 0 ? ResultCategorys3[0].ID : null;
                        await GetProducts();
                    }
                    else
                    {
                        CategoryID = null;
                    }
                }
                else
                {
                    CategoryID = null;
                }
                NotifyStateChanged();

            }

        }

        public async Task GetCategoryParent1(int? parentId)
        {
            if (ResultCategorys1.Count > 0)
            {
                var x1 = ResultCategorys1.Where(s => s.ID == parentId).FirstOrDefault();
                if (x1 != null && x1.ResultCategorys != null && x1.ResultCategorys.Count > 0)
                {
                    ResultCategorys2 = GetCategoryParent(x1);
                    if (ResultCategorys2.Count > 0)
                    {
                        var x2 = ResultCategorys2.FirstOrDefault();
                        addCategory.ParentID2 = x2.ID;

                        if (x2.ResultCategorys.Count > 0)
                        {
                            ResultCategorys3 = GetCategoryParent(x2);
                            CategoryID = ResultCategorys3.Count > 0 ? ResultCategorys3[0].ID : null;
                            await GetProducts();

                        }
                        else
                        {
                            CategoryID = null;
                            ResultProducts = new();
                        }
                    }
                    else
                    {
                        CategoryID = null;
                        ResultProducts = new();

                    }
                }


                else
                {
                    ResultCategorys2 = new List<ResultCategory>();
                    ResultCategorys3 = new List<ResultCategory>();
                    CategoryID = null;
                    ResultProducts = new();


                }


            }
            else
            {
                ResultCategorys2 = new List<ResultCategory>();
                ResultCategorys3 = new List<ResultCategory>();
                CategoryID = null;
                ResultProducts = new();



            }
            NotifyStateChanged();


        }
        public async Task GetCategoryParent2(int? parentId)
        {

            if (ResultCategorys2.Count > 0)
            {
                var x2 = ResultCategorys2.Where(s => s.ID == parentId).FirstOrDefault();
                if (x2 != null && x2.ResultCategorys != null && x2.ResultCategorys.Count > 0)
                {
                    ResultCategorys3 = GetCategoryParent(x2);
                    CategoryID = ResultCategorys3.Count > 0 ? ResultCategorys3[0].ID : null;
                    await GetProducts();

                }

                else
                {
                    ResultCategorys3 = new List<ResultCategory>();
                    CategoryID = null;
                    ResultProducts = new();

                }

            }
            else
            {
                ResultCategorys3 = new List<ResultCategory>();
                CategoryID = null;
                ResultProducts = new();


            }
            NotifyStateChanged();
        }

        public async Task GetProducts()
        {
            var resdataProduct = await RootApiProducts.RunMethodApi($"Products/ByCategoryID?CategoryID={CategoryID}", null, method: Method.Get);
            ResultProducts = resdataProduct.Entities.ToList();
            NotifyStateChanged();
        }


    }
}
