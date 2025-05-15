using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabDomain.Model;

public partial class PublicationType
{
    public int PublicationTypeId { get; set; }
    [MinLength(4, ErrorMessage = "Name must be at least 4 characters long")]
    public string Name { get; set; } = null!;

    public string? Info { get; set; }

    public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();
}
