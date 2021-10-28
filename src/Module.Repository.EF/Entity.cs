﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TrackableEntities;

namespace Module.Repository.EF
{
    public abstract class Entity : ITrackable
    {
        [NotMapped] public TrackingState TrackingState { get; set; }

        [NotMapped] public ICollection<string> ModifiedProperties { get; set; }
    }
}