using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ElectronicJournal.Migrations
{
    public partial class Addedgroupsinmodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupID",
                table: "Subject",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupID",
                table: "Student",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupID",
                table: "Lesson",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subject_GroupID",
                table: "Subject",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_Student_GroupID",
                table: "Student",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_Lesson_GroupID",
                table: "Lesson",
                column: "GroupID");

            migrationBuilder.AddForeignKey(
                name: "FK_Lesson_Group_GroupID",
                table: "Lesson",
                column: "GroupID",
                principalTable: "Group",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Group_GroupID",
                table: "Student",
                column: "GroupID",
                principalTable: "Group",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subject_Group_GroupID",
                table: "Subject",
                column: "GroupID",
                principalTable: "Group",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lesson_Group_GroupID",
                table: "Lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Group_GroupID",
                table: "Student");

            migrationBuilder.DropForeignKey(
                name: "FK_Subject_Group_GroupID",
                table: "Subject");

            migrationBuilder.DropIndex(
                name: "IX_Subject_GroupID",
                table: "Subject");

            migrationBuilder.DropIndex(
                name: "IX_Student_GroupID",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Lesson_GroupID",
                table: "Lesson");

            migrationBuilder.DropColumn(
                name: "GroupID",
                table: "Subject");

            migrationBuilder.DropColumn(
                name: "GroupID",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "GroupID",
                table: "Lesson");
        }
    }
}
