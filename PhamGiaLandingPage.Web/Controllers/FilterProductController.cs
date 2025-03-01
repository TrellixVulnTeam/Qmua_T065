﻿using Microsoft.AspNetCore.Mvc;
using PhamGiaLandingPage.Web.Services.Article.Repository;
using PhamGiaLandingPage.Web.Services.Extra.Repository;
using PhamGiaLandingPage.Web.Services.Product.Repository;
using PhamGiaLandingPage.Web.Services.Product.ViewModel;
using PhamGiaLandingPage.Web.Services.Zone.Repository;
using PhamGiaLandingPage.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhamGiaLandingPage.Web.Controllers
{
    public class FilterProductController : BaseController
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IProductRepository _productRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IExtraRepository _extraRepository;
        private readonly ICookieUtility _cookieUtility;
        public FilterProductController(IZoneRepository zoneRepository, IProductRepository productRepository, IArticleRepository articleRepository, ICookieUtility cookieUtility, IExtraRepository extraRepository)
        {
            _zoneRepository = zoneRepository;
            _productRepository = productRepository;
            _articleRepository = articleRepository;
            _extraRepository = extraRepository;
            _cookieUtility = cookieUtility;
            //cache
            //_distributedCache = distributedCache;
            //_configuration = configuration;
            //_multiplexer = multiplexer;
            // cache end
        }
        public IActionResult FilterProductByRegion(string region, int? pageIndex)
        {

            var pageSize = 24;
            var reg = _zoneRepository.GetZoneByAlias(region, CurrentLanguageCode);
            if (reg != null)
            {
                var total = 0;
                var model = _productRepository.GetProductInRegionByZoneIdMinify(reg.Id, _cookieUtility.SetCookieDefault().LocationId, CurrentLanguageCode, pageIndex == null ? 1 : pageIndex.Value, pageSize, out total);
                if (pageIndex == null)
                {
                    ViewBag.Total = total;
                    ViewBag.Size = pageSize;
                    ViewBag.Url = region;
                    ViewBag.ZoneId = reg.Id;
                    return View(model);
                }
                if (pageIndex.Value > 1)
                {
                    ViewBag.Total = total;
                    ViewBag.Size = pageSize;
                    ViewBag.Url = region;
                    ViewBag.ZoneId = reg.Id;
                    return PartialView("~/Views/Product/FilterProductSharing.cshtml", model);
                }
            }
            return View();
        }
        public IActionResult FilterProductByTag(string tag, int? pageIndex)
        {

            var pageSize = 12;
            var reg = _extraRepository.GetTagTarget(tag);
            if (reg != null)
            {
                var total = 0;
                var model = _productRepository.GetProductInTagMinify(tag, _cookieUtility.SetCookieDefault().LocationId, CurrentLanguageCode, pageIndex == null ? 1 : pageIndex.Value, pageSize, out total);
                if (pageIndex == null)
                {
                    ViewBag.Total = total;
                    ViewBag.Size = pageSize;
                    ViewBag.Url = tag;
                    ViewBag.TagName = reg.Name;
                    return View(model);
                }
                if (pageIndex.Value > 1)
                {
                    ViewBag.Total = total;
                    ViewBag.Size = pageSize;
                    ViewBag.Url = tag;
                    ViewBag.TagName = reg.Name;
                    return PartialView("~/Views/Product/FilterProductSharing.cshtml", model);
                }
            }
            return View();
        }
        [HttpPost]
        public IActionResult FilterProductBySpectification(FilterProductBySpectification fp)
        {
            return ViewComponent("FilterProductBySpectificationsAllZone", new { fp = fp });
        }
    }
}