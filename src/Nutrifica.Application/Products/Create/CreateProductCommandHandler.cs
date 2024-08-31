using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ProductAggregate;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Products.Create;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, int>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = Product.Create(command.Name, command.Price, command.Details);
        _productRepository.Add(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(product.Id.Value);
    }
}