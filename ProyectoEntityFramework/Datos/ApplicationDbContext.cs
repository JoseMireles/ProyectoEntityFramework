using CursoEntityCore.Models;
using Microsoft.EntityFrameworkCore;

namespace CursoEntityCore.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opciones) : base(opciones)
        {
        }

        //Escribir modelos
        public DbSet<Categoria> Categoria { get; set; }

        //Cuando crear migraciones (buenas prácticas)
        //1-Cuando se crea una nueva clase (tabla en la bd)
        //2-Cuando agregue una nueva propiedad (crear un campo nuevo en la bd)
        //3-Modifique un valor de campo en la clase (modificar campo en bd)
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Articulo> Articulo { get; set; }
        public DbSet<DetalleUsuario> DetalleUsuario { get; set; }

        public DbSet<Etiqueta> Etiqueta { get; set; }

        //Agregamos dbset para la tabla de realación ArticuloEtiqueta
        public DbSet<ArticuloEtiqueta> ArticuloEtiqueta { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<ArticuloEtiqueta>().HasKey(ae => new {ae.Etiqueta_Id, ae.Articulo_Id});

            //Siembra de datos se hace en este método
            //var categoria5 = new Categoria() { Categoria_Id = 33, Nombre = "Categoría 5", FechaCreacion = new DateTime(2021, 11, 28), Activo = true};
            //var categoria6 = new Categoria() { Categoria_Id = 34, Nombre = "Categoría 6", FechaCreacion = new DateTime(2021, 11, 29), Activo = false };

            //modelBuilder.Entity<Categoria>().HasData(new Categoria[] { categoria6 });

            //Fluent API para Categoria
            modelBuilder.Entity<Categoria>().HasKey(c => c.Categoria_Id);
            modelBuilder.Entity<Categoria>().Property(c => c.Nombre).IsRequired();
            modelBuilder.Entity<Categoria>().Property(c => c.FechaCreacion).HasColumnType("date");

            //Fluent API para Articulo
            modelBuilder.Entity<Articulo>().HasKey(c => c.Articulo_Id);
            modelBuilder.Entity<Articulo>().Property(c => c.TituloArticulo).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Articulo>().Property(c => c.Descripcion).IsRequired().HasMaxLength(500);
            modelBuilder.Entity<Articulo>().Property(c => c.Fecha).HasColumnType("date");

            //Fluent API nombre de tabla y nombre columna
            modelBuilder.Entity<Articulo>().ToTable("Tbl_Articulo");
            modelBuilder.Entity<Articulo>().Property(c => c.TituloArticulo).HasColumnName("Titulo");

            //Fluent API para Usuario
            modelBuilder.Entity<Usuario>().HasKey(c => c.Id);
            modelBuilder.Entity<Usuario>().Ignore(u => u.Edad);

            //Fluent API para DetalleUsuario
            modelBuilder.Entity<DetalleUsuario>().HasKey(c => c.DetalleUsuario_Id);
            modelBuilder.Entity<DetalleUsuario>().Property(c => c.Cedula).IsRequired();

            //Fluent API para Etiqueta
            modelBuilder.Entity<Etiqueta>().HasKey(c => c.Etiqueta_Id);
            modelBuilder.Entity<Etiqueta>().Property(c => c.Fecha).HasColumnType("date");

            //Fluent API: relación de uno a uno entre Usuario y DetalleUsuario
            modelBuilder.Entity<Usuario>()
                .HasOne(c => c.DetalleUsuario)
                .WithOne(c => c.Usuario).HasForeignKey<Usuario>("DetalleUsuario_Id");

            //Fluent API: relación de uno a muchos entre Categoria y Articulo
            modelBuilder.Entity<Articulo>()
                .HasOne(c => c.Categoria)
                .WithMany(c => c.Articulo).HasForeignKey(c => c.Categoria_Id);

            //Fluent API: relación de muchos a muchos entre Articulo y Etiqueta
            modelBuilder.Entity<ArticuloEtiqueta>().HasKey(ae => new { ae.Etiqueta_Id, ae.Articulo_Id});
            modelBuilder.Entity<ArticuloEtiqueta>()
                .HasOne(a => a.Articulo)
                .WithMany(a => a.ArticuloEtiqueta).HasForeignKey(c => c.Articulo_Id);
            modelBuilder.Entity<ArticuloEtiqueta>()
                .HasOne(a => a.Etiqueta)
                .WithMany(a => a.ArticuloEtiqueta).HasForeignKey(c => c.Etiqueta_Id);


            base.OnModelCreating(modelBuilder);
        }
    }
}
