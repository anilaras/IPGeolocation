using IPLocator.Models.Concrete;
using System;
using System.Collections.Generic;

namespace IPLocator.Models;

public partial class CountryLanguageCode : BaseEntity
{

    public string? LanguageCode { get; set; }

    public int? TldlistId { get; set; }

    public virtual Tldlist? Tldlist { get; set; }
}
