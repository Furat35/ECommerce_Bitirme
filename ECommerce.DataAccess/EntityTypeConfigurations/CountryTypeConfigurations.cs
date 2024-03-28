using ECommerce.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.EntityTypeConfigurations
{
    public class CountryTypeConfigurations : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("Countries");

            builder.HasKey(_ => _.Id);

            builder.Ignore(_ => _.CreatedDate);
            builder.Ignore(_ => _.ModifiedDate);
            builder.Ignore(_ => _.DeletedDate);
        }
    }
}
