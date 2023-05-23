using System;
using logistics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace logistics.Datas.Configuarions
{
	public class AdminTypeConfiguration : IEntityTypeConfiguration<Admin>
	{
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.ToTable<Admin>
        }
    }
}

