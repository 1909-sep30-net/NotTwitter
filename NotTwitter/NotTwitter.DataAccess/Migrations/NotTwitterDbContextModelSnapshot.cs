﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NotTwitter.DataAccess;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DataAccess.Migrations
{
    [DbContext(typeof(NotTwitterDbContext))]
    partial class NotTwitterDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("NotTwitter.DataAccess.Entities.Comments", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<int>("PostId")
                        .HasColumnType("integer")
                        .HasMaxLength(281);

                    b.Property<DateTime>("TimeSent")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("CommentId");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("NotTwitter.DataAccess.Entities.FriendRequests", b =>
                {
                    b.Property<int>("SenderId")
                        .HasColumnType("integer");

                    b.Property<int>("ReceiverId")
                        .HasColumnType("integer");

                    b.Property<int>("FriendRequestStatus")
                        .HasColumnType("integer");

                    b.HasKey("SenderId", "ReceiverId");

                    b.HasIndex("ReceiverId");

                    b.ToTable("FriendRequests");
                });

            modelBuilder.Entity("NotTwitter.DataAccess.Entities.Friendships", b =>
                {
                    b.Property<int>("User1ID")
                        .HasColumnType("integer");

                    b.Property<int>("User2ID")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TimeRequestConfirmed")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("TimeRequestSent")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("User1ID", "User2ID");

                    b.HasIndex("User2ID");

                    b.ToTable("Friendships");
                });

            modelBuilder.Entity("NotTwitter.DataAccess.Entities.Posts", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("TimeSent")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("NotTwitter.DataAccess.Entities.Users", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("character varying(64)")
                        .HasMaxLength(64);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("NotTwitter.DataAccess.Entities.Comments", b =>
                {
                    b.HasOne("NotTwitter.DataAccess.Entities.Posts", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NotTwitter.DataAccess.Entities.Users", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NotTwitter.DataAccess.Entities.FriendRequests", b =>
                {
                    b.HasOne("NotTwitter.DataAccess.Entities.Users", "Receiver")
                        .WithMany("FriendRequestsReceived")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NotTwitter.DataAccess.Entities.Users", "Sender")
                        .WithMany("FriendRequestsSent")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NotTwitter.DataAccess.Entities.Friendships", b =>
                {
                    b.HasOne("NotTwitter.DataAccess.Entities.Users", "User1")
                        .WithMany("IncomingFriends")
                        .HasForeignKey("User1ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NotTwitter.DataAccess.Entities.Users", "User2")
                        .WithMany("OutgoingFriends")
                        .HasForeignKey("User2ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NotTwitter.DataAccess.Entities.Posts", b =>
                {
                    b.HasOne("NotTwitter.DataAccess.Entities.Users", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
