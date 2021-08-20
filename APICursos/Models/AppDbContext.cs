using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace APICursos.Models
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Aluno> Alunos { get; set; }
        public virtual DbSet<Turma> Turmas { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=JOAOPC\\SERVER;Initial Catalog=cursos;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Aluno>(entity =>
            {
                entity.HasKey(e => e.CodMatricula)
                    .HasName("Pk_aluno_cod_aluno");

                entity.ToTable("aluno");

                entity.Property(e => e.CodMatricula).HasColumnName("cod_aluno");

                entity.Property(e => e.CodTurma).HasColumnName("cod_turma");

                entity.Property(e => e.Nome)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("nome");

                entity.HasOne(d => d.CodTurmaNavigation)
                    .WithMany(p => p.Alunos)
                    .HasForeignKey(d => d.CodTurma)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_aluno_turma");
            });

            modelBuilder.Entity<Turma>(entity =>
            {
                entity.HasKey(e => e.CodTurma)
                    .HasName("Pk_turma_cod_turma");

                entity.ToTable("turma");

                entity.Property(e => e.CodTurma).HasColumnName("cod_turma");

                entity.Property(e => e.DataInicio)
                    .HasColumnType("date")
                    .HasColumnName("data_inicio");

                entity.Property(e => e.IdiomaTurma)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("idioma_turma");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.NomeUsuario)
                    .HasName("Pk_usuario_nome_usuario");

                entity.ToTable("usuario");

                entity.Property(e => e.NomeUsuario)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("nome_usuario");

                entity.Property(e => e.Senha)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("senha");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
