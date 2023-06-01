using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendDatabase.Migrations
{
    public partial class AddTriggersProcedures : Migration
    {
        List<string> tables = new List<string>
            { "Elastic","ElasticTypes",
                "Fabric","FabricBrand","FabricTypes",
                "Machine","MiscItemType","MiscObjects",
                "Thread","ThreadColor","ThreadColorFamily","ThreadTypes" };

        protected override void Up(MigrationBuilder migrationBuilder)
        {

            foreach (string table in tables)
            {
                var SqlInsertTrigger = @$"CREATE TRIGGER [dbo].[trg_{table}_Insert] 
                                    ON  [dbo].[{table}]
                                    AFTER Insert
                                    AS 
                                    BEGIN
                                    	SET NOCOUNT ON;
    
	                                    declare @TableName nvarchar(50) = '{table}'
		                                declare @RecordId int
		                                select @RecordId = ID from inserted

		                                exec dbo.AddToUserMapping @TableName, @RecordId
                                    END";

                var SqlDeleteTrigger = $@"CREATE TRIGGER [dbo].[trg_{table}_Delete]
                                    ON [dbo].[{table}]
                                    AFTER DELETE
                                    AS
                                    BEGIN
                                        SET NOCOUNT ON;
                                        
                                        DECLARE @TableName NVARCHAR(50) = '{table}'
                                        DECLARE @RecordId INT
                                    
                                        SELECT @RecordId = ID FROM deleted
                                    
                                        EXEC dbo.DeleteUserMapping @TableName, @RecordId
                                    END
";
                migrationBuilder.Sql(SqlInsertTrigger);
                migrationBuilder.Sql(SqlDeleteTrigger);
            }

            var SqlAddProcedure = @"CREATE PROCEDURE [dbo].[AddToUserMapping] 
                                    @TableName nvarchar(50),
                                    @RecordId int
                                    
                                    AS
                                    BEGIN	
	                                SET NOCOUNT ON;
                                    
	                                declare @UserId nvarchar(50)
                                    SET @UserId = (SELECT TOP 1 Id FROM BackendUserDatabase.dbo.AspNetUsers WHERE UserName = SUSER_SNAME())

                                    INSERT INTO [dbo].[UserMapping] (UserId, TableName, RecordId)
                                    VALUES (@UserId, @TableName, @RecordId)
	                                END";
            migrationBuilder.Sql(SqlAddProcedure);

            var SqlDeleteProcedure = @"CREATE PROCEDURE[dbo].[DeleteFromUserMapping]
                                    @TableName nvarchar(50),
                                    @RecordId int        

                                    AS
                                    BEGIN
                                    
                                    SET NOCOUNT ON;

                                    declare @UserId nvarchar(50)
                                    
                                    SET @UserId = (SELECT TOP 1 Id FROM BackendUserDatabase.dbo.AspNetUsers WHERE UserName = SUSER_SNAME())

                                    delete from[dbo].[UserMapping]
                                        where UserId = @UserId
                                        and TableName = @TableName
                                        and RecordId = @RecordId
                                    END";
            migrationBuilder.Sql(SqlDeleteProcedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            foreach (string table in tables)
            {
                var SqlInsertTrigger = $"trg_{table}_Insert";
                var SqlDeleteTrigger = $"trg_{table}_Delete";
                migrationBuilder.Sql($"DROP TRIGGER [dbo].[{SqlInsertTrigger}]");
                migrationBuilder.Sql($"DROP TRIGGER [dbo].[{SqlDeleteTrigger}]");
            }

            migrationBuilder.Sql("DROP PROCEDURE [dbo].[AddToUserMapping]");
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[DeleteFromUserMapping]");
        }
    }
}
