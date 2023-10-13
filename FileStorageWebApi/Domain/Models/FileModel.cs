using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileStorageWebApi.Domain.Models;

[Table("files")]
public class FileModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("name")]
    public string FileName { get; set; }

    [Column("content", TypeName = "bytea")]
    public byte[] Content { get; set; }

    [Column("owner_id")]
    public int OwnerId { get; set; }

}