using NotTwitter.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;



namespace NotTwitter.DataAccess
{
    public class NotTwitterDbContext : DbContext
    {
        public NotTwitterDbContext()
        { }

        public NotTwitterDbContext(DbContextOptions<NotTwitterDbContext> options) : base(options)
        { }

        // One DBSet per table
        public DbSet<Users> Users { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Friendships> Friendships { get; set; }
        public DbSet<Posts> Posts { get; set; }
		public DbSet<FriendRequests> FriendRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>(entity =>
            {
                // UserID PK
                entity.HasKey(u => u.UserID);

                // Identity(1,1)
                entity.Property(u => u.UserID)
                    .UseIdentityColumn();

                // FirstName NVARCHAR(50)
                entity.Property(u => u.FirstName)
                    .IsRequired() 
                    .HasMaxLength(50); 

                // LastName NVARCHAR(50)
                entity.Property(u => u.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                // Email NVARCHAR(64)
                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(64);

                // Gender Int
                entity.Property(u => u.Gender)
                    .IsRequired();

                // Username NVARCHAR(50)
                entity.Property(u => u.Username)
                    .HasMaxLength(50)
                    .IsRequired();
                 /*
                // Password ??
                entity.Property(u => u.Password)
                    .IsRequired();
					*/
            });

            modelBuilder.Entity<Comments>(entity =>
            {
                entity.HasKey(c => c.CommentId);

                entity.Property(c => c.CommentId)
                    .UseIdentityColumn();

                entity.Property(c => c.PostId)
                    .IsRequired();

                entity.Property(c => c.UserId)
                    .IsRequired();

                entity.Property(c => c.Content)
                    .IsRequired();

                entity.Property(c => c.TimeSent)
                    .IsRequired();

                // Multiplicities for Users
                entity.HasOne(c => c.User)
                    .WithMany(u => u.Comments)
                    .HasForeignKey(c=>c.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                // Multiplicities for Posts
                entity.HasOne(c => c.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(c=>c.PostId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Posts>(entity =>
            {
                entity.HasKey(p => p.PostId);

                entity.Property(p => p.PostId)
                    .UseIdentityColumn();

                entity.Property(p => p.UserId)
                    .IsRequired();

                entity.Property(p => p.Content)
                    .IsRequired();

                entity.Property(p => p.TimeSent)
                    .IsRequired();

                entity.HasOne(p => p.User)
                    .WithMany(u => u.Posts)
                    .HasForeignKey(p => p.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Friendships>(entity =>
            {
                // Composite key formed with User1ID and User2ID
                entity.HasKey(f => new { f.User1ID, f.User2ID });

                // User1ID
                entity.Property(f => f.User1ID)
                    .IsRequired();

                // User2ID
                entity.Property(f => f.User2ID)
                    .IsRequired();

                entity.Property(f => f.TimeRequestConfirmed)
                    .IsRequired();

                // Navigation property User1
                entity.HasOne(f => f.User1)
                    .WithMany(u => u.OutgoingFriends)
                    .HasForeignKey(f => f.User1ID)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                // Navigation property User2
                entity.HasOne(f => f.User2)
                    .WithMany(u => u.IncomingFriends)
                    .HasForeignKey(f => f.User2ID)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

            });

			modelBuilder.Entity<FriendRequests>(entity =>
			{
                // Composite key of senderId and receiverId
                entity.HasKey(fr => new { fr.SenderId, fr.ReceiverId });

                // SenderId
                entity.Property(f => f.SenderId)
                    .IsRequired();

                // ReceiverId
                entity.Property(f => f.ReceiverId)
                    .IsRequired();

                // FriendRequestStatus
                entity.Property(f => f.FriendRequestStatus)
                   .IsRequired();

                // Navigation property for Sender
                entity.HasOne(f => f.Sender)
                    .WithMany(u => u.FriendRequestsSent)
                    .HasForeignKey(f => f.SenderId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                // Navigation property for Receiver
                entity.HasOne(f => f.Receiver)
                    .WithMany(u => u.FriendRequestsReceived)
                    .HasForeignKey(f => f.ReceiverId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

            });
        }
    }
}

