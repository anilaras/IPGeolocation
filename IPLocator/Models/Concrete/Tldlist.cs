using IPLocator.Models.Concrete;
using System;
using System.Collections.Generic;

namespace IPLocator.Models;

public partial class Tldlist : BaseEntity
{

    public string? Tld { get; set; }

    public string? Name { get; set; }

    public string? Language { get; set; }

    public string? LanguageCode { get; set; }

    public virtual ICollection<CountryLanguageCode> CountryLanguageCodes { get; } = new List<CountryLanguageCode>();
}
