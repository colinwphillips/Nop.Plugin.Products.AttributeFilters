using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Seo;
using Nop.Core.Domain.Vendors;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Shipping.Date;
using Nop.Services.Stores;
using Nop.Services.Tax;
using Nop.Services.Vendors;
using Nop.Web.Infrastructure.Cache;
using Nop.Web.Models.Catalog;
using Nop.Web.Models.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Nop.Core.Data;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Stores;
using Nop.Data;
using Nop.Services.Events;
using Nop.Services.Messages;
using ProductModelFactory = Nop.Web.Factories.ProductModelFactory;

namespace Nop.Plugin.Products.AttributeFilters
{
    public partial class ProductModelFactoryCustom : ProductModelFactory
    {
        #region Fields

        private readonly CaptchaSettings _captchaSettings;
        private readonly CatalogSettings _catalogSettings;
        private readonly CustomerSettings _customerSettings;
        private readonly ICategoryService _categoryService;
        private readonly ICurrencyService _currencyService;
        private readonly ICustomerService _customerService;
        private readonly IDateRangeService _dateRangeService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IDownloadService _downloadService;
        private readonly ILocalizationService _localizationService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IPermissionService _permissionService;
        private readonly IPictureService _pictureService;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IProductService _productService;
        private readonly IProductTagService _productTagService;
        private readonly IProductTemplateService _productTemplateService;
        private readonly IReviewTypeService _reviewTypeService;
        private readonly ISpecificationAttributeService _specificationAttributeService;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IStoreContext _storeContext;
        private readonly ITaxService _taxService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IVendorService _vendorService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly MediaSettings _mediaSettings;
        private readonly OrderSettings _orderSettings;
        private readonly SeoSettings _seoSettings;
        private readonly VendorSettings _vendorSettings;

        #endregion

        public ProductModelFactoryCustom(CaptchaSettings captchaSettings,
            CatalogSettings catalogSettings,
            CustomerSettings customerSettings,
            ICategoryService categoryService,
            ICurrencyService currencyService,
            ICustomerService customerService,
            IDateRangeService dateRangeService,
            IDateTimeHelper dateTimeHelper,
            IDownloadService downloadService,
            ILocalizationService localizationService,
            IManufacturerService manufacturerService,
            IPermissionService permissionService,
            IPictureService pictureService,
            IPriceCalculationService priceCalculationService,
            IPriceFormatter priceFormatter,
            IProductAttributeParser productAttributeParser,
            IProductAttributeService productAttributeService,
            IProductService productService,
            IProductTagService productTagService,
            IProductTemplateService productTemplateService,
            IReviewTypeService reviewTypeService,
            ISpecificationAttributeService specificationAttributeService,
            IStaticCacheManager cacheManager,
            IStoreContext storeContext,
            ITaxService taxService,
            IUrlRecordService urlRecordService,
            IVendorService vendorService,
            IWebHelper webHelper,
            IWorkContext workContext,
            MediaSettings mediaSettings,
            OrderSettings orderSettings,
            SeoSettings seoSettings,
            VendorSettings vendorSettings)
            : base(captchaSettings, catalogSettings, customerSettings, categoryService, currencyService, customerService, dateRangeService, dateTimeHelper, 
                downloadService, localizationService, manufacturerService, permissionService, pictureService, priceCalculationService, priceFormatter, 
                productAttributeParser, productAttributeService, productService, productTagService, productTemplateService, reviewTypeService, specificationAttributeService, 
                cacheManager, storeContext, taxService, urlRecordService, vendorService, webHelper, workContext, mediaSettings, orderSettings, seoSettings, vendorSettings)
        {
            this._captchaSettings = captchaSettings;
            this._catalogSettings = catalogSettings;
            this._customerSettings = customerSettings;
            this._categoryService = categoryService;
            this._currencyService = currencyService;
            this._customerService = customerService;
            this._dateRangeService = dateRangeService;
            this._dateTimeHelper = dateTimeHelper;
            this._downloadService = downloadService;
            this._localizationService = localizationService;
            this._manufacturerService = manufacturerService;
            this._permissionService = permissionService;
            this._pictureService = pictureService;
            this._priceCalculationService = priceCalculationService;
            this._priceFormatter = priceFormatter;
            this._productAttributeParser = productAttributeParser;
            this._productAttributeService = productAttributeService;
            this._productService = productService;
            this._productTagService = productTagService;
            this._productTemplateService = productTemplateService;
            this._reviewTypeService = reviewTypeService;
            this._specificationAttributeService = specificationAttributeService;
            this._cacheManager = cacheManager;
            this._storeContext = storeContext;
            this._taxService = taxService;
            this._urlRecordService = urlRecordService;
            this._vendorService = vendorService;
            this._webHelper = webHelper;
            this._workContext = workContext;
            this._mediaSettings = mediaSettings;
            this._orderSettings = orderSettings;
            this._seoSettings = seoSettings;
            this._vendorSettings = vendorSettings;
        }

