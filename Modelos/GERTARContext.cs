using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using GERTAR.Modelos;

namespace GERTAR.Modelos;

public partial class GERTARContext : DbContext
{
    public GERTARContext()
    {
    }

    public GERTARContext(DbContextOptions<GERTARContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TB_PROJETO> TB_PROJETOS { get; set; }

    public virtual DbSet<TB_PROJETO_TAREFA> TB_PROJETO_TAREFAS { get; set; }

    public virtual DbSet<TB_USUARIO> TB_USUARIOS { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TB_PROJETO>(entity =>
        {
            entity.HasKey(e => e.ID_PROJETO);

            entity.ToTable("TB_PROJETOS");

            entity.Property(e => e.NM_PROJETO)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_PROJETO_TAREFA>(entity =>
        {
            entity.HasKey(e => new { e.ID_PROJETO, e.ID_TAREFA });

            entity.ToTable("TB_PROJETO_TAREFAS", tb => tb.HasTrigger("TR_ATU_TAREFA"));

            entity.Property(e => e.COMENTARIO)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.DESCRICAO)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.DT_ATUALIZACAO)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PRIORIDADE)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.STATUS_TAREFA)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TITULO)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VENCIMENTO).HasColumnType("datetime");
        });

        modelBuilder.Entity<TB_USUARIO>(entity =>
        {
            entity.HasKey(e => e.ID_USUARIO);

            entity.ToTable("TB_USUARIOS");

            entity.Property(e => e.NM_USUARIO)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PERFIL)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TB_PROJETO_TAREFAS_HIST>(entity =>
        {
            entity.HasKey(e => e.ID_HISTORICO).HasName("PK_HISTORICO");

            entity.ToTable("TB_PROJETO_TAREFAS_HIST");

            entity.Property(e => e.COMENTARIO)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.DESCRICAO)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.DT_ATUALIZACAO).HasColumnType("datetime");
            entity.Property(e => e.PRIORIDADE)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.STATUS_TAREFA)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TITULO)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VENCIMENTO).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

public DbSet<GERTAR.Modelos.TB_PROJETO_TAREFAS_HIST> TB_PROJETO_TAREFAS_HIST { get; set; } = default!;
}
