using Chair.BLL.CQRS.Minio;
using Chair.BLL.Dto.Minio;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("minio")]
[ApiController]
//[Authorize]
public class MinioController : ControllerBase
{
    private readonly IMediator _mediator;

    public MinioController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("upload")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UploadFileAsync([FromForm] FileWithDescriptionModel model)
    {
        var stream = new MemoryStream();
        model.File.CopyTo(stream);
        stream.Seek(0, SeekOrigin.Begin);
        var query = new UploadMinioFileCommand()
        {
            AddMinioFileDto = new AddMinioFileDto
            {
                FileName = model.File.FileName,
                FileData = stream
            }
        };
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpPost("upload/range")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UploadRangeFileAsync([FromForm] List<FileWithDescriptionModel> models)
    {
        var dtos = new List<AddMinioFileDto>();

        foreach (var model in models)
        {
            var stream = new MemoryStream();
            model.File.CopyTo(stream);
            stream.Seek(0, SeekOrigin.Begin);
            dtos.Add(new AddMinioFileDto
            {
                FileName = model.File.FileName,
                FileData = stream
            });
        }

        var query = new UploadRangeMinioFileCommand()
        {
            AddMinioFileDto = dtos,
        };

        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpDelete("delete/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteFileAsync([FromRoute] Guid id)
    {
        var query = new DeleteMinioFileCommand() { Id = id };
        await _mediator.Send(query);

        return NoContent();
    }

    [HttpGet("download/partial/{id:guid}")]
    public async Task<IActionResult> DownloadPartialFileAsync([FromRoute] Guid id, [FromQuery] string contentType)
    {
        var command = new DownloadMinioFileQuery() { Id = id };
        var result = await _mediator.Send(command);

        return File(result.File, contentType, enableRangeProcessing: true);
    }

    [HttpGet("download/{id:guid}")]
    public async Task<IActionResult> DownloadFileAsync([FromRoute] Guid id)
    {
        var command = new DownloadMinioFileQuery() { Id = id };
        var result = await _mediator.Send(command);

        return File(result.File, "application/octet-stream", result.Name);
    }

}

public class FileWithDescriptionModel
{
    public IFormFile File { get; set; }
}