        /// <inheritdoc />
        /// <summary>
        /// Function remains unmodifed and is present so custom code is called (PrepareProductAttributeModels)
        /// Prepare the product details model
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="updatecartitem">Updated shopping cart item</param>
        /// <param name="isAssociatedProduct">Whether the product is associated</param>
        /// <returns>Product details model</returns>
        public override ProductDetailsModel PrepareProductDetailsModel(Product product,
            ShoppingCartItem updatecartitem = null, bool isAssociatedProduct = false)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            //standard properties
            var model = new ProductDetailsModel
            {
                Id = product.Id,
                Name = _localizationService.GetLocalized(product, x => x.Name),
                ShortDescription = _localizationService.GetLocalized(product, x => x.ShortDescription),
                FullDescription = _localizationService.GetLocalized(product, x => x.FullDescription),
                MetaKeywords = _localizationService.GetLocalized(product, x => x.MetaKeywords),
                MetaDescription = _localizationService.GetLocalized(product, x => x.MetaDescription),
                MetaTitle = _localizationService.GetLocalized(product, x => x.MetaTitle),
                SeName = _urlRecordService.GetSeName(product),
                ProductType = product.ProductType,
                ShowSku = _catalogSettings.ShowSkuOnProductDetailsPage,
                Sku = product.Sku,
                ShowManufacturerPartNumber = _catalogSettings.ShowManufacturerPartNumber,
                FreeShippingNotificationEnabled = _catalogSettings.ShowFreeShippingNotification,
                ManufacturerPartNumber = product.ManufacturerPartNumber,
                ShowGtin = _catalogSettings.ShowGtin,
                Gtin = product.Gtin,
                ManageInventoryMethod = product.ManageInventoryMethod,
                StockAvailability = _productService.FormatStockMessage(product, ""),
                HasSampleDownload = product.IsDownload && product.HasSampleDownload,
                DisplayDiscontinuedMessage = !product.Published && _catalogSettings.DisplayDiscontinuedMessageForUnpublishedProducts
            };

            //automatically generate product description?
            if (_seoSettings.GenerateProductMetaDescription && string.IsNullOrEmpty(model.MetaDescription))
            {
                //based on short description
                model.MetaDescription = model.ShortDescription;
            }

            //shipping info
            model.IsShipEnabled = product.IsShipEnabled;
            if (product.IsShipEnabled)
            {
                model.IsFreeShipping = product.IsFreeShipping;
                //delivery date
                var deliveryDate = _dateRangeService.GetDeliveryDateById(product.DeliveryDateId);
                if (deliveryDate != null)
                {
                    model.DeliveryDate = _localizationService.GetLocalized(deliveryDate, dd => dd.Name);
                }
            }

            //email a friend
            model.EmailAFriendEnabled = _catalogSettings.EmailAFriendEnabled;
            //compare products
            model.CompareProductsEnabled = _catalogSettings.CompareProductsEnabled;
            //store name
            model.CurrentStoreName = _localizationService.GetLocalized(_storeContext.CurrentStore, x => x.Name);

