using Microsoft.EntityFrameworkCore;
using ClasificationProjects.Opi.Back.Infrastructure.Persistence.Entities;

namespace ClasificationProjects.Opi.Back.Infrastructure.Persistence;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbContext(DbContextOptions<DbContext> options) : base(options)
    {
    }

    public DbSet<ChecklistTemplate> ChecklistTemplates => Set<ChecklistTemplate>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<ContractModel> ContractModels => Set<ContractModel>();
    public DbSet<QuestionImpact> QuestionImpacts => Set<QuestionImpact>();
    public DbSet<Evaluation> Evaluations => Set<Evaluation>();

    public DbSet<EvaluationAnswer> EvaluationAnswers => Set<EvaluationAnswer>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

}

