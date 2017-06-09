/* http://www.zkea.net/ Copyright 2016 ZKEASOFT http://www.zkea.net/licenses */

using ZKEACMS.Product.Models;
using ZKEACMS.Product.ViewModel;
using Easy.Extend;
using ZKEACMS.Widget;
using Microsoft.AspNetCore.Http;
using Easy;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Mvc;

namespace ZKEACMS.Product.Service
{
    public class ProductCategoryWidgetService : WidgetService<ProductCategoryWidget, ProductDbContext>
    {
        private readonly IProductCategoryService _productCategoryService;
        public ProductCategoryWidgetService(IWidgetBasePartService widgetService, IProductCategoryService productCategoryService, IApplicationContext applicationContext)
            : base(widgetService, applicationContext)
        {
            _productCategoryService = productCategoryService;
        }

        public override DbSet<ProductCategoryWidget> CurrentDbSet
        {
            get
            {
                return DbContext.ProductCategoryWidget;
            }
        }

        public override WidgetViewModelPart Display(WidgetBase widget, ActionContext actionContext)
        {
            ProductCategoryWidget currentWidget = widget as ProductCategoryWidget;
            int cate = actionContext.RouteData.GetCategory();

            var currentCategory = _productCategoryService.Get(cate == 0 ? currentWidget.ProductCategoryID : cate);
            if (currentCategory != null)
            {
                var page = actionContext.HttpContext.GetLayout().Page;
                page.Title = page.Title + " - " + currentCategory.Title;
            }

            return widget.ToWidgetViewModelPart(new ProductCategoryWidgetViewModel
            {
                Categorys = _productCategoryService.Get(m => m.ParentID == currentWidget.ProductCategoryID),
                CurrentCategory = cate,
                TargetPage = currentWidget.TargetPage.IsNullOrEmpty() ? actionContext.HttpContext.Request.Path.ToString().ToLower() : currentWidget.TargetPage
            });
        }
    }
}