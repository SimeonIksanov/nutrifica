using Nutrifica.Api.Contracts.Products;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Mappings;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Products.Update;

public class ProductUpdateCommandHandler : ICommandHandler<ProductUpdateCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public ProductUpdateCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<ProductDto>> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product is null) return Result.Failure<ProductDto>(ProductError.ProductNotFound);

        product.Name = request.Name;
        product.Details = request.Details;
        product.Price = request.Price;
        product.State = request.State;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var model = await _productRepository.GetProductModelByIdAsync(product.Id, cancellationToken);
        return Result.Success(model!.ToProductDto());
    }
}