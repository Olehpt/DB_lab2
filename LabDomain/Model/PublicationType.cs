using System;
using System.Collections.Generic;

namespace LabDomain.Model;

public partial class PublicationType
{
    public int PublicationTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string? Info { get; set; }
}
