using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Codetester.Migrations
{
    public partial class CourseTeachers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseUser",
                columns: table => new
                {
                    CoursesId = table.Column<int>(type: "integer", nullable: false),
                    TeachersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseUser", x => new { x.CoursesId, x.TeachersId });
                    table.ForeignKey(
                        name: "FK_CourseUser_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseUser_Users_TeachersId",
                        column: x => x.TeachersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 219, 140, 209, 46, 225, 81, 189, 208, 214, 202, 63, 204, 123, 75, 142, 204, 148, 57, 127, 240, 245, 59, 13, 103, 213, 174, 135, 85, 6, 58, 82, 26, 132, 64, 21, 164, 228, 162, 86, 66, 145, 210, 185, 171, 49, 57, 253, 212, 39, 67, 64, 53, 64, 155, 198, 152, 255, 177, 117, 212, 17, 238, 245, 16 }, new byte[] { 254, 149, 219, 154, 19, 146, 38, 194, 244, 168, 40, 175, 46, 47, 25, 141, 11, 246, 246, 184, 86, 84, 175, 110, 151, 188, 166, 135, 60, 104, 38, 61, 203, 62, 185, 226, 71, 221, 68, 9, 51, 1, 79, 48, 65, 231, 78, 26, 135, 18, 79, 8, 73, 13, 227, 193, 249, 140, 70, 194, 143, 73, 223, 26, 142, 121, 94, 131, 201, 253, 111, 20, 165, 57, 132, 56, 201, 90, 142, 138, 231, 115, 216, 132, 57, 90, 56, 218, 185, 135, 229, 93, 198, 14, 133, 122, 166, 38, 129, 67, 217, 25, 86, 163, 147, 198, 122, 100, 161, 65, 185, 151, 133, 103, 47, 150, 32, 240, 224, 70, 224, 96, 121, 83, 15, 124, 60, 110 } });

            migrationBuilder.CreateIndex(
                name: "IX_CourseUser_TeachersId",
                table: "CourseUser",
                column: "TeachersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseUser");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 117, 179, 84, 52, 67, 178, 77, 171, 135, 229, 198, 9, 206, 177, 185, 18, 203, 202, 180, 127, 238, 146, 225, 165, 55, 200, 121, 209, 225, 95, 187, 228, 45, 223, 137, 87, 173, 199, 3, 106, 93, 167, 81, 208, 219, 60, 211, 133, 217, 225, 93, 47, 93, 137, 197, 185, 187, 132, 124, 183, 38, 20, 248, 223 }, new byte[] { 129, 34, 16, 6, 61, 242, 34, 140, 23, 70, 27, 191, 43, 212, 164, 85, 86, 84, 143, 20, 246, 12, 126, 156, 218, 197, 68, 110, 126, 236, 175, 241, 204, 71, 254, 191, 160, 201, 189, 190, 110, 181, 133, 220, 136, 232, 160, 134, 129, 127, 159, 68, 176, 104, 57, 117, 174, 156, 2, 186, 241, 156, 142, 89, 72, 82, 1, 120, 167, 133, 22, 201, 59, 146, 185, 19, 88, 151, 91, 234, 74, 208, 254, 112, 118, 220, 110, 163, 65, 211, 222, 183, 113, 71, 87, 177, 218, 182, 25, 238, 238, 137, 243, 141, 241, 232, 104, 126, 187, 165, 46, 45, 100, 137, 13, 50, 157, 229, 216, 188, 123, 83, 70, 190, 165, 172, 214, 147 } });
        }
    }
}
