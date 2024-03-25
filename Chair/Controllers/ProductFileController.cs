using Chair.BLL.CQRS.ProductFile;
using Chair.BLL.Dto.Minio;
using Chair.BLL.Dto.ProductFile;
using Chair.DAL.Data.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chair.Controllers;

public abstract class ProductFileController<TProduct> : ProductFileController<ProductFile<TProduct>, FileSaveDto, FileViewDto>
    where TProduct : class
{
    protected ProductFileController(IMediator mediator)
        : base(mediator) { }
}

[ApiController]
[Route("[controller]")]
public abstract class ProductFileController<TProductFile, TFileSaveDto, TFileViewDto> : ControllerBase
        where TProductFile : ProductFile, new()
        where TFileSaveDto : FileSaveDto
        where TFileViewDto : class //FileViewDto
{
    private readonly IMediator _mediator;

    public ProductFileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("{productId:guid}/files/filter")]
    [ProducesResponseType(typeof(List<MinioFileDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllByProductId([FromRoute] Guid productId)
    {
        var query = new GetProductFileListQuery<TProductFile, TFileViewDto>()
        {
            ProductId = productId,
        };
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("{productId:guid}/file")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Add([FromRoute] Guid productId, [FromBody] TFileSaveDto file)
    {
        var command = new AddProductFileCommand<TProductFile, TFileSaveDto>()
        {
            ProductId = productId,
            File = file
        };
        await _mediator.Send(command);

        return NoContent();
    }

    [HttpPost]
    [Route("{productId:guid}/files")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddRange([FromRoute] Guid productId, [FromBody] List<TFileSaveDto> files)
    {
        var command = new AddRangeProductFileCommand<TProductFile, TFileSaveDto>()
        {
            ProductId = productId,
            Files = files
        };
        await _mediator.Send(command);

        return NoContent();
    }

    [HttpDelete]
    [Route("{productId:guid}/file")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Remove([FromRoute] Guid productId, [FromBody] Guid fileId)
    {
        var command = new RemoveProductFileCommand<TProductFile>()
        {
            ProductId = productId,
            FileId = fileId
        };
        await _mediator.Send(command);

        return NoContent();
    }

    [HttpDelete]
    [Route("{productId:guid}/files")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveRange([FromRoute] Guid productId, [FromBody] List<Guid> fileIds)
    {
        var command = new RemoveRangeProductFileCommand<TProductFile>()
        {
            ProductId = productId,
            FileIds = fileIds
        };
        await _mediator.Send(command);

        return NoContent();
    }
}