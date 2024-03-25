using AutoMapper;
using AutoMapper.QueryableExtensions;
using Chair.BLL.Dto.ProductFile;
using Chair.DAL.Extension;
using Chair.DAL.Extension.Models;
using Chair.DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.BusinessLogic.ProductFile;

public class ProductFileBusinessLogic<TProductFile, TFileSaveDto, TFileViewDto>
    where TProductFile : DAL.Data.Entities.ProductFile, new()
    where TFileSaveDto : FileSaveDto
    where TFileViewDto : class //FileViewDto
{
    private readonly BaseWithManyRepository<TProductFile> _productFileRepository;
    private readonly IMapper _mapper;

    public ProductFileBusinessLogic(BaseWithManyRepository<TProductFile> productFileRepository,
        IMapper mapper)
    {
        _productFileRepository = productFileRepository;
        _mapper = mapper;
    }

    public async Task<List<TFileViewDto>> GetAllFilesByProductIdAsync(Guid productId)
    {
        var result = await _productFileRepository
            .GetAllAsync()
            .Where(x => x.ProductId == productId)
            .ProjectTo<TFileViewDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        return result;
    }


    public async Task AddAsync(Guid productId, TFileSaveDto file)
    {
        var entity = DtoToEntity(productId, file);

        await _productFileRepository.AddAsync(entity);
        await _productFileRepository.SaveChangesAsync();
    }

    public async Task AddRangeAsync(Guid productId, IEnumerable<TFileSaveDto> files)
    {
        var entities = files
            .Select(x => DtoToEntity(productId, x))
            .ToList();

        await _productFileRepository.AddManyAsync(entities);
        await _productFileRepository.SaveChangesAsync();
    }

    private TProductFile DtoToEntity(Guid productId, TFileSaveDto file)
    {
        var entity = _mapper.Map<TProductFile>(file);
        entity.ProductId = productId;
        return entity;
    }


    public async Task RemoveAsync(Guid productId, Guid fileId)
    {
        var entity = _productFileRepository
            .GetAllByPredicateAsQueryable(x => x.ProductId == productId)
            .Where(x => fileId == x.MinioFileId)
            .First();
        await _productFileRepository.RemoveAsync(entity);
        await _productFileRepository.SaveChangesAsync();
    }

    public async Task RemoveRangeAsync(Guid productId, IEnumerable<Guid> fileIds)
    {
        var entities = _productFileRepository
            .GetAllByPredicateAsQueryable(x => x.ProductId == productId)
            .Where(x => fileIds.Contains(x.MinioFileId))
            .ToList();
        await _productFileRepository.RemoveManyAsync(entities);
        await _productFileRepository.SaveChangesAsync();
    }
}