            //vendor details
            if (_vendorSettings.ShowVendorOnProductDetailsPage)
            {
                var vendor = _vendorService.GetVendorById(product.VendorId);
                if (vendor != null && !vendor.Deleted && vendor.Active)
                {
                    model.ShowVendor = true;

                    model.VendorModel = new VendorBriefInfoModel
                    {
                        Id = vendor.Id,
                        Name = _localizationService.GetLocalized(vendor, x => x.Name),
                        SeName = _urlRecordService.GetSeName(vendor),
                    };
                }
            }

            //page sharing
            if (_catalogSettings.ShowShareButton && !string.IsNullOrEmpty(_catalogSettings.PageShareCode))
            {
                var shareCode = _catalogSettings.PageShareCode;
                if (_webHelper.IsCurrentConnectionSecured())
                {
                    //need to change the add this link to be https linked when the page is, so that the page doesn't ask about mixed mode when viewed in https...
                    shareCode = shareCode.Replace("http://", "https://");
                }
                model.PageShareCode = shareCode;
            }

            //back in stock subscriptions
            if (product.ManageInventoryMethod == ManageInventoryMethod.ManageStock &&
                product.BackorderMode == BackorderMode.NoBackorders &&
                product.AllowBackInStockSubscriptions &&
                _productService.GetTotalStockQuantity(product) <= 0)
            {
                //out of stock
                model.DisplayBackInStockSubscription = true;
            }

            //breadcrumb
            //do not prepare this model for the associated products. anyway it's not used
            if (_catalogSettings.CategoryBreadcrumbEnabled && !isAssociatedProduct)
            {
                model.Breadcrumb = PrepareProductBreadcrumbModel(product);
            }

            //product tags
            //do not prepare this model for the associated products. anyway it's not used
            if (!isAssociatedProduct)
            {
                model.ProductTags = PrepareProductTagModels(product);
            }

            //pictures
            model.DefaultPictureZoomEnabled = _mediaSettings.DefaultPictureZoomEnabled;
            model.DefaultPictureModel = PrepareProductDetailsPictureModel(product, isAssociatedProduct, out IList<PictureModel> allPictureModels);
            model.PictureModels = allPictureModels;

            //price
            model.ProductPrice = PrepareProductPriceModel(product);

            //'Add to cart' model
            model.AddToCart = PrepareProductAddToCartModel(product, updatecartitem);

            //gift card
            if (product.IsGiftCard)
            {
                model.GiftCard.IsGiftCard = true;
                model.GiftCard.GiftCardType = product.GiftCardType;

                if (updatecartitem == null)
                {
                    model.GiftCard.SenderName = _customerService.GetCustomerFullName(_workContext.CurrentCustomer);
                    model.GiftCard.SenderEmail = _workContext.CurrentCustomer.Email;
                }
                else
                {
                    _productAttributeParser.GetGiftCardAttribute(updatecartitem.AttributesXml,
                        out string giftCardRecipientName, out string giftCardRecipientEmail,
                        out string giftCardSenderName, out string giftCardSenderEmail, out string giftCardMessage);

                    model.GiftCard.RecipientName = giftCardRecipientName;
                    model.GiftCard.RecipientEmail = giftCardRecipientEmail;
                    model.GiftCard.SenderName = giftCardSenderName;
                    model.GiftCard.SenderEmail = giftCardSenderEmail;
                    model.GiftCard.Message = giftCardMessage;
                }
            }

            //product attributes
            model.ProductAttributes = PrepareProductAttributeModels(product, updatecartitem);

            //product specifications
            //do not prepare this model for the associated products. anyway it's not used
            if (!isAssociatedProduct)
            {
                model.ProductSpecifications = PrepareProductSpecificationModel(product);
            }

            //product review overview
            model.ProductReviewOverview = PrepareProductReviewOverviewModel(product);

