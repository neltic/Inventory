using FluentAssertions;
using Moq;
using Stock.Application.DTOs;
using Stock.Application.Services;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;

namespace Stock.UnitTest;

public class BoxServiceTests
{
    private readonly Mock<IBoxRepository> _boxRepositoryMock;
    private readonly BoxService _service;

    public BoxServiceTests()
    {
        _boxRepositoryMock = new Mock<IBoxRepository>();        
        _service = new BoxService(_boxRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateWhenNameIsDuplicateShouldThrowInvalidOperationException()
    {
        // Arrange
        var dto = new BoxDto(0, 5, "Duplicate box", 1, 1, 10, 10, 10, "Notes");
                
        _boxRepositoryMock.Setup(r => r.ExistsAsync(dto.Name, dto.ParentBoxId))
            .ReturnsAsync(true);

        // Act
        Func<Task> act = async () => await _service.CreateAsync(dto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("A box with this name already exists in this location.");
                
        _boxRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Box>()), Times.Never);
    }

    [Fact]
    public async Task CreateWhenDataIsValidShouldReturnNewId()
    {
        // Arrange
        var dto = new BoxDto(0, null, "New Box", 1, 1, 5, 5, 5, "Ok");
        var expectedId = 99;
                
        _boxRepositoryMock.Setup(r => r.ExistsAsync(dto.Name, dto.ParentBoxId))
            .ReturnsAsync(false);
                
        _boxRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Box>()))
            .ReturnsAsync(expectedId);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        result.Should().Be(expectedId);
                
        _boxRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Box>()), Times.Once);
    }

    // TODO: Create tests with SQL Containers instead of mocks to verify the actual database interactions and constraints:
    [Theory]
    [InlineData(10, 20, true)]      // Move to another parent
    [InlineData(10, null, true)]    // Move to root
    public async Task MoveBoxShouldInvokeRepositoryWithCorrectParametersAsync(int boxId, int? newParentId, bool expectedResult)
    {
        // Arrange
        _boxRepositoryMock.Setup(r => r.MoveBoxAsync(boxId, newParentId))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _service.MoveBoxAsync(boxId, newParentId);

        // Assert
        result.Should().Be(expectedResult);
                
        _boxRepositoryMock.Verify(r => r.MoveBoxAsync(boxId, newParentId), Times.Once);
    }

    [Fact]
    public async Task MoveBoxWhenRepositoryFailsShouldReturnFalse()
    {
        // Arrange
        _boxRepositoryMock.Setup(r => r.MoveBoxAsync(It.IsAny<int>(), It.IsAny<int?>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.MoveBoxAsync(1, 2);

        // Assert
        result.Should().BeFalse();
    }
}