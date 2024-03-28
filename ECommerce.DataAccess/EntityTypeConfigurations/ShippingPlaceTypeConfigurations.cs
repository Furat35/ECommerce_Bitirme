﻿using ECommerce.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.EntityTypeConfigurations
{
    public class ShippingPlaceTypeConfigurations : IEntityTypeConfiguration<ShippingPlace>
    {
        public void Configure(EntityTypeBuilder<ShippingPlace> builder)
        {
            builder.HasKey(_ => _.Id);
        }
    }
}