            //tier prices
            if (product.HasTierPrices && _permissionService.Authorize(StandardPermissionProvider.DisplayPrices))
            {
                model.TierPrices = PrepareProductTierPriceModels(product);
            }

            //manufacturers
            //do not prepare this model for the associated products. anyway it's not used
            if (!isAssociatedProduct)
            {
                model.ProductManufacturers = PrepareProductManufacturerModels(product);
            }

            //rental products
            if (product.IsRental)
            {
                model.IsRental = true;
                //set already entered dates attributes (if we're going to update the existing shopping cart item)
                if (updatecartitem != null)
                {
                    model.RentalStartDate = updatecartitem.RentalStartDateUtc;
                    model.RentalEndDate = updatecartitem.RentalEndDateUtc;
                }
            }

            //associated products
            if (product.ProductType == ProductType.GroupedProduct)
            {
                //ensure no circular references
                if (!isAssociatedProduct)
                {
                    var associatedProducts = _productService.GetAssociatedProducts(product.Id, _storeContext.CurrentStore.Id);
                    foreach (var associatedProduct in associatedProducts)
                        model.AssociatedProducts.Add(PrepareProductDetailsModel(associatedProduct, null, true));
                }
            }

            return model;
        }


        /// <inheritdoc />
        /// <summary>
        /// See custom code region, rest of function remains unaltered
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="updatecartitem">Updated shopping cart item</param>
        /// <returns>List of product attribute model</returns>
        protected override IList<ProductDetailsModel.ProductAttributeModel> PrepareProductAttributeModels(Product product, ShoppingCartItem updatecartitem)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            //performance optimization
            //We cache a value indicating whether a product has attributes
            IList<ProductAttributeMapping> productAttributeMapping = null;
            var cacheKey = string.Format(ModelCacheEventConsumer.PRODUCT_HAS_PRODUCT_ATTRIBUTES_KEY, product.Id);
            var hasProductAttributesCache = _cacheManager.Get(cacheKey, () =>
            {
                //no value in the cache yet
                //let's load attributes and cache the result (true/false)
                productAttributeMapping = _productAttributeService.GetProductAttributeMappingsByProductId(product.Id);
                return productAttributeMapping.Any();
            });
            if (hasProductAttributesCache && productAttributeMapping == null)
            {
                //cache indicates that the product has attributes
                //let's load them
                productAttributeMapping = _productAttributeService.GetProductAttributeMappingsByProductId(product.Id);
            }
            if (productAttributeMapping == null)
            {
                productAttributeMapping = new List<ProductAttributeMapping>();
            }

            var model = new List<ProductDetailsModel.ProductAttributeModel>();

