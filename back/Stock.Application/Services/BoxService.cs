using Stock.Application.DTOs;
using Stock.Application.Interfaces;
using Stock.Application.Mappings;
using Stock.Domain.Interfaces;

namespace Stock.Application.Services;

public class BoxService(IBoxRepository boxRepository) : IBoxService
{
    /// <inheritdoc />
    public async Task<BoxDetailedDto?> GetBoxByIdAsync(int boxId)
    {
        if (boxId > 0)
        {
            var box = await boxRepository.GetBoxByIdAsync(boxId);

            return box?.ToDto();
        }
        return null;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BoxListDto>> GetBoxesByParentBoxIdAsync(int? parentBoxId)
    {
        var results = await boxRepository.GetBoxesByParentAsync(parentBoxId);

        return results.ToDtoList();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BoxLookupListDto>> GetBoxesLookupAsync()
    {
        var results = await boxRepository.GetBoxesLookupAsync();

        return results.ToDtoList();
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(BoxDto dto)
    {
        var exists = await boxRepository.ExistsAsync(dto.Name, dto.ParentBoxId);

        if (exists)
        {            
            throw new InvalidOperationException("A box with this name already exists in this location.");
        }

        var box = dto.ToEntity(0);

        return await boxRepository.AddAsync(box);
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(int boxId, BoxDto dto)
    {
        var existingBox = await boxRepository.ExistsAsync(boxId);

        if (!existingBox)
        {
            return false;
        }

        var exists = await boxRepository.ExistsAsync(dto.Name, dto.ParentBoxId, dto.BoxId);

        if (exists)
        {
            throw new InvalidOperationException("A box with this name already exists in this location.");
        }

        return await boxRepository.UpdateAsync(dto.ToEntity(boxId));
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int boxId)
    {
        var box = await boxRepository.FindAsync(boxId);
        if (box == null) return false;

        return await boxRepository.DeleteAsync(box);
    }

    /// <inheritdoc />
    public async Task<DateTime> ChangeUpdatedAtAsync(int boxId)
    {
        return await boxRepository.ChangeUpdatedAtAsync(boxId);
    }

    /// <inheritdoc />
    public async Task<BoxDetailedDto> GetEmptyBoxByParentBoxIdAsync(int? parentBoxId)
    {
        var path = await boxRepository.GetBoxFullPathByParentAsync(parentBoxId);
                
        return new(0, parentBoxId, string.Empty, -1, 0, 0, 0, 0, 0, string.Empty, DateTime.Today, DateTime.Today, path);
    }
}
