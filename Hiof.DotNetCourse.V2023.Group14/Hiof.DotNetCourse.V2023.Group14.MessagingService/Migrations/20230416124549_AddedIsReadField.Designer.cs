﻿// <auto-generated />
using System;
using Hiof.DotNetCourse.V2023.Group14.MessagingService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Hiof.DotNetCourse.V2023.Group14.MessagingService.Migrations
{
    [DbContext(typeof(MessagingContext))]
    [Migration("20230416124549_AddedIsReadField")]
    partial class AddedIsReadField
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels.V1ConversationModel", b =>
                {
                    b.Property<Guid>("ConversationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.HasKey("ConversationId");

                    b.ToTable("conversations", "dbo");
                });

            modelBuilder.Entity("Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels.V1Messages", b =>
                {
                    b.Property<Guid>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ConversationId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Sender")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("isRead")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("MessageId");

                    b.HasIndex("ConversationId");

                    b.ToTable("messages", "dbo");
                });

            modelBuilder.Entity("Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels.V1Participant", b =>
                {
                    b.Property<string>("Participant")
                        .HasColumnType("varchar(255)");

                    b.Property<Guid>("ConversationId")
                        .HasColumnType("char(36)");

                    b.HasKey("Participant", "ConversationId");

                    b.HasIndex("ConversationId");

                    b.ToTable("participants", "dbo");
                });

            modelBuilder.Entity("Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels.V1Reactions", b =>
                {
                    b.Property<Guid>("ReactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("char(36)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("ReactionId");

                    b.HasIndex("MessageId");

                    b.ToTable("message_reactions", "dbo");
                });

            modelBuilder.Entity("Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels.V1Messages", b =>
                {
                    b.HasOne("Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels.V1ConversationModel", null)
                        .WithMany("Messages")
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_messages_conversations");
                });

            modelBuilder.Entity("Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels.V1Participant", b =>
                {
                    b.HasOne("Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels.V1ConversationModel", null)
                        .WithMany("Participants")
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels.V1Reactions", b =>
                {
                    b.HasOne("Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels.V1Messages", null)
                        .WithMany("Reactions")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_reactions_messages");
                });

            modelBuilder.Entity("Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels.V1ConversationModel", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("Participants");
                });

            modelBuilder.Entity("Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1.MessageModels.V1Messages", b =>
                {
                    b.Navigation("Reactions");
                });
#pragma warning restore 612, 618
        }
    }
}
