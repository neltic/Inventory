namespace Stock.Domain.Entities.Common;

/// <summary>
/// Defines the application context or scope where a metadata entity (like a Brand or Category) is visible.
/// </summary>
/// <remarks>
/// This enumeration uses the <see cref="FlagsAttribute"/>, allowing for bitwise combinations.
/// For example, a category could be assigned to both Items and Boxes simultaneously.
/// Using <see cref="byte"/> as the underlying type ensures minimal storage footprint in the database.
/// </remarks>
[Flags]
public enum EntityScope : byte
{
    /// <summary>
    /// No scope assigned. The entity will not be visible in standard filtered lookups.
    /// </summary>
    None = 0,

    /// <summary>
    /// The entity is only applicable and visible for Individual Items.
    /// </summary>
    Item = 1,

    /// <summary>
    /// The entity is only applicable and visible for Boxes (Containers).
    /// </summary>
    Box = 2,

    /// <summary>
    /// Global scope. The entity is visible across all modules, including Items and Boxes.
    /// Represents the maximum value (255) for the underlying byte type.
    /// </summary>
    General = 255
}