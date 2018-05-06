using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ElectronicJournal.Migrations
{
    public partial class AddNumberLesson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lesson_Subject_SubjectID",
                table: "Lesson");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectID",
                table: "Lesson",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberLesson",
                table: "Lesson",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Lesson_Subject_SubjectID",
                table: "Lesson",
                column: "SubjectID",
                principalTable: "Subject",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lesson_Subject_SubjectID",
                table: "Lesson");

            migrationBuilder.DropColumn(
                name: "NumberLesson",
                table: "Lesson");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectID",
                table: "Lesson",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Lesson_Subject_SubjectID",
                table: "Lesson",
                column: "SubjectID",
                principalTable: "Subject",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
