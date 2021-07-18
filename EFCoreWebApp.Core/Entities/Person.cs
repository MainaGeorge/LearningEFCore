using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreWebApp.Core.Entities
{
    [Table("persons", Schema = "dbo")]
    public class Person
    {
        [Column("Person_Id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        [Column(TypeName = "nvarchar(255)")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        [Column(TypeName = "nvarchar(255)")]

        public string LastName { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        public List<Address> Addresses { get; set; } = new List<Address>();

    }
}