            foreach (var attribute in productAttributeMapping)
            {
                var attributeModel = new ProductDetailsModel.ProductAttributeModel
                {
                    Id = attribute.Id,
                    ProductId = product.Id,
                    ProductAttributeId = attribute.ProductAttributeId,
                    Name = _localizationService.GetLocalized(attribute.ProductAttribute, x => x.Name),
                    Description = _localizationService.GetLocalized(attribute.ProductAttribute, x => x.Description),
                    TextPrompt = _localizationService.GetLocalized(attribute, x => x.TextPrompt),
                    IsRequired = attribute.IsRequired,
                    AttributeControlType = attribute.AttributeControlType,
                    DefaultValue = updatecartitem != null ? null : attribute.DefaultValue,
                    HasCondition = !string.IsNullOrEmpty(attribute.ConditionAttributeXml)
                };
                if (!string.IsNullOrEmpty(attribute.ValidationFileAllowedExtensions))
                {
                    attributeModel.AllowedFileExtensions = attribute.ValidationFileAllowedExtensions
                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .ToList();
                }

                if (attribute.ShouldHaveValues())
                {
                    //values
                    var attributeValues = _productAttributeService.GetProductAttributeValues(attribute.Id);

                    #region CUSTOM CODE
                    // Only custom code in function lives here!
                    FilterAttributes(product, ref attributeValues);
                    #endregion

                    foreach (var attributeValue in attributeValues)
                    {
                        var valueModel = new ProductDetailsModel.ProductAttributeValueModel
                        {
                            Id = attributeValue.Id,
                            Name = _localizationService.GetLocalized(attributeValue, x => x.Name),
                            ColorSquaresRgb = attributeValue.ColorSquaresRgb, //used with "Color squares" attribute type
                            IsPreSelected = attributeValue.IsPreSelected,
                            CustomerEntersQty = attributeValue.CustomerEntersQty,
                            Quantity = attributeValue.Quantity
                        };
                        attributeModel.Values.Add(valueModel);

                        //display price if allowed
                        if (_permissionService.Authorize(StandardPermissionProvider.DisplayPrices))
                        {
                            var attributeValuePriceAdjustment = _priceCalculationService.GetProductAttributeValuePriceAdjustment(attributeValue, updatecartitem?.Customer ?? _workContext.CurrentCustomer);
                            var priceAdjustmentBase = _taxService.GetProductPrice(product, attributeValuePriceAdjustment, out decimal _);
                            var priceAdjustment = _currencyService.ConvertFromPrimaryStoreCurrency(priceAdjustmentBase, _workContext.WorkingCurrency);

                            if (attributeValue.PriceAdjustmentUsePercentage)
                            {
                                var priceAdjustmentStr = attributeValue.PriceAdjustment.ToString("G29");
                                if (attributeValue.PriceAdjustment > decimal.Zero)
                                    valueModel.PriceAdjustment = "+";
                                valueModel.PriceAdjustment += priceAdjustmentStr + "%";
                            }
                            else
                            {
                                if (priceAdjustmentBase > decimal.Zero)
                                    valueModel.PriceAdjustment = "+" + _priceFormatter.FormatPrice(priceAdjustment, false, false);
                                else if (priceAdjustmentBase < decimal.Zero)
                                    valueModel.PriceAdjustment = "-" + _priceFormatter.FormatPrice(-priceAdjustment, false, false);
                            }

                            valueModel.PriceAdjustmentValue = priceAdjustment;
                        }

                        //"image square" picture (with with "image squares" attribute type only)
                        if (attributeValue.ImageSquaresPictureId > 0)
                        {
                            var productAttributeImageSquarePictureCacheKey = string.Format(ModelCacheEventConsumer.PRODUCTATTRIBUTE_IMAGESQUARE_PICTURE_MODEL_KEY,
                                   attributeValue.ImageSquaresPictureId,
                                   _webHelper.IsCurrentConnectionSecured(),
                                   _storeContext.CurrentStore.Id);
                            valueModel.ImageSquaresPictureModel = _cacheManager.Get(productAttributeImageSquarePictureCacheKey, () =>
                            {
                                var imageSquaresPicture = _pictureService.GetPictureById(attributeValue.ImageSquaresPictureId);
                                if (imageSquaresPicture != null)
                                {
                                    return new PictureModel
                                    {
                                        FullSizeImageUrl = _pictureService.GetPictureUrl(imageSquaresPicture),
                                        ImageUrl = _pictureService.GetPictureUrl(imageSquaresPicture, _mediaSettings.ImageSquarePictureSize)
                                    };
                                }
                                return new PictureModel();
                            });
                        }

                        //picture of a product attribute value
                        valueModel.PictureId = attributeValue.PictureId;
                    }
                }

                //set already selected attributes (if we're going to update the existing shopping cart item)
                if (updatecartitem != null)
                {
                    switch (attribute.AttributeControlType)
                    {
                        case AttributeControlType.DropdownList:
                        case AttributeControlType.RadioList:
                        case AttributeControlType.Checkboxes:
                        case AttributeControlType.ColorSquares:
                        case AttributeControlType.ImageSquares:
                            {
                                if (!string.IsNullOrEmpty(updatecartitem.AttributesXml))
                                {
                                    //clear default selection
                                    foreach (var item in attributeModel.Values)
                                        item.IsPreSelected = false;

                                    //select new values
                                    var selectedValues = _productAttributeParser.ParseProductAttributeValues(updatecartitem.AttributesXml);
                                    foreach (var attributeValue in selectedValues)
                                        foreach (var item in attributeModel.Values)
                                            if (attributeValue.Id == item.Id)
                                            {
                                                item.IsPreSelected = true;

                                                //set customer entered quantity
                                                if (attributeValue.CustomerEntersQty)
                                                    item.Quantity = attributeValue.Quantity;
                                            }
                                }
                            }
                            break;
                        case AttributeControlType.ReadonlyCheckboxes:
                            {
                                //values are already pre-set

                                //set customer entered quantity
                                if (!string.IsNullOrEmpty(updatecartitem.AttributesXml))
                                {
                                    foreach (var attributeValue in _productAttributeParser.ParseProductAttributeValues(updatecartitem.AttributesXml)
                                        .Where(value => value.CustomerEntersQty))
                                    {
                                        var item = attributeModel.Values.FirstOrDefault(value => value.Id == attributeValue.Id);
                                        if (item != null)
                                            item.Quantity = attributeValue.Quantity;
                                    }
                                }
                            }
                            break;
                        case AttributeControlType.TextBox:
                        case AttributeControlType.MultilineTextbox:
                            {
                                if (!string.IsNullOrEmpty(updatecartitem.AttributesXml))
                                {
                                    var enteredText = _productAttributeParser.ParseValues(updatecartitem.AttributesXml, attribute.Id);
                                    if (enteredText.Any())
                                        attributeModel.DefaultValue = enteredText[0];
                                }
                            }
                            break;
                        case AttributeControlType.Datepicker:
                            {
                                //keep in mind my that the code below works only in the current culture
                                var selectedDateStr = _productAttributeParser.ParseValues(updatecartitem.AttributesXml, attribute.Id);
                                if (selectedDateStr.Any())
                                {
                                    if (DateTime.TryParseExact(selectedDateStr[0], "D", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime selectedDate))
                                    {
                                        //successfully parsed
                                        attributeModel.SelectedDay = selectedDate.Day;
                                        attributeModel.SelectedMonth = selectedDate.Month;
                                        attributeModel.SelectedYear = selectedDate.Year;
                                    }
                                }

                            }
                            break;
                        case AttributeControlType.FileUpload:
                            {
                                if (!string.IsNullOrEmpty(updatecartitem.AttributesXml))
                                {
                                    var downloadGuidStr = _productAttributeParser.ParseValues(updatecartitem.AttributesXml, attribute.Id).FirstOrDefault();
                                    Guid.TryParse(downloadGuidStr, out Guid downloadGuid);
                                    var download = _downloadService.GetDownloadByGuid(downloadGuid);
                                    if (download != null)
                                        attributeModel.DefaultValue = download.DownloadGuid.ToString();
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }

                model.Add(attributeModel);
            }

            return model;
        }

        private static void FilterAttributes(Product product, ref IList<ProductAttributeValue> attributeValues)
        {
            if (product == null || attributeValues == null) return;
            attributeValues = (from attr in attributeValues let id = attr.Id let match = product.ProductAttributeCombinations.FirstOrDefault(a => a.AttributesXml.Contains($"<Value>{id}</Value>")) where match != null select attr).ToList();
        }

        public void FilterAttributes(Product product, ref IList<ProductAttributeMapping> productAttributes)
        {
            foreach (var attr in productAttributes)
            {
                var values = (from value in attr.ProductAttributeValues
                    let id = value.Id
                    let match = product.ProductAttributeCombinations.FirstOrDefault(a =>
                        a.AttributesXml.Contains($"<Value>{id}</Value>"))
                    where match != null
                    select value).ToList();
                attr.ProductAttributeValues.Clear();
                foreach(var value in values) // unfortunate double-iteration 
                attr.ProductAttributeValues.Add(value);
            }
        }
    }
}
