using Nop.Services.Catalog;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Stores;
using Nop.Data;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Stores;

namespace Nop.Plugin.Products.AttributeFilters
{
    public partial class ProductServiceCustom : ProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<RelatedProduct> _relatedProductRepository;
        private readonly IRepository<CrossSellProduct> _crossSellProductRepository;
        private readonly IRepository<TierPrice> _tierPriceRepository;
        private readonly IRepository<LocalizedProperty> _localizedPropertyRepository;
        private readonly IRepository<AclRecord> _aclRepository;
        private readonly IRepository<StoreMapping> _storeMappingRepository;
        private readonly IRepository<ProductPicture> _productPictureRepository;
        private readonly IRepository<ProductSpecificationAttribute> _productSpecificationAttributeRepository;
        private readonly IRepository<ProductReview> _productReviewRepository;
        private readonly IRepository<ProductWarehouseInventory> _productWarehouseInventoryRepository;
        private readonly IRepository<SpecificationAttributeOption> _specificationAttributeOptionRepository;
        private readonly IRepository<StockQuantityHistory> _stockQuantityHistoryRepository;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly ILanguageService _languageService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;
        private readonly ICacheManager _cacheManager;
        private readonly IWorkContext _workContext;
        private readonly LocalizationSettings _localizationSettings;
        private readonly CommonSettings _commonSettings;
        private readonly CatalogSettings _catalogSettings;
        private readonly IEventPublisher _eventPublisher;
        private readonly IAclService _aclService;
        private readonly IStoreMappingService _storeMappingService;

        public ProductServiceCustom(ICacheManager cacheManager, IRepository<Product> productRepository, IRepository<RelatedProduct> relatedProductRepository, IRepository<CrossSellProduct> crossSellProductRepository, IRepository<TierPrice> tierPriceRepository, IRepository<ProductPicture> productPictureRepository, IRepository<LocalizedProperty> localizedPropertyRepository, IRepository<AclRecord> aclRepository, IRepository<StoreMapping> storeMappingRepository, IRepository<ProductSpecificationAttribute> productSpecificationAttributeRepository, IRepository<ProductReview> productReviewRepository, IRepository<ProductWarehouseInventory> productWarehouseInventoryRepository, IRepository<SpecificationAttributeOption> specificationAttributeOptionRepository, IRepository<StockQuantityHistory> stockQuantityHistoryRepository, IProductAttributeService productAttributeService, IProductAttributeParser productAttributeParser, ILanguageService languageService, IWorkflowMessageService workflowMessageService, IDataProvider dataProvider, IDbContext dbContext, IWorkContext workContext, LocalizationSettings localizationSettings, CommonSettings commonSettings, CatalogSettings catalogSettings, IEventPublisher eventPublisher, IAclService aclService, IStoreMappingService storeMappingService) 
            : base(cacheManager, productRepository, relatedProductRepository, crossSellProductRepository, tierPriceRepository, productPictureRepository, localizedPropertyRepository, aclRepository, storeMappingRepository, productSpecificationAttributeRepository, productReviewRepository, productWarehouseInventoryRepository, specificationAttributeOptionRepository, stockQuantityHistoryRepository, productAttributeService, productAttributeParser, languageService, workflowMessageService, dataProvider, dbContext, workContext, localizationSettings, commonSettings, catalogSettings, eventPublisher, aclService, storeMappingService)
        {
            this._cacheManager = cacheManager;
            this._productRepository = productRepository;
            this._relatedProductRepository = relatedProductRepository;
            this._crossSellProductRepository = crossSellProductRepository;
            this._tierPriceRepository = tierPriceRepository;
            this._productPictureRepository = productPictureRepository;
            this._localizedPropertyRepository = localizedPropertyRepository;
            this._aclRepository = aclRepository;
            this._storeMappingRepository = storeMappingRepository;
            this._productSpecificationAttributeRepository = productSpecificationAttributeRepository;
            this._productReviewRepository = productReviewRepository;
            this._productWarehouseInventoryRepository = productWarehouseInventoryRepository;
            this._specificationAttributeOptionRepository = specificationAttributeOptionRepository;
            this._stockQuantityHistoryRepository = stockQuantityHistoryRepository;
            this._productAttributeService = productAttributeService;
            this._productAttributeParser = productAttributeParser;
            this._languageService = languageService;
            this._workflowMessageService = workflowMessageService;
            this._dataProvider = dataProvider;
            this._dbContext = dbContext;
            this._workContext = workContext;
            this._localizationSettings = localizationSettings;
            this._commonSettings = commonSettings;
            this._catalogSettings = catalogSettings;
            this._eventPublisher = eventPublisher;
            this._aclService = aclService;
            this._storeMappingService = storeMappingService;
        }
    }
}
