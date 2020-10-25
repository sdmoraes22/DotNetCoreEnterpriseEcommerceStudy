using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NSE.Catalogo.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Catalogo.API.Data.Mappings
{
    public class ProdutoMapping : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey("Id");

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasColumnType("Varchar(250)");

            builder.Property(c => c.Descricao)
                .IsRequired()
                .HasColumnType("Varchar(500)");

            builder.Property(c => c.Imagem)
                .IsRequired()
                .HasColumnType("Varchar(250)");

            builder.ToTable("Produtos");


        }
    }
}
