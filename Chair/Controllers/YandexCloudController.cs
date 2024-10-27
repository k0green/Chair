using Chair.BLL.CQRS.Minio;
using Chair.BLL.CQRS.YandexCloud;
using Chair.BLL.Dto.Minio;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chair.Controllers
{
    [Route("yandex-cloud")]
    [ApiController]
    [Authorize]
    public class YandexCloudController : ControllerBase
    {
        private readonly IMediator _mediator;

        public YandexCloudController(IMediator mediator)
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
            var query = new UploadYandexCloudCommand()
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

        [HttpDelete("delete/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteFileAsync([FromRoute] Guid id)
        {
            var query = new DeleteYandexCloudCommand() { Id = id };
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
            var command = new DownloadYandexCloudQuery() { Id = id };
            var result = await _mediator.Send(command);

            return File(result.File, "application/octet-stream", result.Name);
        }
    }
}