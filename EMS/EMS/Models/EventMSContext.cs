using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EMS.Models
{
    public partial class EventMSContext : DbContext
    {
        public EventMSContext()
        {
        }

        public EventMSContext(DbContextOptions<EventMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AllowedEventGroup> AllowedEventGroups { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<EventInvitation> EventInvitations { get; set; }
        public virtual DbSet<EventStatus> EventStatuses { get; set; }
        public virtual DbSet<EventTicket> EventTickets { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupUser> GroupUsers { get; set; }
        public virtual DbSet<InvitationResponseType> InvitationResponseTypes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("workstation id=EventMS.mssql.somee.com;packet size=4096;user id=fatsfish1_SQLLogin_1;pwd=29c7bm9wpx;data source=EventMS.mssql.somee.com;persist security info=False;initial catalog=EventMS");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AllowedEventGroup>(entity =>
            {
                entity.ToTable("AllowedEventGroup");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.AllowedEventGroups)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__AllowedEv__Event__34C8D9D1");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.AllowedEventGroups)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK__AllowedEv__Group__35BCFE0A");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("comment");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.CreationUserId).HasColumnName("CreationUserID");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.HasOne(d => d.CreationUser)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.CreationUserId)
                    .HasConstraintName("FK__comment__Creatio__398D8EEE");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__comment__EventId__38996AB5");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Event");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.CreationUserId).HasColumnName("CreationUserID");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.EndDateTime).HasColumnType("datetime");

                entity.Property(e => e.IsPublic).HasColumnName("isPublic");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(70);

                entity.Property(e => e.Place)
                    .IsRequired()
                    .HasMaxLength(70);

                entity.Property(e => e.Price).HasColumnType("decimal(14, 2)");

                entity.Property(e => e.RegistrationEndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDateTime).HasColumnType("datetime");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.HasOne(d => d.CreationUser)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.CreationUserId)
                    .HasConstraintName("FK__Event__CreationU__31EC6D26");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK__Event__StatusID__32E0915F");
            });

            modelBuilder.Entity<EventInvitation>(entity =>
            {
                entity.ToTable("EventInvitation");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.InvitationResponseId).HasColumnName("InvitationResponseID");

                entity.Property(e => e.ResponseDate).HasColumnType("datetime");

                entity.Property(e => e.SentDate).HasColumnType("datetime");

                entity.Property(e => e.TextResponse).IsRequired();

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventInvitations)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__EventInvi__Event__3D5E1FD2");

                entity.HasOne(d => d.InvitationResponse)
                    .WithMany(p => p.EventInvitations)
                    .HasForeignKey(d => d.InvitationResponseId)
                    .HasConstraintName("FK__EventInvi__Invit__3F466844");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EventInvitations)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__EventInvi__UserI__3E52440B");
            });

            modelBuilder.Entity<EventStatus>(entity =>
            {
                entity.ToTable("EventStatus");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<EventTicket>(entity =>
            {
                entity.ToTable("EventTicket");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsPaid).HasColumnName("isPaid");

                entity.Property(e => e.OwnerId).HasColumnName("OwnerID");

                entity.Property(e => e.PaidDate).HasColumnType("datetime");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventTickets)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK__EventTick__Event__4222D4EF");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.EventTickets)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK__EventTick__Owner__4316F928");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Group");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<GroupUser>(entity =>
            {
                entity.ToTable("GroupUser");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupUsers)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK__GroupUser__Group__2D27B809");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GroupUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__GroupUser__UserI__2C3393D0");
            });

            modelBuilder.Entity<InvitationResponseType>(entity =>
            {
                entity.ToTable("InvitationResponseType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Bio).IsRequired();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(140);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(70);

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(70);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRole");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__UserRole__RoleId__286302EC");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__UserRole__UserId__276EDEB3");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
