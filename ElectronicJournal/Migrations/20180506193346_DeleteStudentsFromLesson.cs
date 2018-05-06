using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ElectronicJournal.Migrations
{
    public partial class DeleteStudentsFromLesson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Lesson_LessonID",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_LessonID",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "LessonID",
                table: "Student");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LessonID",
                table: "Student",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_LessonID",
                table: "Student",
                column: "LessonID");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Lesson_LessonID",
                table: "Student",
                column: "LessonID",
                principalTable: "Lesson",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
