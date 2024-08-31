using MediatR;

using Microsoft.AspNetCore.Mvc;

using Nutrifica.Api.Contracts.Products;
using Nutrifica.Application.Mappings;
using Nutrifica.Application.Products.Create;
using Nutrifica.Application.Products.Get;
using Nutrifica.Application.Products.GetById;
using Nutrifica.Application.Products.Update;
using Nutrifica.Application.Shared;
using Nutrifica.Domain.Aggregates.ProductAggregate.ValueObjects;
using Nutrifica.Domain.Shared;

namespace Nutrifica.Api.Controllers;

[Route("api/products")]
public class ProductsController : ApiController
{
    public ProductsController(IMediator mediator) : base(mediator) { }

    /// <summary>
    /// Get product list
    /// </summary>
    /// <param name="cancellationToken"></param>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] QueryParams queryParams, CancellationToken ct)
    {
        var query = new GetProductsQuery(queryParams);
        var result = await _mediator.Send(query, ct);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    /// <summary>
    /// Get product 
    /// </summary>
    /// <param name="ct"></param>
    [HttpGet("{productId:int}")]
    public async Task<IActionResult> GetById([FromRoute] int productId, CancellationToken ct)
    {
        var query = new GetProductQuery(ProductId.Create(productId));
        var result = await _mediator.Send(query, ct);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    /// <summary>
    /// Create new product 
    /// </summary>
    /// <param name="ct"></param>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto productDto, CancellationToken ct)
    {
        var command = new CreateProductCommand
        {
            Name = productDto.Name,
            Details = productDto.Details,
            Price = new Money(productDto.Price.Amount, Currency.FromCode(productDto.Price.Currency.Code)),
        };
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { productId = result.Value }, result.Value)
            : HandleFailure(result);
    }

    /// <summary>
    /// Update product
    /// </summary>
    /// <param name="ct"></param>
    [HttpPut("{productId:int}")]
    public async Task<IActionResult> Update([FromRoute] int productId, [FromBody] ProductUpdateDto productDto,
        CancellationToken ct)
    {
        if (productId != productDto.Id)
            return BadRequest();
        var command = new ProductUpdateCommand
        {
            Id = ProductId.Create(productDto.Id),
            Details = productDto.Details,
            Name = productDto.Name,
            Price = productDto.Price.ToMoney(),
            State = productDto.State
        };
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }
}