using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Web.Models;

public class FileModel
{
    /// <summary>
    ///     Id of the file
    /// </summary>
    [NotNull]
    [Required]
    public string Id { get; set; }

    /// <summary>
    ///     Name of the file
    /// </summary>
    [NotNull]
    [Required]
    public string Filename { get; set; }

    /// <summary>
    ///     File's owner
    /// </summary>
    [NotNull]
    [Required]
    public string Username { get; set; }

    /// <summary>
    ///     Content type
    /// </summary>
    [NotNull]
    [Required]
    public string Mime { get; set; }

    /// <summary>
    ///     Virtual location of the file (folder style)
    /// </summary>
    [NotNull]
    [Required]
    public string Location { get; set; }

    /// <summary>
    ///     Size of the file in bytes
    /// </summary>
    [Required]
    public long Size { get; set; }
}