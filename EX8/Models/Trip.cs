﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EX8.Models;

public partial class Trip
{
    public int IdTrip { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime DateFrom { get; set; }

    public DateTime DateTo { get; set; }

    public int MaxPeople { get; set; }
[JsonIgnore]
    public virtual ICollection<ClientTrip> ClientTrips { get; set; } = new List<ClientTrip>();

    public virtual ICollection<Country> IdCountries { get; set; } = new List<Country>();
}
